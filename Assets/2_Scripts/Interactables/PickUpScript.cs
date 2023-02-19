using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpScript : MonoBehaviour
{
  public GameObject originalParent;
  public Rigidbody rb;
  public bool activated = false;

  public GameObject sceneInitObject;
  public SceneInit sceneInitScript;

  private GameObject respawnCircle;
  public GameObject respawnCircleObject;
  public Vector3 scenePos;
  public string sceneName;
  public int id;

  public int backupID; //for head backup points only

  public SpriteRenderer spriteRend;

  public Sprite sprite_blaster;
  public Sprite sprite_missile;
  public Sprite sprite_laser;
  public Sprite sprite_electro;

  public Sprite sprite_workerBoots;
  public Sprite sprite_jumpBoots;

  public Sprite sprite_pickup_0;
  public Sprite sprite_pickup_1;
  public Sprite sprite_pickup_2;
  public Sprite sprite_pickup_3;
  public Sprite sprite_pickup_4;
  public Sprite sprite_pickup_5;
  public Sprite sprite_pickup_6;
  public Sprite sprite_backup;

  public Material defaultSpriteMat;
  public Material outlineSpriteMat;
  public Material whiteSpriteMat;

  public float tintFadeTimer = 0.5f;
  public bool isSpawning = false;

  public GameObject UI_PickUp;
  public SpriteRenderer UISpriteRend;

  public GameObject UI_BackUp;
  public SpriteRenderer BackUpRend;

  public GameObject particlePickup;

  public int pickupType = 0;//0=Head, 1=Body, 2=Drill, 3=Gun, 4=Legs, 9=BackupPoint

  public int gunType;//0=Bullet, 1=Missile, 2=Laser, 3=EMP
  public int drillType;//0=Weak
  public int bodyType;//0=worker body, 1=soldier body
  public int legType;//0=worker boots, 1=jump boots

  public int progress1;
  public int progress2;
  public int progressNum;

  public GameObject pingObject;
  public Animator pingAnim;

  public bool isUsed = false;
  public bool isActivated;

  public bool originalParentSet = false;

  public bool nameActivated = false;

  public GameObject prevBotOwner;
  public GameObject backupObjectsContainer;

  void Awake()
  {
    sceneInitObject = GameObject.Find("SceneInit");
    sceneInitScript = sceneInitObject.GetComponent<SceneInit>();

    if (transform.parent != null)
    {
      originalParent = transform.parent.gameObject;
    }

    rb = GetComponent<Rigidbody>();
    spriteRend = GetComponent<SpriteRenderer>();
    defaultSpriteMat = spriteRend.material;

    UISpriteRend = UI_PickUp.GetComponent<SpriteRenderer>();
    if (pickupType == 9)
    {
      BackUpRend = UI_BackUp.GetComponent<SpriteRenderer>();
    }

    if (GameObject.Find("GameController") != null)
    {

      backupObjectsContainer = PersistentGameObjects.current.backupObjectsContainer;

      AddToList();
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    if (pickupType == 3)
    { //If pickup is a gun
      if (gunType == 0)
      {
        spriteRend.sprite = sprite_blaster;
      }
      else if (gunType == 1)
      {
        spriteRend.sprite = sprite_missile;
      }
      else if (gunType == 2)
      {
        spriteRend.sprite = sprite_laser;
      }
      else if (gunType == 3)
      {
        spriteRend.sprite = sprite_electro;
      }
    }

    if (pickupType == 4)
    { //If pickup is legs
      if (legType == 0)
      {
        spriteRend.sprite = sprite_workerBoots;
      }
      else if (legType == 1)
      {
        spriteRend.sprite = sprite_jumpBoots;
      }
    }
    if (pickupType == 9)
    { //Head Backup Point
      pingAnim = pingObject.GetComponent<Animator>();
      pingObject.SetActive(false);

      Invoke("CheckBackUpID", 0.1f);
    }

    particlePickup.SetActive(false);

    EraseText();
    DrawOutline();
    TransferUpgradeProperties();
  }

  // Update is called once per frame
  void Update()
  {
    if (activated)
    {
      if (Input.GetButtonDown("Interact"))
      {
        PlayerManager.current.interactionAvailable = false;
        TransferUpgradeProperties();
        if (pickupType == 1)
        {
          PlayerManager.current.currentPickup_progress1 = progress1;
          PlayerManager.current.currentPickup_progress2 = progress2;

          PlayerManager.current.PickupBody();
          DestroySelf();
        }
        else if (pickupType == 2)
        {
          PlayerManager.current.currentPickup_progress1 = progress1;
          PlayerManager.current.currentPickup_progress2 = progress2;

          PlayerManager.current.PickupDrill();
          DestroySelf();
        }
        else if (pickupType == 3)
        { //Guns
          PlayerManager.current.currentPickup_progress1 = progress1;
          PlayerManager.current.currentPickup_progress2 = progress2;

          if (gunType == 0)
          {   //Blaster
            PlayerManager.current.PickupBlaster();
          }
          else if (gunType == 1)
          {   //Missile
            PlayerManager.current.PickupMissile();
          }
          else if (gunType == 2)
          {   //EnergyBeam
            PlayerManager.current.PickupEnergyBeam();
          }
          DestroySelf();
        }
        else if (pickupType == 4)
        {
          PlayerManager.current.currentPickup_progress1 = progress1;
          PlayerManager.current.currentPickup_progress2 = progress2;
          if (legType == 0)
          {
            PlayerManager.current.PickupWorkerBoots();
          }
          else if (legType == 1)
          {
            PlayerManager.current.PickupJumpBoots();
          }
          DestroySelf();
        }
        else if (pickupType == 9)
        {
          PlayerManager.current.currentPickup_progress1 = progress1;
          PlayerManager.current.currentPickup_progress2 = progress2;
          PlayerManager.current.currentPickup_activated = isActivated;

          PlayerManager.current.PickupWorkerHead();
          DestroySelf();
        }
        if (PlayerManager.current.pickupSelectableActive)
        {
          PlayerManager.current.pickupSelectableActive = false;
        }
      }
      if (Input.GetButtonDown("Q"))
      {
        if (pickupType == 9)
        {
          SetAsBackup();
        }
      }

      GameController.current.Invoke("HighlightPickups", 0.25f);
    }
  }

  public void SetAsBackup()
  {
    if (!isActivated)
    {
      PlayerManager.current.backupSpawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
      PlayerManager.current.backupBotID = backupID;
      PlayerManager.current.lastBackupBotID = backupID;

      PlayerManager.current.SetBackupPoint(gameObject);

      PersistentGameObjects.current.RemoveGameObject(id);

      pingObject.SetActive(true);
      pingAnim = pingObject.GetComponent<Animator>();

      pingAnim.SetBool("hidden", false);

      DisablePickupParticles();
      EraseOutline();
      EraseText();

      GameController.current.prevBotOwner = prevBotOwner;

      transform.SetParent(backupObjectsContainer.transform);
      activated = false;
      isActivated = true;
    }
  }

  public void FixedUpdate()
  {
    if (isSpawning)
    {
      if (tintFadeTimer > 0)
      {
        tintFadeTimer -= Time.deltaTime;
      }
      else
      {
        isSpawning = false;
        tintFadeTimer = 0.5f;
        spriteRend.material = defaultSpriteMat;
      }
    }
  }

  public void ResetPickup()
  {
    isUsed = false;
    activated = false;
    // transform.SetParent(originalParent.transform);

    pingAnim.SetBool("hidden", true);
    pingObject.SetActive(false);


    DrawOutline();
    EraseText();
    AddToList();
  }
  public void DrawOutline()
  {
    spriteRend.material = outlineSpriteMat;
  }

  public void EraseOutline()
  {
    spriteRend.material = defaultSpriteMat;
  }

  public void DrawText()
  {
    UI_PickUp.SetActive(true);
    nameActivated = true;

    if (pickupType == 9)
    {
      if (!isActivated)
      {
        UI_PickUp.transform.localPosition = new Vector3(.8f, .3f, 0f);
        UI_BackUp.SetActive(true);
      }
      else
      {
        UI_PickUp.transform.localPosition = new Vector3(0f, 0f, 0f);
      }

    }
  }

  public void EraseText()
  {
    UI_PickUp.SetActive(false);
    nameActivated = false;
    if (pickupType == 9)
    {
      if (!isActivated)
      {
        UI_PickUp.transform.localPosition = new Vector3(.8f, .3f, 0f);
        UI_BackUp.SetActive(false);
      }
      else
      {
        UI_PickUp.transform.localPosition = new Vector3(0f, 0f, 0f);
      }
    }
  }

  public void SetScenePos()
  {
    scenePos = transform.position;
  }

  public void DropNewPickup()
  {
    id = PersistentGameObjects.current.nextID;
    PersistentGameObjects.current.nextID++;

    spriteRend.material = whiteSpriteMat;
    isSpawning = true;
    activated = false;

    transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);

    float speed = 15.0f;
    rb.isKinematic = false;
    Vector3 force = transform.forward;
    force = new Vector3(force.x, 3, 0);
    rb.AddForce(force * speed);
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    UI_PickUp.SetActive(false);

    TransferUpgradeProperties();
    GameController.current.Invoke("HighlightPickups", 0.25f);
  }

  public void CheckBackUpID()
  {
    if (id == PlayerManager.current.lastBackupBotID)
    {
      DestroySelf();
      PlayerManager.current.lastBackupBotID = 0;
    }
  }

  public void TransferUpgradeProperties()
  {
    progressNum = progress1 + progress2;

    if (progressNum == 0)
    {
      UISpriteRend.sprite = sprite_pickup_0;
    }
    else if (progressNum == 1)
    {
      UISpriteRend.sprite = sprite_pickup_1;
    }
    else if (progressNum == 2)
    {
      UISpriteRend.sprite = sprite_pickup_2;
    }
    else if (progressNum == 3)
    {
      UISpriteRend.sprite = sprite_pickup_3;
    }
    else if (progressNum == 4)
    {
      UISpriteRend.sprite = sprite_pickup_4;
    }
    else if (progressNum == 5)
    {
      UISpriteRend.sprite = sprite_pickup_5;
    }
    else if (progressNum == 6)
    {
      UISpriteRend.sprite = sprite_pickup_6;
    }

    //rename gameobject
    if (pickupType == 9)
    {
      gameObject.name = "BotHead " + id.ToString();
    }
    else if (pickupType == 1)
    {
      gameObject.name = "WorkerBody " + id.ToString();
    }
    else if (pickupType == 2)
    {
      gameObject.name = "WorkerDrill " + id.ToString();
    }
    else if (pickupType == 3)
    {
      if (gunType == 0)
      {
        gameObject.name = "BlasterGun " + id.ToString();
      }
      else if (gunType == 1)
      {
        gameObject.name = "MissileLauncher " + id.ToString();
      }
      else if (gunType == 2)
      {
        gameObject.name = "EnergyBeam " + id.ToString();
      }
      else if (gunType == 3)
      {
        gameObject.name = "ElectroGun " + id.ToString();
      }
    }
    else if (pickupType == 4)
    {
      if (legType == 0)
      {
        gameObject.name = "WorkerLegs " + id.ToString();
      }
      else if (legType == 1)
      {
        gameObject.name = "WorkerLegs " + id.ToString();
      }
    }
  }

  public void EnablePickupParticles()
  {
    particlePickup.SetActive(true);
  }
  public void DisablePickupParticles()
  {
    particlePickup.SetActive(false);
  }

  public void DefaultText()
  {
    DrawText();
  }

  public void AddToList()
  {
    Scene scene = SceneManager.GetActiveScene();
    sceneName = scene.name;

    GameController.current.ListPickups.Add(gameObject);
    PersistentGameObjects.current.AddGameObject(gameObject, sceneName);
  }

  public void SetOriginalParent()
  {
    if (!originalParentSet)
    {
      originalParent = transform.parent.gameObject;
      originalParentSet = true;
    }
  }

  public void RemoveFromList()
  {
    GameController.current.ListPickups.Remove(gameObject);
    PersistentGameObjects.current.RemoveGameObject(id);
  }

  public void Active()
  {
    GameController.current.ListPickups.Add(gameObject);
    transform.position = scenePos;
    rb.useGravity = true;
    rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
  }
  public void Inactive()
  {
    GameController.current.ListPickups.Remove(gameObject);
    rb.useGravity = false;
    rb.constraints = RigidbodyConstraints.FreezeAll;
    transform.position = new Vector3(9000f, 9000f, 9000f);
  }

  public void DestroySelf()
  {
    GameController.current.ListPickups.Remove(gameObject);
    PersistentGameObjects.current.RemoveGameObject(id);
    Destroy(gameObject);
  }
}
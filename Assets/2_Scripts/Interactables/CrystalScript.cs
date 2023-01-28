using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalScript : MonoBehaviour
{
    public CrystalScriptableObject crystalData;

    [SerializeField] private GameObject damageNum;
    private DamageNum damageNumScript;

    public GameObject sceneInitObject;
    public SceneInit sceneInitScript;

    public GameObject originalParent;
    public Rigidbody rb;
    public bool activated = false;

    public int size = 0;//0 = small, 1 = large
    public int numOfChunks = 0;

    public Vector3 scenePos;
    public string sceneName;
    public int id;

    public PlayerControl controlScript;

    public int _type;
    public float _health;

    private float cooldownTimer = 1.0f;
    private bool drillCooldown = false;

    public SpriteRenderer spriteRend;
    public CapsuleCollider collider;

    public GameObject crystalHitParticle;
    public GameObject crystalChunkObject;

    public GameObject chunkSpawnOrigin;
    public Vector3 chunkSpawnPos;

    public Animator anim;

    public Vector3 drillPos;

    public float chunkSpawnTimer = 0.1f;
    public bool chunkSpawned = false;

    public bool originalParentSet = false;

    void Awake()
    {
        sceneInitObject = GameObject.Find("SceneInit");
        sceneInitScript = sceneInitObject.GetComponent<SceneInit>();

        rb = GetComponent<Rigidbody>();

        if (transform.parent != null)
        {
            originalParent = transform.parent.gameObject;
        }

        AddToList();
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _type = crystalData.type;
        _health = crystalData.health;

        spriteRend = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider>();

        chunkSpawnPos = new Vector3(chunkSpawnOrigin.transform.position.x, chunkSpawnOrigin.transform.position.y, chunkSpawnOrigin.transform.position.z);

        if (size == 0)
        {
            numOfChunks = 3;
        }
        else if (size == 1)
        {
            numOfChunks = 6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (drillCooldown)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                drillCooldown = false;
                cooldownTimer = 1.0f;
                anim.SetBool("Hit", false);
            }
        }

        if (chunkSpawned)
        {
            if (numOfChunks >= 1)
            {
                if (chunkSpawnTimer > 0)
                {
                    chunkSpawnTimer -= Time.deltaTime;
                }
                else
                {
                    SpawnChunks();
                    chunkSpawnTimer = 0.1f;
                }
            }
            else
            {
                chunkSpawned = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PlayerDrill")
        {
            if (!drillCooldown)
            {
                controlScript = PlayerManager.current.currentPlayerObject.GetComponent<PlayerControl>();

                drillPos = other.gameObject.transform.position;
                TakeHit();
            }
        }
    }

    public void TakeHit()
    {
        if (controlScript.facingLeft)
        {
            drillPos = new Vector3(drillPos.x - 1.0f, drillPos.y, drillPos.z);
        }
        else
        {
            drillPos = new Vector3(drillPos.x + 1.0f, drillPos.y, drillPos.z);
        }

        if (_health > 0)
        {
            GameObject hitParticles = Instantiate(crystalHitParticle, new Vector3(drillPos.x, drillPos.y, drillPos.z), Quaternion.identity) as GameObject;
            drillCooldown = true;
            _health -= PlayerManager.current.currentDamage_workerDrill;
            anim.SetBool("Hit", true);

            DisplayDamage();
            AudioManager.current.currentSFXTrack = 20;
            AudioManager.current.PlaySfx();
        }
        else
        {
            DisplayDamage();
            CrystalDestroyed();
            GameObject hitParticles = Instantiate(crystalHitParticle, new Vector3(drillPos.x, drillPos.y, drillPos.z), Quaternion.identity) as GameObject;
            drillCooldown = true;
            AudioManager.current.currentSFXTrack = 21;
            AudioManager.current.PlaySfx();
        }
    }

    public void DisplayDamage()
    {
        if (GameController.current.damageNumOption)
        {
            GameObject newDamageNum = Instantiate(damageNum, new Vector3(drillPos.x, drillPos.y + 0.5f, drillPos.z), Quaternion.identity) as GameObject;
            DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
            damageNumScript.damageNum = PlayerManager.current.currentDamage_workerDrill;
            GameObject canvasObject = GameObject.Find("WorldCanvas");
            newDamageNum.transform.SetParent(canvasObject.transform);
            damageNumScript.DamageInit();
        }
    }

    public void SetScenePos()
    {
        scenePos = transform.position;
    }

    private void SpawnChunks()
    {
        numOfChunks -= 1;
        GameObject crystalChunk = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x, chunkSpawnPos.y, chunkSpawnPos.z), Quaternion.identity) as GameObject;
    }

    private void CrystalDestroyed()
    {
        anim.SetBool("Break", true);
        chunkSpawned = true;
        collider.enabled = false;
    }

    public void AddToList()
    {
        Scene scene = SceneManager.GetActiveScene();
        sceneName = scene.name;

        GameController.current.ListCrystals.Add(gameObject);
        PersistentGameObjects.current.AddGameObject(gameObject, sceneName);
    }

    public void RemoveFromList()
    {
        GameController.current.ListCrystals.Remove(gameObject);
        PersistentGameObjects.current.RemoveGameObject(id);
    }

    public void SetOriginalParent()
    {
        if (!originalParentSet)
        {
            originalParent = transform.parent.gameObject;
            originalParentSet = true;
        }
    }

    public void Active()
    {
        RemoveFromList();
        transform.position = scenePos;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }
    public void Inactive()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.position = new Vector3(8000f, 8000f, 8000f);
    }

    public void DestroySelf()
    {
        GameController.current.ListCrystals.Remove(gameObject);
        PersistentGameObjects.current.RemoveGameObject(id);
        Destroy(gameObject);
    }
}

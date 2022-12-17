using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public Rigidbody rb;
    public bool activated = false;

    public GameObject sceneInitObject;
    public SceneInit sceneInitScript;

    private GameObject respawnCircle;
    public GameObject respawnCircleObject;
    public Vector3 scenePos;

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

    public Material defaultSpriteMat;
    public Material outlineSpriteMat;
    public Material whiteSpriteMat;

    public float tintFadeTimer = 0.5f;
    public bool isSpawning = false;

    public GameObject UI_PickUp;
    public SpriteRenderer UISpriteRend;

    public GameObject particlePickup;

    public int pickupType = 0;//0=Head, 1=Body, 2=Drill, 3=Gun, 4=Legs, 9=BackupPoint

    public int gunType;//0=Bullet, 1=Missile, 2=Laser, 3=EMP
    public int drillType;//0=Weak, 1=Sturdy, 2=Extended
    public int bodyType;//0=worker body, 1=soldier body
    public int legType;//0=worker boots, 1=jump boots

    public int progress1;
    public int progress2;
    public int progressNum;

    public GameObject pingObject;
    public Animator pingAnim;

    public bool isUsed = false;

    public GameObject prevBotOwner;

    // Start is called before the first frame update
    void Start()
    {
        sceneInitObject = GameObject.Find("SceneInit");
        sceneInitScript = sceneInitObject.GetComponent<SceneInit>();

        rb = GetComponent<Rigidbody>();
        spriteRend = GetComponent<SpriteRenderer>();
        defaultSpriteMat = spriteRend.material;

        UISpriteRend = UI_PickUp.GetComponent<SpriteRenderer>();

        if(pickupType == 0 || pickupType == 9){ //Head Backup Point
            pingAnim = pingObject.GetComponent<Animator>();
            pingObject.SetActive(false);
            respawnCircle = Instantiate(respawnCircleObject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
            respawnCircle.transform.parent = transform;
            respawnCircle.transform.localPosition = new Vector3(0,0,0);
        }
        if(pickupType == 3){ //If pickup is a gun
            if(gunType == 0){
                spriteRend.sprite = sprite_blaster;
            }
            else if(gunType == 1){
                spriteRend.sprite = sprite_missile;
            }
            else if(gunType == 2){
                spriteRend.sprite = sprite_laser;
            }
            else if(gunType == 3){
                spriteRend.sprite = sprite_electro;
            }
        }

        if(pickupType == 4){ //If pickup is legs
            if(legType == 0){
                spriteRend.sprite = sprite_workerBoots;
            }
            else if(legType == 1){
                spriteRend.sprite = sprite_jumpBoots;
            }
        }

        particlePickup.SetActive(false);

        GameController.current.ListPickups.Add(gameObject);

        EraseOutline();
        EraseText();
        TransferUpgradeProperties();
    }

    // Update is called once per frame
    void Update()
    {
        if(activated){
            if(Input.GetButtonDown("Interact")){
                TransferUpgradeProperties();
                if(pickupType == 0){
                    if(!isUsed){
                        PlayerManager.current.backupSpawnPos = new Vector3(transform.position.x, transform.position.y,transform.position.z);
                        PlayerManager.current.backupObject = gameObject;
                        PlayerManager.current.Invoke("SetBackupPoint",0.1f);
                        pingObject.SetActive(true);
                        pingAnim.SetBool("hidden", false);
                        isUsed = true;
                        
                        GameController.current.prevBotOwner = null;
                    }
                }
                if(pickupType == 1){
                    if(!PlayerManager.current.hasBody){
                        PlayerManager.current.temp_body_progress1 = progress1;
                        PlayerManager.current.temp_body_progress2 = progress2;
                        PlayerManager.current.temp_body_progressNum = progressNum;
                        PlayerManager.current.Invoke("PickupBody",0.1f);
                        DestroySelf();
                    }
                    else{
                        
                    }
                }
                else if(pickupType == 2){
                    if(!PlayerManager.current.hasDrill){
                        PlayerManager.current.temp_drill_progress1 = progress1;
                        PlayerManager.current.temp_drill_progress2 = progress2;
                        PlayerManager.current.temp_drill_progressNum = progressNum;
                        PlayerManager.current.Invoke("PickupDrill",0.1f);
                        DestroySelf();
                    }
                    else{

                    }
                }
                else if(pickupType == 3){ //Guns
                    if(PlayerManager.current.hasGun){
                        DestroySelf();
                        if(gunType == 0){   //Blaster
                            if(PlayerManager.current.gunType == 0){
                                //Repair Gun
                            }
                            else{
                                PlayerManager.current.temp_gun_progress1 = progress1;
                                PlayerManager.current.temp_gun_progress2 = progress2;
                                PlayerManager.current.temp_gun_progressNum = progressNum;
                                PlayerManager.current.Invoke("SwitchToBlaster",0.1f);
                            }
                        }
                        else if(gunType == 1){   //Missile
                            if(PlayerManager.current.gunType == 1){
                                //Repair Gun
                            }
                            else{
                                PlayerManager.current.temp_gun_progress1 = progress1;
                                PlayerManager.current.temp_gun_progress2 = progress2;
                                PlayerManager.current.temp_gun_progressNum = progressNum;
                                PlayerManager.current.Invoke("SwitchToMissile",0.1f);
                            }
                        }
                        else if(gunType == 2){   //Laser
                            if(PlayerManager.current.gunType == 2){
                                //Repair Gun
                            }
                            else{
                                PlayerManager.current.temp_gun_progress1 = progress1;
                                PlayerManager.current.temp_gun_progress2 = progress2;
                                PlayerManager.current.temp_gun_progressNum = progressNum;
                                PlayerManager.current.Invoke("SwitchToLaser",0.1f);
                            }
                        }
                        else if(gunType == 3){   //Electro
                            if(PlayerManager.current.gunType == 3){
                                //Repair Gun
                            }
                            else{
                                PlayerManager.current.temp_gun_progress1 = progress1;
                                PlayerManager.current.temp_gun_progress2 = progress2;
                                PlayerManager.current.temp_gun_progressNum = progressNum;
                                PlayerManager.current.Invoke("SwitchToElectro",0.1f);
                            }
                        }
                    }
                    else{
                        DestroySelf();
                        if(gunType == 0){  
                            PlayerManager.current.temp_gun_progress1 = progress1;
                            PlayerManager.current.temp_gun_progress2 = progress2;
                            PlayerManager.current.temp_gun_progressNum = progressNum;
                            PlayerManager.current.Invoke("PickupBlaster",0.1f);
                        }
                        else if(gunType == 1){  
                            PlayerManager.current.temp_gun_progress1 = progress1;
                            PlayerManager.current.temp_gun_progress2 = progress2;
                            PlayerManager.current.temp_gun_progressNum = progressNum;
                            PlayerManager.current.Invoke("PickupMissile",0.1f);
                        }
                        else if(gunType == 2){
                            PlayerManager.current.temp_gun_progress1 = progress1;
                            PlayerManager.current.temp_gun_progress2 = progress2;
                            PlayerManager.current.temp_gun_progressNum = progressNum;
                            PlayerManager.current.Invoke("PickupLaser",0.1f);
                        }
                        else if(gunType == 3){
                            PlayerManager.current.temp_gun_progress1 = progress1;
                            PlayerManager.current.temp_gun_progress2 = progress2;
                            PlayerManager.current.temp_gun_progressNum = progressNum;
                            PlayerManager.current.Invoke("PickupElectro",0.1f);
                        }
                    }
                }
                else if(pickupType == 4){
                    if(!PlayerManager.current.hasLegs){
                        DestroySelf();
                        if(legType == 0){
                            PlayerManager.current.temp_legs_progress1 = progress1;
                            PlayerManager.current.temp_legs_progress2 = progress2;
                            PlayerManager.current.temp_legs_progressNum = progressNum;
                            PlayerManager.current.Invoke("PickupWorkerBoots",0.1f);
                        }
                        else if(legType == 1){
                            PlayerManager.current.temp_legs_progress1 = progress1;
                            PlayerManager.current.temp_legs_progress2 = progress2;
                            PlayerManager.current.temp_legs_progressNum = progressNum;
                            PlayerManager.current.Invoke("PickupJumpBoots",0.1f);
                        }
                    }
                    else{
                        DestroySelf();
                        if(legType == 0){
                            PlayerManager.current.temp_legs_progress1 = progress1;
                            PlayerManager.current.temp_legs_progress2 = progress2;
                            PlayerManager.current.temp_legs_progressNum = progressNum;
                            PlayerManager.current.Invoke("SwitchToWorkerBoots",0.1f);
                        }
                        else if(legType == 1){
                            PlayerManager.current.temp_legs_progress1 = progress1;
                            PlayerManager.current.temp_legs_progress2 = progress2;
                            PlayerManager.current.temp_legs_progressNum = progressNum;
                            PlayerManager.current.Invoke("SwitchToJumpBoots",0.1f);
                        }
                    }
                }
                else if(pickupType == 9){
                    if(!isUsed){
                        PlayerManager.current.backupSpawnPos = new Vector3(transform.position.x, transform.position.y,transform.position.z);
                        PlayerManager.current.backupObject = gameObject;
                        PlayerManager.current.Invoke("SetBackupPoint",0.1f);
                        pingObject.SetActive(true);
                        pingAnim.SetBool("hidden", false);

                        DisablePickupParticles();
                        
                        GameController.current.prevBotOwner = prevBotOwner;
                        isUsed = true;
                    }
                }
            }    
        }
    }

    public void FixedUpdate(){
        if(isSpawning){
            if(tintFadeTimer > 0){
                tintFadeTimer -= Time.deltaTime;
            }
            else{
                isSpawning = false;
                tintFadeTimer = 0.5f;
                spriteRend.material = defaultSpriteMat;
            }
        }
    }

    public void DrawOutline(){
        spriteRend.material = outlineSpriteMat;
    }

    public void EraseOutline(){
        spriteRend.material = defaultSpriteMat;
    }

    public void DrawText(){
        UI_PickUp.SetActive(true);
    }

    public void EraseText(){
        UI_PickUp.SetActive(false);
    }

    public void SetScenePos(){
        scenePos = transform.position;
    }

    public void DropNewPickup(){
        spriteRend.material = whiteSpriteMat;
        isSpawning = true;
        activated = false;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);

        float speed = 50.0f;
        rb.isKinematic = false;
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 2, 0);
        rb.AddForce(force * speed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        UI_PickUp.SetActive(false);

        GameController.current.Invoke("HighlightPickups", 0.1f);
        TransferUpgradeProperties();
    }

    public void TransferUpgradeProperties(){
        progressNum = progress1 + progress2;

        if(progressNum == 0){
            UISpriteRend.sprite = sprite_pickup_0;
        }
        else if(progressNum == 1){
            UISpriteRend.sprite = sprite_pickup_1;
        }
        else if(progressNum == 2){
            UISpriteRend.sprite = sprite_pickup_2;
        }
        else if(progressNum == 3){
            UISpriteRend.sprite = sprite_pickup_3;
        }
        else if(progressNum == 4){
            UISpriteRend.sprite = sprite_pickup_4;
        }
        else if(progressNum == 5){
            UISpriteRend.sprite = sprite_pickup_5;
        }
        else if(progressNum == 6){
            UISpriteRend.sprite = sprite_pickup_6;
        }

        //rename gameobject
        if(pickupType == 0){
            gameObject.name = "BotHead " + progressNum.ToString();
        }
        else if(pickupType == 1){
            gameObject.name = "WorkerBody " + progressNum.ToString();
        }
        else if(pickupType == 2){
            gameObject.name = "WorkerDrill " + progressNum.ToString();
        }
        else if(pickupType == 3){
            if(legType == 0){
                gameObject.name = "BlasterGun " + progressNum.ToString();
            }
            else if(legType == 1){
                gameObject.name = "MissileLauncher " + progressNum.ToString();
            }
            else if(legType == 2){
                gameObject.name = "EnergyBeam " + progressNum.ToString();
            }
            else if(legType == 3){
                gameObject.name = "ElectroGun " + progressNum.ToString();
            }
        }
        else if(pickupType == 4){
            if(legType == 0){
                gameObject.name = "WorkerLegs " + progressNum.ToString();
            }
            else if(legType == 1){
                gameObject.name = "WorkerLegs " + progressNum.ToString();
            }
        }
    }

    public void EnablePickupParticles(){
        particlePickup.SetActive(true);
        DrawOutline();
    }
    public void DisablePickupParticles(){
        particlePickup.SetActive(false);
        EraseOutline();
    }

    public void DefaultText(){
        DrawText();
    }

    public void RemoveFromList(){
        GameController.current.ListPickups.Remove(gameObject);
    }

    public void DestroySelf(){
        GameController.current.ListPickups.Remove(gameObject);
        Destroy(gameObject);
    }
}
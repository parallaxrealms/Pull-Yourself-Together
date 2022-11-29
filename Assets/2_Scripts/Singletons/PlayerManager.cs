using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;
    
    private AudioSource audio;
    public AudioClip clip_reboot;
    public AudioClip clip_pickup;
    public AudioClip clip_playerHit;
    public AudioClip clip_playerHitHard;
    public AudioClip clip_dropPart;
    public AudioClip clip_death;

    public PlayerControl controlScript;
    public GroundedCharacterController groundedCharSript;

    public GameObject healthManagerUI;
    public UI_HealthManager playerHealthManager;

    public bool hasHead = false;
    public bool hasBody = false;
    public bool hasDrill = false;
    public bool hasGun = false;
    public bool hasLegs = false;

    public int gunType = 0;
    public int legType = 0;


    public bool gunFacing;

    public bool canMove = true;
    public bool isChangingScene = false;
    public bool isDead = false;
    public bool isGameOver = false;

    public GameObject mainCamera;
    public BasicCameraTracker cameraScript;

    public GameObject currentPlayerObject;
    public PlayerControl playerControlScript;
    public GameObject playerCapsuleTrigger;

    public GameObject player0_Prefab; //Head
    public GameObject player1_Prefab; //Body
    public GameObject player2_Prefab; //Worker Boots
    public GameObject player3_Prefab; //Jump Boots

    public GameObject pickup_HeadObject;
    public GameObject pickup_BodyObject;
    public GameObject pickup_DrillObject;
    public GameObject pickup_BlasterObject;
    public GameObject pickup_MissileObject;
    public GameObject pickup_LaserObject;
    public GameObject pickup_ElectroObject;
    public GameObject pickup_LegsObject;
    public GameObject pickup_LegsJumpObject;

    private ControlledCapsuleCollider playerCollider;
    public CapsuleCollider capsuleCollider;

    public GameObject spawnPosObject;
    public Vector3 spawnPosition;
    public string sceneDirection;

    public GameObject backupObject;
    public Vector3 backupSpawnPos;
    public bool backedUp = false;
    
    public bool isHit = false;
    public float hitCooldown = 0.5f;

    public int numOfCorite;
    public int numOfNymrite;
    public int numOfVelrite;
    public int numOfZyrite;


    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            if(hasLegs){
                if(Input.GetButtonDown("Crouch")){
                    EnableCrouchState();
                }
                if(Input.GetButtonUp("Crouch")){
                    DisableCrouchState();
                }
            }
        }
    }

    private void FixedUpdate() {
        if(isHit){
            if(hitCooldown > 0.0f){
                hitCooldown -= Time.deltaTime;
            }
            else{
                RecoverFromHit();
                hitCooldown = 0.5f;
            }
        }
    }

    private void InitPlayer(){
        GameController.current.Invoke("GetUI", 0.1f);
        mainCamera = GameObject.Find("Camera");
        cameraScript = mainCamera.GetComponent<BasicCameraTracker>();
        playerHealthManager = healthManagerUI.GetComponent<UI_HealthManager>();

        audio = GetComponent<AudioSource>();

        Reboot();
    }

    public void Reboot(){
        GameController.current.playerSpawned = true;
        currentPlayerObject = Instantiate(player0_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        playerHealthManager.Invoke("GainHead", 0.2f);
        audio.clip = clip_reboot;
        audio.Play();
        hasHead = true;

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);
        GameController.current.Invoke("EnableUI", 0.01f);
    }

    public void RebootBackup(){
        isDead = false;
        backedUp = false;
        spawnPosition = currentPlayerObject.transform.position;
        Destroy(currentPlayerObject);
        Destroy(backupObject);
        currentPlayerObject = Instantiate(player0_Prefab, backupSpawnPos, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        cameraScript.m_Target = currentPlayerObject;
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        playerHealthManager.Invoke("GainHead", 0.2f);
        audio.clip = clip_reboot;
        audio.Play();
        hasHead = true;

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);
        GameController.current.Invoke("EnableUI", 0.01f);
    }

    public void RebootNewMap(){
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);
    }

    //Pickup Bodies
    public void PickupBody(){
        DestroyOldSelf();
        hasBody = true;
        audio.clip = clip_pickup;
        audio.Play();
        playerHealthManager.Invoke("GainBody", 0.1f);

        currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

    }

    //Pickup Drills
    public void PickupDrill(){
        hasDrill = true;
        controlScript.Invoke("EnableDrillArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("GainDrillArm", 0.1f);
        audio.clip = clip_pickup;
        audio.Play();
    }

    //Pickup Guns
    public void PickupBlaster(){
        hasGun = true;
        gunType = 0;
        controlScript.gunType = 0;
        controlScript.Invoke("EnableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("GainBlasterGun", 0.1f);
        audio.clip = clip_pickup;
        audio.Play();
    }
    public void PickupMissile(){
        hasGun = true;
        gunType = 1;
        controlScript.gunType = 1;
        controlScript.Invoke("EnableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("GainMissileLauncher", 0.1f);
        audio.clip = clip_pickup;
        audio.Play();
    }
    public void PickupLaser(){
        hasGun = true;
        gunType = 2;
        controlScript.gunType = 2;
        controlScript.Invoke("EnableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("GainLaserBeam", 0.1f);
        audio.clip = clip_pickup;
        audio.Play();
    }
    public void PickupElectro(){
        hasGun = true;
        gunType = 3;
        controlScript.gunType = 3;
        controlScript.Invoke("EnableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("GainAutoBlaster", 0.1f);
        audio.clip = clip_pickup;
        audio.Play();
    }

    //Pickup Legs
    public void PickupWorkerBoots(){
        hasLegs = true;
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player2_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("GainWorkerBoots", 0.01f);
        audio.clip = clip_pickup;
        audio.Play();

        playerCollider = currentPlayerObject.GetComponent<ControlledCapsuleCollider>();
        capsuleCollider = currentPlayerObject.GetComponent<CapsuleCollider>();

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

        playerHealthManager.Invoke("GainWorkerBoots", 0.1f);
        legType = 0;

        if(hasDrill){
            controlScript.Invoke("EnableDrillArm", 0.1f);
        }
        if(hasGun){
            controlScript.Invoke("EnableGunArm", 0.1f);
        }
    }

    public void PickupJumpBoots(){
        hasLegs = true;
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player3_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("GainJumpBoots", 0.01f);
        audio.clip = clip_pickup;
        audio.Play();

        playerCollider = currentPlayerObject.GetComponent<ControlledCapsuleCollider>();
        capsuleCollider = currentPlayerObject.GetComponent<CapsuleCollider>();

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

        playerHealthManager.Invoke("GainJumpBoots", 0.1f);
        legType = 1;

        if(hasDrill){
            controlScript.Invoke("EnableDrillArm", 0.1f);
        }
        if(hasGun){
            controlScript.Invoke("EnableGunArm", 0.1f);
        }
    }

    //Switching Parts
    public void SwitchHead(){
        controlScript.Invoke("SwitchHead", 0.1f);
    }
    public void SwitchBody(){
        controlScript.Invoke("SwitchBody", 0.1f);
    }
    public void SwitchDrill(){
        controlScript.Invoke("SwitchDrill", 0.1f);
    }
    public void SwitchGun(){
        controlScript.Invoke("SwitchGun", 0.1f);
    }
    public void SwitchLegs(){
        controlScript.Invoke("SwitchLegs", 0.1f);
    }


//Losing Parts
    //Drop Guns
    public void DropGun(){
        if(gunType == 0){
            DropBlaster();
        }
        if(gunType == 1){
            DropMissile();
        }
        if(gunType == 2){
            DropLaser();
        }
        if(gunType == 3){
            DropElectro();
        }
    }

    public void DropBlaster(){
        hasGun = false;
        controlScript.Invoke("DisableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("LoseBlasterGun", 0.1f);
        audio.clip = clip_dropPart;
        audio.Play();

        //Spawn Gun Pickup
        GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;
    }
    public void DropMissile(){
        hasGun = false;
        controlScript.Invoke("DisableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("LoseMissileLauncher", 0.1f);
        audio.clip = clip_dropPart;
        audio.Play();

        //Spawn Gun Pickup
        GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;
    }
    public void DropLaser(){
        hasGun = false;
        controlScript.Invoke("DisableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("LoseLaserBeam", 0.1f);
        audio.clip = clip_dropPart;
        audio.Play();

        //Spawn Gun Pickup
        GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;
    }
    public void DropElectro(){
        hasGun = false;
        controlScript.Invoke("DisableGunArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("LoseAutoBlaster", 0.1f);
        audio.clip = clip_dropPart;
        audio.Play();

        //Spawn Gun Pickup
        GameObject pickup_Electro = Instantiate(pickup_ElectroObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Electro.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;
    }

    public void Death(){
        isDead = true;
        GameController.current.Invoke("InactivateEnemies", 0.01f);
        audio.clip = clip_death;
        audio.Play();
        if(backedUp){
            RebootBackup();
        }
        else{
            GameOver();
        }
    }
    public void GameOver(){
        GameController.current.Invoke("GameOver", 0.01f);
    }
    
    public void DropBody(){
        hasBody = false;
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player0_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        audio.clip = clip_dropPart;
        audio.Play();

        playerCollider = currentPlayerObject.GetComponent<ControlledCapsuleCollider>();
        capsuleCollider = currentPlayerObject.GetComponent<CapsuleCollider>();

        playerHealthManager.Invoke("LoseBody", 0.1f);

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

        //Spawn Body Pickup
        GameObject pickup_Body = Instantiate(pickup_BodyObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Body.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 1;
    }
    public void DropDrill(){
        hasDrill = false;
        controlScript.Invoke("DisableDrillArm", 0.1f);
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.Invoke("LoseDrillArm", 0.1f);
        audio.clip = clip_dropPart;
        audio.Play();

        //Spawn Drill Pickup
        GameObject pickup_Drill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Drill.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 2;
    }


    //Drop Legs
    public void DropLegs(){
        if(legType == 0){
            DropWorkerBoots();
        }
        if(legType == 1){
            DropJumpBoots();
        }
    }

    public void DropWorkerBoots(){
        hasLegs = false;
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        audio.clip = clip_dropPart;
        audio.Play();

        playerCollider = currentPlayerObject.GetComponent<ControlledCapsuleCollider>();
        capsuleCollider = currentPlayerObject.GetComponent<CapsuleCollider>();

        playerHealthManager.Invoke("LoseWorkerBoots", 0.1f);

        if(hasDrill){
            controlScript.Invoke("EnableDrillArm", 0.1f);
        }
        if(hasGun){
            controlScript.Invoke("EnableGunArm", 0.1f);
        }

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);


        //Spawn Legs Pickup
        GameObject pickup_Legs = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Legs.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 4;
    }

    public void DropJumpBoots(){
        hasLegs = false;
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        audio.clip = clip_dropPart;
        audio.Play();

        playerCollider = currentPlayerObject.GetComponent<ControlledCapsuleCollider>();
        capsuleCollider = currentPlayerObject.GetComponent<CapsuleCollider>();

        playerHealthManager.Invoke("LoseJumpBoots", 0.1f);

        if(hasDrill){
            controlScript.Invoke("EnableDrillArm", 0.1f);
        }
        if(hasGun){
            controlScript.Invoke("EnableGunArm", 0.1f);
        }

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

        //Spawn Jump Boots Pickup
        GameObject pickup_JumpBoots= Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_JumpBoots.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 4;
        pickUpScript.legType = 1;
    }

    //Switching Guns
    public void SwitchToBlaster(){
        //Spawn Gun Pickup
        GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;

        //Switch to Blaster
        PickupBlaster();
    }
    public void SwitchToMissile(){
        //Spawn Gun Pickup
        GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;
        //Switch to Missile
        PickupMissile();
    }
    public void SwitchToLaser(){
        //Spawn Gun Pickup
        GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;
        //Switch to Laser
        PickupLaser();
    }
    public void SwitchToElectro(){
        //Spawn Gun Pickup
        GameObject pickup_Electro = Instantiate(pickup_ElectroObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Electro.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 3;
        pickUpScript.gunType = gunType;
        //Switch to Electro
        PickupElectro();
    }

    public void SwitchToWorkerBoots(){
        //Spawn Worker Boots Pickup
        GameObject pickup_Legs = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Legs.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 4;
        pickUpScript.legType = legType;
        //Switch to Worker Boots
        PickupWorkerBoots();
    }
    public void SwitchToJumpBoots(){
        //Spawn Worker Boots Pickup
        GameObject pickup_JumpLegs = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_JumpLegs.GetComponent<PickUpScript>();
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.pickupType = 4;
        pickUpScript.legType = legType;
        //Switch to Jump Boots
        PickupJumpBoots();
    }



    public void TakeHit(){
        if(!isHit && !isDead){
            isHit = true;
            controlScript = currentPlayerObject.GetComponent<PlayerControl>();
            controlScript.Invoke("TakeHit", 0.01f);
            audio.clip = clip_playerHit;
            audio.Play();

            if(hasLegs){
                DropLegs();
            }
            else if(hasGun){
                DropGun();
            }
            else if(hasDrill){
                DropDrill();
            }
            else if(hasBody){
                DropBody();
            }
            else if(hasHead){
                Death();
            }
        }
    } 

    public void RecoverFromHit(){
        isHit = false;
    }

    public void TakeHardHit(){
        if(!isHit && !isDead){
            isHit = true;
            controlScript = currentPlayerObject.GetComponent<PlayerControl>();
            controlScript.Invoke("TakeHit", 0.01f);
            audio.clip = clip_playerHitHard;
            audio.Play();

            if(hasLegs){
                DropLegs();

                if(hasBody){
                    DropBody();
                    if(hasGun){
                        DropGun();
                    }
                    if(hasDrill){
                        DropDrill();
                    }
                }
            }
            else if(hasBody){
                DropBody();
                if(hasGun){
                    DropGun();
                }
                if(hasDrill){
                    DropDrill();
                }
            }
            else if(hasHead){
                Death();
            }
        }
    }
    
    public void SetBackupPoint(){
        backedUp = true;
    }

    public void TransferPlayerProperties(){
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        controlScript.gunType = gunType;
        controlScript.legType = legType;

        GameController.current.hasBody = hasBody;

        GameController.current.hasGun = hasGun;
        GameController.current.gunType = gunType;

        GameController.current.hasDrill = hasDrill;

        GameController.current.hasLegs = hasLegs;
        GameController.current.legType = legType;
    }

    public void DestroyOldSelf(){
        spawnPosition = currentPlayerObject.transform.position;
        Destroy(currentPlayerObject);
        GameController.current.Invoke("HighlightPickups",0.1f);
    }

    public void EnableCrouchState(){
        playerCollider.m_Length = 0f;
    }
    public void DisableCrouchState(){
        playerCollider.m_Length = 0.4f;

    }

    public void PauseMovement(){
        canMove = false;
        controlScript.Invoke("PauseMovement", 0.01f);
    }

    public void ResumeMovement(){
        canMove = true;
        controlScript.Invoke("ResumeMovement", 0.01f);
    }
}

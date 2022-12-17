using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject partsUI;
    public UI_Parts partsUIScript;

    public bool hasHead = false;

    public bool hasBody = false;
    public int temp_body_progress1;
    public int temp_body_progress2;
    public int temp_body_progressNum;

    public bool hasDrill = false;
    public int temp_drill_progress1;
    public int temp_drill_progress2;
    public int temp_drill_progressNum;

    public bool hasGun = false;
    public int temp_gun_progress1;
    public int temp_gun_progress2;
    public int temp_gun_progressNum;

    public bool hasLegs = false;
    public int temp_legs_progress1;
    public int temp_legs_progress2;
    public int temp_legs_progressNum;

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

    public bool playerPartDropped = false;

    private ControlledCapsuleCollider playerCollider;
    public CapsuleCollider capsuleCollider;

    public GameObject spawnPosObject;
    public Vector3 spawnPosition;
    public string sceneDirection;

    public GameObject backupObject;
    public Vector3 backupSpawnPos;
    public bool backedUp = false;
    public string backupSceneName;
    
    public bool isHit = false;
    public float hitCooldown = 0.75f;

    public int numOfCorite;
    public int numOfNymrite;
    public int numOfVelrite;
    public int numOfZyrite;

    public float mLength;
    public float colliderHeight;

    public GameObject respawnCircle;
    public GameObject respawnCircleObject;
    public OnRespawnDestroyNearby respawnCircleScript;

    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if(isHit){
            if(hitCooldown > 0.0f){
                hitCooldown -= Time.deltaTime;
            }
            else{
                RecoverFromHit();
                hitCooldown = 0.75f;
            }
        }
    }

    private void InitPlayer(){
        Debug.Log("playerInit");
        GameController.current.Invoke("GetUI", 0.1f);
        mainCamera = GameObject.Find("Camera");
        cameraScript = mainCamera.GetComponent<BasicCameraTracker>();
        playerHealthManager = healthManagerUI.GetComponent<UI_HealthManager>();
        partsUIScript = partsUI.GetComponent<UI_Parts>();

        audio = GetComponent<AudioSource>();
        Reboot();
    }

    private void GetColliderHeights(){
        capsuleCollider = currentPlayerObject.GetComponent<CapsuleCollider>();
        colliderHeight = capsuleCollider.height;

        playerCollider = currentPlayerObject.GetComponent<ControlledCapsuleCollider>();
        mLength = playerCollider.m_Length;
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
        partsUIScript.Invoke("GainWorkerHead", 0.1f);
        
        audio.clip = clip_reboot;
        audio.Play();
        hasHead = true;

        GetColliderHeights();

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);
        GameController.current.Invoke("EnableUI", 0.01f);
    }

    public void RebootBackup(){
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == backupSceneName){
            RebootPlayer();
        }
        else{
            MenuManager.current.Invoke("ChangeSceneAndReboot",0.01f);
            GameController.current.playerRespawning = true;
        }
    }

    public void RebootPlayer(){
        Debug.Log("RebootPlayer");
        isDead = false;
        backedUp = false;
        Destroy(currentPlayerObject); 
        respawnCircle = Instantiate(respawnCircleObject, backupSpawnPos, Quaternion.identity) as GameObject;
        currentPlayerObject = Instantiate(player0_Prefab, backupSpawnPos, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        cameraScript.m_Target = currentPlayerObject;
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        playerHealthManager.Invoke("GainHead", 0.1f);
        partsUIScript.Invoke("GainWorkerHead", 0.1f);

        audio.clip = clip_reboot;
        audio.Play();
        hasHead = true;

        GameController.current.Invoke("RebootFromCheckpoint", 0.01f);
    }

    public void RebootNewMap(){
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
    }

    //Pickup Bodies
    public void PickupBody(){
        DestroyOldSelf();
        hasBody = true;
        audio.clip = clip_pickup;
        audio.Play();
        playerHealthManager.Invoke("GainBody", 0.1f);
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_body_progress1;
        partsUIScript.temp_progress2 = temp_body_progress2;
        partsUIScript.temp_progressNum = temp_body_progressNum;
        partsUIScript.Invoke("GainWorkerBody", 0.1f);

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
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_drill_progress1;
        partsUIScript.temp_progress2 = temp_drill_progress2;
        partsUIScript.temp_progressNum = temp_drill_progressNum;
        partsUIScript.Invoke("GainWorkerDrill", 0.1f);
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
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_gun_progress1;
        partsUIScript.temp_progress2 = temp_gun_progress2;
        partsUIScript.temp_progressNum = temp_gun_progressNum;
        partsUIScript.Invoke("GainBlaster", 0.1f);
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
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_gun_progress1;
        partsUIScript.temp_progress2 = temp_gun_progress2;
        partsUIScript.temp_progressNum = temp_gun_progressNum;
        partsUIScript.Invoke("GainMissileLauncher", 0.1f);
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
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_gun_progress1;
        partsUIScript.temp_progress2 = temp_gun_progress2;
        partsUIScript.temp_progressNum = temp_gun_progressNum;
        partsUIScript.Invoke("GainLaserBeam", 0.1f);
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
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_gun_progress1;
        partsUIScript.temp_progress2 = temp_gun_progress2;
        partsUIScript.temp_progressNum = temp_gun_progressNum;
        //
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
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_legs_progress1;
        partsUIScript.temp_progress2 = temp_legs_progress2;
        partsUIScript.temp_progressNum = temp_legs_progressNum;
        partsUIScript.Invoke("GainWorkerBoots", 0.1f);

        audio.clip = clip_pickup;
        audio.Play();

        GetColliderHeights();

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

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
        //Transfer upgrade nums from pickup script
        partsUIScript.temp_progress1 = temp_legs_progress1;
        partsUIScript.temp_progress2 = temp_legs_progress2;
        partsUIScript.temp_progressNum = temp_legs_progressNum;
        partsUIScript.Invoke("GainJumpBoots", 0.1f);

        audio.clip = clip_pickup;
        audio.Play();

        GetColliderHeights();

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

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
    public void DropAllParts(){
        playerPartDropped = true;
        if(hasLegs){
            DropLegs();
        }
        if(hasDrill){
            DropDrill();
        }
        if(hasGun){
            DropGun();
        }
        if(hasBody){
            DropBody();
        }

        audio.clip = clip_dropPart;
        audio.Play();
    }
    
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
        playerHealthManager.Invoke("LoseBlasterGun", 0.1f);

        if(playerPartDropped){
            //Spawn Gun Pickup
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
        else{
            cameraScript.m_Target = currentPlayerObject;
            audio.clip = clip_dropPart;
            audio.Play();

            //Spawn Gun Pickup
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
    }
    public void DropMissile(){
        hasGun = false;
        controlScript.Invoke("DisableGunArm", 0.1f);
        playerHealthManager.Invoke("LoseMissileLauncher", 0.1f);

        if(playerPartDropped){
           //Spawn Gun Pickup
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
        else{
            cameraScript.m_Target = currentPlayerObject;
            audio.clip = clip_dropPart;
            audio.Play();

            //Spawn Gun Pickup
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
    }
    public void DropLaser(){
        hasGun = false;
        controlScript.Invoke("DisableGunArm", 0.1f);
        playerHealthManager.Invoke("LoseLaserBeam", 0.1f);

        if(playerPartDropped){
           //Spawn Gun Pickup
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
        else{
            cameraScript.m_Target = currentPlayerObject;
            audio.clip = clip_dropPart;
            audio.Play();

            //Spawn Gun Pickup
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
    }
    public void DropElectro(){
        hasGun = false;
        controlScript.Invoke("DisableGunArm", 0.1f);
        playerHealthManager.Invoke("LoseAutoBlaster", 0.1f);

        if(playerPartDropped){
           //Spawn Gun Pickup
            GameObject pickup_Electro = Instantiate(pickup_ElectroObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Electro.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
        else{
            cameraScript.m_Target = currentPlayerObject;
            audio.clip = clip_dropPart;
            audio.Play();

            //Spawn Gun Pickup
            GameObject pickup_Electro = Instantiate(pickup_ElectroObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Electro.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = gunType;
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
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

        GetColliderHeights();

        playerHealthManager.Invoke("LoseBody", 0.1f);

        GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

        //Spawn Body Pickup
        GameObject pickup_Body = Instantiate(pickup_BodyObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Body.GetComponent<PickUpScript>();
        pickUpScript.pickupType = 1;
        pickUpScript.progress1 = temp_body_progress1;
        pickUpScript.progress2 = temp_body_progress2;
        pickUpScript.progressNum = temp_body_progressNum;
        pickUpScript.Invoke("DropNewPickup", 0.01f);

        playerPartDropped = false;
    }
    public void DropDrill(){
        hasDrill = false;
        controlScript.Invoke("DisableDrillArm", 0.1f);
        playerHealthManager.Invoke("LoseDrillArm", 0.1f);

        if(playerPartDropped){
            //Spawn Drill Pickup
            GameObject pickup_Drill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Drill.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.progress1 = temp_drill_progress1;
            pickUpScript.progress2 = temp_drill_progress2;
            pickUpScript.progressNum = temp_drill_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
        else{
            cameraScript.m_Target = currentPlayerObject;
            audio.clip = clip_dropPart;
            audio.Play();

            //Spawn Drill Pickup
            GameObject pickup_Drill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Drill.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.progress1 = temp_drill_progress1;
            pickUpScript.progress2 = temp_drill_progress2;
            pickUpScript.progressNum = temp_drill_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
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
        playerHealthManager.Invoke("LoseWorkerBoots", 0.1f);
        Debug.Log("temps: " + temp_legs_progress1 + " : " + temp_legs_progress2 + " : " + temp_legs_progressNum);
        if(playerPartDropped){
             //Spawn Worker Boots Pickup
            GameObject pickup_WorkerBoots = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_WorkerBoots.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 0;
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
        else{
            DestroyOldSelf();
            currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
            TransferPlayerProperties();
            currentPlayerObject.transform.parent = transform;
            currentPlayerObject.transform.position = spawnPosition;
            cameraScript.m_Target = currentPlayerObject;

            GetColliderHeights();

            GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

            //Spawn Worker Boots Pickup
            GameObject pickup_WorkerBoots = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_WorkerBoots.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 0;
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);

            if(hasDrill){
                controlScript.Invoke("EnableDrillArm", 0.1f);
            }
            if(hasGun){
                controlScript.Invoke("EnableGunArm", 0.1f);
            }

            audio.clip = clip_dropPart;
            audio.Play();
        }
    }

    public void DropJumpBoots(){
        hasLegs = false;
        playerHealthManager.Invoke("LoseJumpBoots", 0.1f);

        if(playerPartDropped){
            //Spawn Jump Boots Pickup
            GameObject pickup_JumpBoots= Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_JumpBoots.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 1;
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);
        }
        else{
            DestroyOldSelf();
            currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
            TransferPlayerProperties();
            currentPlayerObject.transform.parent = transform;
            currentPlayerObject.transform.position = spawnPosition;
            cameraScript.m_Target = currentPlayerObject;

            GetColliderHeights();

            GameController.current.Invoke("ResetEnemyPlayerObjects", 0.01f);

            if(hasDrill){
            controlScript.Invoke("EnableDrillArm", 0.1f);
            }
            if(hasGun){
                controlScript.Invoke("EnableGunArm", 0.1f);
            }

            //Spawn Jump Boots Pickup
            GameObject pickup_JumpBoots = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_JumpBoots.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 1;
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progressNum;
            pickUpScript.Invoke("DropNewPickup", 0.01f);

            audio.clip = clip_dropPart;
            audio.Play();
        }
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
        Destroy(currentPlayerObject);
        GameController.current.Invoke("GameOver", 0.01f);
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

        Scene scene = SceneManager.GetActiveScene();
        backupSceneName = scene.name;
        GameObject sceneInit = GameObject.Find("SceneInit");
        SceneInit sceneInitScript = sceneInit.GetComponent<SceneInit>();
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
        capsuleCollider.height = colliderHeight / 4;
        playerCollider.m_Length = mLength / 4;
    }
    public void DisableCrouchState(){
        capsuleCollider.height = colliderHeight;
        playerCollider.m_Length = mLength;
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

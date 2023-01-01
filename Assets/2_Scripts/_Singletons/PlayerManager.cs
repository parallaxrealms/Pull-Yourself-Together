using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    [SerializeField] private GameObject damageNum;
    private DamageNum damageNumScript;
    private Vector3 dmgNumPos;
    private int damageTaken;

    public PlayerControl controlScript;
    public GroundedCharacterController groundedCharSript;

    public GameObject healthManagerUI;
    public UI_HealthManager playerHealthManager;

    public GameObject partsUI;
    public UI_Parts partsUIScript;

    public int currentPickup_progress1;
    public int currentPickup_progress2;

    public bool hasHead = false;
    public int headType;
    public int head_progress1;
    public int head_progress2;

    public int temp_head_progress1;
    public int temp_head_progress2;
    public int temp_head_progressNum;

    public bool hasBody = false;
    public int bodyType;
    public int body_progress1;
    public int body_progress2;

    public int temp_body_progress1;
    public int temp_body_progress2;
    public int temp_body_progressNum;

    public bool hasDrill = false;
    public int drillType;
    public int drill_progress1;
    public int drill_progress2;

    public int temp_drill_progress1;
    public int temp_drill_progress2;
    public int temp_drill_progressNum;

    public bool hasGun = false;
    public int gunType;
    public int gun_progress1;
    public int gun_progress2;

    public int temp_gun_progress1;
    public int temp_gun_progress2;
    public int temp_gun_progressNum;

    public bool hasLegs = false;
    public int legType;
    public int legs_progress1;
    public int legs_progress2;

    public int temp_legs_progress1;
    public int temp_legs_progress2;
    public int temp_legs_progressNum;

    public bool gunFacing;

    public bool canMove = true;
    public bool isChangingScene = false;
    public bool isDead = false;
    public bool isGameOver = false;

    private GameObject mainCamera;
    private BasicCameraTracker cameraScript;

    public GameObject currentPlayerObject;
    private GameObject playerCapsuleTrigger;

    public GameObject player0_Prefab; //Head
    public GameObject player1_Prefab; //Body
    public GameObject player2_Prefab; //Worker Boots
    public GameObject player3_Prefab; //Jump Boots

    public GameObject currentPickupObject;

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
    private float hitCooldown = 0.75f;

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

    public void InitPlayer(){
        isDead = false;
        GameController.current.EnableUI();
        mainCamera = GameObject.Find("Camera");
        cameraScript = mainCamera.GetComponent<BasicCameraTracker>();
        playerHealthManager = healthManagerUI.GetComponent<UI_HealthManager>();
        partsUIScript = partsUI.GetComponent<UI_Parts>();

        Reboot();
    }

    public void GetColliderHeights(){
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
        playerHealthManager.GainHead();
        partsUIScript.GainWorkerHead();
        hasHead = true;

        GetColliderHeights();

        GameController.current.ResetEnemyPlayerObjects();
        GameController.current.EnableUI();
    }

    public void RebootBackup(){
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == backupSceneName){
            RebootPlayer();
        }
        else{
            MenuManager.current.ChangeSceneAndReboot();
            GameController.current.playerRespawning = true;
        }
    }

    public void RebootPlayer(){
        isDead = false;
        backedUp = false;
        Destroy(currentPlayerObject); 
        respawnCircle = Instantiate(respawnCircleObject, backupSpawnPos, Quaternion.identity) as GameObject;
        currentPlayerObject = Instantiate(player0_Prefab, backupSpawnPos, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        cameraScript.m_Target = currentPlayerObject;
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        playerHealthManager.GainHead();
        partsUIScript.GainWorkerHead();
        hasHead = true;

        GameController.current.RebootFromCheckpoint();
    }

    public void RebootNewMap(){
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
    }

    //Pickup Bodies
    public void PickupBody(){
        DestroyOldSelf();
        hasBody = true;
        playerHealthManager.GainBody();

        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.GainBody();
        //Transfer upgrade nums from pickup script
        body_progress1 = currentPickup_progress1;
        body_progress2 = currentPickup_progress2;
        partsUIScript.temp_progress1 = currentPickup_progress1;
        partsUIScript.temp_progress2 = currentPickup_progress2;
        partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;
        partsUIScript.GainWorkerBody();

        CheckAndRefreshUpgrades();

        currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;

        if(hasDrill){
            controlScript.EnableDrillArm();
        }
        if(hasGun){
            controlScript.EnableGunArm();
        }

        GameController.current.ResetEnemyPlayerObjects();
    }

    //Pickup Drills
    public void PickupDrill(){
        hasDrill = true;
        TransferPlayerProperties();

        controlScript.EnableDrillArm();
        cameraScript.m_Target = currentPlayerObject;

        playerHealthManager.GainDrillArm();
        //Transfer upgrade nums from pickup script
        drill_progress1 = currentPickup_progress1;
        drill_progress2 = currentPickup_progress2;
        partsUIScript.temp_progress1 = currentPickup_progress1;
        partsUIScript.temp_progress2 = currentPickup_progress2;
        partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

        partsUIScript.GainWorkerDrill();
        CheckAndRefreshUpgrades();
    }

    //Pickup Guns
    public void PickupBlaster(){
        hasGun = true;
        gunType = 0;
        TransferPlayerProperties();
        controlScript.gunType = 0;
        controlScript.EnableGunArm();
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.GainBlasterGun();
        //Transfer upgrade nums from pickup script
        gun_progress1 = currentPickup_progress1;
        gun_progress2 = currentPickup_progress2;
        partsUIScript.temp_progress1 = currentPickup_progress1;
        partsUIScript.temp_progress2 = currentPickup_progress2;
        partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

        partsUIScript.GainBlaster();
        CheckAndRefreshUpgrades();
    }
    public void PickupMissile(){
        hasGun = true;
        gunType = 1;
        TransferPlayerProperties();
        controlScript.gunType = 1;
        controlScript.EnableGunArm();
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.GainMissileLauncher();
        //Transfer upgrade nums from pickup script
        gun_progress1 = currentPickup_progress1;
        gun_progress2 = currentPickup_progress2;
        partsUIScript.temp_progress1 = currentPickup_progress1;
        partsUIScript.temp_progress2 = currentPickup_progress2;
        partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

        partsUIScript.GainMissileLauncher();
        CheckAndRefreshUpgrades();
    }
    public void PickupLaser(){
        hasGun = true;
        gunType = 2;
        TransferPlayerProperties();
        controlScript.gunType = 2;
        controlScript.EnableGunArm();
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.GainLaserBeam();
        //Transfer upgrade nums from pickup script
        gun_progress1 = currentPickup_progress1;
        gun_progress2 = currentPickup_progress2;
        partsUIScript.temp_progress1 = currentPickup_progress1;
        partsUIScript.temp_progress2 = currentPickup_progress2;
        partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

        partsUIScript.GainLaserBeam();
        CheckAndRefreshUpgrades();
    }

    //Pickup Legs
    public void PickupWorkerBoots(){
        hasLegs = true;
        legType = 0;
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player2_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.GainWorkerBoots();
        //Transfer upgrade nums from pickup script
        legs_progress1 = currentPickup_progress1;
        legs_progress2 = currentPickup_progress2;
        partsUIScript.temp_progress1 = currentPickup_progress1;
        partsUIScript.temp_progress2 = currentPickup_progress2;
        partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;
        partsUIScript.GainWorkerBoots();
        CheckAndRefreshUpgrades();

        GetColliderHeights();

        GameController.current.ResetEnemyPlayerObjects();

        if(hasDrill){
            controlScript.EnableDrillArm();
        }
        if(hasGun){
            controlScript.EnableGunArm();
        }
    }

    public void PickupJumpBoots(){
        hasLegs = true;
        legType = 1;
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player3_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        playerHealthManager.GainJumpBoots();
        //Transfer upgrade nums from pickup script
        legs_progress1 = currentPickup_progress1;
        legs_progress2 = currentPickup_progress2;
        partsUIScript.temp_progress1 = currentPickup_progress1;
        partsUIScript.temp_progress2 = currentPickup_progress2;
        partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;
        partsUIScript.GainJumpBoots();
        CheckAndRefreshUpgrades();

        GetColliderHeights();

        GameController.current.ResetEnemyPlayerObjects();

        if(hasDrill){
            controlScript.EnableDrillArm();
        }
        if(hasGun){
            controlScript.EnableGunArm();
        }
    }

    //Switching Guns
    public void SwitchToBlaster(){
        //Spawn Gun Pickup
        if(gunType == 0){
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0;
            pickUpScript.DropNewPickup();
        }
        else if(gunType == 1){
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else if(gunType == 2){
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        //Switch to Blaster
        PickupBlaster();
    }
    public void SwitchToMissile(){
        //Spawn Gun Pickup
        if(gunType == 0){
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0;
            pickUpScript.DropNewPickup();
        }
        else if(gunType == 1){
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else if(gunType == 2){
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        //Switch to Missile
        PickupMissile();
    }
    public void SwitchToLaser(){
        //Spawn Gun Pickup
        if(gunType == 0){
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0;
            pickUpScript.DropNewPickup();
        }
        else if(gunType == 1){
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else if(gunType == 2){
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        //Switch to Laser
        PickupLaser();
    }

    public void SwitchToWorkerBoots(){
        //Spawn Worker Boots Pickup
        if(legType == 0){
            GameObject pickup_Legs = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Legs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 0;
            pickUpScript.DropNewPickup();
        }
        else if(legType == 1){
            GameObject pickup_JumpLegs = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_JumpLegs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 1;
            pickUpScript.DropNewPickup();
        }
        //Switch to Worker Boots
        PickupWorkerBoots();
    }
    public void SwitchToJumpBoots(){
        if(legType == 0){
            GameObject pickup_Legs = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Legs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 0;
            pickUpScript.DropNewPickup();

        }
        else if(legType == 1){
            GameObject pickup_JumpLegs = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_JumpLegs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 1;
            pickUpScript.DropNewPickup();
        }
        //Switch to Jump Boots
        PickupJumpBoots();
    }

    //Losing Parts
    public void DropAllParts(){
        playerPartDropped = true;
        if(hasLegs){
            DropLegs();
        }
        if(hasDrill){
            Invoke("DropDrill",0.1f);
        }
        if(hasGun){
            Invoke("DropGun",0.1f);
        }
        if(hasBody){
            Invoke("DropBody",0.25f);
        }
    }

    public void DropLimbs(){
        playerPartDropped = true;
        if(hasLegs){
            DropLegs();
            Debug.Log("DropLegs? 1");
        }
        if(hasDrill){
            DropDrill();
        }
        if(hasGun){
            DropGun();
        }
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
            //DropElectro();
        }
    }

    public void DropBlaster(){
        hasGun = false;
        controlScript.DisableGunArm();
        playerHealthManager.LoseBlasterGun();

        if(playerPartDropped){
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0; 
            pickUpScript.DropNewPickup();
        }
        else{
            cameraScript.m_Target = currentPlayerObject;

            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0; 
            pickUpScript.DropNewPickup();
        }
        ResetUpgrade(3);

        TransferPlayerProperties();
    }
    public void DropMissile(){
        hasGun = false;
        controlScript.DisableGunArm();
        playerHealthManager.LoseMissileLauncher();

        if(playerPartDropped){
           GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else{
            cameraScript.m_Target = currentPlayerObject;

            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        ResetUpgrade(3);

        TransferPlayerProperties();
    }
    public void DropLaser(){
        hasGun = false;
        controlScript.DisableGunArm();
        playerHealthManager.LoseLaserBeam();

        if(playerPartDropped){
           GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = currentPickup_progress1;
            pickUpScript.progress2 = currentPickup_progress2;
            pickUpScript.progressNum = currentPickup_progress1 + currentPickup_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        else{
            cameraScript.m_Target = currentPlayerObject;

            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = currentPickup_progress1;
            pickUpScript.progress2 = currentPickup_progress2;
            pickUpScript.progressNum = currentPickup_progress1 + currentPickup_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        ResetUpgrade(3);

        TransferPlayerProperties();
    }
    
    public void DropBody(){
        hasBody = false;

        DestroyOldSelf();
        currentPlayerObject = Instantiate(player0_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;

        GetColliderHeights();

        playerHealthManager.LoseBody();
        ResetUpgrade(1);
        TransferPlayerProperties();

        GameController.current.ResetEnemyPlayerObjects();

        //Spawn Body Pickup
        GameObject pickup_Body = Instantiate(pickup_BodyObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Body.GetComponent<PickUpScript>();
        pickUpScript.pickupType = 1;
        pickUpScript.progress1 = temp_body_progress1;
        pickUpScript.progress2 = temp_body_progress2;
        pickUpScript.progressNum = temp_body_progressNum;
        pickUpScript.DropNewPickup();
        playerPartDropped = false;
    }

    public void DropDrill(){
        hasDrill = false;
        controlScript.DisableDrillArm();
        playerHealthManager.LoseDrillArm();

        if(playerPartDropped){
            //Spawn Drill Pickup
            GameObject pickup_Drill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Drill.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.progress1 = temp_drill_progress1;
            pickUpScript.progress2 = temp_drill_progress2;
            pickUpScript.progressNum = temp_drill_progressNum;
            pickUpScript.DropNewPickup();
        }
        else{
            cameraScript.m_Target = currentPlayerObject;

            //Spawn Drill Pickup
            GameObject pickup_Drill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Drill.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.progress1 = temp_drill_progress1;
            pickUpScript.progress2 = temp_drill_progress2;
            pickUpScript.progressNum = temp_drill_progressNum;
            pickUpScript.DropNewPickup();
        }

        ResetUpgrade(2);
        TransferPlayerProperties();
    }


    //Drop Legs
    public void DropLegs(){
        if(legType == 0){
            DropWorkerBoots();
            Debug.Log("DropWorkerBoots? 2");
        }
        if(legType == 1){
            DropJumpBoots();
            Debug.Log("DropJumpBoots? 2");
        }
    }

    public void DropWorkerBoots(){
        hasLegs = false;
        playerHealthManager.LoseWorkerBoots();
        
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;

        GetColliderHeights();

        GameController.current.ResetEnemyPlayerObjects();

        //Spawn Worker Boots Pickup
        GameObject pickup_WorkerBoots = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_WorkerBoots.GetComponent<PickUpScript>();
        pickUpScript.pickupType = 4;
        pickUpScript.legType = 0;
        pickUpScript.progress1 = temp_legs_progress1;
        pickUpScript.progress2 = temp_legs_progress2;
        pickUpScript.progressNum = temp_legs_progressNum;
        pickUpScript.DropNewPickup();

        if(hasDrill){
            controlScript.EnableDrillArm();
        }
        if(hasGun){
            controlScript.EnableGunArm();
        }
        ResetUpgrade(4);
        TransferPlayerProperties();
    }

    public void DropJumpBoots(){
        hasLegs = false;
        playerHealthManager.LoseJumpBoots();
        
        DestroyOldSelf();
        currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        GetColliderHeights();
        GameController.current.ResetEnemyPlayerObjects();

        if(hasDrill){
            controlScript.EnableDrillArm();
        }
        if(hasGun){
            controlScript.EnableGunArm();
        }

        //Spawn Jump Boots Pickup
        GameObject pickup_JumpLegs = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x,currentPlayerObject.transform.position.y,currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_JumpLegs.GetComponent<PickUpScript>();
        pickUpScript.pickupType = 4;
        pickUpScript.legType = 1;
        pickUpScript.progress1 = temp_legs_progress1;
        pickUpScript.progress2 = temp_legs_progress2;
        pickUpScript.progressNum = temp_legs_progressNum;
        pickUpScript.DropNewPickup();

        ResetUpgrade(4);
        TransferPlayerProperties();
    }

    public void Death(){
        ResetUpgrade(0);
        isDead = true;
        GameController.current.InactivateEnemies();
        if(backedUp){
            RebootBackup();
        }
        else{
            GameOver();
        }
    }
    public void GameOver(){
        Destroy(currentPlayerObject);
        GameController.current.GameOver();
    }

    public void TakeHit(){
        if(!isHit && !isDead){
            isHit = true;
            DisplayDamage();

            controlScript = currentPlayerObject.GetComponent<PlayerControl>();
            controlScript.TakeHit();

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

    public void TakeHitLimbs(){
        if(!isHit && !isDead){
            damageTaken = 3;
            DisplayDamage();
            isHit = true;
            controlScript = currentPlayerObject.GetComponent<PlayerControl>();
            controlScript.TakeHit();
        
            partsUIScript.TriggerLimbPartsDrop();
        }
    }

    public void TakeHardHit(){
        if(!isHit && !isDead){
            damageTaken = 6;
            DisplayDamage();
            isHit = true;
            controlScript = currentPlayerObject.GetComponent<PlayerControl>();
            controlScript.TakeHit();

            partsUIScript.TriggerAllPartsDrop();
        }
    }
    
    public void DisplayDamage(){
        if(GameController.current.damageNumOption){
            dmgNumPos = currentPlayerObject.transform.position;
            GameObject newDamageNum = Instantiate(damageNum, new Vector3(dmgNumPos.x, dmgNumPos.y, dmgNumPos.z), Quaternion.identity) as GameObject;
            GameObject canvasObject = GameObject.Find("WorldCanvas");
            newDamageNum.transform.SetParent(canvasObject.transform);
            DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
            damageNumScript.damageNum = damageTaken;
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

        GameController.current.HighlightPickups();
    }

    public void CheckAndRefreshUpgrades(){
        if(hasHead){
            if(headType == 0){
                if(head_progress1 == 0){
                    EnableUpgrade(0,0,1,0);
                }
                else if(head_progress1 == 1){
                    EnableUpgrade(0,0,1,1);
                }
                else if(head_progress1 == 2){
                    EnableUpgrade(0,0,1,2);
                }

                if(head_progress2 == 0){
                    EnableUpgrade(0,0,2,0);
                }
                else if(head_progress2 == 1){
                    EnableUpgrade(0,0,2,1);
                }
                else if(head_progress2 == 2){
                    EnableUpgrade(0,0,2,2);
                }
            }
        }

        if(hasBody){
            if(bodyType == 0){
                if(body_progress1 == 0){
                    EnableUpgrade(1,0,1,0);
                }
                else if(body_progress1 == 1){
                    EnableUpgrade(1,0,1,1);
                }
                else if(body_progress1 == 2){
                    EnableUpgrade(1,0,1,2);
                }

                if(body_progress2 == 0){
                    EnableUpgrade(1,0,2,0);
                }
                else if(body_progress2 == 1){
                    EnableUpgrade(1,0,2,1);
                }
                else if(body_progress2 == 2){
                    EnableUpgrade(1,0,2,2);
                }
            }
        }

        if(hasDrill){
            if(drillType == 0){
                if(drill_progress1 == 0){
                    EnableUpgrade(2,0,1,0);
                }
                else if(drill_progress1 == 1){
                    EnableUpgrade(2,0,1,1);
                }
                else if(drill_progress1 == 2){
                    EnableUpgrade(2,0,1,2);
                }

                if(drill_progress2 == 0){
                    EnableUpgrade(2,0,2,0);
                }
                else if(drill_progress2 == 1){
                    EnableUpgrade(2,0,2,1);
                }
                else if(drill_progress2 == 2){
                    EnableUpgrade(2,0,2,2);
                }
            }
        }

        if(hasGun){
            if(gunType == 0){//Blaster
                if(gun_progress1 == 0){
                    EnableUpgrade(3,0,1,0);
                }
                else if(gun_progress1 == 1){
                    EnableUpgrade(3,0,1,1);
                }
                else if(gun_progress1 == 2){
                    EnableUpgrade(3,0,1,2);
                }

                if(gun_progress2 == 0){
                    EnableUpgrade(3,0,2,0);
                }
                else if(gun_progress2 == 1){
                    EnableUpgrade(3,0,2,1);
                }
                else if(gun_progress2 == 2){
                    EnableUpgrade(3,0,2,2);
                }
            }
            if(gunType == 1){//Missile
                if(gun_progress1 == 0){
                    EnableUpgrade(3,1,1,0);
                }
                else if(gun_progress1 == 1){
                    EnableUpgrade(3,1,1,1);
                }
                else if(gun_progress1 == 2){
                    EnableUpgrade(3,1,1,2);
                }

                if(gun_progress2 == 0){
                    EnableUpgrade(3,1,2,0);
                }
                else if(gun_progress2 == 1){
                    EnableUpgrade(3,1,2,1);
                }
                else if(gun_progress2 == 2){
                    EnableUpgrade(3,1,2,2);
                }
            }
            if(gunType == 2){//Enerby Beam
                if(gun_progress1 == 0){
                    EnableUpgrade(3,2,1,0);
                }
                else if(gun_progress1 == 1){
                    EnableUpgrade(3,2,1,1);
                }
                else if(gun_progress1 == 2){
                    EnableUpgrade(3,2,1,2);
                }

                if(gun_progress2 == 0){
                    EnableUpgrade(3,2,2,0);
                }
                else if(gun_progress2 == 1){
                    EnableUpgrade(3,2,2,1);
                }
                else if(gun_progress2 == 2){
                    EnableUpgrade(3,2,2,2);
                }
            }
        }

        if(hasLegs){
            if(legType == 0){ //Worker Legs
               if(legs_progress1 == 0){
                    EnableUpgrade(4,0,1,0);
                }
                else if(legs_progress1 == 1){
                    EnableUpgrade(4,0,1,1);
                }
                else if(legs_progress1 == 2){
                    EnableUpgrade(4,0,1,2);
                }

                if(legs_progress2 == 0){
                    EnableUpgrade(4,0,2,0);
                }
                else if(legs_progress2 == 1){
                    EnableUpgrade(4,0,2,1);
                }
                else if(legs_progress2 == 2){
                    EnableUpgrade(4,0,2,2);
                }
            }
            if(legType == 1){ //Jump Legs
                if(legs_progress1 == 0){
                    EnableUpgrade(4,1,1,0);
                }
                else if(legs_progress1 == 1){
                    EnableUpgrade(4,1,1,1);
                }
                else if(legs_progress1 == 2){
                    EnableUpgrade(4,1,1,2);
                }

                if(legs_progress2 == 0){
                    EnableUpgrade(4,1,2,0);
                }
                else if(legs_progress2 == 1){
                    EnableUpgrade(4,1,2,1);
                }
                else if(legs_progress2 == 2){
                    EnableUpgrade(4,1,2,2);
                }
            }
        }
    }
    
    public void ResetUpgrade(int droppedPart){
        if(droppedPart == 0){
            head_progress1 = 0;
            head_progress2 = 0;
            temp_head_progress1 = 0;
            temp_head_progress2 = 0;
            temp_head_progressNum = 0;
        }
        else if(droppedPart == 1){
            body_progress1 = 0;
            body_progress2 = 0;
            temp_body_progress1 = 0;
            temp_body_progress2 = 0;
            temp_body_progressNum = 0;
        }
        else if(droppedPart == 2){
            drill_progress1 = 0;
            drill_progress2 = 0;
            temp_drill_progress1 = 0;
            temp_drill_progress2 = 0;
            temp_drill_progressNum = 0;
        }
        else if(droppedPart == 3){
            gun_progress1 = 0;
            gun_progress2 = 0;
            temp_gun_progress1 = 0;
            temp_gun_progress2 = 0;
            temp_gun_progressNum = 0;
        }
        else if(droppedPart == 4){
            legs_progress1 = 0;
            legs_progress2 = 0;
            temp_legs_progress1 = 0;
            temp_legs_progress2 = 0;
            temp_legs_progressNum = 0;
        }
        CheckAndRefreshUpgrades();
    }

    public void EnableUpgrade(int partType, int subType, int progressType, int upgradeNum){
        if(partType == 0){ //Head
            if(progressType == 1){
                if(upgradeNum == 0){
                    controlScript.lightUpgradeNum = 0;
                    controlScript.ChangeHeadVisibility();
                }
                else if(upgradeNum == 1){
                    controlScript.lightUpgradeNum = 2;
                    controlScript.ChangeHeadVisibility();
                }
                else if(upgradeNum == 2){
                    controlScript.lightUpgradeNum = 3;
                    controlScript.ChangeHeadVisibility();
                }
            }
            else if(progressType == 2){
                if(upgradeNum == 0){
                    //Head Durability 0
                }
                else if(upgradeNum == 1){
                    //Head Durability 1
                }
                else if(upgradeNum == 2){
                    //Head Durability 2
                }
            }
        }

        if(partType == 1){  //Body
            if(progressType == 1){
                if(upgradeNum == 0){
                    //Gun Slots 0
                }
                else if(upgradeNum == 1){
                    //Gun Slots 1
                }
                else if(upgradeNum == 2){
                    //Gun Slots 2
                }
            }
            else if(progressType == 2){
                if(upgradeNum == 0){
                    //Body Durability 0
                }
                else if(upgradeNum == 1){
                    //Body Durability 1
                }
                else if(upgradeNum == 2){
                    //Body Durability 2
                }
            }
        }

        if(partType == 2){  //Drill
            if(progressType == 1){
                if(upgradeNum == 0){
                    //Damage 1
                }
                else if(upgradeNum == 1){
                    //Damage 2
                }
                else if(upgradeNum == 2){
                    //Damage 3
                }
            }
            else if(progressType == 2){
                if(upgradeNum == 0){
                    //Drill Durability 0
                }
                else if(upgradeNum == 1){
                    //Drill Durability 1
                }
                else if(upgradeNum == 2){
                    //Drill Durability 2
                }
            }
        }

        if(partType == 3){  //Guns
            if(subType == 0){  //Blaster
                if(progressType == 1){
                    if(upgradeNum == 0){
                        //Damage 1
                    }
                    else if(upgradeNum == 1){
                        //Damage 2
                    }
                    else if(upgradeNum == 2){
                        //Damage 3
                    }
                }
                else if(progressType == 2){
                    if(upgradeNum == 0){
                        //Blaster FR 1
                    }
                    else if(upgradeNum == 1){
                        //Blaster FR 2
                    }
                    else if(upgradeNum == 2){
                        //Blaster FR 3
                    }
                }
            }
            if(subType == 1){  //Missile
                if(progressType == 1){
                    if(upgradeNum == 0){
                        //Blast Radius 1
                    }
                    else if(upgradeNum == 1){
                        //Blast Radius 2
                    }
                    else if(upgradeNum == 2){
                        //Blast Radius 3
                    }
                }
                else if(progressType == 2){
                    if(upgradeNum == 0){
                        //Missile FR 1
                    }
                    else if(upgradeNum == 1){
                        //Missile FR 2
                    }
                    else if(upgradeNum == 2){
                        //Missile FR 3
                    }
                }
            }
            if(subType == 2){  //Energy Beam
                if(progressType == 1){
                    if(upgradeNum == 0){
                        //Damage 1
                    }
                    else if(upgradeNum == 1){
                        //Damage 2
                    }
                    else if(upgradeNum == 2){
                        //Damage 3
                    }
                }
                else if(progressType == 2){
                    if(upgradeNum == 0){
                        //EB Width 1
                    }
                    else if(upgradeNum == 1){
                        //EB Width 2
                    }
                    else if(upgradeNum == 2){
                        //EB Width 3
                    }
                }
            }
        }

        if(partType == 4){  //Legs
            if(subType == 0){ //W. Legs
                if(progressType == 1){
                    if(upgradeNum == 0){
                        //Speed 1
                    }
                    else if(upgradeNum == 1){
                        //Speed 2
                    }
                    else if(upgradeNum == 2){
                        //Speed 3
                    }
                }
                else if(progressType == 2){
                    if(upgradeNum == 0){
                        //Legs Durability 0
                    }
                    else if(upgradeNum == 1){
                        //Legs Durability 1
                    }
                    else if(upgradeNum == 2){
                        //Legs Durability 2
                    }
                }
            }

            if(subType == 0){ //Jump Legs
                if(progressType == 1){
                    if(upgradeNum == 0){
                        //Jump 0
                    }
                    else if(upgradeNum == 1){
                        //Jump 1
                    }
                    else if(upgradeNum == 2){
                        //Jump 2
                    }
                }
                else if(progressType == 2){
                    if(upgradeNum == 0){
                        //Legs Durability 0
                    }
                    else if(upgradeNum == 1){
                        //Legs Durability 1
                    }
                    else if(upgradeNum == 2){
                        //Legs Durability 2
                    }
                }
            }
        }
    }

    public void DestroyOldSelf(){
        spawnPosition = currentPlayerObject.transform.position;
        Destroy(currentPlayerObject);
    }
}

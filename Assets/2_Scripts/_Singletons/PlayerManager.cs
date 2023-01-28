using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    public MouseCursor mouseCursorScript;

    public PlayerControl controlScript;
    public GroundedCharacterController groundedCharSript;
    public DoubleJumpModule doubleJumpModuleScipt;

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
    public int currentDurability = 1;
    public int maxDurability = 1;

    public int currentDefenseShield = 0;
    public int maxDefenseShield = 0;
    public bool shieldActive;

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
    public int backupBotID;
    public int lastBackupBotID;
    public string backupSceneName;

    public bool isHit = false;
    private float hitCooldown = 0.75f;

    public int numOfCorite;
    public int numOfNymrite;
    public int numOfVelrite;
    public int numOfZyrite;

    public float mLength;
    public float colliderHeight;

    [SerializeField] private GameObject damageNum;
    private DamageNum damageNumScript;
    private Vector3 dmgNumPos;
    private int damageTaken;

    public float currentDamage_blaster;
    public float currentDamage_missile;
    public float currentDamage_energyBeam;
    public float currentDamage_workerDrill;

    public float currentUpgrade_blaster = 0.0f;
    public float currentUpgrade_energyBeam = 0.0f;
    public float currentUpgrade_workerDrill = 0.0f;

    public int currentRange_workerDrill = 0;

    public int currentUpgrade_extraSlots = 0;
    public bool hasStorage1;
    public GameObject storageObject1;
    public PickUpScript storage1Script;

    public bool hasStorage2;
    public GameObject storageObject2;
    public PickUpScript storage2Script;

    public bool playerOnPickup = false;
    public bool playerHoldingPart = false;
    public int holdingPartNum;

    public int currentUpgrade_defenseShield = 0;

    public int currentSpeed_workerLegs = 0;
    public int currentJump_workerLegs = 0;

    public float currentSpeed_JumpLegs = 0;
    public int currentJump_JumpLegs = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        current = this;

        mouseCursorScript = GameObject.Find("GameController").GetComponent<MouseCursor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isHit)
        {
            if (hitCooldown > 0.0f)
            {
                hitCooldown -= Time.deltaTime;
            }
            else
            {
                RecoverFromHit();
                hitCooldown = 0.75f;
            }
        }
    }

    public void InitPlayer()
    {
        isDead = false;
        GameController.current.EnableUI();
        mainCamera = GameObject.Find("Camera");
        cameraScript = mainCamera.GetComponent<BasicCameraTracker>();
        playerHealthManager = healthManagerUI.GetComponent<UI_HealthManager>();
        partsUIScript = partsUI.GetComponent<UI_Parts>();

        Reboot();
    }

    public void GetColliderHeights()
    {
        capsuleCollider = currentPlayerObject.GetComponent<CapsuleCollider>();
        colliderHeight = capsuleCollider.height;

        playerCollider = currentPlayerObject.GetComponent<ControlledCapsuleCollider>();
        mLength = playerCollider.m_Length;
    }

    public void Reboot()
    {
        GameController.current.playerSpawned = true;
        currentPlayerObject = Instantiate(player0_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        TransferPlayerProperties();
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        playerHealthManager.GainHead();
        partsUIScript.GainWorkerHead(0);
        hasHead = true;

        GetColliderHeights();
        ChangeShieldProperties();

        GameController.current.ResetEnemyPlayerObjects();
        GameController.current.EnableUI();

        AudioManager.current.currentSFXTrack = 33;
        AudioManager.current.PlaySfx();
    }

    public void RebootBackup()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == backupSceneName)
        {
            RebootPlayer();
        }
        else
        {
            MenuManager.current.ChangeSceneAndReboot();
            GameController.current.playerRespawning = true;
        }
    }

    public void RebootPlayer()
    {
        if (backupObject != null)
        {
            Destroy(backupObject);
            backupBotID = 0;
            lastBackupBotID = 0;
            backedUp = false;
        }
        isDead = false;
        Destroy(currentPlayerObject);
        currentPlayerObject = Instantiate(player0_Prefab, backupSpawnPos, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        cameraScript.m_Target = currentPlayerObject;
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        playerHealthManager.GainHead();
        partsUIScript.GainWorkerHead(0);
        hasHead = true;
        controlScript.ChangeHeadDurability(0);

        GameController.current.RebootFromCheckpoint();
        playerHealthManager.NotBackedUp();
        AudioManager.current.currentSFXTrack = 33;
        AudioManager.current.PlaySfx();
    }

    public void RebootNewMap()
    {
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
    }

    public void AttatchMain(string partToAttatch)
    {
        if (partToAttatch == "WorkerHead")
        {

        }
        if (partToAttatch == "WorkerBody")
        {
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
            partsUIScript.GainWorkerBody(0);

            CheckAndRefreshUpgrades("Body");

            currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
            TransferPlayerProperties();
            currentPlayerObject.transform.parent = transform;
            currentPlayerObject.transform.position = spawnPosition;
            cameraScript.m_Target = currentPlayerObject;

            if (hasDrill)
            {
                controlScript.EnableDrillArm();
            }
            if (hasGun)
            {
                controlScript.EnableGunArm();
            }

            GameController.current.ResetEnemyPlayerObjects();
        }
        if (partToAttatch == "WorkerDrill")
        {
            hasDrill = true;
            TransferPlayerProperties();

            controlScript.EnableDrillArm();
            cameraScript.m_Target = currentPlayerObject;
            controlScript.Invoke("CheckMaterials", 0.1f);

            playerHealthManager.GainDrillArm();
            //Transfer upgrade nums from pickup script
            drill_progress1 = currentPickup_progress1;
            drill_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainWorkerDrill(0);
            CheckAndRefreshUpgrades("RightArm");
        }

        if (partToAttatch == "BlasterGun")
        {
            hasGun = true;
            gunType = 0;
            TransferPlayerProperties();

            controlScript.gunType = 0;
            controlScript.EnableGunArm();
            controlScript.Invoke("CheckMaterials", 0.1f);

            cameraScript.m_Target = currentPlayerObject;
            playerHealthManager.GainBlasterGun();
            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainBlaster(0);
            CheckAndRefreshUpgrades("LeftArm");
        }
        if (partToAttatch == "MissileLauncher")
        {
            hasGun = true;
            gunType = 1;
            TransferPlayerProperties();

            controlScript.gunType = 1;
            controlScript.EnableGunArm();
            controlScript.Invoke("CheckMaterials", 0.1f);

            cameraScript.m_Target = currentPlayerObject;
            playerHealthManager.GainMissileLauncher();
            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainMissileLauncher(0);
            CheckAndRefreshUpgrades("LeftArm");

            GameController.current.CheckCursor();
        }
        if (partToAttatch == "EnergyBeam")
        {
            hasGun = true;
            gunType = 2;
            TransferPlayerProperties();
            controlScript.gunType = 2;
            controlScript.EnableGunArm();
            controlScript.Invoke("CheckMaterials", 0.1f);
            cameraScript.m_Target = currentPlayerObject;
            playerHealthManager.GainLaserBeam();
            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainLaserBeam(0);
            CheckAndRefreshUpgrades("LeftArm");

            GameController.current.CheckCursor();
        }

        if (partToAttatch == "WorkerLegs")
        {
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
            partsUIScript.GainWorkerBoots(0);
            CheckAndRefreshUpgrades("Legs");

            GetColliderHeights();

            GameController.current.ResetEnemyPlayerObjects();

            if (hasDrill)
            {
                controlScript.EnableDrillArm();
            }
            if (hasGun)
            {
                controlScript.EnableGunArm();
            }
        }
        if (partToAttatch == "JumpLegs")
        {
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
            partsUIScript.GainJumpBoots(0);
            CheckAndRefreshUpgrades("Legs");

            GetColliderHeights();

            GameController.current.ResetEnemyPlayerObjects();

            if (hasDrill)
            {
                controlScript.EnableDrillArm();
            }
            if (hasGun)
            {
                controlScript.EnableGunArm();
            }
        }
    }

    public void AttatchStorage1(string partToAttatch)
    {
        if (partToAttatch == "WorkerHead")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            head_progress1 = currentPickup_progress1;
            head_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }
        if (partToAttatch == "WorkerBody")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            body_progress1 = currentPickup_progress1;
            body_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }
        if (partToAttatch == "WorkerDrill")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            drill_progress1 = currentPickup_progress1;
            drill_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }

        if (partToAttatch == "BlasterGun")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }
        if (partToAttatch == "MissileLauncher")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }
        if (partToAttatch == "EnergyBeam")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }

        if (partToAttatch == "WorkerLegs")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            legs_progress1 = currentPickup_progress1;
            legs_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }
        if (partToAttatch == "JumpLegs")
        {
            hasStorage1 = true;

            //Transfer upgrade nums from pickup script
            legs_progress1 = currentPickup_progress1;
            legs_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }
    }
    public void AttatchStorage2(string partToAttatch)
    {
        if (partToAttatch == "WorkerHead")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            head_progress1 = currentPickup_progress1;
            head_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(1);
        }
        if (partToAttatch == "WorkerBody")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            body_progress1 = currentPickup_progress1;
            body_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(2);
        }
        if (partToAttatch == "WorkerDrill")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            drill_progress1 = currentPickup_progress1;
            drill_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(2);
        }

        if (partToAttatch == "BlasterGun")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(2);
        }
        if (partToAttatch == "MissileLauncher")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(2);
        }
        if (partToAttatch == "EnergyBeam")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            gun_progress1 = currentPickup_progress1;
            gun_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(2);
        }

        if (partToAttatch == "WorkerLegs")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            legs_progress1 = currentPickup_progress1;
            legs_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(2);
        }
        if (partToAttatch == "JumpLegs")
        {
            hasStorage2 = true;

            //Transfer upgrade nums from pickup script
            legs_progress1 = currentPickup_progress1;
            legs_progress2 = currentPickup_progress2;
            partsUIScript.temp_progress1 = currentPickup_progress1;
            partsUIScript.temp_progress2 = currentPickup_progress2;
            partsUIScript.temp_progressNum = currentPickup_progress1 + currentPickup_progress2;

            partsUIScript.GainJumpBoots(2);
        }
    }

    //Pickup Bodies
    public void PickupBody(int storageSlot)
    {
        if (!playerHoldingPart)
        {
            playerHoldingPart = true;
            holdingPartNum = 1;
            GameController.current.ChangeMouseCursorSelectedPart(1);
            partsUIScript.EnablePartsUI();
            partsUIScript.CheckSelected();

            AudioManager.current.currentSFXTrack = 19;
            AudioManager.current.PlaySfx();
        }
        else
        {
            AudioManager.current.currentSFXTrack = 1;
            AudioManager.current.PlaySfx();
        }
    }

    public void AttatchBody(int storageSlot)
    {
        if (storageSlot == 0)
        {
            AttatchMain("WorkerBody");
        }
        else if (storageSlot == 1)
        {
            AttatchStorage1("WorkerBody");
        }
        else if (storageSlot == 2)
        {
            AttatchStorage2("WorkerBody");
        }
    }



    //Pickup Drills
    public void PickupDrill(int storageSlot)
    {
        if (storageSlot == 0)
        {
            AttatchMain("WorkerDrill");
        }
        else if (storageSlot == 1)
        {
            AttatchStorage1("WorkerDrill");
        }
        else if (storageSlot == 2)
        {
            AttatchStorage2("WorkerDrill");
        }

        AudioManager.current.currentSFXTrack = 19;
        AudioManager.current.PlaySfx();
    }

    //Pickup Guns
    public void PickupBlaster(int storageSlot)
    {
        if (storageSlot == 0)
        {
            AttatchMain("BlasterGun");
        }
        else if (storageSlot == 1)
        {
            AttatchStorage1("BlasterGun");
        }
        else if (storageSlot == 2)
        {
            AttatchStorage2("BlasterGun");
        }
        AudioManager.current.currentSFXTrack = 19;
        AudioManager.current.PlaySfx();
    }
    public void PickupMissile(int storageSlot)
    {
        if (storageSlot == 0)
        {
            AttatchMain("MissileLauncher");
        }
        else if (storageSlot == 1)
        {
            AttatchStorage1("MissileLauncher");
        }
        else if (storageSlot == 2)
        {
            AttatchStorage2("MissileLauncher");
        }
        AudioManager.current.currentSFXTrack = 19;
        AudioManager.current.PlaySfx();
    }
    public void PickupLaser(int storageSlot)
    {
        if (storageSlot == 0)
        {
            AttatchMain("EnergyBeam");
        }
        else if (storageSlot == 1)
        {
            AttatchStorage1("EnergyBeam");
        }
        else if (storageSlot == 2)
        {
            AttatchStorage2("EnergyBeam");
        }

        AudioManager.current.currentSFXTrack = 19;
        AudioManager.current.PlaySfx();
    }

    //Pickup Legs
    public void PickupWorkerBoots(int storageSlot)
    {
        if (storageSlot == 0)
        {
            AttatchMain("WorkerLegs");
        }
        else if (storageSlot == 1)
        {
            AttatchStorage1("WorkerLegs");
        }
        else if (storageSlot == 2)
        {
            AttatchStorage2("WorkerLegs");
        }

        AudioManager.current.currentSFXTrack = 19;
        AudioManager.current.PlaySfx();
    }

    public void PickupJumpBoots(int storageSlot)
    {
        if (storageSlot == 0)
        {
            AttatchMain("JumpLegs");
        }
        else if (storageSlot == 1)
        {
            AttatchStorage1("JumpLegs");
        }
        else if (storageSlot == 2)
        {
            AttatchStorage2("JumpLegs");
        }

        AudioManager.current.currentSFXTrack = 19;
        AudioManager.current.PlaySfx();
    }

    //Switching Body
    public void SwitchToWorkerBody()
    {
        //Spawn Body Pickup
        GameObject pickup_WorkerBody = Instantiate(pickup_BodyObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_WorkerBody.GetComponent<PickUpScript>();
        pickUpScript.progress1 = temp_body_progress1;
        pickUpScript.progress2 = temp_body_progress2;
        pickUpScript.progressNum = temp_body_progress1 + temp_body_progress2;
        pickUpScript.pickupType = 1;
        pickUpScript.DropNewPickup();
    }

    //Switching Drills
    public void SwitchToWorkerDrill()
    {
        //Spawn Drill Pickup
        GameObject pickup_WorkerDrill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_WorkerDrill.GetComponent<PickUpScript>();
        pickUpScript.progress1 = temp_drill_progress1;
        pickUpScript.progress2 = temp_drill_progress2;
        pickUpScript.progressNum = temp_drill_progress1 + temp_drill_progress2;
        pickUpScript.pickupType = 2;
        pickUpScript.DropNewPickup();
    }

    //Switching Guns
    public void SwitchToBlaster()
    {
        //Spawn Gun Pickup
        if (gunType == 0)
        {
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0;
            pickUpScript.DropNewPickup();
        }
        else if (gunType == 1)
        {
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else if (gunType == 2)
        {
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        //Switch to Blaster
        PickupBlaster(0);
    }
    public void SwitchToMissile()
    {
        //Spawn Gun Pickup
        if (gunType == 0)
        {
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0;
            pickUpScript.DropNewPickup();
        }
        else if (gunType == 1)
        {
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else if (gunType == 2)
        {
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        //Switch to Missile
        PickupMissile(0);
    }
    public void SwitchToLaser()
    {
        //Spawn Gun Pickup
        if (gunType == 0)
        {
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0;
            pickUpScript.DropNewPickup();
        }
        else if (gunType == 1)
        {
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else if (gunType == 2)
        {
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        //Switch to Laser
        PickupLaser(0);
    }

    public void SwitchToWorkerBoots()
    {
        //Spawn Worker Boots Pickup
        if (legType == 0)
        {
            GameObject pickup_Legs = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Legs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 0;
            pickUpScript.DropNewPickup();
        }
        else if (legType == 1)
        {
            GameObject pickup_JumpLegs = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_JumpLegs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 1;
            pickUpScript.DropNewPickup();
        }
        //Switch to Worker Boots
        PickupWorkerBoots(0);
    }
    public void SwitchToJumpBoots()
    {
        if (legType == 0)
        {
            GameObject pickup_Legs = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Legs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 0;
            pickUpScript.DropNewPickup();

        }
        else if (legType == 1)
        {
            GameObject pickup_JumpLegs = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_JumpLegs.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_legs_progress1;
            pickUpScript.progress2 = temp_legs_progress2;
            pickUpScript.progressNum = temp_legs_progress1 + temp_legs_progress2;
            pickUpScript.pickupType = 4;
            pickUpScript.legType = 1;
            pickUpScript.DropNewPickup();
        }
        //Switch to Jump Boots
        PickupJumpBoots(0);
    }

    //Losing Parts
    public void DropAllParts()
    {
        playerPartDropped = true;
        if (hasLegs)
        {
            DropLegs();
        }
        if (hasDrill)
        {
            Invoke("DropDrill", 0.01f);
        }
        if (hasGun)
        {
            Invoke("DropGun", 0.01f);
        }
        if (hasBody)
        {
            Invoke("DropBody", 0.1f);
        }
        AudioManager.current.currentSFXTrack = 18;
        AudioManager.current.PlaySfx();
    }

    public void DropLimbs()
    {
        playerPartDropped = true;
        if (hasLegs)
        {
            DropLegs();
        }
        if (hasDrill)
        {
            DropDrill();
        }
        if (hasGun)
        {
            DropGun();
        }
        AudioManager.current.currentSFXTrack = 18;
        AudioManager.current.PlaySfx();
    }

    //Drop Guns
    public void DropGun()
    {
        if (gunType == 0)
        {
            DropBlaster();
        }
        if (gunType == 1)
        {
            DropMissile();
        }
        if (gunType == 2)
        {
            DropLaser();
        }

        GameController.current.CheckCursor();
        AudioManager.current.currentSFXTrack = 32;
        AudioManager.current.PlaySfx();
    }

    public void DropBlaster()
    {
        hasGun = false;
        controlScript.DisableGunArm();
        playerHealthManager.LoseBlasterGun();

        if (playerPartDropped)
        {
            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Blaster.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 0;
            pickUpScript.DropNewPickup();
        }
        else
        {
            cameraScript.m_Target = currentPlayerObject;

            GameObject pickup_Blaster = Instantiate(pickup_BlasterObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

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
    public void DropMissile()
    {
        hasGun = false;
        controlScript.DisableGunArm();
        playerHealthManager.LoseMissileLauncher();

        if (playerPartDropped)
        {
            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Missile.GetComponent<PickUpScript>();
            pickUpScript.progress1 = temp_gun_progress1;
            pickUpScript.progress2 = temp_gun_progress2;
            pickUpScript.progressNum = temp_gun_progress1 + temp_gun_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 1;
            pickUpScript.DropNewPickup();
        }
        else
        {
            cameraScript.m_Target = currentPlayerObject;

            GameObject pickup_Missile = Instantiate(pickup_MissileObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

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
    public void DropLaser()
    {
        hasGun = false;
        controlScript.DisableGunArm();
        playerHealthManager.LoseLaserBeam();

        if (playerPartDropped)
        {
            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Laser.GetComponent<PickUpScript>();
            pickUpScript.progress1 = currentPickup_progress1;
            pickUpScript.progress2 = currentPickup_progress2;
            pickUpScript.progressNum = currentPickup_progress1 + currentPickup_progress2;
            pickUpScript.pickupType = 3;
            pickUpScript.gunType = 2;
            pickUpScript.DropNewPickup();
        }
        else
        {
            cameraScript.m_Target = currentPlayerObject;

            GameObject pickup_Laser = Instantiate(pickup_LaserObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

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

    public void DropBody()
    {
        hasBody = false;
        playerHealthManager.LoseBody();

        DestroyOldSelf();
        currentPlayerObject = Instantiate(player0_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        GetColliderHeights();
        GameController.current.ResetEnemyPlayerObjects();

        //Spawn Body Pickup
        GameObject pickup_Body = Instantiate(pickup_BodyObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_Body.GetComponent<PickUpScript>();
        pickUpScript.progress1 = temp_body_progress1;
        pickUpScript.progress2 = temp_body_progress2;
        pickUpScript.progressNum = temp_body_progressNum;
        pickUpScript.pickupType = 1;
        pickUpScript.DropNewPickup();

        playerPartDropped = false;

        ResetUpgrade(1);
        TransferPlayerProperties();

        AudioManager.current.currentSFXTrack = 32;
        AudioManager.current.PlaySfx();
    }

    public void DropDrill()
    {
        hasDrill = false;
        controlScript.DisableDrillArm();
        playerHealthManager.LoseDrillArm();

        if (playerPartDropped)
        {
            //Spawn Drill Pickup
            GameObject pickup_Drill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Drill.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.progress1 = temp_drill_progress1;
            pickUpScript.progress2 = temp_drill_progress2;
            pickUpScript.progressNum = temp_drill_progressNum;
            pickUpScript.DropNewPickup();
        }
        else
        {
            cameraScript.m_Target = currentPlayerObject;

            //Spawn Drill Pickup
            GameObject pickup_Drill = Instantiate(pickup_DrillObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

            PickUpScript pickUpScript = pickup_Drill.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.progress1 = temp_drill_progress1;
            pickUpScript.progress2 = temp_drill_progress2;
            pickUpScript.progressNum = temp_drill_progressNum;
            pickUpScript.DropNewPickup();
        }

        ResetUpgrade(2);
        TransferPlayerProperties();

        AudioManager.current.currentSFXTrack = 32;
        AudioManager.current.PlaySfx();
    }


    //Drop Legs
    public void DropLegs()
    {
        Debug.Log("DropLegs");
        if (legType == 0)
        {
            DropWorkerBoots();
        }
        if (legType == 1)
        {
            DropJumpBoots();
        }
        AudioManager.current.currentSFXTrack = 32;
        AudioManager.current.PlaySfx();
    }

    public void DropWorkerBoots()
    {
        Debug.Log("DropWorkerBoots");
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
        GameObject pickup_WorkerBoots = Instantiate(pickup_LegsObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

        PickUpScript pickUpScript = pickup_WorkerBoots.GetComponent<PickUpScript>();
        pickUpScript.pickupType = 4;
        pickUpScript.legType = 0;
        pickUpScript.progress1 = temp_legs_progress1;
        pickUpScript.progress2 = temp_legs_progress2;
        pickUpScript.progressNum = temp_legs_progressNum;
        pickUpScript.DropNewPickup();

        if (hasDrill)
        {
            controlScript.EnableDrillArm();
        }
        if (hasGun)
        {
            controlScript.EnableGunArm();
        }
        ResetUpgrade(4);
        TransferPlayerProperties();
    }

    public void DropJumpBoots()
    {
        Debug.Log("DropJumpBoots");
        hasLegs = false;
        playerHealthManager.LoseJumpBoots();

        DestroyOldSelf();
        currentPlayerObject = Instantiate(player1_Prefab, spawnPosition, Quaternion.identity) as GameObject;
        currentPlayerObject.transform.parent = transform;
        currentPlayerObject.transform.position = spawnPosition;
        cameraScript.m_Target = currentPlayerObject;
        GetColliderHeights();
        GameController.current.ResetEnemyPlayerObjects();

        if (hasDrill)
        {
            controlScript.EnableDrillArm();
        }
        if (hasGun)
        {
            controlScript.EnableGunArm();
        }

        //Spawn Jump Boots Pickup
        GameObject pickup_JumpLegs = Instantiate(pickup_LegsJumpObject, new Vector3(currentPlayerObject.transform.position.x, currentPlayerObject.transform.position.y, currentPlayerObject.transform.position.z), Quaternion.identity) as GameObject;

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

    public void Death()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Abyss_Boss")
        {
            CyberMantisScript cyberMantisScript = GameObject.Find("CyberMantis").GetComponent<CyberMantisScript>();
            cyberMantisScript.SetLastPos();
        }

        ResetUpgrade(0);
        isDead = true;
        GameController.current.InactivateEnemies();
        if (backedUp)
        {
            RebootBackup();

        }
        else
        {
            GameOver();
            maxDurability = 1;
            currentDurability = 1;
        }
        AudioManager.current.currentSFXTrack = 30;
        AudioManager.current.PlaySfx();
    }
    public void GameOver()
    {
        Destroy(currentPlayerObject);
        GameController.current.GameOver();
    }

    public void TakeHit()
    {
        if (!isHit && !isDead)
        {
            if (currentDefenseShield > 0)
            {
                isHit = true;
                damageTaken = 0;
                DisplayDamage();

                controlScript = currentPlayerObject.GetComponent<PlayerControl>();
                controlScript.TakeShieldHit();

                AudioManager.current.currentSFXTrack = 09;
                AudioManager.current.PlaySfx();
                partsUIScript.ShieldTakeHit();
            }
            else
            {
                isHit = true;
                damageTaken = 1;
                DisplayDamage();

                controlScript = currentPlayerObject.GetComponent<PlayerControl>();
                controlScript.TakeHit(damageTaken);

                if (hasLegs)
                {
                    DropLegs();
                }
                else if (hasGun)
                {
                    DropGun();
                }
                else if (hasDrill)
                {
                    DropDrill();
                }
                else if (hasBody)
                {
                    DropBody();
                }
                else if (hasHead)
                {
                    if (currentDurability <= 0)
                    {
                        Death();
                    }
                }

                if (head_progress2 > 0)
                {
                    if (maxDurability > currentDurability)
                    {
                        partsUIScript.ShowRepairButton();
                    }
                    else
                    {
                        partsUIScript.HideRepairButton();
                    }
                }

                AudioManager.current.currentSFXTrack = 31;
                AudioManager.current.PlaySfx();
            }
        }
    }


    public void RecoverFromHit()
    {
        isHit = false;
    }

    public void TakeHitLimbs()
    {
        if (!isHit && !isDead)
        {
            damageTaken = 2;
            DisplayDamage();
            isHit = true;
            controlScript = currentPlayerObject.GetComponent<PlayerControl>();
            controlScript.TakeHit(damageTaken);

            partsUIScript.TriggerLimbPartsDrop();

            AudioManager.current.currentSFXTrack = 18;
            AudioManager.current.PlaySfx();
        }
    }

    public void TakeHardHit()
    {
        if (!isHit && !isDead)
        {
            damageTaken = 3;
            DisplayDamage();
            isHit = true;
            controlScript = currentPlayerObject.GetComponent<PlayerControl>();
            controlScript.TakeHit(damageTaken);

            partsUIScript.TriggerAllPartsDrop();

            AudioManager.current.currentSFXTrack = 18;
            AudioManager.current.PlaySfx();
        }
    }

    public void DisplayDamage()
    {
        if (GameController.current.damageNumOption)
        {
            dmgNumPos = currentPlayerObject.transform.position;
            GameObject newDamageNum = Instantiate(damageNum, new Vector3(dmgNumPos.x, dmgNumPos.y, dmgNumPos.z), Quaternion.identity) as GameObject;
            GameObject canvasObject = GameObject.Find("WorldCanvas");
            newDamageNum.transform.SetParent(canvasObject.transform);
            DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
            damageNumScript.damageNum = damageTaken;
        }
    }

    public void SetBackupPoint(GameObject backupObj)
    {
        //Check if player already has a backup
        if (backupObject != null)
        {
            //Reset it so it can be used again
            PickUpScript p1Script = backupObject.GetComponent<PickUpScript>();
            p1Script.ResetPickup();
            backupObject = null;
        }

        //And set backupObject to the new one
        backupObject = backupObj;

        backedUp = true;
        playerHealthManager.BackedUp();

        Scene scene = SceneManager.GetActiveScene();
        backupSceneName = scene.name;

        GameObject sceneInit = GameObject.Find("SceneInit");
        SceneInit sceneInitScript = sceneInit.GetComponent<SceneInit>();

        AudioManager.current.currentSFXTrack = 33;
        AudioManager.current.PlaySfx();
    }

    public void RepairDurability(int repairModifier)
    {
        currentDurability += repairModifier;
    }
    public void RepairSelf()
    {
        RepairDurability(1);
    }

    public void TransferPlayerProperties()
    {
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();
        controlScript.gunType = gunType;
        controlScript.legType = legType;

        GameController.current.HighlightPickups();
        partsUIScript.CheckParts();
    }

    public void ChangeShieldProperties()
    {
        if (currentUpgrade_defenseShield == 0)
        {
            currentDefenseShield = 0;
            maxDefenseShield = 0;
            partsUIScript.HideShieldDefense();
            playerHealthManager.ShieldInactive();
            shieldActive = false;
        }
        if (currentUpgrade_defenseShield == 1)
        {
            currentDefenseShield = 1;
            maxDefenseShield = 1;
            partsUIScript.ShowShieldDefense();
            playerHealthManager.ShieldActive();
            shieldActive = true;
        }
        if (currentUpgrade_defenseShield == 2)
        {
            currentDefenseShield = 2;
            maxDefenseShield = 2;
            partsUIScript.ShowShieldDefense();
            playerHealthManager.ShieldActive(); shieldActive = true;
        }
    }
    public void setShieldActiveUI()
    {
        playerHealthManager.ShieldActive();
    }
    public GameObject FindChildWithDoubleJumpModule()
    {
        // Get all child game objects of the parent game object
        var children = GetComponentsInChildren<Transform>();

        // Iterate through each child game object
        foreach (Transform child in children)
        {
            // Check if the child has a DoubleJumpModule script attached to it
            if (child.GetComponent<DoubleJumpModule>() != null)
            {
                // Return the child game object if it has the script
                doubleJumpModuleScipt = child.GetComponent<DoubleJumpModule>();
                return child.gameObject;
            }
        }
        // Return null if no child game object with the script is found
        return null;
    }

    public void ChangeLegMovement()
    {
        controlScript = currentPlayerObject.GetComponent<PlayerControl>();

        groundedCharSript = controlScript.characterControllerScript;

        if (legType == 0) //Worker Legs
        {
            //Speed
            if (currentSpeed_workerLegs == 0)
            {
                groundedCharSript.m_WalkForce = 12.0f;
            }
            if (currentSpeed_workerLegs == 1)
            {
                groundedCharSript.m_WalkForce = 14.0f;
            }
            if (currentSpeed_workerLegs == 2)
            {
                groundedCharSript.m_WalkForce = 17.0f;
            }

            //Jump Height
            if (currentJump_workerLegs == 0)
            {
                groundedCharSript.m_JumpVelocity = 14.0f;
            }
            if (currentJump_workerLegs == 1)
            {
                groundedCharSript.m_JumpVelocity = 16.0f;
            }
            if (currentJump_workerLegs == 2)
            {
                groundedCharSript.m_JumpVelocity = 18.0f;
            }
        }
        else if (legType == 1) //Jump Legs
        {
            FindChildWithDoubleJumpModule();

            //Speed
            if (currentSpeed_JumpLegs == 0)
            {
                groundedCharSript.m_WalkForce = 10.0f;
            }
            if (currentSpeed_JumpLegs == 1)
            {
                groundedCharSript.m_WalkForce = 12.0f;
            }
            if (currentSpeed_JumpLegs == 2)
            {
                groundedCharSript.m_WalkForce = 15.0f;
            }

            //Jumps
            if (currentJump_JumpLegs == 0)
            {
                doubleJumpModuleScipt.m_AmountOfDoubleJumpsAllowed = 1;
            }
            if (currentJump_JumpLegs == 1)
            {
                doubleJumpModuleScipt.m_AmountOfDoubleJumpsAllowed = 2;
            }
            if (currentJump_JumpLegs == 2)
            {
                doubleJumpModuleScipt.m_AmountOfDoubleJumpsAllowed = 3;
            }
        }

    }
    public void TransferDamageProperties()
    {
        if (hasGun)
        {
            if (gunType == 0)
            {
                currentDamage_blaster = controlScript.weaponValues_blaster.damageAmount + currentUpgrade_blaster;
            }
            if (gunType == 1)
            {
                currentDamage_missile = controlScript.weaponValues_missile.damageAmount;
            }
            if (gunType == 2)
            {
                currentDamage_energyBeam = controlScript.weaponValues_energy.damageAmount + currentUpgrade_energyBeam;
            }
        }
        if (hasDrill)
        {
            currentDamage_workerDrill = controlScript.weaponValues_workerDrill.damageAmount + currentUpgrade_workerDrill;
        }
    }

    public void CheckAndRefreshUpgrades(string partToCheck)
    {
        if (partToCheck == "Head")
        {
            if (hasHead)
            {
                if (headType == 0)
                {
                    if (head_progress1 == 0)
                    {
                        EnableUpgrade(0, 0, 1, 0);
                    }
                    else if (head_progress1 == 1)
                    {
                        EnableUpgrade(0, 0, 1, 1);
                    }
                    else if (head_progress1 == 2)
                    {
                        EnableUpgrade(0, 0, 1, 2);
                    }

                    if (head_progress2 == 0)
                    {
                        EnableUpgrade(0, 0, 2, 0);
                    }
                    else if (head_progress2 == 1)
                    {
                        EnableUpgrade(0, 0, 2, 1);
                    }
                    else if (head_progress2 == 2)
                    {
                        EnableUpgrade(0, 0, 2, 2);
                    }
                }
            }
        }

        if (partToCheck == "Body")
        {
            if (hasBody)
            {
                if (bodyType == 0)
                {
                    if (body_progress1 == 0)
                    {
                        EnableUpgrade(1, 0, 1, 0);
                    }
                    else if (body_progress1 == 1)
                    {
                        EnableUpgrade(1, 0, 1, 1);
                    }
                    else if (body_progress1 == 2)
                    {
                        EnableUpgrade(1, 0, 1, 2);
                    }

                    if (body_progress2 == 0)
                    {
                        EnableUpgrade(1, 0, 2, 0);
                    }
                    else if (body_progress2 == 1)
                    {
                        EnableUpgrade(1, 0, 2, 1);
                    }
                    else if (body_progress2 == 2)
                    {
                        EnableUpgrade(1, 0, 2, 2);
                    }
                }
            }
        }

        if (partToCheck == "RightArm")
        {
            if (hasDrill)
            {
                if (drillType == 0)
                {
                    if (drill_progress1 == 0)
                    {
                        EnableUpgrade(2, 0, 1, 0);
                    }
                    else if (drill_progress1 == 1)
                    {
                        EnableUpgrade(2, 0, 1, 1);
                    }
                    else if (drill_progress1 == 2)
                    {
                        EnableUpgrade(2, 0, 1, 2);
                    }

                    if (drill_progress2 == 0)
                    {
                        EnableUpgrade(2, 0, 2, 0);
                    }
                    else if (drill_progress2 == 1)
                    {
                        EnableUpgrade(2, 0, 2, 1);
                    }
                    else if (drill_progress2 == 2)
                    {
                        EnableUpgrade(2, 0, 2, 2);
                    }
                }
            }
        }

        if (partToCheck == "LeftArm")
        {
            if (hasGun)
            {
                if (gunType == 0)
                {//Blaster
                    if (gun_progress1 == 0)
                    {
                        EnableUpgrade(3, 0, 1, 0);
                    }
                    else if (gun_progress1 == 1)
                    {
                        EnableUpgrade(3, 0, 1, 1);
                    }
                    else if (gun_progress1 == 2)
                    {
                        EnableUpgrade(3, 0, 1, 2);
                    }

                    if (gun_progress2 == 0)
                    {
                        EnableUpgrade(3, 0, 2, 0);
                    }
                    else if (gun_progress2 == 1)
                    {
                        EnableUpgrade(3, 0, 2, 1);
                    }
                    else if (gun_progress2 == 2)
                    {
                        EnableUpgrade(3, 0, 2, 2);
                    }
                }
                if (gunType == 1)
                {//Missile
                    if (gun_progress1 == 0)
                    {
                        EnableUpgrade(3, 1, 1, 0);
                    }
                    else if (gun_progress1 == 1)
                    {
                        EnableUpgrade(3, 1, 1, 1);
                    }
                    else if (gun_progress1 == 2)
                    {
                        EnableUpgrade(3, 1, 1, 2);
                    }

                    if (gun_progress2 == 0)
                    {
                        EnableUpgrade(3, 1, 2, 0);
                    }
                    else if (gun_progress2 == 1)
                    {
                        EnableUpgrade(3, 1, 2, 1);
                    }
                    else if (gun_progress2 == 2)
                    {
                        EnableUpgrade(3, 1, 2, 2);
                    }
                }
                if (gunType == 2)
                {//Enerby Beam
                    if (gun_progress1 == 0)
                    {
                        EnableUpgrade(3, 2, 1, 0);
                    }
                    else if (gun_progress1 == 1)
                    {
                        EnableUpgrade(3, 2, 1, 1);
                    }
                    else if (gun_progress1 == 2)
                    {
                        EnableUpgrade(3, 2, 1, 2);
                    }

                    if (gun_progress2 == 0)
                    {
                        EnableUpgrade(3, 2, 2, 0);
                    }
                    else if (gun_progress2 == 1)
                    {
                        EnableUpgrade(3, 2, 2, 1);
                    }
                    else if (gun_progress2 == 2)
                    {
                        EnableUpgrade(3, 2, 2, 2);
                    }
                }
            }
        }
        if (partToCheck == "Legs")
        {
            if (hasLegs)
            {
                if (legType == 0)
                { //Worker Legs
                    if (legs_progress1 == 0)
                    {
                        EnableUpgrade(4, 0, 1, 0);
                    }
                    else if (legs_progress1 == 1)
                    {
                        EnableUpgrade(4, 0, 1, 1);
                    }
                    else if (legs_progress1 == 2)
                    {
                        EnableUpgrade(4, 0, 1, 2);
                    }

                    if (legs_progress2 == 0)
                    {
                        EnableUpgrade(4, 0, 2, 0);
                    }
                    else if (legs_progress2 == 1)
                    {
                        EnableUpgrade(4, 0, 2, 1);
                    }
                    else if (legs_progress2 == 2)
                    {
                        EnableUpgrade(4, 0, 2, 2);
                    }
                }
                if (legType == 1)
                { //Jump Legs
                    if (legs_progress1 == 0)
                    {
                        EnableUpgrade(4, 1, 1, 0);
                    }
                    else if (legs_progress1 == 1)
                    {
                        EnableUpgrade(4, 1, 1, 1);
                    }
                    else if (legs_progress1 == 2)
                    {
                        EnableUpgrade(4, 1, 1, 2);
                    }

                    if (legs_progress2 == 0)
                    {
                        EnableUpgrade(4, 1, 2, 0);
                    }
                    else if (legs_progress2 == 1)
                    {
                        EnableUpgrade(4, 1, 2, 1);
                    }
                    else if (legs_progress2 == 2)
                    {
                        EnableUpgrade(4, 1, 2, 2);
                    }
                }
            }
        }
    }

    public void ResetUpgrade(int droppedPart)
    {
        if (droppedPart == 0)
        {
            head_progress1 = 0;
            head_progress2 = 0;
            temp_head_progress1 = 0;
            temp_head_progress2 = 0;
            temp_head_progressNum = 0;

            CheckAndRefreshUpgrades("Head");
        }
        else if (droppedPart == 1)
        {
            body_progress1 = 0;
            body_progress2 = 0;
            temp_body_progress1 = 0;
            temp_body_progress2 = 0;
            temp_body_progressNum = 0;
            currentDefenseShield = 0;
            maxDefenseShield = 0;
            currentUpgrade_defenseShield = 0;

            CheckAndRefreshUpgrades("Body");
        }
        else if (droppedPart == 2)
        {
            drill_progress1 = 0;
            drill_progress2 = 0;
            temp_drill_progress1 = 0;
            temp_drill_progress2 = 0;
            temp_drill_progressNum = 0;
            currentDamage_workerDrill = controlScript.weaponValues_workerDrill.damageAmount;

            currentUpgrade_workerDrill = 0;

            TransferDamageProperties();
            CheckAndRefreshUpgrades("RightArm");
        }
        else if (droppedPart == 3)
        {
            gun_progress1 = 0;
            gun_progress2 = 0;
            temp_gun_progress1 = 0;
            temp_gun_progress2 = 0;
            temp_gun_progressNum = 0;
            currentDamage_blaster = controlScript.weaponValues_blaster.damageAmount;
            currentDamage_energyBeam = controlScript.weaponValues_energy.damageAmount;

            currentUpgrade_blaster = 0;
            currentUpgrade_energyBeam = 0;

            TransferDamageProperties();
            CheckAndRefreshUpgrades("LeftArm");
        }
        else if (droppedPart == 4)
        {
            legs_progress1 = 0;
            legs_progress2 = 0;
            temp_legs_progress1 = 0;
            temp_legs_progress2 = 0;
            temp_legs_progressNum = 0;

            CheckAndRefreshUpgrades("Legs");
        }
    }

    public void EnableUpgrade(int partType, int subType, int progressType, int upgradeNum)
    {
        if (partType == 0)
        { //Head
            if (progressType == 1)
            {
                controlScript.ChangeHeadVisibility(upgradeNum);
            }
            else if (progressType == 2)
            {
                controlScript.ChangeHeadDurability(upgradeNum);
            }
            TransferDamageProperties();
        }

        if (partType == 1)
        {  //Body
            if (progressType == 1)
            {
                if (upgradeNum == 0)
                {
                    currentUpgrade_extraSlots = 0;
                }
                else if (upgradeNum == 1)
                {
                    currentUpgrade_extraSlots = 1;
                }
                else if (upgradeNum == 2)
                {
                    currentUpgrade_extraSlots = 2;
                }
            }
            else if (progressType == 2)
            {
                if (upgradeNum == 0)
                {
                    currentUpgrade_defenseShield = 0;
                }
                else if (upgradeNum == 1)
                {
                    currentUpgrade_defenseShield = 1;
                }
                else if (upgradeNum == 2)
                {
                    currentUpgrade_defenseShield = 2;
                }
            }
            ChangeShieldProperties();
        }

        if (partType == 2)
        {  //Drill
            if (progressType == 1)
            {
                if (upgradeNum == 0)
                {
                    currentUpgrade_workerDrill = 0f;
                }
                else if (upgradeNum == 1)
                {
                    currentUpgrade_workerDrill = 1.0f;
                }
                else if (upgradeNum == 2)
                {
                    currentUpgrade_workerDrill = 2.0f;
                }
            }
            else if (progressType == 2)
            {
                if (upgradeNum == 0)
                {
                    currentRange_workerDrill = 0;
                }
                else if (upgradeNum == 1)
                {
                    currentRange_workerDrill = 1;
                }
                else if (upgradeNum == 2)
                {
                    currentRange_workerDrill = 2;
                }
                controlScript.EnableDrillArm();
            }
            TransferDamageProperties();
        }

        if (partType == 3)
        {  //Guns
            if (subType == 0)
            {  //Blaster
                if (progressType == 1)
                {//Damage
                    if (upgradeNum == 0)
                    {
                        currentUpgrade_blaster = 0f;
                    }
                    else if (upgradeNum == 1)
                    {
                        currentUpgrade_blaster = 1.0f;
                    }
                    else if (upgradeNum == 2)
                    {
                        currentUpgrade_blaster = 2.0f;
                    }
                }
                else if (progressType == 2)
                {
                    //Fire Rate
                    controlScript.EnableGunArm();
                }
            }
            if (subType == 1)
            {  //Missile
                if (progressType == 1)
                {
                    //Blast Radius
                }
                else if (progressType == 2)
                {
                    //Fire Rate
                    controlScript.EnableGunArm();
                }
            }
            if (subType == 2)
            {  //Energy Beam
                if (progressType == 1)
                {//Damage
                    if (upgradeNum == 0)
                    {
                        currentUpgrade_energyBeam = 0f;
                    }
                    else if (upgradeNum == 1)
                    {
                        currentUpgrade_energyBeam = 1.0f;
                    }
                    else if (upgradeNum == 2)
                    {
                        currentUpgrade_energyBeam = 2.0f;
                    }
                }
                else if (progressType == 2)
                {
                    //Beam Width
                }
            }
            TransferDamageProperties();
        }

        if (partType == 4)
        {  //Legs
            if (subType == 0)
            {  //Worker Legs
                if (progressType == 1)
                {//Speed
                    if (upgradeNum == 0)
                    {
                        currentSpeed_workerLegs = 0;
                    }
                    else if (upgradeNum == 1)
                    {
                        currentSpeed_workerLegs = 1;
                    }
                    else if (upgradeNum == 2)
                    {
                        currentSpeed_workerLegs = 2;
                    }
                }
                else if (progressType == 2)
                {//Jump Height
                    if (upgradeNum == 0)
                    {
                        currentJump_workerLegs = 0;
                    }
                    else if (upgradeNum == 1)
                    {
                        currentJump_workerLegs = 1;
                    }
                    else if (upgradeNum == 2)
                    {
                        currentJump_workerLegs = 2;
                    }
                }
            }
            if (subType == 1)
            {  //Jump Legs
                if (progressType == 1)
                {//# of jumps
                    if (upgradeNum == 0)
                    {
                        currentJump_JumpLegs = 0;
                    }
                    else if (upgradeNum == 1)
                    {
                        currentJump_JumpLegs = 1;
                    }
                    else if (upgradeNum == 2)
                    {
                        currentJump_JumpLegs = 2;
                    }
                }
                else if (progressType == 2)
                {//speed
                    if (upgradeNum == 0)
                    {
                        currentSpeed_JumpLegs = 0f;
                    }
                    else if (upgradeNum == 1)
                    {
                        currentSpeed_JumpLegs = 1;
                    }
                    else if (upgradeNum == 2)
                    {
                        currentSpeed_JumpLegs = 2;
                    }
                }
            }
            Invoke("ChangeLegMovement", 0.2f);
        }
    }

    public void DestroyOldSelf()
    {
        spawnPosition = currentPlayerObject.transform.position;
        Destroy(currentPlayerObject);
    }

    public void PauseMovement()
    {
        canMove = false;
        controlScript.Invoke("PauseMovement", 0.01f);
        mouseCursorScript.ChangeCursorToDefault();
    }

    public void ResumeMovement()
    {
        canMove = true;
        controlScript.Invoke("ResumeMovement", 0.01f);
        if (hasGun)
        {
            mouseCursorScript.ChangeCursorToGun();
        }
        else
        {
            mouseCursorScript.ChangeCursorToDefault();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostBotScript : MonoBehaviour
{
    public LostBotScriptableObject enemyData;

    public string enemyName;

    [SerializeField] private GameObject damageNum;
    private DamageNum damageNumScript;
    private Vector3 dmgNumPos;

    public GameObject parentObject;
    public LostBotMeta parentScript;

    public GameObject playerObject;

    public CapsuleCollider capsuleCollider;

    public GameObject projectile_bullet;
    public GameObject projecile_missile;
    public GameObject projectile_beam;
    public GameObject projectile_autobullet;

    private Vector3 direction;
    public GameObject bulletObject;

    public Vector3 bulletSpawnPos;
    public GameObject bulletOrigin;

    public GameObject attackTriggerObject;
    public BoxCollider attackCollider;

    public GameObject detectionObject;
    public EnemyDetectScript detectionScript;
    public SphereCollider triggerDetectSphere;

    public GameObject drillObject;
    public Animator drillAnim;
    public GameObject drillSmokeObject;
    private ParticleSystem smokeParticles;

    public Sprite defaultGunSprite;
    public Sprite missileGunSprite;
    public Sprite laserGunSprite;
    public Sprite autoGunSprite;

    public GameObject gunObject;
    public SpriteRenderer gunSpriteRend;


    public Vector3 playerPosition;
    public float distanceToPlayer = 0.0f;

    public bool hasHead = true;
    public bool hasBody = false;
    public bool hasDrill = false;
    public bool hasGun = false;
    public bool hasLegs = false;

    public int gunType = 0;

    public GameObject drop_headObject;
    public GameObject drop_bodyObject;

    public GameObject drop_blasterObject;
    public GameObject drop_missileObject;
    public GameObject drop_laserObject;
    public GameObject drop_autogunObject;

    public GameObject drop_drillObject;
    public GameObject drop_legsObject;

    public bool isActive = true;
    public bool idle = true;
    public bool chasingPlayer = false;
    public bool aimAtPlayer = false;
    public bool movingAway = false;
    public bool activated = false;

    public bool resetDetection = false;

    public float damageTaken;

    public float speed = 1.0f;
    public float idleSpeed = 1.0f;
    public float attackSpeed = 1.0f;
    public float moveAwaySpeed = 1.0f;

    public Material shockMaterial;
    public bool isShocked = false;
    public float shockTimer = 6.0f;

    public float aggroResetTimer = 5.0f;
    public float detectionRadius = 5.0f;

    public bool gunReady = false;
    public bool drillReady = true;

    public float gunCooldownTimer = 1.0f;
    public float drillCooldownTimer = 0.5f;

    private float tintFadeSpeed = 0.25f;
    public bool isHit = true;
    public bool isDead = false;
    public SpriteRenderer spriteRend;
    public Material spriteMaterial;
    public Material hitMaterial;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<LostBotMeta>();

        FindPlayer();

        anim = GetComponent<Animator>();

        capsuleCollider = GetComponent<CapsuleCollider>();
        spriteRend = GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRend.material;
        detectionScript = detectionObject.GetComponent<EnemyDetectScript>();
        triggerDetectSphere = detectionObject.GetComponent<SphereCollider>();

        attackCollider = attackTriggerObject.GetComponent<BoxCollider>();

        if (hasBody)
        {
            if (!hasDrill)
            {
                DisableDrillArm();
            }
            else if (hasDrill)
            {
                EnableDrillArm();
            }

            if (!hasGun)
            {
                DisableGunArm();
            }
            else if (hasGun)
            {
                EnableGunArm();
            }

        }

        if (hasLegs)
        {
            idleSpeed = enemyData.legs_idleSpeed;
            attackSpeed = enemyData.legs_attackSpeed;
            moveAwaySpeed = enemyData.legs_moveAwaySpeed;
        }
        else if (hasBody)
        {
            idleSpeed = enemyData.body_idleSpeed;
            attackSpeed = enemyData.body_attackSpeed;
            moveAwaySpeed = enemyData.body_moveAwaySpeed;
        }
        else
        {
            idleSpeed = enemyData.idleSpeed;
            attackSpeed = enemyData.attackSpeed;
            moveAwaySpeed = enemyData.moveAwaySpeed;
        }

        speed = idleSpeed;

        detectionRadius = enemyData.detectionRadius;
        triggerDetectSphere.radius = detectionRadius;

        aggroResetTimer = enemyData.aggroResetTimer;

        gunCooldownTimer = enemyData.gunCooldownTimer * 2;
        drillCooldownTimer = enemyData.drillCooldownTimer;


    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (activated)
        {
            if (playerObject != null)
            {
                if (chasingPlayer)
                {  //If Chase Player
                    if (!isShocked)
                    {
                        if (transform.position.x > playerObject.transform.position.x)
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                        }
                        else
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                        }

                        distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

                        playerPosition = playerObject.transform.position;

                        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);

                        if (distanceToPlayer < 2.0f)
                        {
                            if (drillReady)
                            {
                                DrillPlayer();
                            }
                        }
                        else
                        {
                            StopDrill();
                        }
                    }
                }

                if (movingAway)
                { //If Move Away
                    if (!isShocked)
                    {
                        if (transform.position.x > playerObject.transform.position.x)
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                        }

                        distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

                        playerPosition = playerObject.transform.position;
                        transform.position = Vector3.MoveTowards(transform.position, playerPosition, -1 * speed * Time.deltaTime);

                        attackCollider.enabled = false;
                    }
                }

                //Aim at Player
                if (aimAtPlayer)
                {
                    if (!isShocked)
                    {
                        if (hasGun)
                        {
                            playerPosition = PlayerManager.current.currentPlayerObject.transform.position;
                            direction = playerPosition - bulletOrigin.transform.position;
                            direction.Normalize();

                            gunObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, playerPosition - transform.position);

                            if (distanceToPlayer < detectionRadius)
                            {
                                if (gunReady)
                                {
                                    ShootPlayer();
                                }
                            }
                            else
                            {
                                StopGun();
                            }

                        }
                    }
                }


                //Cooldowns
                if (!gunReady)
                {
                    if (gunCooldownTimer > 0.0f)
                    {
                        gunCooldownTimer -= Time.deltaTime;
                    }
                    else
                    {
                        gunReady = true;
                        gunCooldownTimer = enemyData.gunCooldownTimer;
                    }
                }
                if (!drillReady)
                {
                    if (drillCooldownTimer > 0.0f)
                    {
                        drillCooldownTimer -= Time.deltaTime;
                    }
                    else
                    {
                        drillReady = true;
                        drillCooldownTimer = enemyData.drillCooldownTimer;
                    }
                }

                //Hit by Player
                if (isHit)
                {
                    if (tintFadeSpeed > 0.0f)
                    {
                        tintFadeSpeed -= Time.deltaTime;
                    }
                    else
                    {
                        tintFadeSpeed = 0.25f;
                        RecoverFromHit();
                    }
                }
            }
        }
    }

    public void AggroReset()
    {
        FindPlayer();
        MoveAwayFromPlayer();
    }

    public void FindPlayer()
    {
        playerObject = PlayerManager.current.currentPlayerObject;
    }


    //AI States
    public void Inactive()
    {
        isActive = false;
    }
    public void Active()
    {
        isActive = true;
    }
    public void Activated()
    {
        anim.SetBool("activated", true);
        AudioManager.current.currentSFXTrack = 34;
        AudioManager.current.PlaySfx();
    }
    public void IsAwake()
    {
        activated = true;
        isDead = false;
    }
    public void IdleState()
    {
        idle = true;
        chasingPlayer = false;
        movingAway = false;
        aimAtPlayer = false;
        anim.SetBool("isWalking", false);
    }
    public void ChasePlayer()
    {
        if (PlayerManager.current.hasHead)
        {
            idle = false;
            chasingPlayer = true;
            movingAway = false;
            aimAtPlayer = false;
            anim.SetBool("isWalking", true);
        }
    }
    public void MoveAwayFromPlayer()
    {
        if (PlayerManager.current.hasHead)
        {
            idle = false;
            chasingPlayer = false;
            movingAway = true;
            aimAtPlayer = false;
            anim.SetBool("isWalking", true);
        }
    }
    public void AimAtPlayer()
    {
        idle = false;
        chasingPlayer = false;
        movingAway = false;
        aimAtPlayer = true;
        anim.SetBool("isAttacking", true);
    }

    public void ShootPlayer()
    {
        if (gunReady)
        {
            UseGun();
            gunReady = false;
        }
    }
    public void DrillPlayer()
    {
        if (drillReady)
        {
            UseDrill();
            drillReady = false;
        }
    }

    private void UseGun()
    {
        anim.SetBool("isAttacking", true);
        bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x, bulletOrigin.transform.position.y, bulletOrigin.transform.position.z);

        if (gunType == 0)
        {
            GameObject newEnemyBullet = Instantiate(projectile_bullet, bulletSpawnPos, Quaternion.identity) as GameObject;
            EnemyBulletPhysics bulletScript = newEnemyBullet.GetComponent<EnemyBulletPhysics>();
            bulletScript.bulletDirection = direction;
            AudioManager.current.currentSFXTrack = 12;
            AudioManager.current.PlaySfx();
        }
        else if (gunType == 1)
        {
            GameObject newEnemyBullet = Instantiate(projecile_missile, bulletSpawnPos, Quaternion.identity) as GameObject;
            EnemyBulletPhysics bulletScript = newEnemyBullet.GetComponent<EnemyBulletPhysics>();
            bulletScript.bulletDirection = direction; AudioManager.current.currentSFXTrack = 15;
            AudioManager.current.PlaySfx();
        }
        else if (gunType == 2)
        {
            GameObject newEnemyBullet = Instantiate(projectile_beam, bulletSpawnPos, Quaternion.identity) as GameObject;
            EnemyBulletPhysics bulletScript = newEnemyBullet.GetComponent<EnemyBulletPhysics>();
            bulletScript.bulletDirection = direction;
            AudioManager.current.currentSFXTrack = 17;
            AudioManager.current.PlaySfx();
        }
    }
    private void StopGun()
    {
        anim.SetBool("isAttacking", false);
    }

    private void UseDrill()
    {
        anim.SetBool("isAttacking", true);
        drillAnim.SetBool("Active", true);
        attackCollider.enabled = true;
        smokeParticles.Play();
        AudioManager.current.currentSFXTrack = 11;
        AudioManager.current.PlaySfx();
    }
    private void StopDrill()
    {
        anim.SetBool("isAttacking", false);
        drillAnim.SetBool("Active", false);
        attackCollider.enabled = false;
        smokeParticles.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Bullet" || other.gameObject.tag == "Player_EnergyBeam")
        {
            if (!isHit)
            {
                if (!isDead)
                {
                    BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
                    damageTaken = bulletScript.damage;
                    TakeHit();
                }
                else
                {
                    TakeHitInactive();
                }
            }
        }
        if (other.gameObject.tag == "MissileAOE")
        {
            if (!isHit)
            {
                if (!isDead)
                {
                    MissileAOE missileAOEscript = other.gameObject.GetComponent<MissileAOE>();
                    damageTaken = missileAOEscript.damage;
                    TakeHardHit();
                }
                else
                {
                    TakeHitInactive();
                }
            }
        }
        if (other.gameObject.tag == "Player_EnergyBeam")
        {
            if (!isHit)
            {
                if (!isDead)
                {
                    BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
                    damageTaken = PlayerManager.current.currentDamage_energyBeam;
                    TakeHit();
                }
                else
                {
                    TakeHitInactive();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PlayerDrill")
        {
            if (!isHit)
            {
                if (!isDead)
                {
                    damageTaken = PlayerManager.current.currentDamage_workerDrill;
                    TakeHit();
                }
                else
                {
                    TakeHitInactive();
                }
            }
        }
    }

    public void TakeHardHit()
    {
        StopGun();
        DisplayDamage(true);
        attackCollider.enabled = false;
        isHit = true;
        spriteRend.material = hitMaterial;

        if (hasLegs)
        {
            LoseLegs();
            LoseBody();
            if (hasGun)
            {
                LoseGun();
            }
            if (hasDrill)
            {
                LoseDrill();
            }
        }
        else if (hasBody)
        {
            LoseBody();
            if (hasGun)
            {
                LoseGun();
            }
            if (hasDrill)
            {
                LoseDrill();
            }
        }
        else if (hasHead)
        {
            speed = 0;
            LoseHead();
        }
    }

    public void TakeHitInactive()
    {
        DisplayDamage(false);
        isHit = true;
        spriteRend.material = hitMaterial;
    }

    public void TakeHit()
    {
        StopGun();
        DisplayDamage(true);
        attackCollider.enabled = false;
        isHit = true;
        spriteRend.material = hitMaterial;

        if (hasLegs)
        {
            LoseLegs();
        }
        else if (hasGun)
        {
            LoseGun();
        }
        else if (hasDrill)
        {
            LoseDrill();
        }
        else if (hasBody)
        {
            LoseBody();
        }
        else if (hasHead)
        {
            speed = 0;
            LoseHead();
        }

        AudioManager.current.currentSFXTrack = 31;
        AudioManager.current.PlaySfx();
    }

    public void RecoverFromHit()
    {
        isHit = false;
        spriteRend.material = spriteMaterial;
        damageTaken = 0.0f;
    }

    public void DisplayDamage(bool active)
    {
        if (GameController.current.damageNumOption)
        {
            dmgNumPos = transform.position;
            GameObject newDamageNum = Instantiate(damageNum, new Vector3(dmgNumPos.x, dmgNumPos.y, dmgNumPos.z), Quaternion.identity) as GameObject;
            GameObject canvasObject = GameObject.Find("WorldCanvas");
            newDamageNum.transform.SetParent(canvasObject.transform);
            DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
            if (active)
            {
                damageNumScript.damageNum = Mathf.RoundToInt(damageTaken);
            }
            else
            {
                damageNumScript.damageNum = 0;
            }

            damageNumScript.DamageInit();
        }
    }

    public void EnableDrillArm()
    {
        smokeParticles = drillSmokeObject.GetComponent<ParticleSystem>();
        drillObject.SetActive(true);
        attackCollider.enabled = false;
        drillAnim = drillObject.GetComponent<Animator>();
        StopDrill();
    }
    public void DisableDrillArm()
    {
        drillObject.SetActive(false);
    }

    public void EnableGunArm()
    {
        gunSpriteRend = gunObject.GetComponent<SpriteRenderer>();
        if (gunType == 0)
        {
            gunSpriteRend.sprite = defaultGunSprite;
        }
        else if (gunType == 1)
        {
            gunSpriteRend.sprite = missileGunSprite;
        }
        else if (gunType == 2)
        {
            gunSpriteRend.sprite = laserGunSprite;
        }
        else if (gunType == 3)
        {
            gunSpriteRend.sprite = autoGunSprite;
        }
        gunObject.SetActive(true);
    }
    public void DisableGunArm()
    {
        gunObject.SetActive(false);
    }

    public void GainDrill()
    {
        hasDrill = true;
        drillObject.SetActive(true);
    }
    public void GainGun()
    {
        hasGun = true;
        gunObject.SetActive(true);
    }
    public void GainBody()
    {
        hasBody = true;
    }
    public void GainLegs()
    {
        hasLegs = true;
    }


    public void LoseDrill()
    {
        if (hasDrill)
        {
            GameObject newDrillPart = Instantiate(drop_drillObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            PickUpScript pickUpScript = newDrillPart.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.Invoke("DropNewPickup", 0.01f);

            hasDrill = false;
            parentScript.hasDrill = false;
            drillObject.SetActive(false);
            parentScript.Invoke("LoseDrill", 0.01f);

            AudioManager.current.currentSFXTrack = 32;
            AudioManager.current.PlaySfx();
        }
    }
    public void LoseGun()
    {
        if (hasGun)
        {
            if (gunType == 0)
            {
                GameObject newGunPart = Instantiate(drop_blasterObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                PickUpScript pickUpScript = newGunPart.GetComponent<PickUpScript>();
                pickUpScript.pickupType = 3;
                pickUpScript.Invoke("DropNewPickup", 0.01f);
            }
            if (gunType == 1)
            {
                GameObject newGunPart = Instantiate(drop_missileObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                PickUpScript pickUpScript = newGunPart.GetComponent<PickUpScript>();
                pickUpScript.pickupType = 3;
                pickUpScript.Invoke("DropNewPickup", 0.01f);
            }
            if (gunType == 2)
            {
                GameObject newGunPart = Instantiate(drop_laserObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                PickUpScript pickUpScript = newGunPart.GetComponent<PickUpScript>();
                pickUpScript.pickupType = 3;
                pickUpScript.Invoke("DropNewPickup", 0.01f);
            }

            hasGun = false;
            parentScript.hasGun = false;
            gunObject.SetActive(false);
            parentScript.Invoke("LoseGun", 0.01f);

            AudioManager.current.currentSFXTrack = 32;
            AudioManager.current.PlaySfx();
        }
    }
    public void LoseBody()
    {
        if (hasBody)
        {
            GameObject newBodyPart = Instantiate(drop_bodyObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            PickUpScript pickUpScript = newBodyPart.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 1;
            pickUpScript.Invoke("DropNewPickup", 0.01f);

            hasBody = false;
            parentScript.hasBody = false;
            parentScript.Invoke("LoseBody", 0.01f);
            activated = true;

            AudioManager.current.currentSFXTrack = 32;
            AudioManager.current.PlaySfx();
        }
    }
    public void LoseLegs()
    {
        if (hasLegs)
        {
            GameObject newLegsPart = Instantiate(drop_legsObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            PickUpScript pickUpScript = newLegsPart.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 4;
            pickUpScript.Invoke("DropNewPickup", 0.01f);

            hasLegs = false;
            parentScript.hasLegs = false;
            parentScript.Invoke("LoseLegs", 0.01f);
            activated = true;

            AudioManager.current.currentSFXTrack = 32;
            AudioManager.current.PlaySfx();
        }
    }

    public void Dead()
    {
        anim.SetBool("inactive", true);
    }

    public void LoseHead()
    {
        GameObject newHeadPart = Instantiate(drop_headObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        PickUpScript pickUpScript = newHeadPart.GetComponent<PickUpScript>();
        pickUpScript.pickupType = 9;
        pickUpScript.backupID = parentScript.id;
        pickUpScript.Invoke("DropNewPickup", 0.01f);
        pickUpScript.prevBotOwner = gameObject;

        isDead = true;
        activated = false;

        AudioManager.current.currentSFXTrack = 30;
        AudioManager.current.PlaySfx();

        DestroySelf();
    }

    public void DestroySelf()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<LostBotMeta>();
        parentScript.DestroySelf();
    }
    public void DropPartsAndDestroySelf()
    {
        if (hasLegs)
        {
            GameObject newLegsPart = Instantiate(drop_legsObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            PickUpScript pickUpScript = newLegsPart.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 4;
            pickUpScript.DropNewPickup();

            hasLegs = false;
        }
        if (hasBody)
        {
            GameObject newBodyPart = Instantiate(drop_bodyObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            PickUpScript pickUpScript = newBodyPart.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 1;
            pickUpScript.DropNewPickup();

            hasBody = false;
        }
        if (hasDrill)
        {
            GameObject newDrillPart = Instantiate(drop_drillObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            PickUpScript pickUpScript = newDrillPart.GetComponent<PickUpScript>();
            pickUpScript.pickupType = 2;
            pickUpScript.DropNewPickup();

            hasDrill = false;
        }
        if (hasGun)
        {
            if (gunType == 0)
            {
                GameObject newGunPart = Instantiate(drop_blasterObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                PickUpScript pickUpScript = newGunPart.GetComponent<PickUpScript>();
                pickUpScript.pickupType = 3;
                pickUpScript.DropNewPickup();
            }
            if (gunType == 1)
            {
                GameObject newGunPart = Instantiate(drop_missileObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                PickUpScript pickUpScript = newGunPart.GetComponent<PickUpScript>();
                pickUpScript.pickupType = 3;
                pickUpScript.DropNewPickup();
            }
            if (gunType == 2)
            {
                GameObject newGunPart = Instantiate(drop_laserObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
                PickUpScript pickUpScript = newGunPart.GetComponent<PickUpScript>();
                pickUpScript.pickupType = 3;
                pickUpScript.DropNewPickup();
            }
            hasGun = false;
        }

        Invoke("DestroySelf", 0.25f);
    }
}

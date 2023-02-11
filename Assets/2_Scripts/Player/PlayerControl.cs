using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerControl : MonoBehaviour
{
  public GameObject UI_CrystalManager;
  public UI_CrystalManager crystalManagerScript;

  public GameObject spriteAnimatorObject;
  public GameObject spriteTransformObject;
  public Animator spriteAnim;
  public SpriteAnimator spriteAnimScript;
  public GameObject playerLight0;
  public GameObject playerLight1;
  public GameObject playerLight2;

  public int lightUpgradeNum;

  public GroundedCharacterController characterControllerScript;

  private Rigidbody rb;

  private Vector3 mousePos;
  private Vector3 screenPoint;
  private Vector3 direction;

  private float horizontalControls;
  public bool facingLeft = false;
  public bool facingRight = true;

  public bool isHit = true;
  public bool isDead = false;

  public GameObject gunArm;
  public SpriteRenderer gunSpriteRend;
  public Material gunMaterial;
  public GameObject bulletOrigin;

  public GameObject drillArm;
  public GameObject drillArm1;
  public GameObject drillArm2;
  public GameObject drillArm3;
  public SpriteRenderer drillSpriteRend;
  public Material drillMaterial;
  public PlayerWeaponScriptableObject weaponValues_workerDrill;
  public Animator drillAnimator;
  public BoxCollider drillCollider;
  public bool drillSound;

  public bool drillStopped = false;

  public GameObject drillSmokeObject;
  private ParticleSystem smokeParticles;

  public Quaternion gunStartRotation;
  public Quaternion gunStartRotationLeft;

  public GameObject headToSwitch;
  public GameObject bodyToSwitch;
  public GameObject drillToSwitch;
  public GameObject gunToSwitch;
  public GameObject legsToSwitch;

  public Vector3 bulletSpawnPos;

  public GameObject projectileObject_blaster;
  public GameObject projectileObject_missile;
  public GameObject projectileObject_laser;

  public int gunType = 0; //0 = blaster, 1 = missile, 2 = laser, 3 = electro
  public int legType = 0; //0 = Worker bot, 1 = Jump Boots

  public PlayerWeaponScriptableObject weaponValues_blaster;
  public PlayerWeaponScriptableObject weaponValues_missile;
  public PlayerWeaponScriptableObject weaponValues_energy;

  public Sprite sprite_blasterGun;
  public Sprite sprite_missileLauncher;
  public Sprite sprite_laserBeam;

  public bool canMove = true;
  public bool isChangingScene = false;

  public bool gunFacing = true;

  public float _gunCooldownTime;
  public float _gunCooldownTime_original;

  public float drillCooldownTime = 0.5f;
  public int drillType = 0;
  public float drillDamage = 1f;

  public bool gunReady = false;
  public bool drillReady = false;

  public SpriteRenderer spriteRend;
  public Material spriteMaterial;
  public Material hitMaterial;
  public Material shieldHitMaterial;
  public Material currentMaterial;
  public float tintFadeSpeed = 0.3f;

  public float startCooldownTimer = 1.0f;
  public bool started = false;

  public GameObject onHitParticles;

  private EnemyScript enemyScript;
  private CyberMantisScript bossScript;

  public bool endingMovement;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    spriteAnim = spriteAnimatorObject.GetComponent<Animator>();
    spriteAnimScript = spriteAnim.GetComponent<SpriteAnimator>();

    characterControllerScript = GetComponent<GroundedCharacterController>();

    screenPoint = Camera.main.WorldToScreenPoint(transform.position);
    direction = (Vector3)(Input.mousePosition - screenPoint);
    direction.Normalize();

    if (PlayerManager.current.hasGun)
    {
      gunStartRotation = gunArm.transform.rotation;
      gunStartRotation *= Quaternion.Euler(Vector3.forward * -90);
      gunStartRotationLeft = Quaternion.Euler(0, 0, 90);
    }
    if (PlayerManager.current.hasDrill)
    {
      drillSmokeObject = GameObject.Find("PlayerDrillSmoke");
      smokeParticles = drillSmokeObject.GetComponent<ParticleSystem>();
    }

    spriteRend = spriteAnimatorObject.GetComponent<SpriteRenderer>();
    spriteMaterial = spriteRend.material;

    UI_CrystalManager = MenuManager.current.CrystalManager;
    crystalManagerScript = UI_CrystalManager.GetComponent<UI_CrystalManager>();
    lightUpgradeNum = PlayerManager.current.head_progress1;
    ChangeHeadVisibility(lightUpgradeNum);

    if (PlayerManager.current.hasBody)
    {
      InitWeapons();
    }
    Invoke("CheckMaterials", 1.0f);
  }

  // Update is called once per frame
  void Update()
  {
    if (!started)
    {
      if (startCooldownTimer > 0f)
      {
        startCooldownTimer -= Time.deltaTime;
      }
      else
      {
        started = true;
      }
    }

    if (!isDead)
    {
      if (canMove)
      {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        horizontalControls = Input.GetAxisRaw("Horizontal");

        //Get which direction player is facing
        if (horizontalControls == 1.0)
        {
          facingRight = true;
          facingLeft = false;
        }
        else if (horizontalControls == -1.0)
        {
          facingRight = false;
          facingLeft = true;
        }
        if (PlayerManager.current.hasGun)
        {
          GunControls();
        }
        if (PlayerManager.current.hasDrill)
        {
          DrillControls();
        }
      }
    }
    if (isHit)
    {
      if (tintFadeSpeed > 0)
      {
        tintFadeSpeed -= Time.deltaTime;
      }
      else
      {
        RecoverFromHit();
        tintFadeSpeed = 0.3f;
      }
    }

    if (endingMovement)
    {
      transform.position += new Vector3(1.5f * Time.deltaTime, 0, 0);
    }
  }

  void FixedUpdate()
  {
    if (!isDead)
    {
      if (PlayerManager.current.hasDrill)
      {
        CooldownTimerDrill();
      }
      if (PlayerManager.current.hasGun)
      {
        CooldownTimerGun();
      }
    }
  }

  private void GunControls()
  {
    if (!MenuManager.current.isMouseOver)
    {
      if (Input.GetButton("Fire1") && gunReady && started)
      {
        gunReady = false;
        if (gunType == 0)
        {
          FireBlaster();
        }
        else if (gunType == 1)
        {
          FireMissile();
        }
        else if (gunType == 2)
        {
          FireLaser();
        }
      }
      if (gunFacing)
      {
        GunArmAim();
      }
      else
      {
        ResetGunAim();
      }
    }
  }

  private void DrillControls()
  {
    if (Input.GetButton("Fire2"))
    {
      if (drillReady)
      {
        UseDrill();
      }
    }
    if (Input.GetButtonUp("Fire2"))
    {
      StopDrill();
    }
  }

  private void CooldownTimerGun()
  {
    if (PlayerManager.current.hasGun)
    {
      if (_gunCooldownTime > 0.0f)
      {
        _gunCooldownTime -= Time.deltaTime;
      }
      else
      {
        gunReady = true;
      }
    }
  }

  private void CooldownTimerDrill()
  {
    if (PlayerManager.current.hasDrill)
    {
      if (drillCooldownTime > 0.0f)
      {
        drillCooldownTime -= Time.deltaTime;
        drillReady = false;
      }
      else
      {
        drillReady = true;
        drillSound = false;
      }
    }
  }

  public void InitWeapons()
  {
    gunSpriteRend = gunArm.GetComponent<SpriteRenderer>();
    gunMaterial = gunSpriteRend.material;

    if (PlayerManager.current.hasDrill)
    {
      EnableDrillArm();
    }
    else
    {
      DisableDrillArm();
    }
    if (!PlayerManager.current.hasGun)
    {
      gunArm.SetActive(false);
    }
    else
    {
      EnableGunArm();
    }
  }

  public void GunArmAim()
  {
    gunArm.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

    screenPoint = Camera.main.WorldToScreenPoint(transform.position);
    direction = (Vector3)(Input.mousePosition - screenPoint);
    direction.Normalize();
  }

  public void ResetGunAim()
  {
    if (!facingLeft)
    {
      gunArm.transform.rotation = gunStartRotation;
      direction = new Vector3(1, 0, 0);
    }
    else
    {
      gunArm.transform.rotation = gunStartRotationLeft;
      direction = new Vector3(-1, 0, 0);
    }
  }

  public void EnableDrillArm()
  {
    if (drillArm != null)
    {
      Destroy(drillArm);
      drillArm = null;
    }
    if (PlayerManager.current.currentRange_workerDrill == 0)
    {
      drillArm = Instantiate(drillArm1, new Vector3(0, 0, 0), Quaternion.identity);
      drillArm.transform.SetParent(spriteTransformObject.transform);
      drillArm.transform.localPosition = new Vector3(0f, 0.06f, 0f);
      if (!facingLeft)
      {
        drillArm.transform.localScale = new Vector3(drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
      else
      {
        drillArm.transform.localScale = new Vector3(-drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
    }
    if (PlayerManager.current.currentRange_workerDrill == 1)
    {
      drillArm = Instantiate(drillArm2, new Vector3(0, 0, 0), Quaternion.identity);
      drillArm.transform.SetParent(spriteTransformObject.transform);
      drillArm.transform.localPosition = new Vector3(0.05f, 0.06f, 0f);
      if (!facingLeft)
      {
        drillArm.transform.localScale = new Vector3(drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
      else
      {
        drillArm.transform.localScale = new Vector3(-drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
    }
    if (PlayerManager.current.currentRange_workerDrill == 2)
    {
      drillArm = Instantiate(drillArm3, new Vector3(0, 0, 0), Quaternion.identity);
      drillArm.transform.SetParent(spriteTransformObject.transform);
      drillArm.transform.localPosition = new Vector3(0.05f, 0.06f, 0f);
      if (!facingLeft)
      {
        drillArm.transform.localScale = new Vector3(drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
      else
      {
        drillArm.transform.localScale = new Vector3(-drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
    }
    if (PlayerManager.current.hasLegs)
    {
      drillArm.transform.localPosition = new Vector3(0.03f, 0.375f, 0f);
    }

    drillSpriteRend = drillArm.GetComponent<SpriteRenderer>();
    drillMaterial = drillSpriteRend.material;

    drillDamage = weaponValues_workerDrill.damageAmount;
    drillArm.SetActive(true);
    drillSmokeObject = GameObject.Find("PlayerDrillSmoke");
    smokeParticles = drillSmokeObject.GetComponent<ParticleSystem>();
    drillAnimator = drillArm.GetComponent<Animator>();
    drillCollider = drillArm.GetComponent<BoxCollider>();
    Invoke("StopDrill", 0.05f);
    Invoke("CheckDrillFacingPos", 0.2f);
  }
  public void DisableDrillArm()
  {
    if (drillArm != null)
    {
      Destroy(drillArm);
      drillArm = null;
    }
  }

  public void CheckDrillFacingPos()
  {
    if (!facingLeft)
    { //Facing Right
      if (drillArm.transform.localScale.x == 1)
      {

      }
      else
      {
        drillArm.transform.localScale = new Vector3(-drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
    }
    else
    { //Facing Left
      if (drillArm.transform.localScale.x == 1)
      {
        drillArm.transform.localScale = new Vector3(drillArm.transform.localScale.x, drillArm.transform.localScale.y, drillArm.transform.localScale.z);
      }
      else
      {

      }
    }
  }

  public void EnableGunArm()
  {
    if (PlayerManager.current.hasGun)
    {
      gunArm.SetActive(true);
      gunSpriteRend = gunArm.GetComponent<SpriteRenderer>();

      gunMaterial = gunSpriteRend.material;

      if (gunType == 0)
      {
        gunSpriteRend.sprite = sprite_blasterGun;
        _gunCooldownTime = weaponValues_blaster.gunCooldownTime;

        //Set fire rate based on Upgrades
        if (PlayerManager.current.gun_progress2 == 0)
        {
          _gunCooldownTime = 0.75f;
        }
        else if (PlayerManager.current.gun_progress2 == 1)
        {
          _gunCooldownTime = 0.6f;
        }
        else if (PlayerManager.current.gun_progress2 == 2)
        {
          _gunCooldownTime = 0.45f;
        }
      }
      else if (gunType == 1)
      {
        gunSpriteRend.sprite = sprite_missileLauncher;
        _gunCooldownTime = weaponValues_missile.gunCooldownTime;

        //Set fire rate based on Upgrades
        if (PlayerManager.current.gun_progress2 == 0)
        {
          _gunCooldownTime = 1.5f;
        }
        else if (PlayerManager.current.gun_progress2 == 1)
        {
          _gunCooldownTime = 1.25f;
        }
        else if (PlayerManager.current.gun_progress2 == 2)
        {
          _gunCooldownTime = 1.0f;
        }
      }
      else if (gunType == 2)
      {
        gunSpriteRend.sprite = sprite_laserBeam;
        _gunCooldownTime = weaponValues_energy.gunCooldownTime;
        //Set  beam width based on Upgrades
        if (PlayerManager.current.gun_progress2 == 0)
        {
          //beam width 0
        }
        else if (PlayerManager.current.gun_progress2 == 1)
        {
          //beam width 1
        }
        else if (PlayerManager.current.gun_progress2 == 2)
        {
          //beam width 2
        }
      }
      _gunCooldownTime_original = _gunCooldownTime;
    }
  }
  public void DisableGunArm()
  {
    if (gunArm != null)
    {
      gunArm.SetActive(false);
    }
  }


  public void FireBlaster()
  {
    bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x, bulletOrigin.transform.position.y + 0.1f, bulletOrigin.transform.position.z);
    GameObject newBullet = Instantiate(projectileObject_blaster, bulletSpawnPos, Quaternion.identity) as GameObject;
    BulletPhysics bulletScript = newBullet.GetComponent<BulletPhysics>();
    bulletScript.bulletDirection = direction;
    _gunCooldownTime = _gunCooldownTime_original;

    AudioManager.current.currentSFXTrack = 13;
    AudioManager.current.PlaySfx();
  }
  public void FireMissile()
  {
    bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x, bulletOrigin.transform.position.y + 0.1f, bulletOrigin.transform.position.z);
    GameObject newMissile = Instantiate(projectileObject_missile, bulletSpawnPos, Quaternion.identity) as GameObject;
    BulletPhysics bulletScript = newMissile.GetComponent<BulletPhysics>();
    bulletScript.bulletDirection = direction;
    _gunCooldownTime = _gunCooldownTime_original;

    AudioManager.current.currentSFXTrack = 15;
    AudioManager.current.PlaySfx();
  }
  public void FireLaser()
  {
    bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x, bulletOrigin.transform.position.y, bulletOrigin.transform.position.z);
    GameObject newBeam = Instantiate(projectileObject_laser, bulletSpawnPos, Quaternion.identity) as GameObject;
    BulletPhysics bulletScript = newBeam.GetComponent<BulletPhysics>();
    bulletScript.bulletDirection = direction;
    _gunCooldownTime = _gunCooldownTime_original;

    AudioManager.current.currentSFXTrack = 17;
    AudioManager.current.PlaySfx();
  }

  public void UseDrill()
  {
    drillCooldownTime = 0.5f;
    if (!drillSound)
    {
      AudioManager.current.currentSFXTrack = 11;
      AudioManager.current.PlaySfx();
      drillSound = true;
    }
    drillSmokeObject = GameObject.Find("PlayerDrillSmoke");
    smokeParticles = drillSmokeObject.GetComponent<ParticleSystem>();
    smokeParticles.Play();
    drillAnimator.SetBool("Active", true);
    drillCollider.enabled = true;
  }
  public void StopDrill()
  {
    drillSmokeObject = GameObject.Find("PlayerDrillSmoke");
    smokeParticles = drillSmokeObject.GetComponent<ParticleSystem>();
    smokeParticles.Stop();
    drillAnimator.SetBool("Active", false);
    drillCollider.enabled = false;
  }

  public void TakeShieldHit()
  {
    currentMaterial = shieldHitMaterial;

    isHit = true;
    spriteRend.material = currentMaterial;

    if (PlayerManager.current.hasGun)
    {
      gunSpriteRend.material = currentMaterial;
    }
    if (PlayerManager.current.hasDrill)
    {
      drillSpriteRend.material = currentMaterial;
    }
  }

  public void TakeHit(int damageNum)
  {
    currentMaterial = hitMaterial;

    isHit = true;
    spriteRend.material = currentMaterial;

    if (!PlayerManager.current.hasBody)
    {
      PlayerManager.current.currentDurability -= damageNum;
    }

    if (PlayerManager.current.hasGun)
    {
      gunSpriteRend.material = currentMaterial;
    }
    if (PlayerManager.current.hasDrill)
    {
      drillSpriteRend.material = currentMaterial;
    }

    transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);

    float speed = 200.0f;
    rb.isKinematic = false;
    Vector3 force = transform.forward;
    force = new Vector3(force.x, 3, 0);
    rb.AddForce(force * speed);
    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

    GameObject onHitParticle = Instantiate(onHitParticles, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
  }

  public void RecoverFromHit()
  {
    isHit = false;
    if (spriteRend != null)
    {
      spriteRend.material = spriteMaterial;
    }

    if (PlayerManager.current.hasGun)
    {
      gunSpriteRend.material = spriteMaterial;
    }
    if (PlayerManager.current.hasDrill)
    {
      drillSpriteRend.material = spriteMaterial;
    }
  }

  public void CheckMaterials()
  {
    if (isHit)
    {
      isHit = false;
      spriteRend.material = spriteMaterial;
    }

    if (PlayerManager.current.hasDrill)
    {
      drillSpriteRend.material = spriteMaterial;
    }
    if (PlayerManager.current.hasGun)
    {
      gunSpriteRend.material = spriteMaterial;
    }
  }

  public void DestroySelf()
  {
    Destroy(gameObject);
  }

  public void PauseMovement()
  {
    canMove = false;
    characterControllerScript.enabled = false;
    spriteAnim.SetBool("MovementPaused", true);
    spriteAnimScript.enabled = false;
  }

  public void ResumeMovement()
  {
    canMove = true;
    characterControllerScript.enabled = true;
    spriteAnim.SetBool("MovementPaused", false);
    spriteAnimScript.enabled = true;
  }
  public void EndingMovementWalkRight()
  {
    if (!endingMovement)
    {
      transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
      facingRight = true;

      characterControllerScript.enabled = false;
      spriteAnimScript.enabled = true;
      spriteAnim.SetBool("MovementPaused", false);
      spriteAnim.SetBool("EndingWalkRight", true);
      endingMovement = true;
    }
  }

  public void ChangeScene()
  {
    isChangingScene = true;
    canMove = false;
    PlayerManager.current.isChangingScene = true;
    PlayerManager.current.canMove = false;
  }

  public void ChangeHeadVisibility(int num)
  {
    if (num == 0)
    {
      playerLight0.SetActive(true);
      playerLight1.SetActive(false);
      playerLight2.SetActive(false);
    }
    else if (num == 1)
    {
      playerLight0.SetActive(false);
      playerLight1.SetActive(true);
      playerLight2.SetActive(false);
    }
    else if (num == 2)
    {
      playerLight0.SetActive(false);
      playerLight1.SetActive(false);
      playerLight2.SetActive(true);
    }
  }

  public void ChangeHeadDurability(int num)
  {
    if (num == 0)
    {
      PlayerManager.current.maxDurability = 1;
    }
    else if (num == 1)
    {
      PlayerManager.current.maxDurability = 2;
    }
    else if (num == 2)
    {
      PlayerManager.current.maxDurability = 3;
    }
  }

  public void ChangeDrillRange()
  {

  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Enemy_Bullet")
    {
      if (!PlayerManager.current.isHit && !PlayerManager.current.isDead)
      {
        EnemyBulletPhysics eBulletPhysics = other.gameObject.GetComponent<EnemyBulletPhysics>();
        PlayerManager.current.TakeHit();
      }
    }
    if (other.gameObject.tag == "MissileAOE")
    {
      if (!PlayerManager.current.isHit && !PlayerManager.current.isDead)
      {
        PlayerManager.current.TakeHitLimbs();
      }
    }
    if (other.gameObject.tag == "Enemy_EnergyBeam")
    {
      if (!PlayerManager.current.isHit && !PlayerManager.current.isDead)
      {
        PlayerManager.current.TakeHit();
      }
    }

    if (other.gameObject.tag == "BossHitCollision")
    {
      if (!PlayerManager.current.isHit && !PlayerManager.current.isDead)
      {
        bossScript = other.gameObject.transform.parent.gameObject.GetComponent<CyberMantisScript>();
        bossScript.attackCollider.enabled = false;
        bossScript.FindPlayer();
        PlayerManager.current.TakeHit();
      }
    }

    if (other.gameObject.name == "BossTrigger")
    {
      GameObject blueBot = GameObject.Find("BlueBot");
      BlueBotScript blueBotScript = blueBot.GetComponent<BlueBotScript>();

      blueBotScript.ActivateDialogue();
      Destroy(other.gameObject);
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.gameObject.tag == "Enemy_Hit")
    {
      if (!PlayerManager.current.isHit && !PlayerManager.current.isDead)
      {
        PlayerManager.current.TakeHit();
      }
    }

    if (other.gameObject.tag == "HitCollision")
    {
      if (!PlayerManager.current.isHit && !PlayerManager.current.isDead)
      {
        enemyScript = other.gameObject.transform.parent.gameObject.GetComponent<EnemyScript>();
        enemyScript.attackCollider.enabled = false;
        enemyScript.AggroReset();
        PlayerManager.current.TakeHit();
      }
    }
  }
}

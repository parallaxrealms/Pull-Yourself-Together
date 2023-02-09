using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberMantisScript : MonoBehaviour
{
  private GameObject gameController;
  public BossScriptableObject bossData;

  [SerializeField] private GameObject damageNum;
  private DamageNum damageNumScript;
  private Vector3 dmgNumPos;

  public GameObject currentBossObject;
  public GameObject cyberMantis_full;
  public GameObject cyberMantis_body;
  public GameObject detectionTrigger;

  public GameObject explodeParticlesObject;
  public GameObject explodeParticles;

  public Animator anim;
  public Rigidbody rb;

  public float health;
  public float damageTaken;
  public float speed;

  public bool idle;
  public bool isActive;
  public bool isDead;
  public bool isAttacking;
  public bool chasingPlayer;

  private float tintFadeSpeed = 0.3f;
  public bool isHit = false;
  public SpriteRenderer spriteRend;
  public Material spriteMaterial;
  public Material hitMaterial;

  public bool resetDetection = false;

  public float aggroResetTimer;
  public float detectionRadius;

  public GameObject playerObject;
  public Vector3 playerPosition;
  public float distanceToPlayer = 0.0f;

  public GameObject deadObject;

  public GameObject attackColliderObject;
  public BoxCollider attackCollider;

  public bool legsBroken = false;

  public bool hasFallen = false;

  public Vector3 lastPos;

  void Awake()
  {
    DontDestroyOnLoad(gameObject);
    name = "CyberMantis";
  }
  // Start is called before the first frame update
  void Start()
  {
    anim = cyberMantis_full.GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();

    playerObject = PlayerManager.current.currentPlayerObject;

    currentBossObject = cyberMantis_full;
    spriteRend = currentBossObject.GetComponent<SpriteRenderer>();
    spriteMaterial = spriteRend.material;

    cyberMantis_body.SetActive(false);

    DisableAttackCollider();

    GameController.current.ListBosses.Add(gameObject);

    health = bossData.health;
    speed = bossData.speed;
  }

  // Update is called once per frame
  void Update()
  {

  }

  void FixedUpdate()
  {
    if (isHit)
    {
      if (tintFadeSpeed > 0.0f)
      {
        tintFadeSpeed -= Time.deltaTime;
      }
      else
      {
        tintFadeSpeed = 0.3f;
        RecoverFromHit();
      }
    }

    if (isActive)
    {
      if (!isDead)
      {
        if (idle)
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

          transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
        }
        if (chasingPlayer)
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

          transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);

          if (distanceToPlayer <= 3.2f)
          {
            AttackPlayer();
            speed = bossData.attackSpeed * 1.0f;
          }
          else if (distanceToPlayer >= 3.21f)
          {
            ChasePlayer();
            speed = bossData.attackSpeed * 1.5f;
          }
          else
          {
            ChasePlayer();
            speed = bossData.attackSpeed * 1.25f;
          }
        }
      }
    }
  }

  public void FindPlayer()
  {
    playerObject = PlayerManager.current.currentPlayerObject;
    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
  }

  public void Spawn()
  {

  }

  public void SetLastPos()
  {
    lastPos = transform.position;

    transform.position = new Vector3(5500f, 5500f, 5500f);
    rb.useGravity = false;
    IdleState();
    Inactive();
  }

  public void Respawn()
  {
    transform.position = lastPos;
    if (hasFallen)
    {
      rb.useGravity = true;
      Active();
      IdleState();
    }
  }

  public void IdleState()
  {
    FindPlayer();
    idle = true;
    chasingPlayer = false;
    anim.SetBool("isIdle", true);
    anim.SetBool("isWalking", false);
    anim.SetBool("isAttacking", false);
  }
  public void ChasePlayer()
  {
    FindPlayer();
    idle = false;
    chasingPlayer = true;
    anim.SetBool("isIdle", false);
    anim.SetBool("isWalking", true);
  }
  public void AttackPlayer()
  {
    FindPlayer();
    anim.SetBool("isAttacking", true);
  }
  public void BreakLegs()
  {
    isActive = false;
    explodeParticles = Instantiate(explodeParticlesObject, new Vector3(currentBossObject.transform.position.x, currentBossObject.transform.position.y, currentBossObject.transform.position.z), Quaternion.identity) as GameObject;
    anim.SetBool("isIdle", false);
    anim.SetBool("isWalking", false);
    anim.SetBool("isAttacking", false);
    anim.SetBool("legsBreak", true);
    ActivateBodyPhase();

    AudioManager.current.currentSFXTrack = 18;
    AudioManager.current.PlaySfx();
  }

  public void ActivateBodyPhase()
  {
    cyberMantis_body.SetActive(true);
    currentBossObject = cyberMantis_body;
    anim = cyberMantis_body.GetComponent<Animator>();

    attackCollider.center = new Vector3(-0.75f, -1f, 0f);
    spriteRend = cyberMantis_body.GetComponent<SpriteRenderer>();
    spriteMaterial = spriteRend.material;
    FindPlayer();
    isActive = true;
  }

  public void BeginFalling()
  {
    rb.useGravity = true;
    hasFallen = true;

    AudioManager.current.currentSFXTrack = 122;
    AudioManager.current.PlaySfx();
  }

  public void DoneFalling()
  {
    anim.SetBool("doneFalling", true);
  }
  public void Active()
  {
    FindPlayer();
    isActive = true;
    GameController.current.Invoke("BossFightStarted", 0.1f);
    PlayerManager.current.Invoke("ResumeMovement", 0.01f);
  }
  public void Inactive()
  {
    isActive = false;
    anim.SetBool("isAttacking", false);
    anim.SetBool("isIdle", true);
  }

  public void EnableAttackCollider()
  {
    if (!isDead)
    {
      attackCollider = attackColliderObject.GetComponent<BoxCollider>();
      attackCollider.enabled = true;
    }
  }
  public void DisableAttackCollider()
  {
    if (!isDead)
    {
      attackCollider = attackColliderObject.GetComponent<BoxCollider>();
      attackCollider.enabled = false;
    }
  }


  public void TakeHit()
  {
    if (!isDead)
    {
      speed = bossData.speed / 1.5f;
      health -= damageTaken;
      isHit = true;
      DisplayDamage();

      spriteRend.material = hitMaterial;
      if (health <= bossData.health / 2)
      {
        if (!legsBroken)
        {
          BreakLegs();
          legsBroken = true;
        }

        AudioManager.current.currentSFXTrack = 125;
        AudioManager.current.PlaySfx();
      }
      if (health <= 0.0f)
      {
        Death();
        speed = 0;
      }
    }
  }
  public void RecoverFromHit()
  {
    speed = bossData.speed;
    isHit = false;
    spriteRend.material = spriteMaterial;
    damageTaken = 0.0f;
  }

  public void DisplayDamage()
  {
    if (GameController.current.damageNumOption)
    {
      dmgNumPos = currentBossObject.transform.position;
      GameObject newDamageNum = Instantiate(damageNum, new Vector3(dmgNumPos.x, dmgNumPos.y + 0.75f, dmgNumPos.z), Quaternion.identity) as GameObject;
      GameObject canvasObject = GameObject.Find("WorldCanvas");
      newDamageNum.transform.SetParent(canvasObject.transform);
      DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
      damageNumScript.damageNum = Mathf.RoundToInt(damageTaken);
      damageNumScript.DamageInit();
    }
  }

  public void Death()
  {
    isDead = true;
    explodeParticles = Instantiate(explodeParticlesObject, new Vector3(currentBossObject.transform.position.x, currentBossObject.transform.position.y, currentBossObject.transform.position.z), Quaternion.identity) as GameObject;
    speed = 0;
    anim.SetBool("isDead", true);
    Invoke("DestroySelf", 1.0f);

    Time.timeScale = .5f;

    AudioManager.current.currentSFXTrack = 126;
    AudioManager.current.PlaySfx();
  }
  public void DestroySelf()
  {
    GameObject deadMantis = Instantiate(deadObject, new Vector3(currentBossObject.transform.position.x, currentBossObject.transform.position.y, currentBossObject.transform.position.z), Quaternion.identity) as GameObject;

    Destroy(gameObject);
    MenuManager.current.CreditsScene();
  }
}
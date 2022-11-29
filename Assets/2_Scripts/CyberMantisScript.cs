using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberMantisScript : MonoBehaviour
{
    private GameObject gameController;
    public BossScriptableObject bossData;

    private AudioSource audio;
    public AudioClip fallingClip;
    public AudioClip activateClip;
    public AudioClip deathClip;
    public AudioClip explodeClip;

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

    public GameObject attackColliderObject;
    public BoxCollider attackCollider;

    public bool legsBroken = false;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
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

    void FixedUpdate() {
        if(isHit){
            if(tintFadeSpeed > 0.0f){
                tintFadeSpeed -= Time.deltaTime;
            }
            else{
                tintFadeSpeed = 0.3f;
                RecoverFromHit();
            }
        }

        if(isActive){
            if(idle){
                if(transform.position.x > playerObject.transform.position.x){
                    transform.localScale = new Vector3(1,1,1);
                }
                else{
                    transform.localScale = new Vector3(-1,1,1);
                }

                distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

                playerPosition = playerObject.transform.position;

                transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
            }
            if(chasingPlayer){
                if(transform.position.x > playerObject.transform.position.x){
                    transform.localScale = new Vector3(1,1,1);
                }
                else{
                    transform.localScale = new Vector3(-1,1,1);
                }

                distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

                playerPosition = playerObject.transform.position;

                transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);

                if(distanceToPlayer < 2.2f){
                    AttackPlayer();
                    speed = bossData.attackSpeed;
                }
                else if(distanceToPlayer > 6.0f){
                    ChasePlayer();
                    speed = bossData.attackSpeed * 2;
                }
                else{
                    ChasePlayer();
                }
            }
        }
    }

    public void FindPlayer(){
        playerObject = PlayerManager.current.currentPlayerObject;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    public void IdleState(){
        idle = true;
        chasingPlayer = false;
        anim.SetBool("isIdle", true);
        anim.SetBool("isWalking", false);
    }
    public void ChasePlayer(){
        FindPlayer();
        idle = false;
        chasingPlayer = true;
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", true);
    }
    public void AttackPlayer(){
        anim.SetBool("isAttacking", true);
    }
    public void BreakLegs(){
        audio.clip = explodeClip;
        audio.Play();
        anim.SetBool("isIdle", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("legsBreak", true);
        cyberMantis_body.SetActive(true);
        currentBossObject = cyberMantis_body;
        anim = cyberMantis_body.GetComponent<Animator>();

        attackCollider.center = new Vector3(-0.75f, -1f, 0f);

        explodeParticles = Instantiate(explodeParticlesObject, new Vector3(transform.position.x,transform.position.y, transform.position.z), Quaternion.identity) as GameObject;

        spriteRend = cyberMantis_body.GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRend.material;
        FindPlayer();
    }

    public void DestroyOldSelf(){
        
    }

    public void BeginFalling(){
        rb.useGravity = true;
        audio.clip = fallingClip;
        audio.Play();
    }

    public void DoneFalling(){
        anim.SetBool("doneFalling", true);
        audio.clip = activateClip;
        audio.Play();
    }
    public void Active(){
        isActive = true;
        GameController.current.Invoke("BossFightStarted", 0.1f);
        PlayerManager.current.Invoke("ResumeMovement", 0.01f);
    }
    public void Inactive(){
        isActive = false;
    }

    public void EnableAttackCollider(){
        attackCollider = attackColliderObject.GetComponent<BoxCollider>();
        attackCollider.enabled = true;
    }
    public void DisableAttackCollider(){
        attackCollider = attackColliderObject.GetComponent<BoxCollider>();
        attackCollider.enabled = false;
    }


    public void TakeHit(){
        speed = bossData.speed / 2;
        health -= damageTaken;
        isHit = true;
        spriteRend.material = hitMaterial;
        if(health <= bossData.health / 2){
            if(!legsBroken){
                health += 30.0f;
                BreakLegs();
                legsBroken = true;
            }
        }
        if(health <= 0.0f){
            Death();
        }
    }
    public void RecoverFromHit(){
        speed = bossData.speed;
        isHit = false;
        spriteRend.material = spriteMaterial;
        damageTaken = 0.0f;
    }

    public void Death(){
        audio.clip = deathClip;
        audio.Play();
        explodeParticles = Instantiate(explodeParticlesObject, new Vector3(transform.position.x,transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
        speed = 0;
        anim.SetBool("isDead", true);
        Destroy(detectionTrigger);
        Destroy(attackColliderObject);
    }
}
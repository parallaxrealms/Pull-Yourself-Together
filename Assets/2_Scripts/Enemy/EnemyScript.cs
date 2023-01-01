using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private GameObject gameController;
    public CreatureScriptableObject enemyData;

    [SerializeField] private GameObject damageNum;
    private DamageNum damageNumScript;
    private Vector3 dmgNumPos;

    public string enemyName;

    public GameObject detectionObject;
    public EnemyDetectScript detectionScript;
    public SphereCollider triggerDetectSphere;

    public BoxCollider collider;

    public GameObject attackTriggerObject;
    public BoxCollider attackCollider;

    private SpriteRenderer rend;

    private Animator anim;

    public GameObject player;
    public GameObject playerObject;

    public Vector3 playerPosition;
    public float distanceToPlayer = 0.0f;

    public bool isActive = true;
    public bool idle = true;
    public bool chasingPlayer = false;
    public bool movingAway = false;

    private float tintFadeSpeed = 0.3f;
    private bool isHit = false;
    public SpriteRenderer spriteRend;
    public Material spriteMaterial;
    public Material hitMaterial;

    public bool resetDetection = false;

    public float health;
    public float damageTaken;

    public float speed;
    public float idleSpeed;
    public float attackSpeed;
    public float moveAwaySpeed;

    public float aggroResetTimer;
    public float detectionRadius;

    void Awake(){
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
        spriteRend = GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRend.material;
        detectionScript = detectionObject.GetComponent<EnemyDetectScript>();
        triggerDetectSphere = detectionObject.GetComponent<SphereCollider>();
        attackCollider = attackTriggerObject.GetComponent<BoxCollider>();

        health = enemyData.health;
        speed = enemyData.speed;
        attackSpeed = enemyData.attackSpeed;
        moveAwaySpeed = enemyData.moveAwaySpeed;
        aggroResetTimer = enemyData.aggroResetTimer;
        detectionRadius = enemyData.detectionRadius;
        idleSpeed = speed;

        //Change detection collider radius to
        triggerDetectSphere.radius = detectionRadius;

        if(enemyName == "Worm" || enemyName == "Green Worm"){
            GameController.current.ListWorms.Add(gameObject);
        }
        if(enemyName == "Buzzer"  || enemyName == "Red Buzzer"){
            GameController.current.ListBuzzers.Add(gameObject);
        }

        if(PlayerManager.current.hasHead){
            playerObject = PlayerManager.current.currentPlayerObject;
        }
    }
    // Start is called before the first frame update
    void Start()
    {  
        
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
            if(playerObject != null){
                if(idle){
                    
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

                    if(distanceToPlayer < 3.0f){
                        anim.SetBool("isAttacking", true);
                        attackCollider.enabled = true;
                    }
                    else{
                        anim.SetBool("isAttacking", false);
                        attackCollider.enabled = false;
                    }
                }
                if(movingAway){
                    if(transform.position.x > playerObject.transform.position.x){
                        transform.localScale = new Vector3(-1,1,1);
                    }
                    else{
                        transform.localScale = new Vector3(1,1,1);
                    }

                    distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);

                    playerPosition = playerObject.transform.position;
                    transform.position = Vector3.MoveTowards(transform.position, playerPosition,  -1 * speed * Time.deltaTime);

                    anim.SetBool("isAttacking", false);
                    attackCollider.enabled = false;

                    if(aggroResetTimer > 0){
                        aggroResetTimer -= Time.deltaTime;
                    }
                    else{
                        ChasePlayer();
                        aggroResetTimer = enemyData.aggroResetTimer;
                    }
                }
            }
        }
    }

    public void AggroReset(){
        FindPlayer();
        MoveAwayFromPlayer();
    }

    public void FindPlayer(){
        Active();
        playerObject = PlayerManager.current.currentPlayerObject;
    }

    //AI States
    public void Inactive(){
        isActive = false;
    }
    public void Active(){
        isActive = true;
    }
    public void IdleState(){
        idle = true;
        chasingPlayer = false;
        movingAway = false;
        speed = idleSpeed;
    }
    public void ChasePlayer(){
        if(PlayerManager.current.hasHead){
            idle = false;
            chasingPlayer = true;
            movingAway = false;
            speed = attackSpeed;
        }
    }
    public void MoveAwayFromPlayer(){
        if(PlayerManager.current.hasHead){
            idle = false;
            chasingPlayer = false;
            movingAway = true;
            speed = moveAwaySpeed;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player_Bullet"){
            if(!isHit){
                BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
                damageTaken = bulletScript.damage;
                TakeHit();
            }
        }
        if(other.gameObject.tag == "PlayerDrill"){
            if(!isHit){
                damageTaken = 1;
                TakeHit();
            }
        }
        if(other.gameObject.tag == "Player_Electro"){
            if(!isHit){
                BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
                damageTaken = bulletScript.damage;
                TakeHit();
            }
        }
        if(other.gameObject.tag == "MissileAOE"){
            if(!isHit){
                MissileAOE missileAOEscript = other.gameObject.GetComponent<MissileAOE>();
                damageTaken = missileAOEscript.damage;
                TakeHit();
            }
        }
    }

    public void TakeHit(){
        speed = enemyData.speed / 2;
        health -= damageTaken;
        isHit = true;
        spriteRend.material = hitMaterial;
        if(health <= 0.0f){
            Death();
        }
        DisplayDamage();
    }
    public void RecoverFromHit(){
        speed = enemyData.speed;
        isHit = false;
        spriteRend.material = spriteMaterial;
        damageTaken = 0.0f;
    }

    public void DisplayDamage(){
        if(GameController.current.damageNumOption){
            dmgNumPos = transform.position;
            GameObject newDamageNum = Instantiate(damageNum, new Vector3(dmgNumPos.x, dmgNumPos.y + 0.5f, dmgNumPos.z), Quaternion.identity) as GameObject;
            GameObject canvasObject = GameObject.Find("WorldCanvas");
            newDamageNum.transform.SetParent(canvasObject.transform);
            DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
            damageNumScript.damageNum = Mathf.RoundToInt(damageTaken);
        }
    }

    public void Death(){
        anim.SetBool("isDead", true);
        speed = 0;
        collider.enabled = false;
        attackCollider.enabled = false;
    }

    public void HitSelf(){
        anim.SetBool("isHit", true);
    }

    public void DestroySelf(){
        if(enemyName == "Worm" || enemyName == "Green Worm"){
            GameController.current.ListWorms.Remove(gameObject);
        }
        if(enemyName == "Buzzer" || enemyName == "Red Buzzer"){
            GameController.current.ListBuzzers.Remove(gameObject);
        }
        Destroy(gameObject);
    }
    
}

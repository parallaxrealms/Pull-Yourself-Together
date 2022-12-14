using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public SpawnerScriptableObject spawnerData;

    [SerializeField] private GameObject damageNum;
    private DamageNum damageNumScript;
    private Vector3 dmgNumPos;
    private int damageTaken;

    public int spawnerType = 0; //0 = Ground, 1 = Hanging/Single

    public GameObject enemyToSpawn;
    public GameObject openParticles;
    public GameObject deathParticles;
    public Animator anim;

    public SphereCollider collider;
    public UnityEngine.Rendering.Universal.Light2D light;

    public float health;
    
    public bool activated = false;
    public bool enemySpawned = false;
    public float spawnRateTimer;
    public float spawnRate;

    private float tintFadeSpeed = 0.1f;
    private bool isHit = false;
    private bool isDestroyed = false;
    public SpriteRenderer spriteRend;
    public Material spriteMaterial;
    public Material hitMaterial;

    public int maxEnemiesToSpawn = 12;
    public int enemiesSpawned = 0;

    private float cooldownTimer = 1.0f;
    private bool drillCooldown = false;

    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRend.material;
        spawnerType = spawnerData.spawnerType;
        spawnRateTimer = spawnerData.spawnRate;
        spawnRate = spawnRateTimer;

        health = spawnerData.health;

        if(spawnerType == 1){
            collider = GetComponent<SphereCollider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(drillCooldown){
            if(cooldownTimer > 0){
                cooldownTimer -= Time.deltaTime;
            }
            else{
                drillCooldown = false;
                cooldownTimer = 1.0f;
            }
        }

        if(health <= 0 ){
            DestroySelf();
        }
    }

    private void FixedUpdate() {
        if(spawnerType == 0){
            if(activated){
                if(spawnRate > 0){
                    spawnRate -= Time.deltaTime;
                }
                else{
                    if(enemiesSpawned < maxEnemiesToSpawn){
                        SpawnEnemy(enemyToSpawn);
                        spawnRate = spawnRateTimer;
                    }
                }
            }
        }
        else{
            if(activated){
                if(!enemySpawned){
                    SpawnEnemy(enemyToSpawn);
                }
            }
        }

        if(isHit){
            if(tintFadeSpeed > 0.0f){
                tintFadeSpeed -= Time.deltaTime;
            }
            else{
                tintFadeSpeed = 0.1f;
                RecoverFromHit();
            }
        }
    }

    private void SpawnEnemy(GameObject enemy){
        if(spawnerType == 1){
            DestroyEggSack();
        }
        enemiesSpawned++;
        GameObject newEnemy = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        enemySpawned = true;
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player_Bullet"){
            if(!isHit){
                BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();

                TakeHit(bulletScript.damage);
            }
        }
        if(other.gameObject.tag == "MissileAOE"){
            if(!isHit){
                MissileAOE missileAOEscript = other.gameObject.GetComponent<MissileAOE>();

                TakeHit(missileAOEscript.damage);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "PlayerDrill"){
            if(!drillCooldown){
                TakeHit(1);
                drillCooldown = true;
            }
        }
    }

    private void DestroySelf(){
        if(!isDestroyed){
            if(spawnerType == 1){
                DestroyEggSack();
            }
            else{
                GameObject deathParticle = Instantiate(deathParticles, new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity) as GameObject;
                GameObject openParticle = Instantiate(openParticles, new Vector3(transform.position.x,transform.position.y + 0.4f,transform.position.z),Quaternion.identity) as GameObject;
                Destroy(gameObject);
                Destroy(light);
            }
            isDestroyed = true;
        }
    }

    public void DestroyEggSack(){
        if(!enemySpawned){
            anim.SetBool("BreakOpen", true);
            collider.enabled = false;
            GameObject deathParticle = Instantiate(deathParticles, new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity) as GameObject;
            GameObject openParticle = Instantiate(openParticles, new Vector3(transform.position.x,transform.position.y + 0.4f,transform.position.z),Quaternion.identity) as GameObject;
            enemySpawned = true;
            Destroy(light);
        }
    }

    public void TakeHit(float amountOfDMG){
        isHit = true;
        health -= amountOfDMG;
        spriteRend.material = hitMaterial;
        damageTaken = Mathf.RoundToInt(amountOfDMG);
        DisplayDamage();
    }
    public void RecoverFromHit(){
        isHit = false;
        spriteRend.material = spriteMaterial;
    }

    public void DisplayDamage(){
        if(GameController.current.damageNumOption){
            dmgNumPos = transform.position;
            GameObject newDamageNum = Instantiate(damageNum, new Vector3(dmgNumPos.x, dmgNumPos.y, dmgNumPos.z), Quaternion.identity) as GameObject;
            GameObject canvasObject = GameObject.Find("WorldCanvas");
            newDamageNum.transform.SetParent(canvasObject.transform);
            DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
            damageNumScript.damageNum = damageTaken;
        }
    }
}

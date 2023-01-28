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
    public GameObject hangingParticles;
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

        activated = false;
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
            AudioManager.current.currentSFXTrack = 100;
            AudioManager.current.PlaySfx();
        }
        else{
            AudioManager.current.currentSFXTrack = 104;
            AudioManager.current.PlaySfx();
        }
        enemiesSpawned++;
        GameObject newEnemy = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        enemySpawned = true;
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player_Bullet"){
            if(!isHit){
                BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
                TakeHit(PlayerManager.current.currentDamage_blaster);
            }
        }
        if(other.gameObject.tag == "MissileAOE"){
            if(!isHit){
                MissileAOE missileAOEscript = other.gameObject.GetComponent<MissileAOE>();
                TakeHit(PlayerManager.current.currentDamage_missile);
            }
        }
        if(other.gameObject.tag == "Player_EnergyBeam"){
            if(!isHit){
                BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
                TakeHit(PlayerManager.current.currentDamage_energyBeam);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "PlayerDrill"){
            if(!drillCooldown){
                TakeHit(PlayerManager.current.currentDamage_workerDrill);
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
                AudioManager.current.currentSFXTrack = 112;
                AudioManager.current.PlaySfx();
            }
            isDestroyed = true;
        }
    }

    public void DestroyEggSack(){
        if(!enemySpawned){
            Destroy(hangingParticles);
            anim.SetBool("BreakOpen", true);
            collider.enabled = false;
            GameObject deathParticle = Instantiate(deathParticles, new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity) as GameObject;
            GameObject openParticle = Instantiate(openParticles, new Vector3(transform.position.x,transform.position.y + 0.4f,transform.position.z),Quaternion.identity) as GameObject;
            enemySpawned = true;
            Destroy(light);

            AudioManager.current.currentSFXTrack = 102;
            AudioManager.current.PlaySfx();
        }
    }

    public void TakeHit(float amountOfDMG){
        isHit = true;
        health -= amountOfDMG;
        spriteRend.material = hitMaterial;
        damageTaken = Mathf.RoundToInt(amountOfDMG);
        DisplayDamage();
        AudioManager.current.currentSFXTrack = 110;
        AudioManager.current.PlaySfx();
    }
    public void RecoverFromHit(){
        isHit = false;
        spriteRend.material = spriteMaterial;
    }

    public void DisplayDamage(){
        if(GameController.current.damageNumOption){
            dmgNumPos = transform.position;
            GameObject newDamageNum = Instantiate(damageNum, new Vector3(dmgNumPos.x, dmgNumPos.y, dmgNumPos.z), Quaternion.identity) as GameObject;
            DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
            float damageTakenFl = (float)damageTaken;
            damageNumScript.damageNum = damageTakenFl;
            GameObject canvasObject = GameObject.Find("WorldCanvas");
            newDamageNum.transform.SetParent(canvasObject.transform);
            damageNumScript.DamageInit();
        }
    }
}

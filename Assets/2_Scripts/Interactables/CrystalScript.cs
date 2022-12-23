using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    public CrystalScriptableObject crystalData;

    [SerializeField] private GameObject damageNum;
    private DamageNum damageNumScript;

    public int size = 0;//0 = small, 1 = large
    public int numOfChunks = 0;

    public Vector3 scenePos;

    public PlayerControl controlScript;

    public int _type;
    public float _health;
    
    private float cooldownTimer = 1.0f;
    private bool drillCooldown = false;

    public SpriteRenderer spriteRend;
    public CapsuleCollider collider;

    public GameObject crystalHitParticle;
    public GameObject crystalChunkObject;

    public GameObject chunkSpawnOrigin;
    public Vector3 chunkSpawnPos;

    public Animator anim;

    public Vector3 drillPos;

    public float chunkSpawnTimer = 0.1f;
    public bool chunkSpawned = false;
    
    void Awake(){
        GameController.current.ListCrystals.Add(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _type = crystalData.type;
        _health = crystalData.health;

        spriteRend = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider>();

        chunkSpawnPos = new Vector3(chunkSpawnOrigin.transform.position.x,chunkSpawnOrigin.transform.position.y,chunkSpawnOrigin.transform.position.z);

        if(size == 0){
            numOfChunks = 3;
        }
        else if(size == 1){
            numOfChunks = 6;
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
                anim.SetBool("Hit", false);
            }
        }

        if(chunkSpawned){
            if(numOfChunks >= 1){
                if(chunkSpawnTimer > 0){
                    chunkSpawnTimer -= Time.deltaTime;
                }
                else{
                    SpawnChunks();
                    chunkSpawnTimer = 0.1f;
                }
            }
            else{
                chunkSpawned = false;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "PlayerDrill"){
            if(!drillCooldown){
                controlScript = PlayerManager.current.currentPlayerObject.GetComponent<PlayerControl>();

                drillPos = other.gameObject.transform.position;
                TakeHit();
            }
        }
    }

    public void TakeHit(){
        if(controlScript.facingLeft){
            drillPos = new Vector3(drillPos.x - 1.0f, drillPos.y, drillPos.z);
        }
        else{
            drillPos = new Vector3(drillPos.x + 1.0f, drillPos.y, drillPos.z);
        }

        if(_health > 1){
            GameObject hitParticles = Instantiate(crystalHitParticle, new Vector3(drillPos.x, drillPos.y, drillPos.z), Quaternion.identity) as GameObject;
            drillCooldown = true;
            _health--;
            anim.SetBool("Hit", true);

            DisplayDamage();
        }
        else if(_health == 1){
            DisplayDamage();
            CrystalDestroyed();
            GameObject hitParticles = Instantiate(crystalHitParticle, new Vector3(drillPos.x, drillPos.y, drillPos.z), Quaternion.identity) as GameObject;
            drillCooldown = true;
        }
    }

    public void DisplayDamage(){
        GameObject newDamageNum = Instantiate(damageNum, new Vector3(drillPos.x, drillPos.y + 0.5f, drillPos.z), Quaternion.identity) as GameObject;
        GameObject canvasObject = GameObject.Find("WorldCanvas");
        newDamageNum.transform.SetParent(canvasObject.transform);
        DamageNum damageNumScript = newDamageNum.GetComponent<DamageNum>();
        damageNumScript.damageNum = 1;
    }

    public void SetScenePos(){
        scenePos = transform.position;
    }

    private void SpawnChunks(){
        numOfChunks -= 1;
        GameObject crystalChunk = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
    }

    private void CrystalDestroyed(){
        anim.SetBool("Break", true);
        chunkSpawned = true;
        collider.enabled = false;
    }

    public void RemoveFromList(){
        GameController.current.ListCrystals.Remove(gameObject);
    }

    private void DestroySelf(){
        Destroy(gameObject);
        GameController.current.ListCrystals.Remove(gameObject);
    }
}

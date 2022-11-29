using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    public CrystalScriptableObject crystalData;

    public int size = 0;//0 = small, 1 = large

    public PlayerControl controlScript;

    public int _type;
    public float _health;
    
    private float cooldownTimer = 1.0f;
    private bool drillCooldown = false;

    public GameObject crystalHitParticle;
    public GameObject crystalChunkObject;

    public GameObject chunkSpawnOrigin;
    public Vector3 chunkSpawnPos;

    public Animator anim;

    public Vector3 drillPos;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _type = crystalData.type;
        _health = crystalData.health;

        chunkSpawnPos = new Vector3(chunkSpawnOrigin.transform.position.x,chunkSpawnOrigin.transform.position.y,chunkSpawnOrigin.transform.position.z);
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
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "PlayerDrill"){
            if(!drillCooldown){
                controlScript = PlayerManager.current.currentPlayerObject.GetComponent<PlayerControl>();

                drillPos = other.gameObject.transform.position;
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
                }
                else if(_health == 1){
                    DestroySelf();
                    GameObject hitParticles = Instantiate(crystalHitParticle, new Vector3(drillPos.x, drillPos.y, drillPos.z), Quaternion.identity) as GameObject;
                    drillCooldown = true;
                }
            }
        }
    }

    private void SpawnChunks(){
        if(size == 0){
            GameObject crystalChunk = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk2 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x + 0.5f,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk3 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x - 0.5f,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk4 = Instantiate(crystalChunkObject,new Vector3(chunkSpawnPos.x, chunkSpawnPos.y + 0.5f,chunkSpawnPos.z), Quaternion.identity) as GameObject;
        }
        else if(size == 1){
            GameObject crystalChunk = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk2 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x + 0.5f,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk3 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x - 0.5f,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk4 = Instantiate(crystalChunkObject,new Vector3(chunkSpawnPos.x + 0.25f, chunkSpawnPos.y + 0.5f,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk5 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x - 0.25f,chunkSpawnPos.y,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk6 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x,chunkSpawnPos.y - 0.25f,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk7 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x,chunkSpawnPos.y + 0.25f,chunkSpawnPos.z), Quaternion.identity) as GameObject;
            GameObject crystalChunk8 = Instantiate(crystalChunkObject, new Vector3(chunkSpawnPos.x - 0.25f,chunkSpawnPos.y -0.25f,chunkSpawnPos.z), Quaternion.identity) as GameObject;
        }
    }
    private void DestroySelf(){
        SpawnChunks();
        Destroy(gameObject);
    }
}

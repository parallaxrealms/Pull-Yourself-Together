using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitTrigger : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject parentParent;

    public EnemyScript enemyScript;
    public LostBotScript lostBotScript;
    public CyberMantisScript bossScript;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentParent = parentObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy_Bullet"){
            if(!PlayerManager.current.isHit && !PlayerManager.current.isDead){
                EnemyBulletPhysics eBulletPhysics = other.gameObject.GetComponent<EnemyBulletPhysics>();
                if(eBulletPhysics.bulletType == 1){
                    PlayerManager.current.TakeHardHit();
                }
                else{
                    PlayerManager.current.TakeHit();
                }
            }
        }
        if(other.gameObject.tag == "MissileAOE"){
            if(!PlayerManager.current.isHit && !PlayerManager.current.isDead){
                PlayerManager.current.TakeHitLimbs();
            }
        }

        if(other.gameObject.tag == "BossHitCollision"){
            if(!PlayerManager.current.isHit && !PlayerManager.current.isDead){
                bossScript = other.gameObject.transform.parent.gameObject.GetComponent<CyberMantisScript>();
                bossScript.attackCollider.enabled = false;
                bossScript.FindPlayer();
                PlayerManager.current.TakeHit();
            }
        }

        if(other.gameObject.name == "BossTrigger"){
            GameObject blueBot = GameObject.Find("BlueBot");
            BlueBotScript blueBotScript = blueBot.GetComponent<BlueBotScript>();

            blueBotScript.ActivateDialogue();
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Enemy_Hit"){
            PlayerManager.current.TakeHit();
        }

        if(other.gameObject.tag == "HitCollision"){
            if(!PlayerManager.current.isHit && !PlayerManager.current.isDead){
                enemyScript = other.gameObject.transform.parent.gameObject.GetComponent<EnemyScript>();
                enemyScript.attackCollider.enabled = false;
                enemyScript.AggroReset();
                PlayerManager.current.TakeHit();
            }
        }
    }
}
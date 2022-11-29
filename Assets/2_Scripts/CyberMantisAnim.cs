using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberMantisAnim : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip crashSound;
    public AudioClip attackSound;
    
    public GameObject parentObject;
    public CyberMantisScript parentScript;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<CyberMantisScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Active(){
        parentScript.Invoke("Active",0.1f);
    }

    public void CrashSound(){
        audio.clip = crashSound;
        audio.Play();
    }

    public void EnableAttackCollider(){
        audio.clip = attackSound;
        audio.Play();
        parentScript.Invoke("EnableAttackCollider",0.1f);
    }
    public void DisableAttackCollider(){
        parentScript.Invoke("DisableAttackCollider",0.1f);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player_Bullet"){
            if(!parentScript.isHit){
                if(!parentScript.isDead){
                    BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
                    parentScript.damageTaken = bulletScript.damage;
                    parentScript.Invoke("TakeHit", 0.1f);
                }
            }
        }
    }


    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "PlayerDrill"){
            if(!parentScript.isHit){
                if(!parentScript.isDead){
                    DrillArmScript drillArmScript = other.gameObject.GetComponent<DrillArmScript>();
                    parentScript.damageTaken = drillArmScript.damage;
                    parentScript.Invoke("TakeHit", 0.1f);
                }
            }
        }
    }


    public void DestroySelf(){
        Destroy(gameObject);
    }
}

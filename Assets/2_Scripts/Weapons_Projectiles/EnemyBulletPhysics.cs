using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPhysics : MonoBehaviour
{
    public PlayerWeaponScriptableObject defaultGunValues;
    
    public SphereCollider collider;
    public BoxCollider collider_missile;

    public Animator anim;

    public int bulletType;

    public float _speed;
    public float damage;
    private bool hit = false;

    public Vector3 bulletDirection;
    public GameObject player;
    public GameObject playerObject;
    private Vector3 playerPos;


    public Vector3 lastHitPos;

    public GameObject bulletImpactParticles;
    public GameObject bulletImpactParticles_white;
    public GameObject missileAOE;
    public GameObject missileHitAOE;
    public SphereCollider missileHitCollider;
    public GameObject missileImpactParticles;
    public GameObject laserHit;

    private Rigidbody rb;

    public float _lifespanTimer;

    private float destroyTimer = 0.1f;
    private bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = PlayerManager.current.currentPlayerObject;

        playerPos = playerObject.transform.position;

         _speed = defaultGunValues.bulletSpeed / 2;
         _lifespanTimer = defaultGunValues.lifespanTime;
         bulletType = defaultGunValues.bulletType;

        rb = GetComponent<Rigidbody>();
        rb.AddForce(bulletDirection * _speed);

        damage = defaultGunValues.damageAmount;

        if(bulletType == 1){
            collider_missile = GetComponent<BoxCollider>();
            anim = GetComponent<Animator>();
            
            transform.rotation = Quaternion.LookRotation(Vector3.forward, playerPos - transform.position);
            transform.rotation *= Quaternion.Euler(Vector3.forward * 90);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_lifespanTimer > 0.0f){
            _lifespanTimer -= Time.deltaTime;
        }
        else{
            DestroySelf();
        }
    }
        private void OnTriggerEnter(Collider other) {  
        if(other.gameObject.tag == "Ground"){
            lastHitPos = other.gameObject.transform.position;
            if(bulletType == 0){
                BlasterBulletCollision();   
            }
            else if(bulletType == 1){
                MissileLauncherCollision();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, lastHitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "BulletCollision"){
            lastHitPos = other.gameObject.transform.position;
            if(bulletType == 0){
                BlasterBulletCollision();   
            }
            else if(bulletType == 1){
                MissileLauncherCollision();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, lastHitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "Player"){
            lastHitPos = other.gameObject.transform.position;
            if(bulletType == 0){
                BlasterBulletCollision();   
            }
            else if(bulletType == 1){
                MissileLauncherCollision();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, lastHitPos, Quaternion.identity) as GameObject;
            }
        }
    }

    public void MissileLauncherCollision(){
        EnableAOECollider();
        rb.velocity = Vector3.zero;
        collider_missile.enabled = false;
        GameObject impactParticles = Instantiate(missileImpactParticles, lastHitPos, Quaternion.identity) as GameObject;
        anim.SetBool("isHit", true);
    }

    public void BlasterBulletCollision(){
        //play blaster sound
        GameObject impactParticles = Instantiate(bulletImpactParticles, lastHitPos, Quaternion.identity) as GameObject;
        DestroySelf();  
    }

    public void EnableAOECollider(){
        missileAOE = Instantiate(missileHitAOE, lastHitPos, Quaternion.identity) as GameObject;
        MissileAOE missileAOEscript = missileAOE.GetComponent<MissileAOE>();
        missileAOEscript.damage = damage;
    }

    public void DisableAOECollider(){
        missileHitCollider.enabled = false;
    }
    
    public void DestroySelf(){
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPhysics : MonoBehaviour
{
    public PlayerWeaponScriptableObject defaultGunValues;

    public AudioSource audio;
    public AudioClip clip_missileExplode;
    public AudioClip clip_hitImpactSound;


    public Animator anim;

    public int bulletType;

    public float _speed;
    public float damage;
    private bool hit = false;

    public Vector3 bulletDirection;
    public GameObject player;
    public GameObject playerObject;
    private Vector3 playerPos;

    public GameObject bulletImpactParticles;
    public GameObject bulletImpactParticles_white;
    public GameObject missileAOE;
    public GameObject missileHitAOE;
    public SphereCollider missileHitCollider;
    public GameObject missileImpactParticles;
    public GameObject laserHit;

    private Rigidbody rb;

    public float _lifespanTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = PlayerManager.current.currentPlayerObject;

        playerPos = playerObject.transform.position;

         _speed = defaultGunValues.bulletSpeed;
         _lifespanTimer = defaultGunValues.lifespanTime;
         bulletType = defaultGunValues.bulletType;

        rb = GetComponent<Rigidbody>();
        rb.AddForce(bulletDirection * _speed);

        damage = defaultGunValues.damageAmount;

        if(bulletType == 1){
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

    public void OnTriggerEnter(Collider other){
        Vector3 hitPos = other.gameObject.transform.position;
        if(other.gameObject.tag == "Ground"){
            if(bulletType == 0){
                GameObject impactParticles = Instantiate(bulletImpactParticles, hitPos, Quaternion.identity) as GameObject;
                DestroySelf();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "BulletCollision"){
            if(bulletType == 0){
                GameObject impactParticles_white = Instantiate(bulletImpactParticles_white, hitPos, Quaternion.identity) as GameObject;
                DestroySelf();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "Player"){
            if(bulletType == 0){
                GameObject impactParticles_white = Instantiate(bulletImpactParticles_white, hitPos, Quaternion.identity) as GameObject;
                DestroySelf();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
    }

     public void OnTriggerExit(Collider other){

    }

    public void DisableAOECollider(){
        missileHitCollider.enabled = false;
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}

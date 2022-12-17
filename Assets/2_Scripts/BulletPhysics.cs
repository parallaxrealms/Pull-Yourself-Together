using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    public PlayerWeaponScriptableObject defaultGunValues;

    public Animator anim;
    public SpriteRenderer spriteRend;

    public Vector3 lastHitPos;

    public SphereCollider collider;

    public int bulletType;

    public float _speed;
    public float damage;
    private bool hit = false;

    public Vector3 bulletDirection;
    private Vector3 mousePos;

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
        _speed = defaultGunValues.bulletSpeed / 2;
        _lifespanTimer = defaultGunValues.lifespanTime;
        bulletType = defaultGunValues.bulletType;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(bulletDirection * _speed);

        damage = defaultGunValues.damageAmount;
        if(bulletType == 0){
            collider = GetComponent<SphereCollider>();
            spriteRend = GetComponent<SpriteRenderer>();
        }
        if(bulletType == 1){
            anim = GetComponent<Animator>();
            
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
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
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, lastHitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, lastHitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;
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
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, lastHitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, lastHitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, lastHitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "Spawner"){
            lastHitPos = other.gameObject.transform.position;
             if(bulletType == 0){
                BlasterBulletCollision();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, lastHitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, lastHitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, lastHitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "Enemy"){
            lastHitPos = other.gameObject.transform.position;
            if(bulletType == 0){
                BlasterBulletCollision();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, lastHitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, lastHitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, lastHitPos, Quaternion.identity) as GameObject;
            }
        }
    }

    public void BlasterBulletCollision(){
        GameObject currentPlayerObject = PlayerManager.current.currentPlayerObject;
        PlayerControl playerScript = currentPlayerObject.GetComponent<PlayerControl>();
        playerScript.Invoke("PlayBlasterImpact", 0.01f);
        GameObject impactParticles = Instantiate(bulletImpactParticles, lastHitPos, Quaternion.identity) as GameObject;
        DestroySelf();  
    }

    public void DisableAOECollider(){
        missileHitCollider.enabled = false;
    }
    
    public void DestroySelf(){
        Destroy(gameObject);
    }
}
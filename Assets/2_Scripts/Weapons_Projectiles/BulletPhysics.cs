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
    public BoxCollider collider_missile;

    public int bulletType;

    public float _speed;
    public float damage;
    private bool hit = false;

    public Vector3 bulletDirection;
    private Vector3 mousePos;

    public GameObject bulletImpactParticles;
    public GameObject bulletImpactParticles_white;
    public GameObject missileAOE;
    public GameObject missile_0_AOE;
    public GameObject missile_1_AOE;
    public GameObject missile_2_AOE;
    public GameObject missile_3_AOE;

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

            collider_missile = GetComponent<BoxCollider>();
            
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
        if(other.gameObject.tag == "Spawner"){
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
        if(other.gameObject.tag == "Enemy"){
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
        GameObject impactParticles = Instantiate(missileImpactParticles, transform.position, Quaternion.identity) as GameObject;
        anim.SetBool("isHit", true);
    }

    public void BlasterBulletCollision(){
        AudioManager.current.currentSFXTrack = 0;
        AudioManager.current.PlaySfx();

        GameObject impactParticles = Instantiate(bulletImpactParticles, transform.position, Quaternion.identity) as GameObject;

        DestroySelf();  
    }

    public void EnableAOECollider(){
        if(PlayerManager.current.gunType == 1){
            AudioManager.current.currentSFXTrack = 1;
            AudioManager.current.PlaySfx();

            if(PlayerManager.current.gun_progress1 == 0){
                missileAOE = Instantiate(missile_1_AOE, transform.position, Quaternion.identity) as GameObject;
            }
            else if(PlayerManager.current.gun_progress1 == 1){
                missileAOE = Instantiate(missile_2_AOE, transform.position, Quaternion.identity) as GameObject;
            }
            else if(PlayerManager.current.gun_progress1 == 2){
                missileAOE = Instantiate(missile_3_AOE, transform.position, Quaternion.identity) as GameObject;
            }
        }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip clip_missileExplode;

    public AudioClip clip_hitImpactSound;

    public PlayerWeaponScriptableObject defaultGunValues;

    public Animator anim;
    public SpriteRenderer spriteRend;

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
        audio = GetComponent<AudioSource>();

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
            Destroy(gameObject);
        }

        if(destroyed){
            if(destroyTimer > 0f){
                destroyTimer -= Time.deltaTime;
            }
            else{
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other) {
        Vector3 hitPos = other.gameObject.transform.position;
        if(other.gameObject.tag == "Enemy"){
            if(bulletType == 0){
                spriteRend.enabled = false;
                collider.enabled = false; 
                GameObject impactParticles_white = Instantiate(bulletImpactParticles_white, hitPos, Quaternion.identity) as GameObject;
                DestroySelf();

                audio.clip = clip_hitImpactSound;
                audio.Play();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;

                audio.clip = clip_missileExplode;
                audio.Play();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
    }

    public void OnTriggerEnter(Collider other){
        Vector3 hitPos = other.gameObject.transform.position;
        if(other.gameObject.tag == "Ground"){
            if(bulletType == 0){
                spriteRend.enabled = false;
                collider.enabled = false; 
                GameObject impactParticles = Instantiate(bulletImpactParticles, hitPos, Quaternion.identity) as GameObject;

                audio.clip = clip_hitImpactSound;
                audio.Play();

                DestroySelf();      
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;

                audio.clip = clip_missileExplode;
                audio.Play();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "BulletCollision"){
            if(bulletType == 0){
                spriteRend.enabled = false;
                collider.enabled = false; 
                GameObject impactParticles_white = Instantiate(bulletImpactParticles_white, hitPos, Quaternion.identity) as GameObject;
                DestroySelf();

                audio.clip = clip_hitImpactSound;
                audio.Play();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;

                audio.clip = clip_missileExplode;
                audio.Play();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "Enemy"){
            if(bulletType == 0){
                spriteRend.enabled = false;
                collider.enabled = false; 
                GameObject impactParticles_white = Instantiate(bulletImpactParticles_white, hitPos, Quaternion.identity) as GameObject;
                DestroySelf();

                audio.clip = clip_hitImpactSound;
                audio.Play();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;

                audio.clip = clip_missileExplode;
                audio.Play();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
        if(other.gameObject.tag == "Spawner"){
             if(bulletType == 0){
                spriteRend.enabled = false;
                collider.enabled = false; 
                GameObject impactParticles_white = Instantiate(bulletImpactParticles_white, hitPos, Quaternion.identity) as GameObject;
                DestroySelf();

                audio.clip = clip_hitImpactSound;
                audio.Play();
            }
            else if(bulletType == 1){
                rb.velocity = Vector3.zero;
                anim.SetBool("isHit", true);
                GameObject impactParticles = Instantiate(missileImpactParticles, hitPos, Quaternion.identity) as GameObject;
                missileAOE = Instantiate(missileHitAOE, hitPos, Quaternion.identity) as GameObject;
                missileHitCollider = missileAOE.GetComponent<SphereCollider>();
                missileHitCollider.enabled = false;

                audio.clip = clip_missileExplode;
                audio.Play();
            }
            else if(bulletType == 2){
                GameObject impactParticles = Instantiate(laserHit, hitPos, Quaternion.identity) as GameObject;
            }
        }
    }

    public void OnTriggerExit(Collider other){
        spriteRend.enabled = false;
        collider.enabled = false; 
    }

    public void DisableAOECollider(){
        missileHitCollider.enabled = false;
    }
    
    public void DestroySelf(){
        destroyed = true;
    }
}
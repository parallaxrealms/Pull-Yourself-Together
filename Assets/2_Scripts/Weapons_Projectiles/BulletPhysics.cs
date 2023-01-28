using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{
    public PlayerWeaponScriptableObject defaultGunValues;

    public Animator anim;
    public SpriteRenderer spriteRend;

    public Vector3 lastHitPos;

    public SphereCollider sphereCollider;
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
    public GameObject laserShootParticles;

    public TrailRenderer trailRend;
    public float width_0 = 0.2f;
    public float width_1 = 0.5f;
    public float width_2 = 1.0f;

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

        if (bulletType == 0)
        { //Blaster
            sphereCollider = GetComponent<SphereCollider>();
            spriteRend = GetComponent<SpriteRenderer>();
            damage = PlayerManager.current.currentDamage_blaster;
        }
        if (bulletType == 1)
        { //Missile
            anim = GetComponent<Animator>();

            collider_missile = GetComponent<BoxCollider>();

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
            transform.rotation *= Quaternion.Euler(Vector3.forward * 90);

            damage = PlayerManager.current.currentDamage_missile;
        }
        if (bulletType == 2)
        { //Energy Beam
            damage = PlayerManager.current.currentDamage_energyBeam;

            trailRend = GetComponent<TrailRenderer>();
            if (PlayerManager.current.gunType == 2)
            {
                // Set the width of the trail
                if (PlayerManager.current.gun_progress2 == 0)
                {
                    trailRend.widthCurve = new AnimationCurve(new Keyframe(0, width_0), new Keyframe(1, width_0));
                }
                else if (PlayerManager.current.gun_progress2 == 1)
                {
                    trailRend.widthCurve = new AnimationCurve(new Keyframe(0, width_1), new Keyframe(1, width_1));
                }
                else if (PlayerManager.current.gun_progress2 == 2)
                {
                    trailRend.widthCurve = new AnimationCurve(new Keyframe(0, width_2), new Keyframe(1, width_2));
                }
            }
            //Create Blast Particles
            GameObject laserBlast = Instantiate(laserShootParticles, transform.position, Quaternion.identity) as GameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_lifespanTimer > 0.0f)
        {
            _lifespanTimer -= Time.deltaTime;
        }
        else
        {
            DestroySelf();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            lastHitPos = other.gameObject.transform.position;
            if (bulletType == 0)
            {
                BlasterBulletCollision();
            }
            else if (bulletType == 1)
            {
                MissileLauncherCollision();
            }
            else if (bulletType == 2)
            {
                EnergyBeamCollision();
            }
        }
        if (other.gameObject.tag == "BulletCollision")
        {
            lastHitPos = other.gameObject.transform.position;
            if (bulletType == 0)
            {
                BlasterBulletCollision();
            }
            else if (bulletType == 1)
            {
                MissileLauncherCollision();
            }
            else if (bulletType == 2)
            {
                EnergyBeamCollision();
            }
        }
        if (other.gameObject.tag == "Spawner")
        {
            lastHitPos = other.gameObject.transform.position;
            if (bulletType == 0)
            {
                BlasterBulletCollision();
            }
            else if (bulletType == 1)
            {
                MissileLauncherCollision();
            }
            else if (bulletType == 2)
            {
                EnergyBeamCollision();
            }
        }
        if (other.gameObject.tag == "Enemy")
        {
            lastHitPos = other.gameObject.transform.position;
            if (bulletType == 0)
            {
                BlasterBulletCollision();
            }
            else if (bulletType == 1)
            {
                MissileLauncherCollision();
            }
            else if (bulletType == 2)
            {
                EnergyBeamCollision();
            }
        }
    }

    public void BlasterBulletCollision()
    {
        GameObject impactParticles = Instantiate(bulletImpactParticles_white, transform.position, Quaternion.identity) as GameObject;

        DestroySelf();
        AudioManager.current.currentSFXTrack = 12;
        AudioManager.current.PlaySfx();
    }

    public void MissileLauncherCollision()
    {
        EnableAOECollider();
        rb.velocity = Vector3.zero;
        collider_missile.enabled = false;
        GameObject impactParticles = Instantiate(missileImpactParticles, transform.position, Quaternion.identity) as GameObject;
        anim.SetBool("isHit", true);
    }

    public void EnableAOECollider()
    {
        if (PlayerManager.current.gunType == 1)
        {
            if (PlayerManager.current.gun_progress1 == 0)
            {
                missileAOE = Instantiate(missile_1_AOE, transform.position, Quaternion.identity) as GameObject;
            }
            else if (PlayerManager.current.gun_progress1 == 1)
            {
                missileAOE = Instantiate(missile_2_AOE, transform.position, Quaternion.identity) as GameObject;
            }
            else if (PlayerManager.current.gun_progress1 == 2)
            {
                missileAOE = Instantiate(missile_3_AOE, transform.position, Quaternion.identity) as GameObject;
            }
        }
        if (missileAOE != null)
        {
            MissileAOE missileAOEscript = missileAOE.GetComponent<MissileAOE>();
            missileAOEscript.damage = PlayerManager.current.currentDamage_missile;

            AudioManager.current.currentSFXTrack = 14;
            AudioManager.current.PlaySfx();
        }
    }

    public void DisableAOECollider()
    {
        missileHitCollider.enabled = false;
    }

    public void EnergyBeamCollision()
    {
        GameObject impactParticles = Instantiate(laserHit, lastHitPos, Quaternion.identity) as GameObject;

        AudioManager.current.currentSFXTrack = 16;
        AudioManager.current.PlaySfx();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
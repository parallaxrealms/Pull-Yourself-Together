using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject UI_CrystalManager;
    public UI_CrystalManager crystalManagerScript;
    
    public GameObject spriteAnimatorObject;
    public Animator spriteAnim;
    public SpriteAnimator spriteAnimScript;

    public GroundedCharacterController characterControllerScript;

    private Rigidbody rb;

    private AudioSource audio;
    public AudioClip clip_shoot;
    public AudioClip clip_shootMissile;
    public AudioClip clip_shootLaser;

    private Vector3 mousePos;
    private Vector3 screenPoint;
    private Vector3 direction;

    private float horizontalControls;
    public bool facingLeft = false;
    public bool facingRight = true;

    public bool isHit = true;
    public bool isDead = false;

    public GameObject gunArm;
    public SpriteRenderer gunSpriteRend;
    public Material gunMaterial;
    public GameObject bulletOrigin;
    public WeaponArmScript gunArmScript;

    public GameObject drillArm;
    public SpriteRenderer drillSpriteRend;
    public Material drillMaterial;
    private DrillArmScript drillArmScript;
    public Animator drillAnimator;
    public BoxCollider drillCollider;

    public GameObject drillSmokeObject;
    private ParticleSystem smokeParticles;

    public Quaternion gunStartRotation;
    public Quaternion gunStartRotationLeft;

    public GameObject headToSwitch;
    public GameObject bodyToSwitch;
    public GameObject drillToSwitch;
    public GameObject gunToSwitch;
    public GameObject legsToSwitch;

    public Vector3 bulletSpawnPos;

    public GameObject projectileObject_blaster;
    public GameObject projectileObject_missile;
    public GameObject projectileObject_laser;
    public GameObject projectileObject_electro;

    public bool gunFacing = true;

    public int gunType = 0; //0 = blaster, 1 = missile, 2 = laser, 3 = electro
    public int legType = 0; //0 = Worker bot, 1 = Jump Boots

    public PlayerWeaponScriptableObject blasterValues;
    public PlayerWeaponScriptableObject missileValues;
    public PlayerWeaponScriptableObject laserValues;
    public PlayerWeaponScriptableObject electroValues;

    public Sprite sprite_blasterGun;
    public Sprite sprite_missileLauncher;
    public Sprite sprite_laserBeam;
    public Sprite sprite_electroGrenade;

    public float _gunFireRate;
    public float _gunCooldownTime;

    public float drillCooldownTime = 1.0f;
    public bool canMove = true;
    public bool isChangingScene = false;
    public bool gunReady = true;
    public bool drillReady = true;

    public SpriteRenderer spriteRend;
    public Material spriteMaterial;
    public Material hitMaterial;
    public float tintFadeSpeed = 0.25f;

    public GameObject onHitParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteAnim = spriteAnimatorObject.GetComponent<Animator>();
        spriteAnimScript = spriteAnim.GetComponent<SpriteAnimator>();

        characterControllerScript = GetComponent<GroundedCharacterController>();

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        direction = (Vector3)(Input.mousePosition-screenPoint);
        direction.Normalize();

        if(PlayerManager.current.hasGun){
            gunStartRotation = gunArm.transform.rotation;
            gunStartRotation *= Quaternion.Euler(Vector3.forward * -90);
            gunStartRotationLeft = Quaternion.Euler(0,0,90);
        }
        if(PlayerManager.current.hasDrill){
            smokeParticles = drillSmokeObject.GetComponent<ParticleSystem>();
        }

        if(PlayerManager.current.hasBody){
            InitWeapons();
        }

        audio = GetComponent<AudioSource>();

        spriteRend = spriteAnimatorObject.GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRend.material;

        UI_CrystalManager = MenuManager.current.CrystalManager;
        crystalManagerScript = UI_CrystalManager.GetComponent<UI_CrystalManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead){
            if(canMove){
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                horizontalControls = Input.GetAxisRaw("Horizontal");

                //Get which direction player is facing
                if(horizontalControls == 1.0){
                    facingRight = true;
                    facingLeft = false;
                }
                else if(horizontalControls == -1.0){
                    facingRight = false;
                    facingLeft = true;
                }
                if(PlayerManager.current.hasGun){
                    GunControls();
                }
                if(PlayerManager.current.hasDrill){
                    DrillControls();
                }
            }
        }
        if(isHit){
            if(tintFadeSpeed > 0){
                tintFadeSpeed -= Time.deltaTime;
            }
            else{
                RecoverFromHit();
                tintFadeSpeed = 0.25f;
            }
        }
    }

    void FixedUpdate(){
        if(!isDead){
            CooldownTimer();
        }
    }

    private void GunControls(){
        if(!MenuManager.current.isMouseOver){
            if(Input.GetButton("Fire1") && gunReady){
                gunReady = false;
                if(gunType == 0){
                    FireBlaster();
                }
                else if(gunType == 1){
                    FireMissile();
                }
                else if(gunType == 2){
                    FireLaser();
                }
                else if(gunType == 3){
                    FireElectro();
                }
            }
            if(gunFacing){
                GunArmAim();
            }
            else{
                ResetGunAim();
            }
        }
    }

    private void DrillControls(){
        if(Input.GetButtonDown("Fire2")){
            if(drillReady){
                UseDrill();
            }
        }
        if(Input.GetButtonUp("Fire2")){
            if(drillReady){
                StopDrill();
            }
        }
    }


    private void CooldownTimer(){
        if(_gunCooldownTime > 0.0f){
            _gunCooldownTime -= Time.deltaTime;
        }
        else{
            gunReady = true;
        }

        if(drillCooldownTime > 0.0f){
            drillCooldownTime -= Time.deltaTime;
            drillReady = false;
        }
        else{
            drillReady = true;
        }
    }

    private void InitWeapons(){
        gunSpriteRend = gunArm.GetComponent<SpriteRenderer>();
        drillSpriteRend = drillArm.GetComponent<SpriteRenderer>();
        gunMaterial = gunSpriteRend.material;
        drillMaterial = drillSpriteRend.material;
        if(!PlayerManager.current.hasDrill){
            drillArm.SetActive(false);
        }
        else{
             EnableDrillArm();
        }
        if(!PlayerManager.current.hasGun){
            gunArm.SetActive(false);
        }
        else{
            EnableGunArm();
        }
    }

    public void GunArmAim(){
        gunArm.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        direction = (Vector3)(Input.mousePosition-screenPoint);
        direction.Normalize();
    }

    public void ResetGunAim(){
        if(!facingLeft){
            gunArm.transform.rotation = gunStartRotation;
            direction = new Vector3(1,0,0);
        }
        else{
            gunArm.transform.rotation = gunStartRotationLeft;
            direction = new Vector3(-1,0,0);
        }
    }

    public void EnableDrillArm(){
        smokeParticles = drillSmokeObject.GetComponent<ParticleSystem>();
        drillArm.SetActive(true);
        drillArmScript = drillArm.GetComponent<DrillArmScript>();
        drillCollider = drillArm.GetComponent<BoxCollider>();
        drillAnimator = drillArm.GetComponent<Animator>();
        StopDrill();
    }
    public void DisableDrillArm(){
        drillArm.SetActive(false);
    }

    public void EnableGunArm(){
        if(gunType == 0){
            gunSpriteRend.sprite = sprite_blasterGun;
            _gunFireRate = blasterValues.gunFireRate;
            _gunCooldownTime = blasterValues.gunCooldownTime;
        }
        else if(gunType == 1){
            gunSpriteRend.sprite = sprite_missileLauncher;
            _gunFireRate = missileValues.gunFireRate;
            _gunCooldownTime = missileValues.gunCooldownTime;
        }
        else if(gunType == 2){
            gunSpriteRend.sprite = sprite_laserBeam;
            _gunFireRate = laserValues.gunFireRate;
            _gunCooldownTime = laserValues.gunCooldownTime;
        }
        else if(gunType == 3){
            gunSpriteRend.sprite = sprite_electroGrenade;
            _gunFireRate = electroValues.gunFireRate;
            _gunCooldownTime = electroValues.gunCooldownTime;
        }
        gunArm.SetActive(true);
        gunArmScript = gunArm.GetComponent<WeaponArmScript>();
    }
    public void DisableGunArm(){
        gunArm.SetActive(false);
    }


    private void FireBlaster(){
            bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x,bulletOrigin.transform.position.y + 0.15f, bulletOrigin.transform.position.z);
            GameObject newBullet = Instantiate(projectileObject_blaster, bulletSpawnPos, Quaternion.identity) as GameObject;
            BulletPhysics bulletScript = newBullet.GetComponent<BulletPhysics>();
            bulletScript.bulletDirection = direction;
            _gunCooldownTime = blasterValues.gunFireRate;
            audio.clip = clip_shoot;
            audio.Play();
    }
    private void FireMissile(){
            bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x,bulletOrigin.transform.position.y + 0.15f, bulletOrigin.transform.position.z);
            GameObject newMissile = Instantiate(projectileObject_missile, bulletSpawnPos, Quaternion.identity) as GameObject;
            BulletPhysics bulletScript = newMissile.GetComponent<BulletPhysics>();
            bulletScript.bulletDirection = direction;
            _gunCooldownTime = missileValues.gunFireRate;
            audio.clip = clip_shootMissile;
            audio.Play();
    }
    private void FireLaser(){
            bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x,bulletOrigin.transform.position.y + 0.15f, bulletOrigin.transform.position.z);
            GameObject newBeam = Instantiate(projectileObject_laser, bulletSpawnPos, Quaternion.identity) as GameObject;
            BulletPhysics bulletScript = newBeam.GetComponent<BulletPhysics>();
            bulletScript.bulletDirection = direction;
            _gunCooldownTime = blasterValues.gunFireRate;
            audio.clip = clip_shootLaser;
            audio.Play();
    }
    private void FireElectro(){
            bulletSpawnPos = new Vector3(bulletOrigin.transform.position.x,bulletOrigin.transform.position.y + 0.15f, bulletOrigin.transform.position.z);
            GameObject newGrenade = Instantiate(projectileObject_electro, bulletSpawnPos, Quaternion.identity) as GameObject;
            BulletPhysics bulletScript = newGrenade.GetComponent<BulletPhysics>();
            bulletScript.bulletDirection = direction;
            _gunCooldownTime = electroValues.gunFireRate;
            audio.clip = clip_shoot;
            audio.Play();
    }

    private void UseDrill(){
        drillAnimator.SetBool("Active", true);
        drillCollider.enabled = true;
        smokeParticles.Play();
        drillArmScript.Invoke("DrillOn",0.01f);

    }
    private void StopDrill(){
        drillAnimator.SetBool("Active", false);
        drillCollider.enabled = false;
        smokeParticles.Stop();
        drillArmScript.Invoke("DrillOff",0.01f);
    }

    public void SwitchHead(){

    }
    public void SwitchBody(){
        
    }
    public void SwitchDrill(){
        
    }
    public void SwitchGun(){
        
    }
    public void SwitchLegs(){
        
    }

    public void PlayBlasterImpact(){
        if(PlayerManager.current.hasGun){
            WeaponArmScript weaponScript = gunArm.GetComponent<WeaponArmScript>();
            weaponScript.Invoke("PlayBlasterImpact",0.01f);
        }
    }

    public void TakeHit(){
        isHit = true;
        spriteRend.material = hitMaterial;
        
        if(PlayerManager.current.hasGun){
            gunSpriteRend.material = hitMaterial;
        }
        if(PlayerManager.current.hasDrill){
            drillSpriteRend.material = hitMaterial;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);

        float speed = 200.0f;
        rb.isKinematic = false;
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 3, 0);
        rb.AddForce(force * speed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

        GameObject onHitParticle = Instantiate(onHitParticles, new Vector3(transform.position.x, transform.position.y, transform.position.z),Quaternion.identity) as GameObject;
    }


    public void RecoverFromHit(){
        isHit = false;
        spriteRend.material = spriteMaterial;
        if(PlayerManager.current.hasGun){
            gunSpriteRend.material = gunMaterial;
        }
        if(PlayerManager.current.hasDrill){
            drillSpriteRend.material = drillMaterial;
        }
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }

    public void PauseMovement(){
        canMove = false;
        characterControllerScript.enabled = false;
        spriteAnimScript.enabled = false;
    }

    public void ResumeMovement(){
        canMove = true;
        characterControllerScript.enabled = true;
        spriteAnimScript.enabled = true;
    }

    public void ChangeScene(){
        isChangingScene = true;
        canMove = false;
        PlayerManager.current.isChangingScene = true;
        PlayerManager.current.canMove = false;
    }
}

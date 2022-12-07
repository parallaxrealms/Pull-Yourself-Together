using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChunkScript : MonoBehaviour
{
    public GameObject UI_CrystalManager;
    public UI_CrystalManager crystalManagerScript;
    public AudioSource audio;
    public AudioClip clip_pickup;

    public Animator anim;
    public SphereCollider collider;
    public Rigidbody rb;
    public UnityEngine.Rendering.Universal.Light2D light;

    public bool pickedUp = false;
    public string crystalType;

    // Start is called before the first frame update
    void Start()
    {
        UI_CrystalManager = GameObject.Find("CrystalManager");
        crystalManagerScript = UI_CrystalManager.GetComponent<UI_CrystalManager>();
        anim = GetComponent<Animator>();
        collider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        audio = GetComponent<AudioSource>();

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);

        float speed = 20.0f;
        rb.isKinematic = false;
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, 0);
        rb.AddForce(force * speed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            if(!pickedUp){
                PlayerPickup(crystalType);
            }
        }
    }

    public void PlayerPickup(string typeOfCrystal){
        crystalManagerScript.Invoke("ShowCrystalUITemp",0.01f);
        anim.SetBool("Pickup", true);
        pickedUp = true;
        audio.clip = clip_pickup;
        audio.Play();

        if(typeOfCrystal == "Corite"){
            crystalManagerScript.Invoke("GainCorite", 0.01f);
        }
        if(typeOfCrystal == "Nymrite"){
            crystalManagerScript.Invoke("GainNymrite", 0.01f);
        }
        if(typeOfCrystal == "Velrite"){
            crystalManagerScript.Invoke("GainVelrite", 0.01f);
        }
        if(typeOfCrystal == "Zyrite"){
            crystalManagerScript.Invoke("GainZyrite", 0.01f);
        }
    }

    void DestroySelf(){
        Destroy(gameObject);
    }
}

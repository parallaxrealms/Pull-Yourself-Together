using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggeredScript : MonoBehaviour
{

    private AudioSource audio;
    public AudioClip doorOpenSound;

    public int doorType; //0 = normal, 1 = velrite, 2 = nymrite, 3 = zyrite

    public GameObject doorTop;
    public GameObject doorSlide;

    public DoorHitTrigger triggerScript;
    public DoorSliderScript sliderScript;

    public Animation doorTopAnim;
    public BoxCollider doorCollider;

    private ParticleSystem doorParticles;

    public bool isTriggered = false;
    public bool closeTriggered = false;
    public bool staysOpen = false;

    public float doorMoveSpeed = 1.0f;
    public float doorOpenTimer = 5.0f;
    public float doorClosedTimer = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        triggerScript = doorTop.GetComponent<DoorHitTrigger>();
        sliderScript = doorSlide.GetComponent<DoorSliderScript>();
        doorParticles = GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(isTriggered){
            if(doorOpenTimer > 0){
                doorOpenTimer -= Time.deltaTime;
                doorSlide.transform.Translate(new Vector3(0f,doorMoveSpeed,0f) * Time.deltaTime, Space.World);
            }
            else{
                isTriggered = false;
                if(!staysOpen){
                    closeTriggered = true;
                    triggerScript.Invoke("CloseDoor",0.01f);
                }
                doorOpenTimer = 5.0f;
            }
        }

        if(closeTriggered){
            if(doorClosedTimer > 0){
                doorClosedTimer -= Time.deltaTime;
                doorSlide.transform.Translate(new Vector3(0f,-doorMoveSpeed,0f) * Time.deltaTime, Space.World);
            }
            else{
                closeTriggered = false;
                doorClosedTimer = 5.0f;
                triggerScript.Invoke("ResetTrigger",0.01f);
            }
        }
    }

    public void OpenDoor(){
        audio.clip = doorOpenSound;
        audio.Play();
        isTriggered = true;
        doorParticles.Play();
    }
}

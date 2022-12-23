using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillArmScript : MonoBehaviour
{
    private GameObject parentObject;
    private GameObject parentParentObject;
    private PlayerControl parentScript;
    
    public int entityNum;//0= player, 1=enemy
    private AudioSource audio;
    public AudioClip clip_drill;

    private BoxCollider collider;

    public int drillType = 0;
    public float damage = 1f;

    public bool drillOn = false;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        if(entityNum == 0){
            parentObject = transform.parent.gameObject;
            parentParentObject = parentObject.transform.parent.gameObject;
            parentScript = parentParentObject.GetComponent<PlayerControl>();

            collider = GetComponent<BoxCollider>();
            DrillOff();
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void DrillOn(){
        audio.clip = clip_drill;
        audio.loop = true;
        audio.Play();
        if(entityNum == 0){
            if(!drillOn){
                drillOn = true;
                collider.enabled = true;
            }
        }
    }
    public void DrillOff(){
        audio.loop = false;
        audio.Stop();
        if(entityNum == 0){
            drillOn = false;
            collider.enabled = false;
        }
    }
}

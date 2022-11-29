using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillArmScript : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip clip_drill;

    public int drillType = 0;
    public float damage = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrillOn(){
        audio.clip = clip_drill;
        audio.loop = true;
        audio.Play();
    }
    public void DrillOff(){
        audio.loop = false;
        audio.Stop();
    }
}

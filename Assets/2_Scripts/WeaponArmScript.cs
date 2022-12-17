using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArmScript : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip blasterImpactSound;
    public AudioClip missileImpactSound;
    public AudioClip beamImpactSound;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBlasterImpact(){
        audio.clip = blasterImpactSound;
        audio.Play();
    }
}

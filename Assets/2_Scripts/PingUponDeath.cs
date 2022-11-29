using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingUponDeath : MonoBehaviour
{
    private Animator anim;

    private GameObject metaPlayer;

    public bool isActive = true;

    public AudioSource audio;
    public AudioClip clip_ping;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            if(PlayerManager.current.hasHead){
                HidePing();
            }
            else{
                ShowPing();
            }
        }
    }

    public void ShowPing(){
        anim.SetBool("hidden", false);
    }
    public void HidePing(){
        anim.SetBool("hidden", true);
    }

    public void PingSound(){
        audio.clip = clip_ping;
        audio.Play();
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}

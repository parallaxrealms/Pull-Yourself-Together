using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager current;

    public bool musicPlaying;

    public int currentTrackNum = 0; //0 = menu

    private AudioSource audio;
    
    public AudioClip track_0;
    public AudioClip track_1;
    public AudioClip track_2;
    public AudioClip track_3;
    public AudioClip track_4;
    public AudioClip track_5;
    public AudioClip track_6;

    public float trackTimer = 500f;
    public bool musicTimerOn = true;


    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        musicPlaying = DebugManager.current.musicPlaying;
        musicTimerOn = false;
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if(musicTimerOn){
            if(trackTimer > 0){
                trackTimer -= Time.deltaTime;
            }
            else{
                musicTimerOn = false;
                trackTimer = 500f;
            }
        }
        else{
            PlayMusic();
        }
    }

    public void PlayMusic(){
        if(musicPlaying){
            audio.Stop();
            musicTimerOn = true;
            trackTimer = 500f;
            if(currentTrackNum == 0){//Menu Theme
                audio.clip = track_5;
                audio.Play();
            }
            if(currentTrackNum == 1){//GameOver Theme
                audio.clip = track_5;
                audio.Play();
            }
            if(currentTrackNum == 2){//CoL 1
                audio.clip = track_1;
                audio.Play();
            }
            if(currentTrackNum == 3){//CoL 2
                audio.clip = track_2;
                audio.Play();
            }
            if(currentTrackNum == 4){//Abyss 1 
                audio.clip = track_0;
                audio.Play();
            }
            if(currentTrackNum == 5){//Boss Theme
                audio.clip = track_4;
                audio.Play();
            }

            if(currentTrackNum == 99){//No Music
                audio.Stop();
            }
        }
    }
}

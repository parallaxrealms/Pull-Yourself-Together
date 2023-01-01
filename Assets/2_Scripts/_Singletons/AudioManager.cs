using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	public static AudioManager current;

	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	private bool audioPlaying;
	public bool musicTimerOn;
	public float trackTimer = 500f;

	public int currentTrackNum = 0; //0 = menu

	//Music
	[SerializeField] private AudioClip NPC_theme; //0
	[SerializeField] private AudioClip mystical_forest;//1
	[SerializeField] private AudioClip glittering_shadows;//2
	[SerializeField] private AudioClip obisdian_sanctum;//3
	[SerializeField] private AudioClip advent_spire;//4
	[SerializeField] private AudioClip time_is_ticking;//5
	[SerializeField] private AudioClip day_dreams;//6

	public int currentSFXTrack = 0;

	//SFX
	[SerializeField] private AudioClip ui_select; //0
	[SerializeField] private AudioClip ui_deny; //1
	[SerializeField] private AudioClip ui_upgrade; //2
	[SerializeField] private AudioClip ui_click; //3
	[SerializeField] private AudioClip playerJump; //4
	[SerializeField] private AudioClip playerDrill; //5
	[SerializeField] private AudioClip blasterImpact; //6
	[SerializeField] private AudioClip blasterShoot; //7
	[SerializeField] private AudioClip missileImpact; //8
	[SerializeField] private AudioClip missileShoot; //9
	[SerializeField] private AudioClip energyBeamImpact; //10
	[SerializeField] private AudioClip energyBeamShoot; //11
	[SerializeField] private AudioClip hitHard; //12
	[SerializeField] private AudioClip partPickup; //13
	[SerializeField] private AudioClip crystal_hit; //14
	[SerializeField] private AudioClip crystal_break; //15
	[SerializeField] private AudioClip crystal_pickup; //16
	[SerializeField] private AudioClip botDeath; //20
	[SerializeField] private AudioClip botHit; //21
	[SerializeField] private AudioClip botLosePart; //22
	[SerializeField] private AudioClip botReboot; //23
	[SerializeField] private AudioClip botActivation; //24
	[SerializeField] private AudioClip doorOpen; //25
	[SerializeField] private AudioClip worm_spawn; //40
	[SerializeField] private AudioClip worm_attack; //41
	[SerializeField] private AudioClip worm_jump; //42
	[SerializeField] private AudioClip worm_hit; //43
	[SerializeField] private AudioClip worm_death; //44
	[SerializeField] private AudioClip spawner_worm_hit; //45
	[SerializeField] private AudioClip spawner_worm_spawn; //46
	[SerializeField] private AudioClip spawner_worm_death; //47
	[SerializeField] private AudioClip buzzer_spawn; //50
	[SerializeField] private AudioClip buzzer_attack; //51
	[SerializeField] private AudioClip buzzer_jump; //52
	[SerializeField] private AudioClip buzzer_hit; //53
	[SerializeField] private AudioClip buzzer_death; //54
	[SerializeField] private AudioClip spawner_buzzer_hit; //55
	[SerializeField] private AudioClip spawner_buzzer_spawn; //56
	[SerializeField] private AudioClip spawner_buzzer_death; //57
	[SerializeField] private AudioClip cyberMantis_spawn; //60
	[SerializeField] private AudioClip cyberMantis_attack; //61
	[SerializeField] private AudioClip cyberMantis_jump; //62
	[SerializeField] private AudioClip cyberMantis_hit; //63
	[SerializeField] private AudioClip cyberMantis_death; //64

	public float musicVolume = 1f; // range from 0 to 1
	public float sfxVolume = 1f; // range from 0 to 1

	private void Awake(){
		DontDestroyOnLoad(this);
		current = this;
	}

	void Start()
	{
		audioPlaying = DebugManager.current.musicPlaying;
		musicTimerOn = false;

		musicSource.volume = musicVolume;
		sfxSource.volume = sfxVolume;
	}

	public void Update(){
		if(musicTimerOn){
	      if(trackTimer > 0){
	        	trackTimer -= Time.deltaTime;
	      }
	      else{
	        	musicTimerOn = false;
	        	trackTimer = 400f;
	      }
	  	}
	  	else{
	    	PlayMusicTrack();
			musicTimerOn = true;
	  	}
	}

	public void PlayMusicTrack(){
		if(audioPlaying){
			if(currentTrackNum == 0){
				musicSource.clip = null;
				musicSource.Stop();
			}
			else if(currentTrackNum == 1){
				musicSource.clip = NPC_theme;
				musicSource.Play();
			}
			else if(currentTrackNum == 2){
				musicSource.clip = mystical_forest;
				musicSource.Play();
			}
		}
	}

	public void PlaySfx(){
		if(audioPlaying){
			if(currentSFXTrack == 0){
				sfxSource.clip = ui_select;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 1){
				sfxSource.clip = ui_deny;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 2){
				sfxSource.clip = ui_upgrade;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 3){
				sfxSource.clip = ui_click;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 4){
				sfxSource.clip = playerJump;
				sfxSource.Play();
			}
		}
	}
}
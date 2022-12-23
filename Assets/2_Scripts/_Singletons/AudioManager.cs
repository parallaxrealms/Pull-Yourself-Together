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
	[SerializeField] private AudioClip mystical_forest;
	[SerializeField] private AudioClip glittering_shadows;
	[SerializeField] private AudioClip obisdian_sanctum;
	[SerializeField] private AudioClip advent_spire;
	[SerializeField] private AudioClip time_is_ticking;
	[SerializeField] private AudioClip NPC_theme;
	[SerializeField] private AudioClip day_dreams;

	public int currentSFXTrack = 0;

	//SFX
	[SerializeField] private AudioClip blasterImpact; //0
	[SerializeField] private AudioClip missileExplode; //1
	[SerializeField] private AudioClip laserHit; //2
	[SerializeField] private AudioClip enemyBotDeath; //3
	[SerializeField] private AudioClip upgradeSound; //4

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
	// if(musicTimerOn){
	//       if(trackTimer > 0){
	//           trackTimer -= Time.deltaTime;
	//       }
	//       else{
	//           musicTimerOn = false;
	//           trackTimer = 500f;
	//       }
	//   }
	//   else{
	//       PlayMusic();
	//   }
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
				sfxSource.clip = blasterImpact;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 1){
				sfxSource.clip = missileExplode;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 2){
				sfxSource.clip = laserHit;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 3){
				sfxSource.clip = enemyBotDeath;
				sfxSource.Play();
			}
			else if(currentSFXTrack == 4){
				sfxSource.clip = upgradeSound;
				sfxSource.Play();
			}
		}
	}
}
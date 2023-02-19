using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
  public static AudioManager current;

  [SerializeField] public AudioSource musicSource;
  [SerializeField] public AudioSource sfxSource;

  private bool audioPlaying;
  private bool sfxPlaying;
  public bool musicTimerOn;
  public float trackTimer = 500f;

  public int currentTrackNum = 0; //0 = menu

  //Music
  [SerializeField] private AudioClip NPC_theme; //1
  [SerializeField] private AudioClip mystical_forest;//2
  [SerializeField] private AudioClip glittering_shadows;//3
  [SerializeField] private AudioClip obisdian_sanctum;//4
  [SerializeField] private AudioClip advent_spire;//5
  [SerializeField] private AudioClip time_is_ticking;//6
  [SerializeField] private AudioClip day_dreams;//7
  [SerializeField] private AudioClip pluto;//8
  [SerializeField] private AudioClip lost_in_the_city;//9

  public int currentSFXTrack = 0;

  //SFX
  [SerializeField] private AudioClip ui_select; //0
  [SerializeField] private AudioClip ui_deny; //1
  [SerializeField] private AudioClip ui_upgrade; //2
  [SerializeField] private AudioClip ui_click; //3

  [SerializeField] private AudioClip playerShieldHit; //09
  [SerializeField] private AudioClip playerJump; //10
  [SerializeField] private AudioClip playerDrill; //11
  [SerializeField] private AudioClip blasterImpact; //12
  [SerializeField] private AudioClip blasterShoot; //13
  [SerializeField] private AudioClip missileImpact; //14
  [SerializeField] private AudioClip missileShoot; //15
  [SerializeField] private AudioClip energyBeamImpact; //16
  [SerializeField] private AudioClip energyBeamShoot; //17
  [SerializeField] private AudioClip hitHard; //18
  [SerializeField] private AudioClip partPickup; //19

  [SerializeField] private AudioClip crystal_hit; //20
  [SerializeField] private AudioClip crystal_break; //21
  [SerializeField] private AudioClip crystal_pickup; //22

  [SerializeField] private AudioClip botDeath; //30
  [SerializeField] private AudioClip botHit; //31
  [SerializeField] private AudioClip botLosePart; //32
  [SerializeField] private AudioClip botReboot; //33
  [SerializeField] private AudioClip botActivation; //34
  [SerializeField] private AudioClip doorOpen; //35

  [SerializeField] private AudioClip worm_spawn; //50
  [SerializeField] private AudioClip worm_attack; //51
  [SerializeField] private AudioClip worm_jump; //52
  [SerializeField] private AudioClip worm_hit; //53
  [SerializeField] private AudioClip worm_death; //54

  [SerializeField] private AudioClip buzzer_spawn; //60
  [SerializeField] private AudioClip buzzer_attack; //61
  [SerializeField] private AudioClip buzzer_hit; //62
  [SerializeField] private AudioClip buzzer_death; //63

  [SerializeField] private AudioClip spawner_worm_hit; //100
  [SerializeField] private AudioClip spawner_worm_spawn; //101
  [SerializeField] private AudioClip spawner_worm_death; //102

  [SerializeField] private AudioClip spawner_buzzer_hit; //110
  [SerializeField] private AudioClip spawner_buzzer_spawn; //111
  [SerializeField] private AudioClip spawner_buzzer_death; //112

  [SerializeField] private AudioClip cyberMantis_spawn; //120
  [SerializeField] private AudioClip cyberMantis_attack; //121
  [SerializeField] private AudioClip cyberMantis_falling; //122
  [SerializeField] private AudioClip cyberMantis_crash; //123
  [SerializeField] private AudioClip cyberMantis_activation; //124
  [SerializeField] private AudioClip cyberMantis_hit; //125
  [SerializeField] private AudioClip cyberMantis_death; //126
  [SerializeField] private AudioClip cyberMantis_synth; //127
  [SerializeField] private AudioClip cyberMantis_lunge; //128

  public float musicVolume = 1f; // range from 0 to 1
  public float sfxVolume = 1f; // range from 0 to 1

  public bool UIHover;

  private void Awake()
  {
    DontDestroyOnLoad(this);
    current = this;
  }

  void Start()
  {
    audioPlaying = DebugManager.current.musicPlaying;
    sfxPlaying = DebugManager.current.sfxPlaying;
    musicTimerOn = false;

    musicVolume = .7f;
    sfxVolume = .7f;
    musicSource.volume = musicVolume;
    sfxSource.volume = sfxVolume;
  }

  public void Update()
  {
    if (musicTimerOn)
    {
      if (trackTimer > 0)
      {
        trackTimer -= Time.deltaTime;
      }
      else
      {
        musicTimerOn = false;
        trackTimer = 500f;
      }
    }
    else
    {
      PlayMusicTrack();
      musicTimerOn = true;
    }
  }

  public void PlayMusicTrack()
  {
    if (audioPlaying)
    {
      if (currentTrackNum == 0)
      {
        musicSource.clip = null;
        musicSource.Stop();
      }
      else if (currentTrackNum == 1)
      {
        musicSource.Stop();
        musicSource.clip = NPC_theme;
        musicSource.Play();
      }
      else if (currentTrackNum == 2)
      {
        musicSource.Stop();
        musicSource.clip = mystical_forest;
        musicSource.Play();
      }
      else if (currentTrackNum == 3)
      {
        musicSource.Stop();
        musicSource.clip = glittering_shadows;
        musicSource.Play();
      }
      else if (currentTrackNum == 4)
      {
        musicSource.Stop();
        musicSource.clip = obisdian_sanctum;
        musicSource.Play();
      }
      else if (currentTrackNum == 5)
      {
        musicSource.Stop();
        musicSource.clip = advent_spire;
        musicSource.Play();
      }
      else if (currentTrackNum == 6)
      {
        musicSource.Stop();
        musicSource.clip = time_is_ticking;
        musicSource.Play();
      }
      else if (currentTrackNum == 7)
      {
        musicSource.Stop();
        musicSource.clip = day_dreams;
        musicSource.Play();
      }
      else if (currentTrackNum == 8)
      {
        musicSource.Stop();
        musicSource.clip = pluto;
        musicSource.Play();
      }
      else if (currentTrackNum == 9)
      {
        musicSource.Stop();
        musicSource.clip = lost_in_the_city;
        musicSource.Play();
      }
    }
  }

  public void PlaySfx()
  {
    if (sfxPlaying)
    {
      if (currentSFXTrack == 0)
      {
        sfxSource.PlayOneShot(ui_select);
      }
      else if (currentSFXTrack == 1)
      {
        sfxSource.PlayOneShot(ui_deny);
      }
      else if (currentSFXTrack == 2)
      {
        sfxSource.PlayOneShot(ui_upgrade);
      }
      else if (currentSFXTrack == 3)
      {
        sfxSource.PlayOneShot(ui_click);
      }

      else if (currentSFXTrack == 09)
      {
        sfxSource.PlayOneShot(playerShieldHit);
      }
      else if (currentSFXTrack == 10)
      {
        sfxSource.PlayOneShot(playerJump);
      }
      else if (currentSFXTrack == 11)
      {
        sfxSource.PlayOneShot(playerDrill);
      }
      else if (currentSFXTrack == 12)
      {
        sfxSource.PlayOneShot(blasterImpact);
      }
      else if (currentSFXTrack == 13)
      {
        sfxSource.PlayOneShot(blasterShoot);
      }
      else if (currentSFXTrack == 14)
      {
        sfxSource.PlayOneShot(missileImpact);
      }
      else if (currentSFXTrack == 15)
      {
        sfxSource.PlayOneShot(missileShoot);
      }
      else if (currentSFXTrack == 16)
      {
        sfxSource.PlayOneShot(energyBeamImpact);
      }
      else if (currentSFXTrack == 17)
      {
        sfxSource.PlayOneShot(energyBeamShoot);
      }
      else if (currentSFXTrack == 18)
      {
        sfxSource.PlayOneShot(hitHard);
      }
      else if (currentSFXTrack == 19)
      {
        sfxSource.PlayOneShot(partPickup);
      }

      else if (currentSFXTrack == 20)
      {
        sfxSource.PlayOneShot(crystal_hit);
      }
      else if (currentSFXTrack == 21)
      {
        sfxSource.PlayOneShot(crystal_break);
      }
      else if (currentSFXTrack == 22)
      {
        sfxSource.PlayOneShot(crystal_pickup);
      }

      else if (currentSFXTrack == 30)
      {
        sfxSource.PlayOneShot(botDeath);
      }
      else if (currentSFXTrack == 31)
      {
        sfxSource.PlayOneShot(botHit);
      }
      else if (currentSFXTrack == 32)
      {
        sfxSource.PlayOneShot(botLosePart);
      }
      else if (currentSFXTrack == 33)
      {
        sfxSource.PlayOneShot(botReboot);
      }
      else if (currentSFXTrack == 34)
      {
        sfxSource.PlayOneShot(botActivation);
      }
      else if (currentSFXTrack == 35)
      {
        sfxSource.PlayOneShot(doorOpen);
      }

      else if (currentSFXTrack == 50)
      {
        sfxSource.PlayOneShot(worm_spawn);
      }
      else if (currentSFXTrack == 51)
      {
        sfxSource.PlayOneShot(worm_attack);
      }
      else if (currentSFXTrack == 52)
      {
        sfxSource.PlayOneShot(worm_jump);
      }
      else if (currentSFXTrack == 53)
      {
        sfxSource.PlayOneShot(worm_hit);
      }
      else if (currentSFXTrack == 54)
      {
        sfxSource.PlayOneShot(worm_death);
      }

      else if (currentSFXTrack == 60)
      {
        sfxSource.PlayOneShot(buzzer_spawn);
      }
      else if (currentSFXTrack == 61)
      {
        sfxSource.PlayOneShot(buzzer_attack);
      }
      else if (currentSFXTrack == 62)
      {
        sfxSource.PlayOneShot(buzzer_hit);
      }
      else if (currentSFXTrack == 63)
      {
        sfxSource.PlayOneShot(buzzer_death);
      }

      else if (currentSFXTrack == 100)
      {
        sfxSource.PlayOneShot(spawner_worm_spawn);
      }
      else if (currentSFXTrack == 101)
      {
        sfxSource.PlayOneShot(spawner_worm_hit);
      }
      else if (currentSFXTrack == 102)
      {
        sfxSource.PlayOneShot(spawner_worm_death);
      }

      else if (currentSFXTrack == 110)
      {
        sfxSource.PlayOneShot(spawner_buzzer_spawn);
      }
      else if (currentSFXTrack == 111)
      {
        sfxSource.PlayOneShot(spawner_buzzer_hit);
      }
      else if (currentSFXTrack == 112)
      {
        sfxSource.PlayOneShot(spawner_buzzer_death);
      }

      else if (currentSFXTrack == 120)
      {
        sfxSource.PlayOneShot(cyberMantis_spawn);
      }
      else if (currentSFXTrack == 121)
      {
        sfxSource.PlayOneShot(cyberMantis_attack);
      }
      else if (currentSFXTrack == 122)
      {
        sfxSource.PlayOneShot(cyberMantis_falling);
      }
      else if (currentSFXTrack == 123)
      {
        sfxSource.PlayOneShot(cyberMantis_crash);
      }
      else if (currentSFXTrack == 124)
      {
        sfxSource.PlayOneShot(cyberMantis_activation);
      }
      else if (currentSFXTrack == 125)
      {
        sfxSource.PlayOneShot(cyberMantis_hit);
      }
      else if (currentSFXTrack == 126)
      {
        sfxSource.PlayOneShot(cyberMantis_death);
      }
      else if (currentSFXTrack == 127)
      {
        sfxSource.PlayOneShot(cyberMantis_synth);
      }
      else if (currentSFXTrack == 128)
      {
        sfxSource.PlayOneShot(cyberMantis_lunge);
      }
    }
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInit : MonoBehaviour
{
  public GameObject playerSpawnPoint;
  public string triggerSpawnName;

  public bool isFalling = false;
  public int sceneTrackNum;
  public bool backedUp = false;

  public int levelID;
  public bool levelVisited;

  public bool bossReSpawned = false;
  public GameObject cyberMantis;
  public GameObject cyberMantisObject;

  void Awake()
  {
    if (GameObject.Find("GameController") == null)
    {
      SceneManager.LoadScene("Init");
    }
    else
    {
      triggerSpawnName = GameController.current.triggerSpawnName;
      AudioManager.current.currentTrackNum = sceneTrackNum;
      AudioManager.current.PlayMusicTrack();

      MenuManager.current.currentLevelID = levelID;

      if (levelID == 1)
      {
        if (!GameController.current.init_CoL_0)
        {
          levelVisited = false;
          GameController.current.init_CoL_0 = true;
        }
        else
        {
          levelVisited = true;
        }
        GameController.current.playerFellAbyss = false;
      }
      else if (levelID == 2)
      {
        if (!GameController.current.init_CoL_1)
        {
          levelVisited = false;
          GameController.current.init_CoL_1 = true;
        }
        else
        {
          levelVisited = true;
        }
        GameController.current.playerFellAbyss = false;
      }
      else if (levelID == 3)
      {
        if (!GameController.current.init_CoL_2)
        {
          levelVisited = false;
          GameController.current.init_CoL_2 = true;
        }
        else
        {
          levelVisited = true;
        }
        GameController.current.playerFellAbyss = false;
      }
      else if (levelID == 4)
      {
        if (!GameController.current.init_Abyss_0)
        {
          levelVisited = false;
          GameController.current.init_Abyss_0 = true;
        }
        else
        {
          levelVisited = true;
        }
      }
      else if (levelID == 5)
      {
        if (!GameController.current.init_Abyss_1)
        {
          levelVisited = false;
          GameController.current.init_Abyss_1 = true;
        }
        else
        {
          levelVisited = true;
        }
      }
      else if (levelID == 6)
      {
        if (!GameController.current.init_Abyss_Boss)
        {
          levelVisited = false;
          GameController.current.init_Abyss_Boss = true;
          SpawnBoss();
        }
        else
        {
          levelVisited = true;
          RespawnBoss();

          AudioManager.current.currentSFXTrack = 6;
          AudioManager.current.PlayMusicTrack();
        }
      }
      if (!levelVisited)
      {
        GameController.current.newScene = true;
      }
      else
      {
        GameController.current.newScene = false;
      }
      AudioManager.current.trackTimer = 400f;
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    //Decide where to put the player when the scene loads
    if (!GameController.current.playerSpawned)
    {
      playerSpawnPoint = GameObject.Find("SpawnStartPos");

      PlayerManager.current.spawnPosition = playerSpawnPoint.transform.position;
      PlayerManager.current.InitPlayer();
    }
    else if (GameController.current.playerRespawning)
    {
      PlayerManager.current.spawnPosition = PlayerManager.current.backupSpawnPos;
      PlayerManager.current.RebootPlayer();
      GameController.current.playerRespawning = false;
    }
    else if (GameController.current.sceneChanging)
    {
      playerSpawnPoint = GameObject.Find(triggerSpawnName);

      if (PlayerManager.current.sceneDirection == "None")
      {
        PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x, playerSpawnPoint.transform.position.y, playerSpawnPoint.transform.position.z);
        PlayerManager.current.Invoke("PauseMovement", 0.2f);
      }
      if (PlayerManager.current.sceneDirection == "Left")
      {
        PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x - 15.0f, playerSpawnPoint.transform.position.y, playerSpawnPoint.transform.position.z);
      }
      if (PlayerManager.current.sceneDirection == "Right")
      {
        PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x + 15.0f, playerSpawnPoint.transform.position.y, playerSpawnPoint.transform.position.z);
      }
      if (PlayerManager.current.sceneDirection == "Down")
      {
        PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x, playerSpawnPoint.transform.position.y - 12.0f, playerSpawnPoint.transform.position.z);
      }
      if (PlayerManager.current.sceneDirection == "Up")
      {
        playerSpawnPoint = GameObject.Find("TopClimbSpawn");
        PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x, playerSpawnPoint.transform.position.y, playerSpawnPoint.transform.position.z);
      }
      PlayerManager.current.RebootNewMap();
      GameController.current.sceneChanging = false;
    }
    MenuManager.current.Invoke("FadeFromBlack", 0.5f);
    MenuManager.current.ResetTitleCard();
    GameController.current.Invoke("HighlightPickups", 0.25f);
  }

  // Update is called once per frame
  void Update()
  {

  }

  void SpawnBoss()
  {
    cyberMantis = Instantiate(cyberMantisObject, new Vector3(32f, 16f, 0f), Quaternion.identity);

    CyberMantisScript bossScript = cyberMantis.GetComponent<CyberMantisScript>();
    bossScript.Spawn();
  }

  void RespawnBoss()
  {
    cyberMantis = GameObject.Find("CyberMantis");
    bossReSpawned = true;
    CyberMantisScript bossScript = cyberMantis.GetComponent<CyberMantisScript>();
    bossScript.Respawn();

    if (GameController.current.playerMetBlueBot)
    {
      BlueBotScript blueBotScript = GameObject.Find("BlueBot").GetComponent<BlueBotScript>();
      blueBotScript.RemoveSelf();

      GameObject bossTrigger = GameObject.Find("BossTrigger");
      Destroy(bossTrigger);
    }
  }
}

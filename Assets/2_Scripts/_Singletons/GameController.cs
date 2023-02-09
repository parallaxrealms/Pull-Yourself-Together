using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
  public static GameController current;

  private MouseCursor mouseCursorScript;

  private GameObject crystalManager;
  private UI_CrystalManager crystalManagerScript;

  public GameObject gameCamera;
  private Vector3 camOriginalPos;

  public string sceneChangeName;
  public GameObject triggerSpawn;
  public string triggerSpawnName;

  public bool init_CoL_0;
  public bool init_CoL_1;
  public bool init_CoL_2;
  public bool init_Abyss_0;
  public bool init_Abyss_1;
  public bool init_Abyss_Boss;

  public bool gameStarted = false;
  public bool playerSpawned = false;
  public bool playerRespawning = false;
  public bool sceneChanging = false;
  public bool newScene = true;

  public GameObject UI_Object;

  public List<GameObject> ListWorms = new List<GameObject>();
  public List<GameObject> ListBuzzers = new List<GameObject>();
  public List<GameObject> ListBots = new List<GameObject>();
  public List<GameObject> ListBosses = new List<GameObject>();

  public List<GameObject> ListCrystals = new List<GameObject>();
  public List<GameObject> ListPickups = new List<GameObject>();

  public GameObject prevBotOwner;

  public bool playerFellAbyss;
  public bool playerMetBlueBot;
  public bool introDialogue;

  public bool damageNumOption = true;

  private void Awake()
  {
    DontDestroyOnLoad(this);
    current = this;

    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

    gameCamera = GameObject.Find("Camera");
    camOriginalPos = new Vector3(0f, 0f, -100f);

    crystalManager = GameObject.Find("CrystalManager");
    crystalManagerScript = crystalManager.GetComponent<UI_CrystalManager>();

    mouseCursorScript = GetComponent<MouseCursor>();

    DOTween.Init();
  }

  public event Action onEnableDebug;
  public void EnableDebug()
  {
    if (onEnableDebug != null)
    {
      onEnableDebug();
    }
  }

  public event Action onDisableDebug;
  public void DisableDebug()
  {
    if (onDisableDebug != null)
    {
      onEnableDebug();
    }
  }

  private void ResetGameController()
  {
    if (init_Abyss_Boss)
    {
      GameObject bossObject = GameObject.Find("CyberMantis");
      if (bossObject != null)
      {
        Destroy(bossObject);
      }
    }
    init_CoL_0 = false;
    init_CoL_1 = false;
    init_CoL_2 = false;
    init_Abyss_0 = false;
    init_Abyss_1 = false;
    init_Abyss_Boss = false;
    crystalManagerScript.Reset();
    playerFellAbyss = false;
    playerMetBlueBot = false;
  }

  public void NewGame()
  {
    if (!gameStarted)
    {
      ResetGameController();
      MenuManager.current.NewGame();
      SceneManager.LoadScene("CoL_0");
      gameStarted = true;
      Invoke("HighlightPickups", .5f);
    }
  }

  public void TestGame()
  {
    if (!gameStarted)
    {
      MenuManager.current.NewGame();
      SceneManager.LoadScene("TestLevel");
      gameStarted = true;
    }
  }

  public void GameOver()
  {
    gameCamera.transform.position = camOriginalPos;
    gameStarted = false;
    playerSpawned = false;
    DestroyAllEnemyObjects();
    PersistentGameObjects.current.ResetPersistentGameObjects();
    SceneManager.LoadScene("MainMenu");
    MenuManager.current.GameOver();
  }

  public void CheckCursor()
  {
    if (PlayerManager.current.hasGun)
    {
      mouseCursorScript.ChangeCursorToGun();
    }
    else
    {
      mouseCursorScript.ChangeCursorToDefault();
    }
  }
  public void ChangeMouseCursorDefault()
  {
    mouseCursorScript.ChangeCursorToDefault();
    mouseCursorScript.HideCursorSelectedPart();
  }
  public void ChangeMouseCursorSelectedPart(int partNum)
  {
    mouseCursorScript.ChangeCursorToSelectedPart(partNum);
  }

  public void BossFightStarted()
  {
    AudioManager.current.currentTrackNum = 6;
    AudioManager.current.PlayMusicTrack();
  }
  public void StartEndingScene()
  {
    PlayerManager.current.sceneDirection = "None";
    sceneChangeName = "CreditsScene";
    triggerSpawnName = "CreditsSpawnPos";
    sceneChanging = true;
    MenuManager.current.ChangeSceneTo();
  }
  public void EnableUI()
  {
    UI_Object.SetActive(true);
    gameStarted = true;
  }

  public void DisableUI()
  {
    UI_Object.SetActive(false);
  }

  public void ResetEnemyPlayerObjects()
  {
    foreach (GameObject worm in ListWorms)
    {
      EnemyScript enemyScript = worm.GetComponent<EnemyScript>();
      enemyScript.FindPlayer();
    }

    foreach (GameObject buzzer in ListBuzzers)
    {
      EnemyScript enemyScript = buzzer.GetComponent<EnemyScript>();
      enemyScript.FindPlayer();
    }

    foreach (GameObject bot in ListBots)
    {
      LostBotMeta botScript = bot.GetComponent<LostBotMeta>();
      botScript.FindPlayer();
    }

    foreach (GameObject boss in ListBosses)
    {
      CyberMantisScript bossScript = boss.GetComponent<CyberMantisScript>();
      bossScript.FindPlayer();
    }
  }

  public void DestroyClosestBotAfterSpawn()
  {
    GameObject prevBot = prevBotOwner;
    LostBotMeta prevBotScript = prevBot.GetComponent<LostBotMeta>();
    prevBotScript.Invoke("LoseLegs", 0.01f);
    prevBotScript.Invoke("LoseDrill", 0.01f);
    prevBotScript.Invoke("LoseGun", 0.01f);
    prevBotScript.Invoke("LoseBody", 0.01f);
    GameController.current.ListBots.Remove(prevBotOwner);
    Destroy(prevBotOwner);
  }

  public void InactivateEnemies()
  {
    foreach (GameObject worm in ListWorms)
    {
      EnemyScript enemyScript = worm.GetComponent<EnemyScript>();
      enemyScript.Inactive();
    }

    foreach (GameObject buzzer in ListBuzzers)
    {
      EnemyScript enemyScript = buzzer.GetComponent<EnemyScript>();
      enemyScript.Inactive();
    }

    foreach (GameObject bot in ListBots)
    {
      LostBotMeta botScript = bot.GetComponent<LostBotMeta>();
      botScript.Inactive();
    }
    foreach (GameObject boss in ListBosses)
    {
      CyberMantisScript bossScript = boss.GetComponent<CyberMantisScript>();
      bossScript.Inactive();
    }
  }

  public void HighlightPickups()
  {
    foreach (GameObject pickup in ListPickups)
    {
      if (pickup != null)
      {
        PickUpScript pickupScript = pickup.GetComponent<PickUpScript>();

        if (pickupScript.pickupType == 9)
        { //Head Reset
          if (!PlayerManager.current.backedUp)
          {
            if (!pickupScript.isUsed)
            {
              pickupScript.EnablePickupParticles();
              pickupScript.DrawOutline();
            }
          }
          else
          {
            if (!pickupScript.isUsed)
            {
              pickupScript.DrawOutline();
            }
            else
            {
              pickupScript.EraseOutline();
              pickupScript.DisablePickupParticles();
            }
          }
        }
        if (pickupScript.pickupType == 1)
        { //Body
          if (PlayerManager.current.hasBody == false)
          {
            pickupScript.EnablePickupParticles();
            pickupScript.DrawOutline();
          }
          else
          {
            pickupScript.DisablePickupParticles();
            pickupScript.DrawOutline();
          }
        }
        if (pickupScript.pickupType == 2)
        { //Drill
          if (PlayerManager.current.hasBody == true)
          {
            if (PlayerManager.current.hasDrill == false)
            {
              pickupScript.EnablePickupParticles();
              pickupScript.DrawOutline();
            }
            else
            {
              pickupScript.DisablePickupParticles();
              pickupScript.DrawOutline();
            }
          }
          else
          {
            pickupScript.DisablePickupParticles();
            pickupScript.DrawOutline();
          }
        }
        if (pickupScript.pickupType == 3)
        { //Gun
          if (PlayerManager.current.hasBody == true)
          {
            if (PlayerManager.current.hasGun == false)
            {
              pickupScript.EnablePickupParticles();
              pickupScript.DrawOutline();
            }
            else
            {
              if (pickupScript.gunType != PlayerManager.current.gunType)
              {
                pickupScript.EnablePickupParticles();
                pickupScript.DrawOutline();
              }
              else
              {
                pickupScript.DisablePickupParticles();
                pickupScript.DrawOutline();
              }
            }
          }
          else
          {
            pickupScript.DisablePickupParticles();
            pickupScript.DrawOutline();

          }
        }
        if (pickupScript.pickupType == 4)
        { //Legs
          if (PlayerManager.current.hasBody == true)
          {
            if (!PlayerManager.current.hasLegs)
            {
              pickupScript.EnablePickupParticles();
              pickupScript.DrawOutline();
            }
            else
            {
              if (pickupScript.legType != PlayerManager.current.legType)
              {
                pickupScript.EnablePickupParticles();
                pickupScript.DrawOutline();
              }
              else
              {
                pickupScript.DisablePickupParticles();
                pickupScript.DrawOutline();
              }
            }
          }
          else
          {
            pickupScript.DisablePickupParticles();
            pickupScript.DrawOutline();
          }
        }
      }
    }
  }

  public void ActivatePickupNames()
  {
    foreach (GameObject pickup in ListPickups)
    {
      PickUpScript pickupScript = pickup.GetComponent<PickUpScript>();
      if (!pickupScript.isUsed)
      {
        pickupScript.nameActivated = true;
        pickupScript.DrawText();
        pickupScript.DrawOutline();
      }
    }
  }
  public void DeActivatePickupNames()
  {
    foreach (GameObject pickup in ListPickups)
    {
      PickUpScript pickupScript = pickup.GetComponent<PickUpScript>();
      if (!pickupScript.isUsed)
      {
        if (!pickupScript.activated)
        {
          pickupScript.nameActivated = false;
          pickupScript.EraseText();
          pickupScript.EraseOutline();
        }
      }
    }
    HighlightPickups();
  }

  public void RebootFromCheckpoint()
  {
    EnableUI();
    if (prevBotOwner != null)
    {
      DestroyClosestBotAfterSpawn();
    }
    ResetEnemyPlayerObjects();
    HighlightPickups();
  }

  public void DestroyAllEnemyObjects()
  {
    ListWorms.Clear();
    ListBuzzers.Clear();
    ListBots.Clear();
    ListBosses.Clear();
    ListPickups.Clear();
    ListCrystals.Clear();
  }

  public void ChangeSceneTo()
  {
    DestroyAllEnemyObjects();
    if (sceneChangeName != null)
    {
      SceneManager.LoadScene(sceneChangeName, LoadSceneMode.Single);
      sceneChangeName = null;
    }
  }
  public void ChangeSceneAndReboot()
  {
    DestroyAllEnemyObjects();
    string backupScene = PlayerManager.current.backupSceneName;
    if (backupScene != null)
    {
      SceneManager.LoadScene(backupScene, LoadSceneMode.Single);
      backupScene = null;
    }
  }

  public void PauseGame()
  {
    MenuManager.current.isMouseOver = true;
    Time.timeScale = 0;
  }
  public void ResumeGame()
  {
    MenuManager.current.isMouseOver = false;
    Time.timeScale = 1;
    if (MenuManager.current.controlsMenu)
    {
      MenuManager.current.CloseControlsMenu();
    }
  }

  public void ScreenMode_Change(Int32 choice)
  {
    if (choice == 0)
    {
      Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }
    else if (choice == 1)
    {
      Screen.fullScreen = false;
    }
    else if (choice == 2)
    {
      Screen.SetResolution(1280, 720, false);
    }
    AudioManager.current.currentSFXTrack = 0;
    AudioManager.current.PlaySfx();
  }

  public void ChangeDamageOption(bool enabled)
  {
    if (enabled)
    {
      damageNumOption = true;
    }
    else
    {
      damageNumOption = false;
    }
    AudioManager.current.currentSFXTrack = 0;
    AudioManager.current.PlaySfx();
  }
}

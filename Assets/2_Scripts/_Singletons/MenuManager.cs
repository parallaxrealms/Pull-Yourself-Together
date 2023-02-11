using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
  public static MenuManager current;

  public int menuState = 0; //0 = foreword, 1 = Main Menu, 2 = Playing, 3 = Game Over

  public int currentLevelID = 0;

  public GameObject mainMenuScreen;
  public GameObject gameOverScreen;
  public GameObject blackScreen;
  public GameObject forewordScreen;
  public GameObject pauseMenuScreen;
  public GameObject pressAnyKeyText;
  public GameObject introDialogueScreen;
  public GameObject dialogueDone;
  public GameObject creditRollScreen;
  public MoveUpSlowly creditRollScript;

  public Sprite[] introDialogueSprites;
  private int dialogueSprite = 0;
  public SpriteRenderer dialogueSpriteRend;

  public GameObject controlsScreen;
  public bool controlsMenu;

  public bool isPaused = false;
  public bool onCreditsRoll = false;

  public GameObject titleCard;
  public SpriteRenderer titleSpriteRend;
  public Sprite title_CavernsOfLight;
  public Sprite title_TheAbyss;
  public Color title_alpha_original;
  public Color title_alpha;
  public Color TempT;

  public SpriteRenderer whiteScreenSprite;
  public SpriteRenderer blackScreenSprite;
  public Color originalColorValue;
  public Color aColorValue;
  public Color TempC;

  private float startAlpha;

  public bool sceneChanging = false;

  public bool fadeTitleStarted = false;

  public bool fadeFromBlackStarted = false;
  public bool fadeToBlackStarted = false;
  public float extraSlowFade = 0.1f;
  public float slowFade = 0.25f;
  public float speedFade = 0.3f;
  private float devFade = 2.0f;

  public float titleStayTimer = 3f;

  public float creditsTimer = 8f;
  public bool creditsStart = false;

  public float endingTimer = 10f;
  public bool endingStarted;

  public GameObject PartsUI;
  public UI_Parts UIPartsScript;

  public GameObject CrystalManager;
  public UI_CrystalManager crystalManagerScript;

  public bool isMouseOver;

  public bool pickupNamesEnabled = false;


  private void Awake()
  {
    DontDestroyOnLoad(this);
    current = this;

    PartsUI = GameObject.Find("UI_Player_Parts");
    UIPartsScript = PartsUI.GetComponent<UI_Parts>();

    CrystalManager = GameObject.Find("CrystalManager");
    crystalManagerScript = CrystalManager.GetComponent<UI_CrystalManager>();

    pauseMenuScreen = GameObject.Find("PauseMenu");
    pauseMenuScreen.SetActive(false);
    controlsScreen.SetActive(false);

    GameController.current.Invoke("DisableUI", 0.1f);
  }

  // Start is called before the first frame update
  void Start()
  {
    DontDestroyOnLoad(gameObject);
    if (DebugManager.current.testingMode)
    {
      speedFade = devFade;
      slowFade = devFade;
    }

    dialogueSpriteRend = introDialogueScreen.GetComponent<SpriteRenderer>();
    dialogueSpriteRend.sprite = introDialogueSprites[dialogueSprite];
    dialogueDone.SetActive(false);

    blackScreenSprite = blackScreen.GetComponent<SpriteRenderer>();
    aColorValue = blackScreenSprite.color;
    originalColorValue = aColorValue;
    TempC = aColorValue;
    startAlpha = 0f;

    titleSpriteRend = titleCard.GetComponent<SpriteRenderer>();
    titleSpriteRend.enabled = false;

    title_alpha = titleSpriteRend.color;
    title_alpha_original = title_alpha;
    TempT = title_alpha;

    Foreword();
    ClosePartsUI();
  }
  // Update is called once per frame
  void Update()
  {
    if (!fadeToBlackStarted)
    {
      if (menuState == 1 || menuState == 3)
      {
        if (Input.anyKey)
        {
          if (!fadeFromBlackStarted)
          {
            if (DebugManager.current.testingMode)
            {
              if (!GameController.current.introDialogue)
              {
                IntroDialogue();
              }
              else
              {
                GameController.current.NewGame();
              }
            }
            else
            {
              if (!GameController.current.introDialogue)
              {
                IntroDialogue();
              }
              else
              {
                GameController.current.NewGame();
              }
            }
          }
        }
      }
      if (menuState == 0)
      {
        if (Input.anyKey)
        {
          if (!fadeFromBlackStarted)
          {
            MainMenu();
          }
        }
      }

      if (menuState == 4)
      {
        if (!fadeFromBlackStarted)
        {
          if (Input.anyKeyDown)
          {
            dialogueSprite++;
            FadeFromBlack();
            if (dialogueSprite < 5)
            {
              if (dialogueSprite >= introDialogueSprites.Length)
              {
                dialogueSprite = 0;
              }
              dialogueSpriteRend.sprite = introDialogueSprites[dialogueSprite];
            }
            else
            {
              GameController.current.NewGame();
              if (!GameController.current.introDialogue)
              {
                GameController.current.introDialogue = true;
              }
            }
            dialogueDone.SetActive(false);

            AudioManager.current.currentTrackNum = 3;
            AudioManager.current.PlaySfx();
          }
        }

        if (Input.GetButton("Shift") && Input.GetButton("Q"))
        {
          GameController.current.NewGame();
          if (!GameController.current.introDialogue)
          {
            GameController.current.introDialogue = true;
          }
        }
      }
    }

    if (GameController.current.gameStarted)
    {
      if (Input.GetButtonDown("Tab"))
      {
        if (!UIPartsScript.isEnabled)
        {
          OpenPartsUI();
        }
        else
        {
          ClosePartsUI();
          if (PlayerManager.current.playerHoldingPart)
          {
            PlayerManager.current.DropHeldObject(PlayerManager.current.holdingPartNum);
          }
        }
      }

      if (Input.GetButtonDown("Shift"))
      {
        if (!pickupNamesEnabled)
        {
          GameController.current.ActivatePickupNames();
          pickupNamesEnabled = true;
        }
      }
      if (Input.GetButtonUp("Shift"))
      {
        if (pickupNamesEnabled)
        {
          GameController.current.DeActivatePickupNames();
          pickupNamesEnabled = false;
        }
      }

      if (Input.GetKeyDown(KeyCode.Escape))
      {
        if (isPaused)
        {
          pauseMenuScreen.SetActive(false);
          GameController.current.ResumeGame();
          isPaused = false;
        }
        else
        {
          UIPartsScript.CloseAllWindows();
          ClosePartsUI();
          pauseMenuScreen.SetActive(true);
          GameController.current.PauseGame();
          isPaused = true;
        }
      }
    }

    if (fadeFromBlackStarted)
    {
      if (aColorValue.a > 0f)
      {
        TempC.a -= (slowFade * Time.deltaTime);
        aColorValue = TempC;
        blackScreenSprite.color = aColorValue;
      }
      else
      {
        blackScreenSprite.color = originalColorValue;
        aColorValue = originalColorValue;
        TempC = aColorValue;
        blackScreen.SetActive(false);
        fadeFromBlackStarted = false;
        if (menuState == 4)
        {
          dialogueDone.SetActive(true);
        }
      }
    }
    if (fadeToBlackStarted)
    {
      if (aColorValue.a < 1.0f)
      {
        TempC.a += (slowFade * Time.deltaTime);
        aColorValue = TempC;
        blackScreenSprite.color = aColorValue;
      }
      else
      {
        fadeToBlackStarted = false;
      }
    }

    if (fadeTitleStarted)
    {
      if (titleStayTimer > 0f)
      {
        titleStayTimer -= Time.deltaTime;
      }
      else
      {
        if (title_alpha.a > 0f)
        {
          TempT.a -= (speedFade * Time.deltaTime);
          title_alpha = TempT;
          titleSpriteRend.color = title_alpha;
        }
        else
        {
          title_alpha = title_alpha_original;
          TempT = title_alpha;
          titleStayTimer = 3f;
          HideTitleCard();
        }
      }
    }

    if (creditsStart)
    {
      if (creditsTimer > 0f)
      {
        creditsTimer -= Time.deltaTime;
      }
      else
      {
        creditsStart = false;
        creditsTimer = 5f;
        GameController.current.StartEndingScene();
        UIPartsScript.CloseAllWindows();
        ClosePartsUI();
      }
    }

    if (onCreditsRoll)
    {
      if (creditsStart)
      {
        if (creditsTimer > 0f)
        {
          creditsTimer -= Time.deltaTime;
        }
        else
        {
          creditsStart = false;
        }
      }
      else
      {
        if (Input.anyKey)
        {
          GameController.current.RestartGame();
          onCreditsRoll = false;
        }
      }
    }
  }

  public void RollCredits()
  {
    GameObject credits = Instantiate(creditRollScreen, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
    creditRollScript = credits.GetComponent<MoveUpSlowly>();
    GameObject player = PlayerManager.current.currentPlayerObject;
    Vector3 playerPos = player.transform.position;
    credits.transform.position = new Vector3(playerPos.x, playerPos.y - 13f, playerPos.z);
    creditRollScript.moving = true;

    onCreditsRoll = true;
    creditsTimer = 8f;
    creditsStart = true;
  }

  public void ResetTitleCard()
  {
    titleSpriteRend = titleCard.GetComponent<SpriteRenderer>();

    if (currentLevelID == 1)
    {
      titleSpriteRend.sprite = title_CavernsOfLight;
      ShowTitleCard();
    }
    if (currentLevelID == 4)
    {
      titleSpriteRend.sprite = title_TheAbyss;
      ShowTitleCard();
    }
  }

  public void ShowTitleCard()
  {
    titleSpriteRend.enabled = true;
    fadeTitleStarted = true;
  }

  public void HideTitleCard()
  {
    titleSpriteRend.enabled = false;
    fadeTitleStarted = false;
  }

  public void FadeFromBlack()
  {
    blackScreen.SetActive(true);
    fadeFromBlackStarted = true;
  }
  public void FadeToBlack()
  {
    blackScreen.SetActive(true);
    aColorValue.a = 1f;
    TempC.a = 1f;
    blackScreenSprite.color = aColorValue;
    fadeToBlackStarted = true;
  }

  public void Foreword()
  {
    FadeFromBlack();
    introDialogueScreen.SetActive(false);
    forewordScreen.SetActive(true);
    mainMenuScreen.SetActive(false);
    gameOverScreen.SetActive(false);
    pressAnyKeyText.SetActive(false);
    AudioManager.current.currentTrackNum = 1;
    AudioManager.current.PlayMusicTrack();
  }
  public void MainMenu()
  {
    FadeFromBlack();
    introDialogueScreen.SetActive(false);
    forewordScreen.SetActive(false);
    mainMenuScreen.SetActive(true);
    gameOverScreen.SetActive(false);
    pressAnyKeyText.SetActive(true);
    menuState = 1;
  }
  public void GameOver()
  {
    FadeFromBlack();
    introDialogueScreen.SetActive(false);
    mainMenuScreen.SetActive(false);
    gameOverScreen.SetActive(true);
    menuState = 3;

    AudioManager.current.currentTrackNum = 8;
    AudioManager.current.PlayMusicTrack();
  }
  public void NewGame()
  {
    FadeFromBlack();
    introDialogueScreen.SetActive(false);
    mainMenuScreen.SetActive(false);
    gameOverScreen.SetActive(false);
    pressAnyKeyText.SetActive(false);
    menuState = 2;
    ClosePartsUI();
  }
  public void IntroDialogue()
  {
    FadeFromBlack();
    introDialogueScreen.SetActive(true);
    mainMenuScreen.SetActive(false);
    gameOverScreen.SetActive(false);
    pressAnyKeyText.SetActive(false);
    menuState = 4;
    AudioManager.current.currentTrackNum = 9;
    AudioManager.current.PlayMusicTrack();
  }
  public void StartCreditsTimer()
  {
    creditsStart = true;
  }
  public void CreditsScene()
  {
    Time.timeScale = 1;
    StartCreditsTimer();
  }
  public void ChangeSceneTo()
  {
    PersistentGameObjects.current.TransferPersistentObjects();
    GameController.current.Invoke("ChangeSceneTo", 2.0f);
    FadeToBlack();
  }
  public void ChangeSceneAndReboot()
  {
    PersistentGameObjects.current.TransferPersistentObjects();
    GameController.current.ChangeSceneAndReboot();
    FadeFromBlack();
  }

  public void OpenPartsUI()
  {
    UIPartsScript.EnablePartsUI();
  }
  public void ClosePartsUI()
  {
    UIPartsScript.DisablePartsUI();
    isMouseOver = false;
  }

  public void ToggleControlsMenu()
  {
    if (!controlsMenu)
    {
      OpenControlsMenu();
    }
    else if (controlsMenu)
    {
      CloseControlsMenu();
    }
  }
  public void OpenControlsMenu()
  {
    controlsScreen.SetActive(true);
    controlsMenu = true;
  }
  public void CloseControlsMenu()
  {
    controlsScreen.SetActive(false);
    controlsMenu = false;
  }
}
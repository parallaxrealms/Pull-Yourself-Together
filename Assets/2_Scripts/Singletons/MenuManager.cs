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

    public bool sceneChanging = false;

    public bool fadeTitleStarted = false;

    public bool fadeFromBlackStarted = false;
    public bool fadeToBlackStarted = false;
    public float slowFade = 0.25f;
    public float speedFade = 0.3f;
    private float devFade = 1.0f;

    public float titleStayTimer = 3f;

    public GameObject PartsUI;
    public UI_Parts UIPartsScript;

    public GameObject CrystalManager;
    public UI_CrystalManager crystalManagerScript;

    public bool isMouseOver;
    
    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;

        PartsUI = GameObject.Find("UI_Player_Parts");
        UIPartsScript = PartsUI.GetComponent<UI_Parts>();
        UIPartsScript.Invoke("DisablePartsUI", 0.1f);

        CrystalManager = GameObject.Find("CrystalManager");
        crystalManagerScript = CrystalManager.GetComponent<UI_CrystalManager>();

        GameController.current.Invoke("DisableUI", 0.2f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if(DebugManager.current.testingMode){
            speedFade = devFade;
            slowFade = devFade;
        }

        blackScreenSprite = blackScreen.GetComponent<SpriteRenderer>();
        aColorValue = blackScreenSprite.color;
        originalColorValue = aColorValue;
        TempC = aColorValue;

        titleSpriteRend = titleCard.GetComponent<SpriteRenderer>();
        titleSpriteRend.enabled = false;

        title_alpha = titleSpriteRend.color;
        title_alpha_original = title_alpha;
        TempT = title_alpha;

        Foreword();
    }
    // Update is called once per frame
    void Update()
    {
        if(!fadeToBlackStarted){
            if(menuState == 1 || menuState == 3){
                if (Input.anyKey){
                    if(!fadeFromBlackStarted){
                        if(DebugManager.current.testingMode){
                            GameController.current.Invoke("NewGame", 0.01f);
                        }
                        else{
                            GameController.current.Invoke("NewGame", 0.01f);
                        }
                    }
                }
            }
            if(menuState == 0){
                if (Input.anyKey){
                    if(!fadeFromBlackStarted){
                        Invoke("MainMenu", 0.01f);
                    }
                }
            }
        }

        if(GameController.current.gameStarted){
            if(Input.GetButtonUp("Tab")){
                if(!UIPartsScript.isEnabled){
                    OpenPartsUI();
                }
                else{
                    ClosePartsUI();
                }
            }
        }

        if(fadeFromBlackStarted){
            if (aColorValue.a > 0f){
                TempC.a -= (slowFade * Time.deltaTime);
                aColorValue = TempC;
                blackScreenSprite.color = aColorValue;
            }
            else{
                blackScreenSprite.color = originalColorValue;
                aColorValue = originalColorValue;
                TempC = aColorValue;
                blackScreen.SetActive(false);
                fadeFromBlackStarted = false;
            }
        } 

        if(fadeTitleStarted){
            if(titleStayTimer > 0f){
                titleStayTimer -= Time.deltaTime;
            }
            else{
                if (title_alpha.a > 0f){
                    TempT.a -= (speedFade * Time.deltaTime);
                    title_alpha = TempT;
                    titleSpriteRend.color = title_alpha;
                }
                else{
                    title_alpha = title_alpha_original;
                    TempT = title_alpha;
                    titleStayTimer = 3f;
                    HideTitleCard();
                }
            }
        } 
    }

    public void ResetTitleCard(){
        titleSpriteRend = titleCard.GetComponent<SpriteRenderer>();

        if(currentLevelID == 1){
            titleSpriteRend.sprite = title_CavernsOfLight;
            ShowTitleCard();
        }
        if(currentLevelID == 4){
            titleSpriteRend.sprite = title_TheAbyss;
            ShowTitleCard();
        }
    }
    public void ShowTitleCard(){
        titleSpriteRend.enabled = true;
        fadeTitleStarted = true;
    }

    public void HideTitleCard(){
        titleSpriteRend.enabled = false;
        fadeTitleStarted = false;
    }

    public void FadeFromBlack(){
        blackScreen.SetActive(true);
        fadeFromBlackStarted = true;
    }

    public void Foreword(){
        FadeFromBlack();
        forewordScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }
    public void MainMenu(){
        FadeFromBlack();
        forewordScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        menuState = 1;
    }
    public void GameOver(){
        FadeFromBlack();
        mainMenuScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        menuState = 3;
    }
    public void NewGame(){
        FadeFromBlack();
        mainMenuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        menuState = 2;
    }
    public void ChangeSceneTo(){
        GameController.current.Invoke("ChangeSceneTo", 0.1f);
        FadeFromBlack();
    }
    public void ChangeSceneAndReboot(){
        GameController.current.Invoke("ChangeSceneAndReboot", 0.1f);
        FadeFromBlack();
    }

    public void OpenPartsUI(){
        UIPartsScript.Invoke("EnablePartsUI", 0.1f);
    }
    public void ClosePartsUI(){
        UIPartsScript.Invoke("DisablePartsUI", 0.1f);
        isMouseOver = false;
    }

    public void CloseWindows(){
        
    }
}
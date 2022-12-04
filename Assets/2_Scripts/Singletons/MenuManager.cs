using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public static MenuManager current;

    public int menuState = 0; //0 = foreword, 1 = Main Menu, 2 = Playing, 3 = Game Over
    public GameObject mainMenuScreen;
    public GameObject gameOverScreen;
    public GameObject blackScreen;
    public GameObject forewordScreen;
    public SpriteRenderer blackScreenSprite;
    public Color originalColorValue;
    public Color aColorValue;
    public Color TempC;    
    public bool sceneChanging = false;
    public bool fadeFromBlackStarted = false;
    public bool fadeToBlackStarted = false;
    public float speedFade = 0.25f;

    public GameObject PartsUI;
    public UI_Parts UIPartsScript;
    
    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;

        PartsUI = GameObject.Find("UI_Player_Parts");
        UIPartsScript = PartsUI.GetComponent<UI_Parts>();
        UIPartsScript.Invoke("DisablePartsUI", 0.1f);

        GameController.current.Invoke("DisableUI", 0.2f);
    }
    
    // Start is called before the first frame update
    void Start()
    {        
        DontDestroyOnLoad(gameObject);

        blackScreenSprite = blackScreen.GetComponent<SpriteRenderer>();
        aColorValue = blackScreenSprite.color;
        originalColorValue = aColorValue;
        TempC = aColorValue;

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
                            GameController.current.Invoke("TestGame", 0.01f);
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
    }

    void FixedUpdate()
    {
        if(fadeFromBlackStarted){
            if(sceneChanging){
                GameController.current.Invoke("ChangeSceneTo", 0.1f);
                sceneChanging = false;
            }

            if (aColorValue.a > 0f){
                TempC.a -= (speedFade * Time.deltaTime);
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
        sceneChanging = true;
        FadeFromBlack();
    }


    public void OpenPartsUI(){
        UIPartsScript.isEnabled = true;
        UIPartsScript.Invoke("EnablePartsUI", 0.1f);
    }
    public void ClosePartsUI(){
        UIPartsScript.isEnabled = false;
        UIPartsScript.Invoke("DisablePartsUI", 0.1f);
    }
}
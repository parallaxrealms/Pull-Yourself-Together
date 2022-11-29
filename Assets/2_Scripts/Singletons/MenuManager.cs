using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public static MenuManager current;

    public int menuState = 1; //1 = Main Menu, 2 = Playing, 3 = Game Over
    public GameObject mainMenuScreen;
    public GameObject gameOverScreen;
    public GameObject blackScreen;
    public SpriteRenderer blackScreenSprite;
    public Color originalColorValue;
    public Color aColorValue;
    public Color TempC;    
    public bool sceneChanging = false;
    public bool fadeFromBlackStarted = false;
    public bool fadeToBlackStarted = false;
    public float speedFade = 0.25f;

    public bool musicPlaying;
    public AudioSource audio;
    public AudioClip menuMusic;
    public AudioClip gameOverMusic;
    public AudioClip abyssMusic;
    public AudioClip bossMusic;

    public GameObject PartsUI;
    public UI_Parts UIPartsScript;
    
    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;

        PartsUI = GameObject.Find("UI_Player_Parts");
        UIPartsScript = PartsUI.GetComponent<UI_Parts>();
        UIPartsScript.Invoke("DisablePartsUI", 0.1f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        musicPlaying = DebugManager.current.musicPlaying;

        DontDestroyOnLoad(gameObject);

        blackScreenSprite = blackScreen.GetComponent<SpriteRenderer>();
        aColorValue = blackScreenSprite.color;
        originalColorValue = aColorValue;
        TempC = aColorValue;

        MainMenu();
    }
    // Update is called once per frame
    void Update()
    {
        if(!fadeToBlackStarted){
            if(menuState == 1 || menuState == 3){
                if (Input.anyKey){
                    if(DebugManager.current.testingMode){
                        GameController.current.Invoke("TestGame", 0.01f);
                    }
                    else{
                        GameController.current.Invoke("NewGame", 0.01f);
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


    public void MainMenu(){
        if(musicPlaying){
            audio.clip = menuMusic;
            audio.Play();
        }
        FadeFromBlack();
        mainMenuScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        menuState = 1;
    }
    public void GameOver(){
        if(musicPlaying){
            audio.clip = gameOverMusic;
            audio.Play();
        }
        FadeFromBlack();
        mainMenuScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        menuState = 3;
    }
    public void NewGame(){
        FadeFromBlack();
        if(musicPlaying){
            audio.clip = abyssMusic;
            audio.Play();
        }
        mainMenuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        menuState = 2;
    }
    public void BossMusic(){
        if(musicPlaying){
            audio.clip = bossMusic;
            audio.Play();
        }
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
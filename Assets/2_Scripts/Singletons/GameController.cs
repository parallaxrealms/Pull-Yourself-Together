using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController current;

    public string sceneChangeName;
    public GameObject triggerSpawn;
    public string triggerSpawnName;

    public bool gameStarted = false;
    public bool playerSpawned = false;
    public bool playerRespawning = false;

    public GameObject UI_Object;

    public List<GameObject> ListWorms = new List<GameObject>();
    public List<GameObject> ListBuzzers = new List<GameObject>();
    public List<GameObject> ListBots = new List<GameObject>();
    public List<GameObject> ListBosses = new List<GameObject>();

    public List<GameObject> ListDoors = new List<GameObject>();
    public List<GameObject> ListCrystals = new List<GameObject>();
    public List<GameObject> ListPickups = new List<GameObject>();

    public bool hasBody;
    public bool hasLegs;
    public bool hasGun;
    public bool hasDrill;

    public int gunType;
    public int legType;

    public GameObject prevBotOwner;

    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }
    
    public event Action onEnableDebug;
    public void EnableDebug(){
        if(onEnableDebug != null){
            onEnableDebug();
        }
    }

    public event Action onDisableDebug;
    public void DisableDebug(){
        if(onDisableDebug != null){
            onEnableDebug();
        }
    }

    public event Action onPauseGame;
    public void PauseGame(){
        if(onPauseGame != null){
            onPauseGame();
        }
    }

    public event Action onResumeGame;
    public void ResumeGame(){
        if(onResumeGame != null){
            onResumeGame();
        }
    }



    public void GetUI(){
        Invoke("EnableUI", 0.01f);
    }

    public void NewGame(){
        if(!gameStarted){
            MenuManager.current.Invoke("NewGame", 0.01f);
            SceneManager.LoadScene("TheAbyss_0");
            gameStarted = true;
            Invoke("HighlightPickups",2f);
        }
    }

    public void TestGame(){
        if(!gameStarted){
            MenuManager.current.Invoke("NewGame", 0.01f);
            SceneManager.LoadScene("TestLevel");
            gameStarted = true;
        }
    }

    public void GameOver(){
        gameStarted = false;
        playerSpawned = false;
        DestroyAllEnemyObjects();
        SceneManager.LoadScene("MainMenu");
        MenuManager.current.Invoke("GameOver", 0.1f);
    }

    public void BossFightStarted(){
        MusicManager.current.currentTrackNum = 6;
        MusicManager.current.Invoke("PlayMusic", 0.1f);
    }

    public void EnableUI(){
        UI_Object.SetActive(true);
        gameStarted = true;
    }

    public void DisableUI(){
        UI_Object.SetActive(false);
    }

    public void ResetEnemyPlayerObjects(){
        foreach (GameObject worm in ListWorms)
        {
            EnemyScript enemyScript = worm.GetComponent<EnemyScript>();
            enemyScript.Invoke("FindPlayer", 0.01f);
        }

        foreach (GameObject buzzer in ListBuzzers)
        {
            EnemyScript enemyScript = buzzer.GetComponent<EnemyScript>();
            enemyScript.Invoke("FindPlayer", 0.01f);
        }

        foreach (GameObject bot in ListBots)
        {
            LostBotMeta botScript = bot.GetComponent<LostBotMeta>();
            botScript.Invoke("FindPlayer", 0.01f);
        }

        foreach (GameObject boss in ListBosses)
        {
            CyberMantisScript bossScript = boss.GetComponent<CyberMantisScript>();
            bossScript.Invoke("FindPlayer", 0.01f);
        }
    }

    public void DestroyClosestBotAfterSpawn(){
        GameObject prevBot = prevBotOwner;
        Debug.Log("prevbot: " + prevBotOwner);
        LostBotMeta prevBotScript = prevBot.GetComponent<LostBotMeta>();
        prevBotScript.Invoke("LoseLegs", 0.01f);
        prevBotScript.Invoke("LoseDrill", 0.01f);
        prevBotScript.Invoke("LoseGun", 0.01f);
        prevBotScript.Invoke("LoseBody", 0.01f);
        GameController.current.ListBots.Remove(prevBotOwner);
        Destroy(prevBotOwner);
    }

    public void InactivateEnemies(){
        foreach (GameObject worm in ListWorms)
        {
            EnemyScript enemyScript = worm.GetComponent<EnemyScript>();
            enemyScript.Invoke("Inactive", 0.01f);
        }

        foreach (GameObject buzzer in ListBuzzers)
        {
            EnemyScript enemyScript = buzzer.GetComponent<EnemyScript>();
            enemyScript.Invoke("Inactive", 0.01f);
        }

        foreach (GameObject bot in ListBots)
        {
            LostBotMeta botScript = bot.GetComponent<LostBotMeta>();
            botScript.Invoke("Inactive", 0.01f);
        }
        foreach (GameObject boss in ListBosses)
        {
            CyberMantisScript bossScript = boss.GetComponent<CyberMantisScript>();
            bossScript.Invoke("Inactive", 0.01f);
        }
    }

    public void HighlightPickups(){
        foreach (GameObject pickup in ListPickups)
        {
            PickUpScript pickupScript = pickup.GetComponent<PickUpScript>();

            if(pickupScript.pickupType == 0 || pickupScript.pickupType == 9){ //Head Reset
                if(!pickupScript.isUsed){
                    pickupScript.Invoke("EnablePickupParticles", 0.01f);
                }
            }
            if(pickupScript.pickupType == 1){ //Body
                if(hasBody == false){
                    pickupScript.Invoke("EnablePickupParticles", 0.01f);
                }
                else{
                    pickupScript.Invoke("DisablePickupParticles", 0.01f);
                    pickupScript.Invoke("DrawOutline", 0.01f);
                }
            }
            if(pickupScript.pickupType == 2){ //Drill
                if(hasBody == true){
                    if(hasDrill == false){
                        pickupScript.Invoke("EnablePickupParticles", 0.01f);
                    }
                    else{
                        pickupScript.Invoke("DisablePickupParticles", 0.01f);
                        pickupScript.Invoke("DrawOutline", 0.01f);
                    }
                }
                else{
                    pickupScript.Invoke("DisablePickupParticles", 0.01f);
                }
            }
            if(pickupScript.pickupType == 3){ //Gun
                if(hasBody == true){
                    if(hasGun == false){
                        pickupScript.Invoke("EnablePickupParticles", 0.01f);
                    }
                    else{
                        pickupScript.Invoke("DisablePickupParticles", 0.01f);
                        pickupScript.Invoke("DrawOutline", 0.01f);
                    }
                }
                else{
                    pickupScript.Invoke("DisablePickupParticles", 0.01f);
                }
            }
            if(pickupScript.pickupType == 4){ //Legs
                if(hasBody == true){
                    if(hasLegs == false){
                        pickupScript.Invoke("EnablePickupParticles", 0.01f);
                    }
                    else{
                        pickupScript.Invoke("DisablePickupParticles", 0.01f);
                        pickupScript.Invoke("DrawOutline", 0.01f);
                    }
                }
                else{
                    pickupScript.Invoke("DisablePickupParticles", 0.01f);
                }
            }
        }
    }

    public void RebootFromCheckpoint(){
        DestroyAllEnemyObjects();
        ResetEnemyPlayerObjects();
        EnableUI();
        HighlightPickups();
        if(prevBotOwner != null){
            DestroyClosestBotAfterSpawn();
        }
    }

    public void DestroyAllEnemyObjects(){
        ListWorms.Clear();
        ListBuzzers.Clear();
        ListBots.Clear();
        ListBosses.Clear();
        ListPickups.Clear();
    }

    public void ChangeSceneTo(){
        SceneManager.LoadScene(sceneChangeName);
        sceneChangeName = null;
    }

    public void TransitionToSceneChange(){
        MenuManager.current.Invoke("ChangeSceneTo", 0.1f);
    }
}

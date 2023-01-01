using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController current;

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
    public bool newScene = true;

    public GameObject UI_Object;

    public List<GameObject> ListWorms = new List<GameObject>();
    public List<GameObject> ListBuzzers = new List<GameObject>();
    public List<GameObject> ListBots = new List<GameObject>();
    public List<GameObject> ListBosses = new List<GameObject>();

    public List<GameObject> ListCrystals = new List<GameObject>();
    public List<GameObject> ListPickups = new List<GameObject>();

    public bool hasBody;
    public bool hasLegs;
    public bool hasGun;
    public bool hasDrill;

    public int gunType;
    public int legType;

    public GameObject prevBotOwner;

    public bool playerFellAbyss;

    public bool damageNumOption = true;

    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;

        gameCamera = GameObject.Find("Camera");
        camOriginalPos = gameCamera.transform.position;

        crystalManager = GameObject.Find("CrystalManager");
        crystalManagerScript = crystalManager.GetComponent<UI_CrystalManager>();

        DOTween.Init();
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

    private void ResetGameController(){
        init_CoL_0 = false;
        init_CoL_1 = false;
        init_CoL_2 = false;
        init_Abyss_0 = false;
        init_Abyss_1 = false;
        init_Abyss_Boss = false;
        crystalManagerScript.Reset();
    }

    public void NewGame(){
        if(!gameStarted){
            ResetGameController();
            MenuManager.current.NewGame();
            SceneManager.LoadScene("CoL_0");
            gameStarted = true;
            Invoke("HighlightPickups",1f);
        }
    }

    public void TestGame(){
        if(!gameStarted){
            MenuManager.current.NewGame();
            SceneManager.LoadScene("TestLevel");
            gameStarted = true;
        }
    }

    public void GameOver(){
        gameStarted = false;
        playerSpawned = false;
        DestroyAllEnemyObjects();
        SceneManager.LoadScene("MainMenu");
        MenuManager.current.GameOver();
        gameCamera.transform.position = camOriginalPos;
    }

    public void BossFightStarted(){
        AudioManager.current.currentTrackNum = 6;
        AudioManager.current.PlayMusicTrack();
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

    public void DestroyClosestBotAfterSpawn(){
        GameObject prevBot = prevBotOwner;
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

    public void HighlightPickups(){
        foreach (GameObject pickup in ListPickups)
        {
            PickUpScript pickupScript = pickup.GetComponent<PickUpScript>();

            if(pickupScript.pickupType == 0 || pickupScript.pickupType == 9){ //Head Reset
                if(!pickupScript.isUsed){
                    pickupScript.EnablePickupParticles();
                    pickupScript.DrawOutline();
                }
            }
            if(pickupScript.pickupType == 1){ //Body
                if(hasBody == false){
                    pickupScript.EnablePickupParticles();
                    pickupScript.DrawOutline();
                }
                else{
                    pickupScript.DisablePickupParticles();
                    pickupScript.EraseOutline();
                }
            }
            if(pickupScript.pickupType == 2){ //Drill
                if(hasBody == true){
                    if(hasDrill == false){
                        pickupScript.EnablePickupParticles();
                        pickupScript.DrawOutline();
                    }
                    else{
                        pickupScript.DisablePickupParticles();
                        pickupScript.EraseOutline();
                    }
                }
                else{
                    pickupScript.DisablePickupParticles();
                    pickupScript.EraseOutline();

                }
            }
            if(pickupScript.pickupType == 3){ //Gun
                if(hasBody == true){
                    if(hasGun == false){
                        pickupScript.EnablePickupParticles();
                        pickupScript.DrawOutline();
                    }
                    else{
                        if(pickupScript.gunType != PlayerManager.current.gunType){
                            pickupScript.EnablePickupParticles();
                            pickupScript.DrawOutline();
                        }
                        else{
                            pickupScript.DisablePickupParticles();
                            pickupScript.EraseOutline();
                        }
                    }
                }
                else{
                    pickupScript.DisablePickupParticles();
                    pickupScript.EraseOutline();

                }
            }
            if(pickupScript.pickupType == 4){ //Legs
                if(hasBody == true){
                    if(hasLegs == false){
                        pickupScript.EnablePickupParticles();
                        pickupScript.DrawOutline();
                    }
                    else{
                        if(pickupScript.legType != PlayerManager.current.legType){
                            pickupScript.EnablePickupParticles();
                            pickupScript.DrawOutline();
                        }
                        else{
                            pickupScript.DisablePickupParticles();
                            pickupScript.EraseOutline();
                        }
                    }
                }
                else{
                    pickupScript.DisablePickupParticles();
                    pickupScript.EraseOutline();

                }
            }
        }
    }

    // public void ResetSceneObjects(){
    //     if(!newScene){
    //        foreach (GameObject pickup in ListPickups.ToArray()){
    //             PickUpScript pickupScript = pickup.GetComponent<PickUpScript>();
    //             pickupScript.DestroySelf();
    //        }
    //        foreach (GameObject crystal in ListCrystals.ToArray()){
    //             CrystalScript crystalScript = crystal.GetComponent<CrystalScript>();
    //             crystalScript.DestroySelf();
    //        }
    //        DestroyAllEnemyObjects();
    //        ObjectManager.current.SpawnCachedPickups();
    //        ObjectManager.current.SpawnCachedCrystals();
    //     } 
    // }

    public void RebootFromCheckpoint(){
        EnableUI();
        if(prevBotOwner != null){
            DestroyClosestBotAfterSpawn();
        }
        ResetEnemyPlayerObjects();
        HighlightPickups();
    }

    public void DestroyAllEnemyObjects(){
        ListWorms.Clear();
        ListBuzzers.Clear();
        ListBots.Clear();
        ListBosses.Clear();
        ListPickups.Clear();
        ListCrystals.Clear();
    }

    public void ChangeSceneTo(){
        DestroyAllEnemyObjects();
        if(sceneChangeName != null){
            SceneManager.LoadScene(sceneChangeName, LoadSceneMode.Single);
            sceneChangeName = null;
        }
    }
    public void ChangeSceneAndReboot(){
        DestroyAllEnemyObjects();
        string backupScene = PlayerManager.current.backupSceneName;
        if(backupScene != null){
            SceneManager.LoadScene(backupScene, LoadSceneMode.Single);
            backupScene = null;
        }
    }

    public void PauseGame(){
        Time.timeScale = 0;
    }
    public void ResumeGame(){
        Time.timeScale = 1;
    }

    public void ScreenMode_Change(Int32 choice){
        if(choice == 0){
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if(choice == 1){
            Screen.fullScreen = false;
        }
        else if(choice == 2){
            Screen.SetResolution(1280, 720, false);
        }
    }

    public void ChangeDamageOption(bool enabled){
        if(enabled){
            damageNumOption = true;
        }
        else{
            damageNumOption = false;
        }
    }
}

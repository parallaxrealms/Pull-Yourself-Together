using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInit : MonoBehaviour
{
    public GameObject playerSpawnPoint;
    public string triggerSpawnName;

    public bool isFalling = false;
    public int sceneTrackNum;
    public bool backedUp = false;

    public int levelID;
    public bool levelVisited;

    void Awake(){
        triggerSpawnName = GameController.current.triggerSpawnName;
        MusicManager.current.currentTrackNum = sceneTrackNum;
        MusicManager.current.Invoke("PlayMusic",0.1f);

        MenuManager.current.currentLevelID = levelID;
        ObjectManager.current.currentLevelID = levelID;
        if(levelID == 1){
            if(!GameController.current.init_CoL_0){
                levelVisited = false;
                GameController.current.init_CoL_0 = true;
            }
            else{
                levelVisited = true;
            }
        }
        else if(levelID == 2){
            if(!GameController.current.init_CoL_1){
                levelVisited = false;
                GameController.current.init_CoL_1 = true;
            }
            else{
                levelVisited = true;
            }
        }
        else if(levelID == 3){
            if(!GameController.current.init_CoL_2){
                levelVisited = false;
                GameController.current.init_CoL_2 = true;
            }
            else{
                levelVisited = true;
            }
        }
        else if(levelID == 4){
            if(!GameController.current.init_Abyss_0){
                levelVisited = false;
                GameController.current.init_Abyss_0 = true;
            }
            else{
                levelVisited = true;
            }
        }
        else if(levelID == 5){
            if(!GameController.current.init_Abyss_1){
                levelVisited = false;
                GameController.current.init_Abyss_1 = true;
            }
            else{
                levelVisited = true;
            }
        }
        else if(levelID == 6){
            if(!GameController.current.init_Abyss_Boss){
                levelVisited = false;
                GameController.current.init_Abyss_Boss = true;
            }
            else{
                levelVisited = true;
            }
        }
        if(!levelVisited){
            GameController.current.newScene = true;
        }
        else{
            GameController.current.newScene = false;
        }
        GameController.current.Invoke("ResetScenePickups",0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Decide where to put the player when the scene loads
        if(!GameController.current.playerSpawned){            
            playerSpawnPoint = GameObject.Find("SpawnStartPos");

            PlayerManager.current.spawnPosition = playerSpawnPoint.transform.position;
            PlayerManager.current.Invoke("InitPlayer", 0.01f);
        }
        else if(GameController.current.playerRespawning){
            PlayerManager.current.spawnPosition = PlayerManager.current.backupSpawnPos;
            PlayerManager.current.Invoke("RebootPlayer", 0.01f);
            GameController.current.playerRespawning = false;
        }
        else{
            playerSpawnPoint = GameObject.Find(triggerSpawnName);

            if(PlayerManager.current.sceneDirection == "Left"){
                PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x - 15.0f,playerSpawnPoint.transform.position.y,playerSpawnPoint.transform.position.z);
            }
            if(PlayerManager.current.sceneDirection == "Right"){
                PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x + 15.0f,playerSpawnPoint.transform.position.y,playerSpawnPoint.transform.position.z);
            }
            if(PlayerManager.current.sceneDirection == "Down"){
                PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x,playerSpawnPoint.transform.position.y - 12.0f,playerSpawnPoint.transform.position.z);
            }
            if(PlayerManager.current.sceneDirection == "Up"){
                PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x,playerSpawnPoint.transform.position.y + 40.0f,playerSpawnPoint.transform.position.z);
            }
            PlayerManager.current.Invoke("RebootNewMap", 0.1f);
        }

        MenuManager.current.Invoke("ResetTitleCard", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

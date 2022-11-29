using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInit : MonoBehaviour
{

    public GameObject playerSpawnPoint;
    public string triggerSpawnName;

    public bool isFalling = false;

    void Awake(){
        triggerSpawnName = GameController.current.triggerSpawnName;
        MenuManager.current.Invoke("FadeFromBlack",0.1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!GameController.current.playerSpawned){            
            playerSpawnPoint = GameObject.Find("SpawnStartPos");

            PlayerManager.current.spawnPosition = playerSpawnPoint.transform.position;
            PlayerManager.current.Invoke("InitPlayer", 0.01f);
        }
        else{
            playerSpawnPoint = GameObject.Find(triggerSpawnName);

            if(PlayerManager.current.sceneDirection == "Left"){
                PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x - 10.0f,playerSpawnPoint.transform.position.y,playerSpawnPoint.transform.position.z);
            }
            if(PlayerManager.current.sceneDirection == "Right"){
                PlayerManager.current.spawnPosition = new Vector3(playerSpawnPoint.transform.position.x + 10.0f,playerSpawnPoint.transform.position.y,playerSpawnPoint.transform.position.z);
            }
            PlayerManager.current.Invoke("RebootNewMap", 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

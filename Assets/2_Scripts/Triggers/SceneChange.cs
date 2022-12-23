using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public string triggerName;
    public string triggerConnectionName;
    public string sceneToName;
    public string playerWalkDirection;

    public bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        triggerName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            if(!triggered){
                GameController.current.Invoke("SetCurrentScenePos", 0.01f);
                PlayerManager.current.sceneDirection = playerWalkDirection;
                GameController.current.sceneChangeName = sceneToName;
                GameController.current.triggerSpawnName = triggerConnectionName;
                ObjectManager.current.Invoke("SaveCurrentObjects",0.1f);
                MenuManager.current.Invoke("ChangeSceneTo", 0.01f);
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player"){
            
        }
    }
}

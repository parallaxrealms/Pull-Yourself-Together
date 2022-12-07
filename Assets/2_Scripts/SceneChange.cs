using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public string triggerName;
    public string triggerConnectionName;
    public string sceneToName;
    public string playerWalkDirection;

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
            PlayerManager.current.sceneDirection = playerWalkDirection;
            GameController.current.sceneChangeName = sceneToName;
            GameController.current.triggerSpawnName = triggerConnectionName;
            GameController.current.Invoke("TransitionToSceneChange", 0.01f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player"){
            
        }
    }
}

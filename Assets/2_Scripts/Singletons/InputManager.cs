using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager current;

    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            if(Input.GetKeyDown(KeyCode.D)){
                if(!DebugManager.current.enableDebugMode){
                    GameController.current.EnableDebug();
                }
                else{
                    GameController.current.DisableDebug();
                }
            }
        }
    }

}

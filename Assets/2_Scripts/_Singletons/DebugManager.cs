using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager current;

    public bool enableDebugMode;
    public bool musicPlaying;

    public bool testingMode = false;

    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameController.current.onEnableDebug += OpenDebugPanel;
        GameController.current.onDisableDebug += CloseDebugPanel;
    }
    
    private void OpenDebugPanel(){
       enableDebugMode = true;
    }

    private void CloseDebugPanel(){
       enableDebugMode = false;
    }

}

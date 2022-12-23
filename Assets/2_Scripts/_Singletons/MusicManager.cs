using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager current;
    private AudioSource audio;
    
    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}

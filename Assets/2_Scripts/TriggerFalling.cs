using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFalling : MonoBehaviour
{

    private bool entered;
    // Start is called before the first frame update
    void Start()
    {
        entered = GameController.current.playerFellAbyss;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            if(!entered){
                PlayerManager.current.Invoke("TakeHardHit",0.01f);
                GameController.current.playerFellAbyss = true;
            }
        }
    }
}

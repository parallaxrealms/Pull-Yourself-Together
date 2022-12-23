using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostBotDetectScript : MonoBehaviour
{
    public GameObject parentObject;
    public LostBotScript parentScript;

    void Awake()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<LostBotScript>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(parentScript.activated){
            if(parentScript.hasGun){
                if(other.gameObject.tag == "Player"){
                    parentScript.Invoke("FindPlayer",0.1f);
                    parentScript.Invoke("AimAtPlayer",0.2f);
                }
            }
            else if(parentScript.hasDrill){
                if(other.gameObject.tag == "Player"){
                    parentScript.Invoke("FindPlayer",0.1f);
                    parentScript.Invoke("ChasePlayer",0.2f);
                }
            }
            else{
                if(other.gameObject.tag == "Player"){
                    parentScript.Invoke("FindPlayer",0.1f);
                    parentScript.Invoke("MoveAwayFromPlayer",0.2f);
                }
            }
        }
        else{
            if(other.gameObject.tag == "Player"){
                parentScript.Invoke("FindPlayer",0.1f);
                parentScript.Invoke("Activated",0.02f);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(!parentScript.idle){
            if(other.gameObject.tag == "Player"){
                parentScript.Invoke("FindPlayer",0.1f);
                parentScript.Invoke("IdleState",0.2f);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(parentScript.activated){
            if(parentScript.hasGun){
                if(other.gameObject.tag == "Player"){
                    parentScript.Invoke("FindPlayer",0.1f);
                    parentScript.Invoke("AimAtPlayer",0.2f);
                }
            }
            else if(parentScript.hasDrill){
                if(other.gameObject.tag == "Player"){
                    parentScript.Invoke("FindPlayer",0.1f);
                    parentScript.Invoke("ChasePlayer",0.2f);
                }
            }
            else{
                if(other.gameObject.tag == "Player"){
                    parentScript.Invoke("FindPlayer",0.1f);
                    parentScript.Invoke("MoveAwayFromPlayer",0.2f);
                }
            }
        }
    }
}

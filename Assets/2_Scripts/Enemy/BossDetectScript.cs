using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetectScript : MonoBehaviour
{
    public GameObject parentObject;
    public CyberMantisScript parentScript;

    void Awake()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<CyberMantisScript>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            parentScript.Invoke("ChasePlayer",0.1f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(!parentScript.idle){
            if(other.gameObject.tag == "Player"){
                parentScript.Invoke("IdleState",0.1f);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "Player"){
            parentScript.Invoke("ChasePlayer",0.1f);
        }
    }
}

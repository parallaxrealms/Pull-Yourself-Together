using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDetectPlayer : MonoBehaviour
{
    public GameObject parentObject;
    public SpawnerScript parentScript;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<SpawnerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            parentScript.activated = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player"){
            parentScript.activated = false;
        }
    }
}

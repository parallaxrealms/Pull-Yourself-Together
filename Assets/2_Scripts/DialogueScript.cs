using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    public GameObject parentObject;
    public BlueBotScript parentScript;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<BlueBotScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DialogueDone()
    {
        parentScript.Invoke("DialogueDone", 0.01f);
    }
}
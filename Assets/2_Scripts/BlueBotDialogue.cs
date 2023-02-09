using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBotDialogue : MonoBehaviour
{
    private GameObject parentObject;
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

    public void FadeToCredits()
    {
        MenuManager.current.RollCredits();
        parentScript.ActivateEndingWalk();
        Destroy(gameObject);
    }
}

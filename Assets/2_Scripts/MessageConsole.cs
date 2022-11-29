using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageConsole : MonoBehaviour
{

    public GameObject messagePanel;
    public GameObject messagePanelObject;
    public SpriteRenderer message_spriteRend;
    public Vector3 messagePos;

    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        messagePos = new Vector3(transform.position.x, transform.position.y + 2f, 0);

        messagePanelObject = Instantiate(messagePanel, messagePos, Quaternion.identity) as GameObject;

        message_spriteRend = messagePanelObject.GetComponent<SpriteRenderer>();

        message_spriteRend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TurnConsoleOn(){
        message_spriteRend.enabled = true;
    }

    void TurnConsoleOff(){
        message_spriteRend.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            TurnConsoleOn();
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag =="Player"){
            TurnConsoleOff();
        }
    }
}

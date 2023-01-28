using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageConsole : MonoBehaviour
{
    private Animator anim;
    public GameObject messagePanel;
    public GameObject messagePanelObject;
    public SpriteRenderer message_spriteRend;
    public Vector3 messagePos;

    public GameObject ui_read_message;
    public SpriteRenderer ui_rend;

    public bool showingMessage = false;
    public bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        messagePos = new Vector3(transform.position.x, transform.position.y + 2.2f, 0);

        messagePanelObject = Instantiate(messagePanel, messagePos, Quaternion.identity) as GameObject;

        message_spriteRend = messagePanelObject.GetComponent<SpriteRenderer>();

        message_spriteRend.enabled = false;
        ui_rend = ui_read_message.GetComponent<SpriteRenderer>();
        ui_rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")){
            if(isActivated){
                TurnConsoleOn();
            }
        }
    }

    void TurnConsoleOn(){
        message_spriteRend.enabled = true;
        anim.SetBool("activated", true);

        AudioManager.current.currentSFXTrack = 0;
        AudioManager.current.PlaySfx();
    }

    void TurnConsoleOff(){
        message_spriteRend.enabled = false;
        anim.SetBool("activated", false);
    }

    void ActivateConsole(){
        ui_rend.enabled = true;
        isActivated = true;
    }
    void DeActivateConsole(){
        ui_rend.enabled = false;
        isActivated = false;
        TurnConsoleOff();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            ActivateConsole();
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag =="Player"){
            DeActivateConsole();
        }
    }
}

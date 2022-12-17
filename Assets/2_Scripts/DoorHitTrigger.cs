using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHitTrigger : MonoBehaviour
{
    public BoxCollider collider;
    public GameObject parentObject;
    public DoorTriggeredScript parentScript;

    public SpriteRenderer spriteRend;
    public Sprite coriteRend;
    public Sprite coriteRend_Active;
    public Sprite velriteRend;
    public Sprite velriteRend_Active;
    public Sprite nymriteRend;
    public Sprite nymriteRend_Active;
    public Sprite zyriteRend;
    public Sprite zyriteRend_Active;

    public GameObject triggerLight;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<DoorTriggeredScript>();

        triggerLight.SetActive(true);

        spriteRend = GetComponent<SpriteRenderer>();

        if(parentScript.doorType == 0){
            spriteRend.sprite = coriteRend;
            parentScript.staysOpen = false;
        }
        if(parentScript.doorType == 1){
            spriteRend.sprite = velriteRend;
            parentScript.staysOpen = true;
        }
        if(parentScript.doorType == 2){
            spriteRend.sprite = nymriteRend;
            parentScript.staysOpen = true;
        }
        if(parentScript.doorType == 3){
            spriteRend.sprite = zyriteRend;
            parentScript.staysOpen = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player_Bullet"){
            BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();

            if(parentScript.doorType == 0){
                parentScript.Invoke("OpenDoor", 0.01f);
                OpenDoor();
                collider.enabled = false;
                triggerLight.SetActive(false);
            }
            else if(parentScript.doorType == bulletScript.bulletType){
                parentScript.Invoke("OpenDoor", 0.01f);
                OpenDoor();
                collider.enabled = false;
                triggerLight.SetActive(false);
            }
        }
    }

    private void OpenDoor(){
        if(parentScript.doorType == 0){
            spriteRend.sprite = coriteRend_Active;
        }
        if(parentScript.doorType == 1){
            spriteRend.sprite = velriteRend_Active;
        }
        if(parentScript.doorType == 2){
            spriteRend.sprite = nymriteRend_Active;
        }
        if(parentScript.doorType == 3){
            spriteRend.sprite = zyriteRend_Active;
        }
    }

    public void ResetTrigger(){
        collider.enabled = true;
        triggerLight.SetActive(true);
    }
}

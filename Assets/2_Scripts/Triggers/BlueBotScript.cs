using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBotScript : MonoBehaviour
{
    public Animator anim;

    public GameObject dialogueBubble;
    public Animator dialogueAnim;

    public GameObject explodeParticlesObject;
    public GameObject explodeParticles;

    public GameObject legsDropObject;
    public GameObject bodyDropObject;
    public GameObject gunDropObject;
    public GameObject drillDropObject;

    public GameObject cyberMantis;
    public CyberMantisScript cyberMantisScript;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dialogueAnim = dialogueBubble.GetComponent<Animator>();

        cyberMantisScript = cyberMantis.GetComponent<CyberMantisScript>();

        dialogueBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy"){
            Explode();
        }
    }

    public void ActivateDialogue(){
        PlayerManager.current.Invoke("PauseMovement", 0.01f);
        dialogueBubble.SetActive(true);
        dialogueAnim.SetBool("activated", true);
    }

    public void DialogueDone(){
        dialogueBubble.SetActive(false);
        cyberMantisScript.Invoke("BeginFalling", 0.01f);
    }

    public void Explode(){
        anim.SetBool("explode", true);
        explodeParticles = Instantiate(explodeParticlesObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;

        GameObject newJumpLegs = Instantiate(legsDropObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;            
        PickUpScript pickUpScript0 = newJumpLegs.GetComponent<PickUpScript>();
        pickUpScript0.pickupType = 4;
        pickUpScript0.Invoke("DropNewPickup", 0.01f);

        GameObject newWorkerBody = Instantiate(bodyDropObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;            
        PickUpScript pickUpScript1 = newWorkerBody.GetComponent<PickUpScript>();
        pickUpScript1.pickupType = 1;
        pickUpScript1.Invoke("DropNewPickup", 0.01f);

        GameObject newWorkerDrill = Instantiate(drillDropObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;            
        PickUpScript pickUpScript2 = newWorkerDrill.GetComponent<PickUpScript>();
        pickUpScript2.pickupType = 2;
        pickUpScript2.Invoke("DropNewPickup", 0.01f);

        GameObject newAutoGun = Instantiate(gunDropObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;            
        PickUpScript pickUpScript3 = newAutoGun.GetComponent<PickUpScript>();
        pickUpScript3.pickupType = 3;
        pickUpScript3.Invoke("DropNewPickup", 0.01f);
    }

    public void DestroySelf(){
        cyberMantisScript.Invoke("DoneFalling", 0.1f);
        Destroy(gameObject);
    }
}

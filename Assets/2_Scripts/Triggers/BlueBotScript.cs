using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBotScript : MonoBehaviour
{
    public bool endMode;
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

    public bool endingMovement = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dialogueAnim = dialogueBubble.GetComponent<Animator>();

        if (endMode)
        {

        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (endingMovement)
        {
            transform.position += new Vector3(1.0f * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Explode();
        }
    }

    public void ActivateEndingWalk()
    {
        transform.localScale = new Vector3(1, 1, 1);
        anim.SetBool("isWalking", true);
        PlayerManager.current.WalkEndingMovement();
        endingMovement = true;
    }

    public void ActivateDialogue()
    {
        if (!endMode)
        {
            cyberMantisScript = GameObject.Find("CyberMantis").GetComponent<CyberMantisScript>();

            PlayerManager.current.Invoke("PauseMovement", 0.01f);
            GameController.current.playerMetBlueBot = true;
            dialogueBubble.SetActive(true);
            dialogueAnim.SetBool("activated", true);
        }
        else
        {
            dialogueBubble.SetActive(true);
        }

        AudioManager.current.currentSFXTrack = 0;
        AudioManager.current.PlaySfx();
    }

    public void DialogueDone()
    {
        dialogueBubble.SetActive(false);
        cyberMantisScript.Invoke("BeginFalling", 0.01f);
    }

    public void Explode()
    {
        AudioManager.current.currentSFXTrack = 120;
        AudioManager.current.PlaySfx();


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

    public void DestroySelf()
    {
        cyberMantisScript.DoneFalling();
        Destroy(gameObject);
    }
    public void RemoveSelf()
    {
        Destroy(gameObject);
    }
}

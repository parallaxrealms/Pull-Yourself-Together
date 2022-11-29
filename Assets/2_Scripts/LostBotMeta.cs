using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostBotMeta : MonoBehaviour
{
    public bool hasHead = true;
    public bool hasBody = false;
    public bool hasDrill = false;
    public bool hasGun = false;
    public bool hasLegs = false;

    public int gunType = 0;

    public GameObject lostBot0;
    public GameObject lostBot1;
    public GameObject lostBot2;
    public GameObject currentBotObject;
    public LostBotScript lostBotScript;

    // Start is called before the first frame update
    void Start()
    {
        if(hasLegs){
            currentBotObject = Instantiate(lostBot2, new Vector3(transform.position.x, transform.position.y,transform.position.z), Quaternion.identity) as GameObject;
            currentBotObject.transform.parent = transform;
        }
        else if(hasBody){
            currentBotObject = Instantiate(lostBot1, new Vector3(transform.position.x, transform.position.y,transform.position.z), Quaternion.identity) as GameObject;
            currentBotObject.transform.parent = transform;
        }
        else{
            currentBotObject = Instantiate(lostBot0, new Vector3(transform.position.x, transform.position.y,transform.position.z), Quaternion.identity) as GameObject;
            currentBotObject.transform.parent = transform;
        }
        lostBotScript = currentBotObject.GetComponent<LostBotScript>();
        lostBotScript.hasHead = hasHead;
        lostBotScript.hasBody = hasBody;
        lostBotScript.hasDrill = hasDrill;
        lostBotScript.hasGun = hasGun;
        lostBotScript.hasLegs = hasLegs;
        lostBotScript.gunType = gunType;

        GameController.current.ListBots.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void FindPlayer(){
        lostBotScript.playerObject = PlayerManager.current.currentPlayerObject;
    }

    public void ResetBot(){
        lostBotScript = currentBotObject.GetComponent<LostBotScript>();
        lostBotScript.hasHead = hasHead;
        lostBotScript.hasBody = hasBody;
        lostBotScript.hasDrill = hasDrill;
        lostBotScript.hasGun = hasGun;
        lostBotScript.hasLegs = hasLegs;
        lostBotScript.gunType = gunType;

        lostBotScript.activated = true;
    }

    public void LoseGun(){
        ResetBot();
    }

    public void LoseDrill(){
        ResetBot();
    }

    public void LoseBody(){
        Destroy(currentBotObject);
        currentBotObject = Instantiate(lostBot0, new Vector3(currentBotObject.transform.position.x, currentBotObject.transform.position.y,currentBotObject.transform.position.z), Quaternion.identity) as GameObject;
        currentBotObject.transform.parent = transform;
        ResetBot();
    }
    
    public void LoseLegs(){
        Destroy(currentBotObject);
        currentBotObject = Instantiate(lostBot1, new Vector3(currentBotObject.transform.position.x, currentBotObject.transform.position.y,currentBotObject.transform.position.z), Quaternion.identity) as GameObject;
        currentBotObject.transform.parent = transform;
        ResetBot();
    }

    public void DestroySelf(){
        GameController.current.ListBots.Remove(gameObject);
        Destroy(gameObject);
    }
}
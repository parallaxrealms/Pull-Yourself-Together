using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    private GameObject parentObject;
    private PickUpScript pickUpScript;

    public float spawnTimer = .7f;
    public bool isSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        pickUpScript = parentObject.GetComponent<PickUpScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(isSpawning){
            if(spawnTimer > 0){
                spawnTimer -= Time.deltaTime;
            }
            else{
                isSpawning = false;
                spawnTimer = .7f;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(!isSpawning){
            if(other.gameObject.tag == "Player"){
                if(pickUpScript.pickupType == 1){ //If this pickup is Body
                    if(PlayerManager.current.hasHead){
                        if(!PlayerManager.current.hasBody){
                            pickUpScript.activated = true;
                            pickUpScript.DefaultText();
                            pickUpScript.DrawOutline();
                        }
                        else{
                            pickUpScript.EraseOutline();
                        }
                    }
                }
                else if(pickUpScript.pickupType == 2){ //If this pickup is Drill
                    if(PlayerManager.current.hasBody){
                        if(!PlayerManager.current.hasDrill){
                            pickUpScript.activated = true;
                            pickUpScript.DefaultText();
                            pickUpScript.DrawOutline();
                        }
                        else{
                            pickUpScript.EraseOutline();
                        }
                    }
                }
                else if(pickUpScript.pickupType == 3){ //If this pickup is Gun
                    if(PlayerManager.current.hasBody){
                        if(!PlayerManager.current.hasGun){
                            pickUpScript.activated = true;
                            pickUpScript.DefaultText();
                            pickUpScript.DrawOutline();
                        }
                        else{
                            if(pickUpScript.gunType != PlayerManager.current.gunType){
                                pickUpScript.activated = true;
                                pickUpScript.DefaultText();
                                pickUpScript.DrawOutline();
                            }
                            else{
                                pickUpScript.EraseOutline();
                            }
                        }
                    }
                }
                else if(pickUpScript.pickupType == 4){ //If this pickup is Legs
                    if(PlayerManager.current.hasBody){
                        if(!PlayerManager.current.hasLegs){
                            pickUpScript.activated = true;
                            pickUpScript.DefaultText();
                            pickUpScript.DrawOutline();
                        }
                        else{
                            if(pickUpScript.legType != PlayerManager.current.legType){
                                pickUpScript.activated = true;

                                pickUpScript.DefaultText();
                                pickUpScript.DrawOutline();
                            }
                        }
                    }
                }
                else if(pickUpScript.pickupType == 0 || pickUpScript.pickupType == 9){ //If this pickup is Backup
                    if(PlayerManager.current.hasHead){
                        if(!pickUpScript.isUsed){
                            pickUpScript.activated = true;
                            pickUpScript.DefaultText();
                            pickUpScript.DrawOutline();
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player"){
            pickUpScript.activated = false;
            pickUpScript.EraseText();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRespawnDestroyNearby : MonoBehaviour
{
    public bool destroyTimerOn = false;
    public float destroyTimer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        destroyTimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyTimerOn){
            if(destroyTimer > 0){
                destroyTimer -= Time.deltaTime;
            }
            else{
                DestroySelf();
                destroyTimerOn = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(gameObject.tag == "Player"){
            if(other.gameObject.tag == "Interactable_Bot"){
                destroyTimerOn = false;
                PickUpScript pickupScript = other.gameObject.GetComponent<PickUpScript>();
                pickupScript.Invoke("DestroySelf", 0.1f);
                DestroySelf();
            }
        }
        else if(gameObject.tag == "Respawn"){
            if(other.gameObject.tag == "Enemy"){
                destroyTimerOn = false;
                LostBotScript lostBotScript = other.gameObject.GetComponent<LostBotScript>();
                lostBotScript.Invoke("DestroySelf", 0.1f);
                DestroySelf();
            }
        }
    }

    private void DestroySelf(){
        Destroy(gameObject);
    }
}

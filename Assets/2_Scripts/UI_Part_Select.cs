using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_Select : MonoBehaviour
{
    public int partType; //0= head, 1 = body, 2 = r_arm, 3 = l_arm, 4 = legs
    public BoxCollider collider;
    private SpriteRenderer spriteRend;

    public bool partEnabled;

    public GameObject activeHeldObject;
    public SpriteRenderer objectSprite;
    public Sprite object1;
    public Sprite object2;
    public Sprite object3;

    public GameObject dropButton;
    public GameObject infoButton;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        spriteRend = GetComponent<SpriteRenderer>(); 
        objectSprite = activeHeldObject.GetComponent<SpriteRenderer>();   

        disablePartUI();

        if(partType == 0){
            partEnabled = true;
            objectSprite.sprite = object1;
        }
        else{
            objectSprite.sprite = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void enablePartUI(){
        if(partEnabled){
            objectSprite.enabled = true;
            collider.enabled = true;   
            spriteRend.enabled = true;
        }
    }
    private void disablePartUI(){
        objectSprite.enabled = false;
        collider.enabled = false;  
        spriteRend.enabled = false;
    }

    public void GainWorkerHead(){
        partEnabled = true;
        objectSprite.sprite = object1;
    }
    public void GainWorkerBody(){
        partEnabled = true;
        objectSprite.sprite = object1;
    }
    public void GainWorkerDrill(){
        partEnabled = true;
        objectSprite.sprite = object1;
    }
    public void GainBlaster(){
        partEnabled = true;
        objectSprite.sprite = object1;
    }
    public void GainMissileLauncher(){
        partEnabled = true;
        objectSprite.sprite = object2;
    }
    public void GainLaserBeam(){
        partEnabled = true;
        objectSprite.sprite = object3;
    }
    public void GainWorkerBoots(){
        partEnabled = true;
        objectSprite.sprite = object1;
    }
    public void GainJumpBoots(){
        partEnabled = true;
        objectSprite.sprite = object2;
    }

    public void LoseWorkerHead(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
    public void LoseWorkerBody(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
    public void LoseWorkerDrill(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
    public void LoseBlaster(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
    public void LoseMissileLauncher(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
    public void LoseLaserBeam(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
    public void LoseWorkerBoots(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
    public void LoseJumpBoots(){
        partEnabled = false;
        objectSprite.sprite = null;
    }
}

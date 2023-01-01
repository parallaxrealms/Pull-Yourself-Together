using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_Select : MonoBehaviour
{
    public UI_Parts parentScript;
    public int partType; //0= head, 1 = body, 2 = r_arm, 3 = l_arm, 4 = legs
    public int partSubType;

    public int progress1;
    public int progress2;
    public int progressNum;

    public BoxCollider collider;
    private SpriteRenderer spriteRend;

    public GameObject windowInfoObject;
    public GameObject windowInfo;
    public UI_Window_Part windowInfoScript;
    private Vector3 windowPos;
    private Vector3 windowClosedPos;
    public bool windowOpen;

    public GameObject windowInfo_type1;
    public GameObject windowInfo_type2;
    public GameObject windowInfo_type3;

    public Sprite bg_active;
    public Sprite bg_inactive;

    public bool partEnabled;

    public GameObject activeHeldObject;
    public SpriteRenderer objectSprite;
    public Sprite object1;
    public Sprite object2;
    public Sprite object3;

    public GameObject dropButton;

    public SpriteRenderer buttonSpriteRend;
    public BoxCollider buttonCollider;
    public PartButtonScript buttonScript;

    public GameObject selfDButton;

    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<UI_Parts>();
        collider = GetComponent<BoxCollider>();
        spriteRend = GetComponent<SpriteRenderer>(); 

        objectSprite = activeHeldObject.GetComponent<SpriteRenderer>();
        windowClosedPos = new Vector3(0,-100f,0);

        windowInfo = Instantiate(windowInfoObject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        windowPos = new Vector3(5.5f,0f,-1f);
        windowInfo.transform.parent = transform;
        windowInfo.transform.localPosition = windowPos;
        windowInfoScript = windowInfo.GetComponent<UI_Window_Part>();
        windowInfo.transform.position = windowClosedPos;

        if(partType == 0){
            partEnabled = true;
            objectSprite.sprite = object1;

            buttonSpriteRend = selfDButton.GetComponent<SpriteRenderer>();
            buttonCollider = selfDButton.GetComponent<BoxCollider>();
            buttonScript = selfDButton.GetComponent<PartButtonScript>();
        }
        else{
            objectSprite.sprite = null;

            buttonSpriteRend = dropButton.GetComponent<SpriteRenderer>();
            buttonCollider = dropButton.GetComponent<BoxCollider>();
            buttonScript = dropButton.GetComponent<PartButtonScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPart(){
        Destroy(windowInfo);
        windowInfo = null;
        
        objectSprite = activeHeldObject.GetComponent<SpriteRenderer>();

        if(partType == 3){//Guns
            if(partSubType == 0){
                objectSprite.sprite = object1;
                windowInfoObject = windowInfo_type1;
            }
            if(partSubType == 1){
                objectSprite.sprite = object2;
                windowInfoObject = windowInfo_type2;
            }
            if(partSubType == 2){
                objectSprite.sprite = object3;
                windowInfoObject = windowInfo_type3;
            }
        }
        if(partType == 4){//Legs
            if(partSubType == 0){
                objectSprite.sprite = object1;
                windowInfoObject = windowInfo_type1;
            }
            if(partSubType == 1){
                objectSprite.sprite = object2;
                windowInfoObject = windowInfo_type2;
            }
        }

        windowInfo = Instantiate(windowInfoObject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        windowPos = new Vector3(5.5f,0f,-1f);
        windowInfo.transform.parent = transform;
        windowInfo.transform.localPosition = windowClosedPos;
        windowInfoScript = windowInfo.GetComponent<UI_Window_Part>();

        partEnabled = true;

        if(partType != 0){
            buttonSpriteRend = dropButton.GetComponent<SpriteRenderer>();
            buttonCollider = dropButton.GetComponent<BoxCollider>();
            buttonScript = dropButton.GetComponent<PartButtonScript>();
        }
        if(windowOpen){
            CloseWindow(); 
        }
    }

    public void enablePartUI(){
        if(partEnabled){
            spriteRend.sprite = bg_inactive;
            objectSprite.enabled = true;
            collider.enabled = true;   
            spriteRend.enabled = true;
            
            buttonSpriteRend.enabled = true;
            buttonCollider.enabled = true;
        }
    }
    public void disablePartUI(){
        if(objectSprite != null){
            objectSprite.enabled = false;
            collider.enabled = false;  
            spriteRend.enabled = false;

            buttonSpriteRend.enabled = false;
            buttonCollider.enabled = false;
            if(windowOpen){
                CloseWindow();
            }
        }
    }

    public void DropPart(){
        TransferPartProperties();
        DropPartDisableUI();
        if(partType == 0){//Head
            parentScript.DropOtherParts();
            PlayerManager.current.DropAllParts();
            PlayerManager.current.Death();
            parentScript.DisablePartsUI();
        }
        else if(partType == 1){//Body
            if(PlayerManager.current.hasLegs || PlayerManager.current.hasDrill 
            || PlayerManager.current.hasGun){
                parentScript.DropOtherParts();
                PlayerManager.current.DropAllParts();
            }
            else{
                PlayerManager.current.DropBody();
            }
            parentScript.DisablePartsUI();
        }
        else if(partType == 2){//Drill
            PlayerManager.current.DropDrill();
            parentScript.DisablePartsUI();
        }
        else if(partType == 3){//Gun
            PlayerManager.current.DropGun();
            parentScript.DisablePartsUI();
        }
        else if(partType == 4){//Legs
            Debug.Log("DropPart Legs");
            PlayerManager.current.DropLegs();
            parentScript.DisablePartsUI();
        }
        Invoke("turnMouseOverOffDelayed", 0.1f);
    }

    public void TransferPartProperties(){
        progressNum = progress1 + progress2;
        if(partType == 1){
            PlayerManager.current.temp_body_progress1 = progress1;
            PlayerManager.current.temp_body_progress2 = progress2;
            PlayerManager.current.temp_body_progressNum = progressNum;
        }
        if(partType == 2){
            PlayerManager.current.temp_drill_progress1 = progress1;
            PlayerManager.current.temp_drill_progress2 = progress2;
            PlayerManager.current.temp_drill_progressNum = progressNum;
        }
        if(partType == 3){
            PlayerManager.current.temp_gun_progress1 = progress1;
            PlayerManager.current.temp_gun_progress2 = progress2;
            PlayerManager.current.temp_gun_progressNum = progressNum;
        }
        if(partType == 4){
            PlayerManager.current.temp_legs_progress1 = progress1;
            PlayerManager.current.temp_legs_progress2 = progress2;
            PlayerManager.current.temp_legs_progressNum = progressNum;
        }
    }

    public void GainWorkerHead(){
        partEnabled = true;
        objectSprite.sprite = object1;
        if(parentScript.isEnabled){
            enablePartUI();
        }
    }
    public void GainWorkerBody(){
        partEnabled = true;
        objectSprite.sprite = object1;
        if(partType != 0){
            dropButton.SetActive(true);
        }
        if(parentScript.isEnabled){
            enablePartUI();
        }
    }
    public void GainWorkerDrill(){
        partEnabled = true;
        objectSprite.sprite = object1;
        if(partType != 0){
            dropButton.SetActive(true);
        }
        if(parentScript.isEnabled){
            enablePartUI();
        }
    }
    public void GainBlaster(){
        partEnabled = true;
        partSubType = 0;
        objectSprite.sprite = object1;
        if(partType != 0){
            dropButton.SetActive(true);
        }
        if(parentScript.isEnabled){
            enablePartUI();
        }
        ResetPart();
    }
    public void GainMissileLauncher(){
        partEnabled = true;
        partSubType = 1;
        objectSprite.sprite = object2;
        if(partType != 0){
            dropButton.SetActive(true);
        }
        if(parentScript.isEnabled){
            enablePartUI();
        }
        ResetPart();
    }
    public void GainLaserBeam(){
        partEnabled = true;
        partSubType = 2;
        objectSprite.sprite = object3;
        if(partType != 0){
            dropButton.SetActive(true);
        }
        if(parentScript.isEnabled){
            enablePartUI();
        }
        ResetPart();
    }
    public void GainWorkerBoots(){
        partEnabled = true;
        partSubType = 0;
        objectSprite.sprite = object1;
        if(partType != 0){
            dropButton.SetActive(true);
        }
        if(parentScript.isEnabled){
            enablePartUI();
        }
        ResetPart();
    }
    public void GainJumpBoots(){
        partEnabled = true;
        partSubType = 1;
        objectSprite.sprite = object2;
        if(partType != 0){
            dropButton.SetActive(true);
        }
        if(parentScript.isEnabled){
            enablePartUI();
        }
        ResetPart();
    }

    public void DropPartDisableUI(){
        partEnabled = false;
        objectSprite.sprite = null;
        buttonSpriteRend.sprite = buttonScript.spriteInactive;
        if(partType != 0){
            dropButton.SetActive(false);
        }
        progress1 = 0;
        progress2 = 0;
        progressNum = 0;
    }

    public void OpenWindow(){
        windowInfo.transform.localPosition = windowPos;
        windowInfoScript.Reset();
        windowOpen = true;
    }

    public void CloseWindow(){
        if(windowOpen){
            windowInfo.transform.localPosition = windowClosedPos;
            windowOpen = false;
        }
        Invoke("turnMouseOverOffDelayed",0.2f);
    }
    public void turnMouseOverOffDelayed(){
        MenuManager.current.isMouseOver = false;
    }

    public void CloseAllWindows(){
        parentScript.CloseAllWindows();
    }

    private void OnMouseOver(){
        spriteRend.sprite = bg_active;
        MenuManager.current.isMouseOver = true;
    }

    private void OnMouseExit(){
        spriteRend.sprite = bg_inactive;
        MenuManager.current.isMouseOver = false;
    }

    private void OnMouseDown(){
        CloseAllWindows();
        OpenWindow();
    }
}

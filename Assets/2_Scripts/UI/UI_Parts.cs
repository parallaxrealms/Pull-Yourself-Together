using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parts : MonoBehaviour
{
    public GameObject CrystalManager;
    public UI_CrystalManager crystalManagerScript;
    
    public bool isEnabled = false;
    public SpriteRenderer spriteRend;

    public GameObject headPart;
    public UI_Part_Select headPartScript;

    public GameObject leftArmPart;
    public UI_Part_Select leftArmScript;

    public GameObject rightArmPart;
    public UI_Part_Select rightArmScript;

    public GameObject bodyPart;
    public UI_Part_Select bodyPartScript;

    public GameObject legsPart;
    public UI_Part_Select legsPartScript;

    public bool timerEnabled = false;
    public float timer = 3.0f; 

    public int temp_progress1;
    public int temp_progress2;
    public int temp_progressNum;
    
    // Start is called before the first frame update
    void Start()
    {
        CrystalManager = MenuManager.current.CrystalManager;
        crystalManagerScript = CrystalManager.GetComponent<UI_CrystalManager>();

        spriteRend = GetComponent<SpriteRenderer>();

        headPartScript = headPart.GetComponent<UI_Part_Select>();
        leftArmScript = leftArmPart.GetComponent<UI_Part_Select>();
        rightArmScript = rightArmPart.GetComponent<UI_Part_Select>();
        bodyPartScript = bodyPart.GetComponent<UI_Part_Select>();
        legsPartScript = legsPart.GetComponent<UI_Part_Select>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerEnabled){
            if(timer > 0){
                timer -= Time.deltaTime;
            }
            else{
                timerEnabled = false;   
                DisablePartsUI();
                timer = 5f;
            }
        }
        else{

        }
    }

    public void EndTimer(){
        // timerEnabled = false;   
        timer = 5f;
    }
    public void StartTimer(){
        // timerEnabled = true;   
        timer = 5f;
    }

    public void EnablePartsUI(){
        isEnabled = true;
        spriteRend.enabled = true;
        headPartScript.enablePartUI();
        leftArmScript.enablePartUI();
        rightArmScript.enablePartUI();
        bodyPartScript.enablePartUI();
        legsPartScript.enablePartUI();
        crystalManagerScript.ShowCrystalUI();
    }

    public void DisablePartsUI(){
        headPartScript.disablePartUI();
        leftArmScript.disablePartUI();
        rightArmScript.disablePartUI();
        bodyPartScript.disablePartUI();
        legsPartScript.disablePartUI();
        crystalManagerScript.HideCrystalUI();
        isEnabled = false;
        spriteRend.enabled = false;
    }
    public void CloseAllWindows(){
        headPartScript.CloseWindow();
        leftArmScript.CloseWindow();
        rightArmScript.CloseWindow();
        bodyPartScript.CloseWindow();
        legsPartScript.CloseWindow();
        MenuManager.current.isMouseOver = false;
    }

    public void TriggerAllPartsDrop(){
        if(PlayerManager.current.hasBody){
            bodyPartScript.DropPart();
        }
    }

    public void DropOtherParts(){
        if(PlayerManager.current.hasDrill){
            rightArmScript.DropPart();
        }
        if(PlayerManager.current.hasGun){
            leftArmScript.DropPart();
        }
        if(PlayerManager.current.hasLegs){
            legsPartScript.DropPart();
        }
    }

    public void TriggerLimbPartsDrop(){
        if(PlayerManager.current.hasDrill){
            rightArmScript.DropPart();
        }
        if(PlayerManager.current.hasGun){
            leftArmScript.DropPart();
        }
        if(PlayerManager.current.hasLegs){
            Debug.Log("TriggerLimbPartsDrop");
            legsPartScript.DropPart();
        }
    }

    public void GainWorkerHead(){
        headPartScript.progress1 = temp_progress1;
        headPartScript.progress2 = temp_progress2;
        headPartScript.progressNum = temp_progressNum;
        headPartScript.GainWorkerHead();
    }
    public void GainWorkerBody(){
        bodyPartScript.progress1 = temp_progress1;
        bodyPartScript.progress2 = temp_progress2;
        bodyPartScript.progressNum = temp_progressNum;
        bodyPartScript.GainWorkerBody();
    }
    public void GainWorkerDrill(){
        rightArmScript.progress1 = temp_progress1;
        rightArmScript.progress2 = temp_progress2;
        rightArmScript.progressNum = temp_progressNum;
        rightArmScript.GainWorkerDrill();
    }
    public void GainBlaster(){
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.GainBlaster();
    }
    public void GainMissileLauncher(){
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.GainMissileLauncher();
    }
    public void GainLaserBeam(){
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.GainLaserBeam();
    }
    public void GainWorkerBoots(){
        legsPartScript.progress1 = temp_progress1;
        legsPartScript.progress2 = temp_progress2;
        legsPartScript.progressNum = temp_progressNum;
        legsPartScript.GainWorkerBoots();
    }
    public void GainJumpBoots(){
        legsPartScript.progress1 = temp_progress1;
        legsPartScript.progress2 = temp_progress2;
        legsPartScript.progressNum = temp_progressNum;
        legsPartScript.GainJumpBoots();
    }
}

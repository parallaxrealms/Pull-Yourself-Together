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

        DisablePartsUI();
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
        headPartScript.Invoke("enablePartUI", 0.01f);
        leftArmScript.Invoke("enablePartUI", 0.01f);
        rightArmScript.Invoke("enablePartUI", 0.01f);
        bodyPartScript.Invoke("enablePartUI", 0.01f);
        legsPartScript.Invoke("enablePartUI", 0.01f);
        crystalManagerScript.Invoke("ShowCrystalUI", 0.1f);
    }

    public void DisablePartsUI(){
        isEnabled = false;
        spriteRend.enabled = false;
        headPartScript.Invoke("disablePartUI", 0.01f);
        leftArmScript.Invoke("disablePartUI", 0.01f);
        rightArmScript.Invoke("disablePartUI", 0.01f);
        bodyPartScript.Invoke("disablePartUI", 0.01f);
        legsPartScript.Invoke("disablePartUI", 0.01f);
        crystalManagerScript.Invoke("HideCrystalUI", 0.1f);
    }
    public void CloseAllWindows(){
        headPartScript.Invoke("CloseWindow", 0.01f);
        leftArmScript.Invoke("CloseWindow", 0.01f);
        rightArmScript.Invoke("CloseWindow", 0.01f);
        bodyPartScript.Invoke("CloseWindow", 0.01f);
        legsPartScript.Invoke("CloseWindow", 0.01f);
        MenuManager.current.isMouseOver = false;
    }
    public void PlayerDroppedParts(){
        if(PlayerManager.current.hasLegs){
            legsPartScript.Invoke("DropPartDisableUI", 0.01f);
        }
        if(PlayerManager.current.hasDrill){
            rightArmScript.Invoke("DropPartDisableUI", 0.01f);
        }
        if(PlayerManager.current.hasGun){
            leftArmScript.Invoke("DropPartDisableUI", 0.01f);
        }
        if(PlayerManager.current.hasBody){
            bodyPartScript.Invoke("DropPartDisableUI", 0.01f);
        }
    }

    public void GainWorkerHead(){
        headPartScript.progress1 = temp_progress1;
        headPartScript.progress2 = temp_progress2;
        headPartScript.progressNum = temp_progressNum;
        headPartScript.Invoke("GainWorkerHead", 0.01f);
    }
    public void GainWorkerBody(){
        bodyPartScript.progress1 = temp_progress1;
        bodyPartScript.progress2 = temp_progress2;
        bodyPartScript.progressNum = temp_progressNum;
        bodyPartScript.Invoke("GainWorkerBody", 0.01f);
    }
    public void GainWorkerDrill(){
        rightArmScript.progress1 = temp_progress1;
        rightArmScript.progress2 = temp_progress2;
        rightArmScript.progressNum = temp_progressNum;
        rightArmScript.Invoke("GainWorkerDrill", 0.01f);
    }
    public void GainBlaster(){
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.Invoke("GainBlaster", 0.01f);
    }
    public void GainMissileLauncher(){
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.Invoke("GainMissileLauncher", 0.01f);
    }
    public void GainLaserBeam(){
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.Invoke("GainLaserBeam", 0.01f);
    }
    public void GainWorkerBoots(){
        legsPartScript.progress1 = temp_progress1;
        legsPartScript.progress2 = temp_progress2;
        legsPartScript.progressNum = temp_progressNum;
        legsPartScript.Invoke("GainWorkerBoots", 0.01f);
    }
    public void GainJumpBoots(){
        legsPartScript.progress1 = temp_progress1;
        legsPartScript.progress2 = temp_progress2;
        legsPartScript.progressNum = temp_progressNum;
        legsPartScript.Invoke("GainJumpBoots", 0.01f);
    }
}

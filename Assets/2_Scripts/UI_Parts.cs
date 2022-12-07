using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parts : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
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
                timer = 3f;
            }
        }
        else{

        }
    }

    public void EnablePartsUI(){
        spriteRend.enabled = true;
        headPartScript.Invoke("enablePartUI", 0.01f);
        leftArmScript.Invoke("enablePartUI", 0.01f);
        rightArmScript.Invoke("enablePartUI", 0.01f);
        bodyPartScript.Invoke("enablePartUI", 0.01f);
        legsPartScript.Invoke("enablePartUI", 0.01f);
    }

    public void DisablePartsUI(){
        spriteRend.enabled = false;
        headPartScript.Invoke("disablePartUI", 0.01f);
        leftArmScript.Invoke("disablePartUI", 0.01f);
        rightArmScript.Invoke("disablePartUI", 0.01f);
        bodyPartScript.Invoke("disablePartUI", 0.01f);
        legsPartScript.Invoke("disablePartUI", 0.01f);
    }

    public void GainWorkerHead(){
        headPartScript.Invoke("GainWorkerHead", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
    public void GainWorkerBody(){
        bodyPartScript.Invoke("GainWorkerBody", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
    public void GainWorkerDrill(){
        rightArmScript.Invoke("GainWorkerDrill", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
    public void GainBlaster(){
        leftArmScript.Invoke("GainBlaster", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
    public void GainMissileLauncher(){
        leftArmScript.Invoke("GainMissileLauncher", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
    public void GainLaserBeam(){
        leftArmScript.Invoke("GainLaserBeam", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
    public void GainWorkerBoots(){
        legsPartScript.Invoke("GainWorkerLegs", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
    public void GainJumpBoots(){
        legsPartScript.Invoke("GainJumpLegs", 0.01f);
        EnablePartsUI();
        timerEnabled = true;
    }
}

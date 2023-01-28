using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parts : MonoBehaviour
{
    public GameObject CrystalManager;
    public UI_CrystalManager crystalManagerScript;

    public bool isEnabled = false;
    public SpriteRenderer spriteRend;

    public Sprite sprite_inactive0;
    public Sprite sprite_inactive1;
    public Sprite sprite_inactive2;

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

    public GameObject storageSlot1;
    public UI_Part_Select storage1Script;

    public GameObject storageSlot2;
    public UI_Part_Select storage2Script;

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
        storage1Script = storageSlot1.GetComponent<UI_Part_Select>();
        storage2Script = storageSlot2.GetComponent<UI_Part_Select>();

        CheckStorageSlots();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerEnabled)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timerEnabled = false;
                DisablePartsUI();
                timer = 5f;
            }
        }
        else
        {

        }
    }

    public void EndTimer()
    {
        // timerEnabled = false;   
        timer = 5f;
    }
    public void StartTimer()
    {
        // timerEnabled = true;   
        timer = 5f;
    }

    public void EnablePartsUI()
    {
        CheckParts();
        isEnabled = true;
        spriteRend.enabled = true;
        headPartScript.enablePartUI();
        leftArmScript.enablePartUI();
        rightArmScript.enablePartUI();
        bodyPartScript.enablePartUI();
        legsPartScript.enablePartUI();
        if (PlayerManager.current.currentUpgrade_extraSlots == 0)
        {
            storage1Script.disablePartUI();
            storage2Script.disablePartUI();
        }
        if (PlayerManager.current.currentUpgrade_extraSlots == 1)
        {
            storage1Script.enablePartUI();
            storage2Script.disablePartUI();
        }
        if (PlayerManager.current.currentUpgrade_extraSlots == 2)
        {
            storage1Script.enablePartUI();
            storage2Script.enablePartUI();
        }

        crystalManagerScript.ShowCrystalUI();

        headPartScript.OpenWindow();
    }

    public void DisablePartsUI()
    {
        headPartScript.disablePartUI();
        leftArmScript.disablePartUI();
        rightArmScript.disablePartUI();
        bodyPartScript.disablePartUI();
        legsPartScript.disablePartUI();
        storage1Script.disablePartUI();
        storage2Script.disablePartUI();

        crystalManagerScript.HideCrystalUI();

        isEnabled = false;
        spriteRend.enabled = false;
    }

    public void ShowRepairButton()
    {
        headPartScript.showRepair = true;
    }
    public void HideRepairButton()
    {
        headPartScript.showRepair = false;
    }

    public void ShowShieldDefense()
    {
        bodyPartScript.showShield = true;
    }
    public void HideShieldDefense()
    {
        bodyPartScript.showShield = false;
    }

    public void CheckStorageSlots()
    {
        if (PlayerManager.current.currentUpgrade_extraSlots == 0)
        {
            spriteRend.sprite = sprite_inactive0;
        }
        if (PlayerManager.current.currentUpgrade_extraSlots == 1)
        {
            spriteRend.sprite = sprite_inactive1;
        }
        if (PlayerManager.current.currentUpgrade_extraSlots == 2)
        {
            spriteRend.sprite = sprite_inactive2;
        }
    }

    public void ShieldTakeHit()
    {
        if (PlayerManager.current.maxDefenseShield == 1)
        {
            if (PlayerManager.current.currentDefenseShield == 1)
            {
                bodyPartScript.ShieldTakeHit();

            }
        }
        else if (PlayerManager.current.maxDefenseShield == 2)
        {
            if (PlayerManager.current.currentDefenseShield == 1)
            {
                bodyPartScript.ShieldTakeHit();
            }
            if (PlayerManager.current.currentDefenseShield == 2)
            {
                bodyPartScript.ShieldTakeHit();
            }
        }
    }

    public void CheckParts()
    {
        if (!PlayerManager.current.hasBody)
        {
            bodyPartScript.DropPartDisableUI();

            if (PlayerManager.current.hasStorage1)
            {
                //dropPArtDisableUI1
            }
            if (PlayerManager.current.hasStorage2)
            {
                //dropPArtDisableUI2
            }
        }
        if (!PlayerManager.current.hasDrill)
        {
            rightArmScript.DropPartDisableUI();
        }
        if (!PlayerManager.current.hasGun)
        {
            leftArmScript.DropPartDisableUI();
        }
        if (!PlayerManager.current.hasLegs)
        {
            legsPartScript.DropPartDisableUI();
        }
    }
    public void CloseAllWindows()
    {
        headPartScript.CloseWindow();
        leftArmScript.CloseWindow();
        rightArmScript.CloseWindow();
        bodyPartScript.CloseWindow();
        legsPartScript.CloseWindow();
        MenuManager.current.isMouseOver = false;
    }

    public void TriggerAllPartsDrop()
    {
        if (PlayerManager.current.hasBody)
        {
            bodyPartScript.DropPart();
        }
    }

    public void DropOtherParts()
    {
        if (PlayerManager.current.hasDrill)
        {
            rightArmScript.DropPart();
        }
        if (PlayerManager.current.hasGun)
        {
            leftArmScript.DropPart();
        }
        if (PlayerManager.current.hasLegs)
        {
            legsPartScript.DropPart();
        }
    }

    public void TriggerLimbPartsDrop()
    {
        if (PlayerManager.current.hasDrill)
        {
            rightArmScript.DropPart();
        }
        if (PlayerManager.current.hasGun)
        {
            leftArmScript.DropPart();
        }
        if (PlayerManager.current.hasLegs)
        {
            legsPartScript.DropPart();
        }
    }

    public void CheckSelected()
    {
        if (PlayerManager.current.playerHoldingPart)
        {
            if (PlayerManager.current.holdingPartNum == 0)
            {
                headPartScript.EnableSelectionUI();
            }

            if (PlayerManager.current.holdingPartNum == 1)
            {
                bodyPartScript.EnableSelectionUI();
            }

            if (PlayerManager.current.holdingPartNum == 2)
            {
                rightArmScript.EnableSelectionUI();
            }

            if (PlayerManager.current.holdingPartNum == 3)
            {
                leftArmScript.EnableSelectionUI();
            }
            if (PlayerManager.current.holdingPartNum == 4)
            {
                leftArmScript.EnableSelectionUI();
            }
            if (PlayerManager.current.holdingPartNum == 5)
            {
                leftArmScript.EnableSelectionUI();
            }

            if (PlayerManager.current.holdingPartNum == 6)
            {
                legsPartScript.EnableSelectionUI();
            }
            if (PlayerManager.current.holdingPartNum == 7)
            {
                legsPartScript.EnableSelectionUI();
            }

        }
    }

    public void GainWorkerHead(int attatchStyle)
    {
        headPartScript.progress1 = temp_progress1;
        headPartScript.progress2 = temp_progress2;
        headPartScript.progressNum = temp_progressNum;
        headPartScript.GainWorkerHead();
    }
    public void GainWorkerBody(int attatchStyle)
    {
        bodyPartScript.progress1 = temp_progress1;
        bodyPartScript.progress2 = temp_progress2;
        bodyPartScript.progressNum = temp_progressNum;
        bodyPartScript.GainWorkerBody();
    }
    public void GainWorkerDrill(int attatchStyle)
    {
        rightArmScript.progress1 = temp_progress1;
        rightArmScript.progress2 = temp_progress2;
        rightArmScript.progressNum = temp_progressNum;
        rightArmScript.GainWorkerDrill();
    }
    public void GainBlaster(int attatchStyle)
    {
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.GainBlaster();
    }
    public void GainMissileLauncher(int attatchStyle)
    {
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.GainMissileLauncher();
    }
    public void GainLaserBeam(int attatchStyle)
    {
        leftArmScript.progress1 = temp_progress1;
        leftArmScript.progress2 = temp_progress2;
        leftArmScript.progressNum = temp_progressNum;
        leftArmScript.GainLaserBeam();
    }
    public void GainWorkerBoots(int attatchStyle)
    {
        legsPartScript.progress1 = temp_progress1;
        legsPartScript.progress2 = temp_progress2;
        legsPartScript.progressNum = temp_progressNum;
        legsPartScript.GainWorkerBoots();
    }
    public void GainJumpBoots(int attatchStyle)
    {
        legsPartScript.progress1 = temp_progress1;
        legsPartScript.progress2 = temp_progress2;
        legsPartScript.progressNum = temp_progressNum;
        legsPartScript.GainJumpBoots();
    }
}

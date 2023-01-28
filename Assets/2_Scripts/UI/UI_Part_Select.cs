using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_Select : MonoBehaviour
{
    public GameObject healthManagerUI;
    public UI_HealthManager healthManagerScript;

    public UI_Parts parentScript;
    public int partType; //0= head, 1 = body, 2 = r_arm, 3 = l_arm, 4 = legs
    public int partSubType;

    public int progress1;
    public int progress2;
    public int progressNum;

    public BoxCollider boxCollider;
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

    public GameObject DefenseShieldObject_1;
    public GameObject DefenseShieldObject_2;
    public UI_DefenseShield defenseShield1Script;
    public UI_DefenseShield defenseShield2Script;
    public bool showShield = false;

    public GameObject dropButton;

    public SpriteRenderer buttonSpriteRend;
    public BoxCollider buttonCollider;
    public PartButtonScript buttonScript;

    public GameObject selfDButton;

    public GameObject repairButton;
    public SpriteRenderer button2SpriteRend;
    public BoxCollider button2Collider;
    public PartButtonScript button2Script;
    public GameObject repairNum;
    public GameObject repairCorite;
    public bool showRepair = false;

    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<UI_Parts>();
        boxCollider = GetComponent<BoxCollider>();
        spriteRend = GetComponent<SpriteRenderer>();

        objectSprite = activeHeldObject.GetComponent<SpriteRenderer>();
        windowClosedPos = new Vector3(0, -100f, 0);

        windowInfo = Instantiate(windowInfoObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        windowPos = new Vector3(14f, 0f, -1f);
        windowInfo.transform.parent = transform;
        windowInfo.transform.localPosition = windowPos;
        windowInfoScript = windowInfo.GetComponent<UI_Window_Part>();
        windowInfo.transform.position = windowClosedPos;

        if (partType == 0)
        {
            partEnabled = true;
            objectSprite.sprite = object1;

            buttonSpriteRend = selfDButton.GetComponent<SpriteRenderer>();
            buttonCollider = selfDButton.GetComponent<BoxCollider>();
            buttonScript = selfDButton.GetComponent<PartButtonScript>();

            button2SpriteRend = repairButton.GetComponent<SpriteRenderer>();
            button2Collider = repairButton.GetComponent<BoxCollider>();
            button2Script = repairButton.GetComponent<PartButtonScript>();

            button2SpriteRend.enabled = false;
            button2Collider.enabled = false;
            repairNum.SetActive(false);
            repairCorite.SetActive(false);
        }
        else if (partType == 1)
        {
            objectSprite.sprite = null;

            buttonSpriteRend = dropButton.GetComponent<SpriteRenderer>();
            buttonCollider = dropButton.GetComponent<BoxCollider>();
            buttonScript = dropButton.GetComponent<PartButtonScript>();

            defenseShield1Script = DefenseShieldObject_1.GetComponent<UI_DefenseShield>();
            defenseShield1Script.HideUI();

            defenseShield2Script = DefenseShieldObject_2.GetComponent<UI_DefenseShield>();
            defenseShield2Script.HideUI();
        }
        else
        {
            objectSprite.sprite = null;

            buttonSpriteRend = dropButton.GetComponent<SpriteRenderer>();
            buttonCollider = dropButton.GetComponent<BoxCollider>();
            buttonScript = dropButton.GetComponent<PartButtonScript>();
        }

        healthManagerUI = PlayerManager.current.healthManagerUI;
        healthManagerScript = healthManagerUI.GetComponent<UI_HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetPart()
    {
        Destroy(windowInfo);
        windowInfo = null;

        objectSprite = activeHeldObject.GetComponent<SpriteRenderer>();

        if (partType == 3)
        {//Guns
            if (partSubType == 0)
            {
                objectSprite.sprite = object1;
                windowInfoObject = windowInfo_type1;
            }
            if (partSubType == 1)
            {
                objectSprite.sprite = object2;
                windowInfoObject = windowInfo_type2;
            }
            if (partSubType == 2)
            {
                objectSprite.sprite = object3;
                windowInfoObject = windowInfo_type3;
            }
        }
        if (partType == 4)
        {//Legs
            if (partSubType == 0)
            {
                objectSprite.sprite = object1;
                windowInfoObject = windowInfo_type1;
            }
            if (partSubType == 1)
            {
                objectSprite.sprite = object2;
                windowInfoObject = windowInfo_type2;
            }
        }

        windowInfo = Instantiate(windowInfoObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        windowPos = new Vector3(14f, 0f, -1f);
        windowInfo.transform.parent = transform;
        windowInfo.transform.localPosition = windowClosedPos;
        windowInfoScript = windowInfo.GetComponent<UI_Window_Part>();

        partEnabled = true;

        if (partType != 0)
        {
            buttonSpriteRend = dropButton.GetComponent<SpriteRenderer>();
            buttonCollider = dropButton.GetComponent<BoxCollider>();
            buttonScript = dropButton.GetComponent<PartButtonScript>();
        }
        if (windowOpen)
        {
            CloseWindow();
        }
    }

    public void EnableSelectionWhenHoldingPart()
    {
        if (PlayerManager.current.playerHoldingPart)
        {
            if (partType == 0)
            {
                if (PlayerManager.current.holdingPartNum == 0)
                {
                    EnableSelectionUI();
                }
            }
            if (partType == 1)
            {
                if (PlayerManager.current.holdingPartNum == 1)
                {
                    EnableSelectionUI();
                }
            }
            if (partType == 2)
            {
                if (PlayerManager.current.holdingPartNum == 2)
                {
                    EnableSelectionUI();
                }
            }
            if (partType == 3)
            {
                if (PlayerManager.current.holdingPartNum == 3 && PlayerManager.current.holdingPartNum == 4 && PlayerManager.current.holdingPartNum == 5)
                {
                    EnableSelectionUI();
                }
            }
            if (partType == 4)
            {
                if (PlayerManager.current.holdingPartNum == 6 && PlayerManager.current.holdingPartNum == 7)
                {
                    EnableSelectionUI();
                }
            }
        }
    }

    public void EnableSelectionUI()
    {
        spriteRend.sprite = bg_inactive;
        boxCollider.enabled = true;
        spriteRend.enabled = true;
    }

    public void enablePartUI()
    {
        if (partEnabled)
        {
            spriteRend.sprite = bg_inactive;
            objectSprite.enabled = true;
            boxCollider.enabled = true;
            spriteRend.enabled = true;

            buttonSpriteRend.enabled = true;
            buttonCollider.enabled = true;
            if (showRepair)
            {
                button2SpriteRend.enabled = true;
                button2Collider.enabled = true;
                repairNum.SetActive(true);
                repairCorite.SetActive(true);
            }
            if (showShield)
            {
                if (PlayerManager.current.maxDefenseShield == 1)
                {
                    defenseShield1Script.ShowUI();
                }
                if (PlayerManager.current.maxDefenseShield == 2)
                {
                    defenseShield1Script.ShowUI();
                    defenseShield2Script.ShowUI();
                }
            }
        }
        EnableSelectionWhenHoldingPart();
    }
    public void disablePartUI()
    {
        if (objectSprite != null)
        {
            objectSprite.enabled = false;
            boxCollider.enabled = false;
            spriteRend.enabled = false;

            buttonSpriteRend.enabled = false;
            buttonCollider.enabled = false;
            if (showRepair)
            {
                button2SpriteRend.enabled = false;
                button2Collider.enabled = false;
                repairNum.SetActive(false);
                repairCorite.SetActive(false);
            }
            if (showShield)
            {
                if (PlayerManager.current.maxDefenseShield == 1)
                {
                    defenseShield1Script.HideUI();
                }
                if (PlayerManager.current.maxDefenseShield == 2)
                {
                    defenseShield1Script.HideUI();
                    defenseShield2Script.HideUI();
                }
            }
            if (windowOpen)
            {
                CloseWindow();
            }
        }
    }
    public void HideRepairButton()
    {
        if (showRepair)
        {
            button2SpriteRend.enabled = false;
            button2Collider.enabled = false;
            repairNum.SetActive(false);
            repairCorite.SetActive(false);
            showRepair = false;
        }
    }
    public void CheckShieldUpgrade()
    {
        if (showShield)
        {
            if (PlayerManager.current.maxDefenseShield == 1)
            {
                defenseShield1Script.ShowUI();
            }
            if (PlayerManager.current.maxDefenseShield == 2)
            {
                defenseShield1Script.ShowUI();
                defenseShield2Script.ShowUI();
            }
        }
        else
        {
            defenseShield1Script.HideUI();
            defenseShield2Script.HideUI();
        }
    }

    public void ShieldTakeHit()
    {
        if (PlayerManager.current.currentUpgrade_defenseShield == 1)
        {
            if (PlayerManager.current.currentDefenseShield == 1)
            {
                defenseShield1Script.TakeHit();
                PlayerManager.current.shieldActive = false;
                healthManagerScript.ShieldInactive();
            }
        }
        else if (PlayerManager.current.currentUpgrade_defenseShield == 2)
        {
            if (PlayerManager.current.currentDefenseShield == 1)
            {
                if (defenseShield2Script.isFilling)
                {
                    defenseShield1Script.TakeHit();
                    PlayerManager.current.shieldActive = false;
                    healthManagerScript.ShieldInactive();
                }
                else if (defenseShield1Script.isFilling)
                {
                    defenseShield2Script.TakeHit();
                    PlayerManager.current.shieldActive = false;
                    healthManagerScript.ShieldInactive();
                }
            }
            if (PlayerManager.current.currentDefenseShield == 2)
            {
                defenseShield2Script.TakeHit();
            }
        }
    }
    public void CheckUpgrades()
    {
        parentScript.CheckStorageSlots();
        if (partType == 1)
        {
            CheckShieldUpgrade();
        }
    }
    public void DropPart()
    {
        TransferPartProperties();
        DropPartDisableUI();
        if (partType == 0)
        {//Head
            parentScript.DropOtherParts();
            PlayerManager.current.DropAllParts();
            PlayerManager.current.Invoke("Death", 0.2f);
            parentScript.DisablePartsUI();
        }
        else if (partType == 1)
        {//Body
            if (PlayerManager.current.hasLegs || PlayerManager.current.hasDrill
            || PlayerManager.current.hasGun)
            {
                parentScript.DropOtherParts();
                PlayerManager.current.DropAllParts();
            }
            else
            {
                PlayerManager.current.DropBody();
            }
            parentScript.DisablePartsUI();
        }
        else if (partType == 2)
        {//Drill
            PlayerManager.current.DropDrill();
            parentScript.DisablePartsUI();
        }
        else if (partType == 3)
        {//Gun
            PlayerManager.current.DropGun();
            parentScript.DisablePartsUI();
        }
        else if (partType == 4)
        {//Legs
            PlayerManager.current.DropLegs();
            parentScript.DisablePartsUI();
        }
        Invoke("turnMouseOverOffDelayed", 0.1f);
    }

    public void RepairSelf()
    {
        PlayerManager.current.RepairSelf();
    }
    public void TransferPartProperties()
    {
        progressNum = progress1 + progress2;
        if (partType == 1)
        {
            PlayerManager.current.temp_body_progress1 = progress1;
            PlayerManager.current.temp_body_progress2 = progress2;
            PlayerManager.current.temp_body_progressNum = progressNum;
        }
        if (partType == 2)
        {
            PlayerManager.current.temp_drill_progress1 = progress1;
            PlayerManager.current.temp_drill_progress2 = progress2;
            PlayerManager.current.temp_drill_progressNum = progressNum;
        }
        if (partType == 3)
        {
            PlayerManager.current.temp_gun_progress1 = progress1;
            PlayerManager.current.temp_gun_progress2 = progress2;
            PlayerManager.current.temp_gun_progressNum = progressNum;
        }
        if (partType == 4)
        {
            PlayerManager.current.temp_legs_progress1 = progress1;
            PlayerManager.current.temp_legs_progress2 = progress2;
            PlayerManager.current.temp_legs_progressNum = progressNum;
        }
    }

    public void GainWorkerHead()
    {
        partEnabled = true;
        objectSprite.sprite = object1;
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
    }
    public void GainWorkerBody()
    {
        partEnabled = true;
        objectSprite.sprite = object1;
        if (partType != 0)
        {
            dropButton.SetActive(true);
        }
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
    }
    public void GainWorkerDrill()
    {
        partEnabled = true;
        objectSprite.sprite = object1;
        if (partType != 0)
        {
            dropButton.SetActive(true);
        }
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
    }
    public void GainBlaster()
    {
        partEnabled = true;
        partSubType = 0;
        objectSprite.sprite = object1;
        if (partType != 0)
        {
            dropButton.SetActive(true);
        }
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
        ResetPart();
    }
    public void GainMissileLauncher()
    {
        partEnabled = true;
        partSubType = 1;
        objectSprite.sprite = object2;
        if (partType != 0)
        {
            dropButton.SetActive(true);
        }
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
        ResetPart();
    }
    public void GainLaserBeam()
    {
        partEnabled = true;
        partSubType = 2;
        objectSprite.sprite = object3;
        if (partType != 0)
        {
            dropButton.SetActive(true);
        }
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
        ResetPart();
    }
    public void GainWorkerBoots()
    {
        partEnabled = true;
        partSubType = 0;
        objectSprite.sprite = object1;
        if (partType != 0)
        {
            dropButton.SetActive(true);
        }
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
        ResetPart();
    }
    public void GainJumpBoots()
    {
        partEnabled = true;
        partSubType = 1;
        objectSprite.sprite = object2;
        if (partType != 0)
        {
            dropButton.SetActive(true);
        }
        if (parentScript.isEnabled)
        {
            enablePartUI();
        }
        ResetPart();
    }

    public void DropPartDisableUI()
    {
        partEnabled = false;
        partSubType = 0;
        objectSprite.sprite = null;
        buttonSpriteRend.sprite = buttonScript.spriteInactive;
        if (partType != 0)
        {
            dropButton.SetActive(false);
        }
        progress1 = 0;
        progress2 = 0;
        progressNum = 0;
        disablePartUI();
    }

    public void OpenWindow()
    {
        windowInfo.transform.localPosition = windowPos;
        windowInfoScript.Reset();
        windowOpen = true;
    }

    public void CloseWindow()
    {
        if (windowOpen)
        {
            windowInfo.transform.localPosition = windowClosedPos;
            windowOpen = false;
        }
        Invoke("turnMouseOverOffDelayed", 0.2f);
    }
    public void turnMouseOverOffDelayed()
    {
        MenuManager.current.isMouseOver = false;
    }

    public void CloseAllWindows()
    {
        parentScript.CloseAllWindows();
    }

    private void OnMouseOver()
    {
        spriteRend.sprite = bg_active;
        MenuManager.current.isMouseOver = true;
        GameController.current.ChangeMouseCursorDefault();
        if (!AudioManager.current.UIHover)
        {
            AudioManager.current.currentSFXTrack = 3;
            AudioManager.current.PlaySfx();
            AudioManager.current.UIHover = true;
        }
    }

    private void OnMouseExit()
    {
        spriteRend.sprite = bg_inactive;
        MenuManager.current.isMouseOver = false;
        GameController.current.ChangeMouseCursorBack();
        AudioManager.current.UIHover = false;
    }

    private void OnMouseDown()
    {
        if (!partEnabled)
        {
            if (PlayerManager.current.playerHoldingPart)
            {
                if (partType == 0)
                {
                    if (PlayerManager.current.holdingPartNum == 0)
                    {
                        GainWorkerHead();
                        enablePartUI();
                    }
                }
                if (partType == 1)
                {
                    if (PlayerManager.current.holdingPartNum == 1)
                    {
                        GainWorkerBody();
                        enablePartUI();
                        PlayerManager.current.AttatchBody(0);
                    }
                }
                if (partType == 2)
                {
                    if (PlayerManager.current.holdingPartNum == 2)
                    {
                        GainWorkerDrill();
                        enablePartUI();
                    }
                }
                if (partType == 3)
                {
                    if (PlayerManager.current.holdingPartNum == 3)
                    {
                        GainBlaster();
                        enablePartUI();
                    }
                    if (PlayerManager.current.holdingPartNum == 4)
                    {
                        GainMissileLauncher();
                        enablePartUI();
                    }
                    if (PlayerManager.current.holdingPartNum == 5)
                    {
                        GainLaserBeam();
                        enablePartUI();
                    }
                }
                if (partType == 4)
                {
                    if (PlayerManager.current.holdingPartNum == 6)
                    {
                        GainWorkerBoots();
                        enablePartUI();
                    }
                    if (PlayerManager.current.holdingPartNum == 7)
                    {
                        GainJumpBoots();
                        enablePartUI();
                    }
                }

                PlayerManager.current.playerHoldingPart = false;
                PlayerManager.current.holdingPartNum = 0;
            }

            AudioManager.current.currentSFXTrack = 3;
            AudioManager.current.PlaySfx();
        }
        else
        {
            CloseAllWindows();
            OpenWindow();
        }

        AudioManager.current.currentSFXTrack = 0;
        AudioManager.current.PlaySfx();
    }
}

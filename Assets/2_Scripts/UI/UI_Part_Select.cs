using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_Select : MonoBehaviour
{
  public GameObject healthManagerUI;
  public UI_HealthManager healthManagerScript;

  public UI_Parts parentScript;
  public int partType; //0= head, 1 = body, 2 = r_arm, 3 = l_arm, 4 = legs, 5=storage1, 6=storage2
  public int partSubType;
  public int storagePartNum;

  public int progress1;
  public int progress2;
  public int progressNum;

  public BoxCollider boxCollider;
  private SpriteRenderer spriteRend;

  public GameObject partAttachObject;
  public SpriteRenderer partAttachSpriteRend;
  public Sprite attachSprite;
  public Sprite disableSprite;
  public Sprite switchSprite;

  public GameObject windowInfoObject;
  public GameObject windowInfo;
  public UI_Window_Part windowInfoScript;
  private Vector3 windowPos;
  private Vector3 windowClosedPos;
  public bool windowOpen;

  public GameObject windowInfo_type1;
  public GameObject windowInfo_type2;
  public GameObject windowInfo_type3;
  public GameObject windowInfo_type4;
  public GameObject windowInfo_type5;
  public GameObject windowInfo_type6;
  public GameObject windowInfo_type7;
  public GameObject windowInfo_type8;

  public Sprite bg_active;
  public Sprite bg_inactive;

  public bool partEnabled;

  public GameObject activeHeldObject;
  public SpriteRenderer objectSprite;
  public Sprite object1;
  public Sprite object2;
  public Sprite object3;
  public Sprite object4;
  public Sprite object5;
  public Sprite object6;
  public Sprite object7;
  public Sprite object8;

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

    partAttachSpriteRend = partAttachObject.GetComponent<SpriteRenderer>();
    attachSprite = partAttachSpriteRend.sprite;
    partAttachSpriteRend.enabled = false;

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

    if (partType == 5)
    {
      windowInfoScript.storagePart = 1;

      buttonSpriteRend = dropButton.GetComponent<SpriteRenderer>();
      buttonCollider = dropButton.GetComponent<BoxCollider>();
      buttonScript = dropButton.GetComponent<PartButtonScript>();
    }
    if (partType == 6)
    {
      windowInfoScript.storagePart = 2;

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

    if (partType == 5)
    {
      if (storagePartNum == 0)
      {
        objectSprite.sprite = object1;
        windowInfoObject = windowInfo_type1;
      }
      if (storagePartNum == 1)
      {
        objectSprite.sprite = object2;
        windowInfoObject = windowInfo_type2;
      }
      if (storagePartNum == 2)
      {
        objectSprite.sprite = object3;
        windowInfoObject = windowInfo_type3;
      }
      if (storagePartNum == 3)
      {
        objectSprite.sprite = object4;
        windowInfoObject = windowInfo_type4;
      }
      if (storagePartNum == 4)
      {
        objectSprite.sprite = object5;
        windowInfoObject = windowInfo_type5;
      }
      if (storagePartNum == 5)
      {
        objectSprite.sprite = object6;
        windowInfoObject = windowInfo_type6;
      }
      if (storagePartNum == 6)
      {
        objectSprite.sprite = object7;
        windowInfoObject = windowInfo_type7;
      }
      if (storagePartNum == 7)
      {
        objectSprite.sprite = object8;
        windowInfoObject = windowInfo_type8;
      }
    }
    if (partType == 6)
    {
      if (storagePartNum == 0)
      {
        objectSprite.sprite = object1;
        windowInfoObject = windowInfo_type1;
      }
      if (storagePartNum == 1)
      {
        objectSprite.sprite = object2;
        windowInfoObject = windowInfo_type2;
      }
      if (storagePartNum == 2)
      {
        objectSprite.sprite = object3;
        windowInfoObject = windowInfo_type3;
      }
      if (storagePartNum == 3)
      {
        objectSprite.sprite = object4;
        windowInfoObject = windowInfo_type4;
      }
      if (storagePartNum == 4)
      {
        objectSprite.sprite = object5;
        windowInfoObject = windowInfo_type5;
      }
      if (storagePartNum == 5)
      {
        objectSprite.sprite = object6;
        windowInfoObject = windowInfo_type6;
      }
      if (storagePartNum == 6)
      {
        objectSprite.sprite = object7;
        windowInfoObject = windowInfo_type7;
      }
      if (storagePartNum == 7)
      {
        objectSprite.sprite = object8;
        windowInfoObject = windowInfo_type8;
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
    if (partType == 5)
    {
      windowInfoScript.storagePart = 1;
    }
    if (partType == 6)
    {
      windowInfoScript.storagePart = 2;
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
        if (PlayerManager.current.holdingPartNum == 9)
        {
          EnableSelectionUI();
        }
        else
        {
          DisableSelectionUI();
        }
      }
      else if (partType == 1)
      {
        if (PlayerManager.current.holdingPartNum == 1)
        {
          EnableSelectionUI();
        }
        else
        {
          DisableSelectionUI();
        }
      }
      else if (partType == 2)
      {
        if (PlayerManager.current.holdingPartNum == 2)
        {
          EnableSelectionUI();
        }
        else
        {
          DisableSelectionUI();
        }
      }
      else if (partType == 3)
      {
        if (PlayerManager.current.holdingPartNum == 3 && PlayerManager.current.holdingPartNum == 4 && PlayerManager.current.holdingPartNum == 5)
        {
          EnableSelectionUI();
        }
        else
        {
          DisableSelectionUI();
        }
      }
      else if (partType == 4)
      {
        if (PlayerManager.current.holdingPartNum == 6 && PlayerManager.current.holdingPartNum == 7)
        {
          EnableSelectionUI();
        }
        else
        {
          DisableSelectionUI();
        }
      }
      else if (partType == 5)
      {//Storage 1
        EnableSelectionUI();
      }
      else if (partType == 6)
      {//Storage 2
        EnableSelectionUI();
      }
    }
  }

  public void EnableSelectionUI()
  {
    if (partEnabled)
    {
      partAttachSpriteRend.sprite = switchSprite;
    }
    else
    {
      partAttachSpriteRend.sprite = attachSprite;
    }
    partAttachSpriteRend.enabled = true;
    boxCollider.enabled = true;
    spriteRend.enabled = true;
  }

  public void DisableSelectionUI()
  {
    partAttachSpriteRend.sprite = disableSprite;
    partAttachSpriteRend.enabled = true;
    if (partEnabled)
    {
      boxCollider.enabled = false;
      spriteRend.enabled = true;
    }
    else
    {
      boxCollider.enabled = false;
      spriteRend.enabled = false;
    }
  }
  public void ResetSelectionUI()
  {
    partAttachSpriteRend.enabled = false;
    if (partEnabled)
    {
      enablePartUI();
    }
    else
    {
      disablePartUI();
    }
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
        if (PlayerManager.current.maxDurability > 1)
        {
          button2SpriteRend.enabled = true;
          button2Collider.enabled = true;
          repairNum.SetActive(true);
          repairCorite.SetActive(true);
        }
        else
        {
          button2SpriteRend.enabled = false;
          button2Collider.enabled = false;
          repairNum.SetActive(false);
          repairCorite.SetActive(false);
        }
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
      partAttachSpriteRend.enabled = false;

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
  public void HideShieldsReset()
  {
    if (PlayerManager.current.maxDefenseShield == 0)
    {
      defenseShield1Script.HideUI();
      defenseShield2Script.HideUI();
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

    else if (partType == 5)
    {//Storage1
      PlayerManager.current.DropStorageObject(storagePartNum, 1);
      storagePartNum = 0;
      parentScript.DisablePartsUI();
    }
    else if (partType == 6)
    {//Storage2
      PlayerManager.current.DropStorageObject(storagePartNum, 2);
      storagePartNum = 0;
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
    if (partType != 0)
    {
      dropButton.SetActive(true);
    }
    if (parentScript.isEnabled)
    {
      enablePartUI();
    }
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 0;
      objectSprite.sprite = object1;
    }
    ResetPart();
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
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 1;
      objectSprite.sprite = object2;
    }
    ResetPart();
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
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 2;
      objectSprite.sprite = object3;
    }
    ResetPart();
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
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 3;
      objectSprite.sprite = object4;
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
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 4;
      objectSprite.sprite = object5;
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
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 5;
      objectSprite.sprite = object6;
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
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 6;
      objectSprite.sprite = object7;
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
    if (partType == 5 || partType == 6)
    {
      storagePartNum = 7;
      objectSprite.sprite = object8;
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

    windowInfo.transform.localPosition = windowClosedPos;
    windowOpen = false;

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


  public void QuickAttach(int partNum, int storageSlot)
  {
    if (storageSlot == 0)
    {
      if (partNum == 0)
      {
        PlayerManager.current.AttachWorkerHead(0);
      }
      if (partNum == 1)
      {
        PlayerManager.current.AttachBody(0);
      }
      if (partNum == 2)
      {
        PlayerManager.current.AttachDrill(0);
      }
      if (partNum == 3)
      {
        PlayerManager.current.AttachBlaster(0);
      }
      if (partNum == 4)
      {
        PlayerManager.current.AttachMissileLauncher(0);
      }
      if (partNum == 5)
      {
        PlayerManager.current.AttachEnergyBeam(0);
      }
      if (partNum == 6)
      {
        PlayerManager.current.AttachWorkerBoots(0);
      }
      if (partNum == 7)
      {
        PlayerManager.current.AttachJumpLegs(0);
      }
    }

    if (storageSlot == 1)
    {
      if (partNum == 1)
      {
        PlayerManager.current.AttachWorkerHead(1);
      }
      if (partNum == 1)
      {
        PlayerManager.current.AttachBody(1);
      }
      if (partNum == 2)
      {
        PlayerManager.current.AttachDrill(1);
      }
      if (partNum == 3)
      {
        PlayerManager.current.AttachBlaster(1);
      }
      if (partNum == 4)
      {
        PlayerManager.current.AttachMissileLauncher(1);
      }
      if (partNum == 5)
      {
        PlayerManager.current.AttachEnergyBeam(1);
      }
      if (partNum == 6)
      {
        PlayerManager.current.AttachWorkerBoots(1);
      }
      if (partNum == 7)
      {
        PlayerManager.current.AttachJumpLegs(1);
      }
    }

    if (storageSlot == 2)
    {
      if (partNum == 2)
      {
        PlayerManager.current.AttachWorkerHead(2);
      }
      if (partNum == 1)
      {
        PlayerManager.current.AttachBody(2);
      }
      if (partNum == 2)
      {
        PlayerManager.current.AttachDrill(2);
      }
      if (partNum == 3)
      {
        PlayerManager.current.AttachBlaster(2);
      }
      if (partNum == 4)
      {
        PlayerManager.current.AttachMissileLauncher(2);
      }
      if (partNum == 5)
      {
        PlayerManager.current.AttachEnergyBeam(2);
      }
      if (partNum == 6)
      {
        PlayerManager.current.AttachWorkerBoots(2);
      }
      if (partNum == 7)
      {
        PlayerManager.current.AttachJumpLegs(2);
      }
    }


    PlayerManager.current.playerHoldingPart = false;
    parentScript.ResetSelectionUI();

    AudioManager.current.currentSFXTrack = 3;
    AudioManager.current.PlaySfx();
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
    GameController.current.CheckCursor();
    AudioManager.current.UIHover = false;
  }

  private void OnMouseDown()
  {
    if (PlayerManager.current.playerHoldingPart)
    {
      if (partType == 0)
      {
        if (PlayerManager.current.holdingPartNum == 9)
        {
          storagePartNum = 9;
          if (partEnabled)
          {
            PlayerManager.current.SwitchHead();
          }
          else
          {
            PlayerManager.current.AttachWorkerHead(0);
          }
          GainWorkerHead();
          enablePartUI();
        }
      }
      if (partType == 1)
      {
        if (PlayerManager.current.holdingPartNum == 1)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchBody();
          }
          else
          {
            PlayerManager.current.AttachBody(0);
          }
          GainWorkerBody();
          enablePartUI();
          storagePartNum = 1;
        }
      }
      if (partType == 2)
      {
        if (PlayerManager.current.holdingPartNum == 2)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchRArm();
          }
          else
          {
            PlayerManager.current.AttachDrill(0);
          }
          GainWorkerDrill();
          enablePartUI();
          storagePartNum = 2;
        }
      }
      if (partType == 3)
      {
        if (PlayerManager.current.holdingPartNum == 3)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchLArm(0);
          }
          else
          {
            PlayerManager.current.AttachBlaster(0);
          }
          GainBlaster();
          enablePartUI();
          storagePartNum = 3;
        }
        if (PlayerManager.current.holdingPartNum == 4)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchLArm(1);
          }
          else
          {
            PlayerManager.current.AttachMissileLauncher(0);
          }
          GainMissileLauncher();
          enablePartUI();
          storagePartNum = 4;
        }
        if (PlayerManager.current.holdingPartNum == 5)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchLArm(2);
          }
          else
          {
            PlayerManager.current.AttachEnergyBeam(0);
          }
          GainLaserBeam();
          enablePartUI();
          storagePartNum = 5;
        }
      }
      if (partType == 4)
      {
        if (PlayerManager.current.holdingPartNum == 6)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchLegs(0);
          }
          else
          {
            PlayerManager.current.AttachWorkerBoots(0);
          }
          GainWorkerBoots();
          enablePartUI();
          storagePartNum = 6;
        }
        if (PlayerManager.current.holdingPartNum == 7)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchLegs(1);
          }
          else
          {
            PlayerManager.current.AttachJumpLegs(0);
          }
          GainJumpBoots();
          enablePartUI();
          storagePartNum = 7;
        }
      }

      if (partType == 5)
      {
        if (PlayerManager.current.holdingPartNum == 9)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("WorkerHead");
          }
          else
          {
            PlayerManager.current.AttachWorkerHead(1);

          }
          GainWorkerHead();
          enablePartUI();
          storagePartNum = 9;
        }
        if (PlayerManager.current.holdingPartNum == 1)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("WorkerBody");
          }
          else
          {
            PlayerManager.current.AttachBody(1);

          }
          GainWorkerBody();
          enablePartUI();
          storagePartNum = 1;
        }
        if (PlayerManager.current.holdingPartNum == 2)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("WorkerDrill");
          }
          else
          {
            PlayerManager.current.AttachDrill(1);

          }
          GainWorkerDrill();
          enablePartUI();
          storagePartNum = 2;
        }
        if (PlayerManager.current.holdingPartNum == 3)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("BlasterGun");
          }
          else
          {
            PlayerManager.current.AttachBlaster(1);

          }
          GainBlaster();
          enablePartUI();
          storagePartNum = 3;
        }
        if (PlayerManager.current.holdingPartNum == 4)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("MissileLauncher");
          }
          else
          {
            PlayerManager.current.AttachMissileLauncher(1);
          }
          GainMissileLauncher();
          enablePartUI();
          storagePartNum = 4;
        }
        if (PlayerManager.current.holdingPartNum == 5)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("EnergyBeam");
          }
          else
          {
            PlayerManager.current.AttachEnergyBeam(1);
          }
          GainLaserBeam();
          enablePartUI();
          storagePartNum = 5;
        }
        if (PlayerManager.current.holdingPartNum == 6)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("WorkerLegs");
          }
          else
          {
            PlayerManager.current.AttachWorkerBoots(1);
          }
          GainWorkerBoots();
          enablePartUI();
          storagePartNum = 6;
        }
        if (PlayerManager.current.holdingPartNum == 7)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage1("JumpLegs");
          }
          else
          {
            PlayerManager.current.AttachJumpLegs(1);
          }
          GainJumpBoots();
          enablePartUI();
          storagePartNum = 0;
        }
      }

      if (partType == 6)
      {
        if (PlayerManager.current.holdingPartNum == 9)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("WorkerHead");
          }
          else
          {
            PlayerManager.current.AttachWorkerHead(2);
          }
          GainWorkerHead();
          enablePartUI();
          storagePartNum = 9;
        }
        if (PlayerManager.current.holdingPartNum == 1)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("WorkerBody");
          }
          else
          {
            PlayerManager.current.AttachBody(2);

          }
          GainWorkerBody();
          enablePartUI();
          storagePartNum = 1;
        }
        if (PlayerManager.current.holdingPartNum == 2)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("WorkerDrill");
          }
          else
          {
            PlayerManager.current.AttachDrill(2);

          }
          GainWorkerDrill();
          enablePartUI();
          storagePartNum = 2;
        }
        if (PlayerManager.current.holdingPartNum == 3)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("BlasterGun");
          }
          else
          {
            PlayerManager.current.AttachBlaster(2);
          }
          GainBlaster();
          enablePartUI();
          storagePartNum = 3;
        }
        if (PlayerManager.current.holdingPartNum == 4)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("MissileLauncher");
          }
          else
          {
            PlayerManager.current.AttachMissileLauncher(2);
          }
          GainMissileLauncher();
          enablePartUI();
          storagePartNum = 4;
        }
        if (PlayerManager.current.holdingPartNum == 5)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("EnergyBeam");
          }
          else
          {
            PlayerManager.current.AttachEnergyBeam(2);
          }
          GainLaserBeam();
          enablePartUI();
          storagePartNum = 5;
        }
        if (PlayerManager.current.holdingPartNum == 6)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("WorkerLegs");
          }
          else
          {
            PlayerManager.current.AttachWorkerBoots(2);
          }
          GainWorkerBoots();
          enablePartUI();
          storagePartNum = 6;
        }
        if (PlayerManager.current.holdingPartNum == 7)
        {
          if (partEnabled)
          {
            PlayerManager.current.SwitchStorage2("JumpLegs");
          }
          else
          {
            PlayerManager.current.AttachJumpLegs(2);
          }
          GainJumpBoots();
          enablePartUI();
          storagePartNum = 7;
        }
      }

      PlayerManager.current.playerHoldingPart = false;
      parentScript.ResetSelectionUI();

      AudioManager.current.currentSFXTrack = 3;
      AudioManager.current.PlaySfx();
    }
    else
    {
      CloseAllWindows();
      OpenWindow();

      AudioManager.current.currentSFXTrack = 0;
      AudioManager.current.PlaySfx();
    }
  }
}

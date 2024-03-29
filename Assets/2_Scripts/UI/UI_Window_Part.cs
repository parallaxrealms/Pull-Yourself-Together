using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Window_Part : MonoBehaviour
{
  public GameObject parentObject;
  public UI_Part_Select parentScript;

  public string partName;
  public int partType;//0=head,1=body,2=drill,3=gun,4=legs
  public int subType;
  public int storagePart;

  public int currentTab;//0=info,1=upgrade

  public int upgrade1Progress;
  public int upgrade2Progress;
  public int upgradeNum;

  public Sprite sprite_num_0;
  public Sprite sprite_num_1;
  public Sprite sprite_num_2;
  public Sprite sprite_num_3;
  public Sprite sprite_num_4;

  public SpriteRenderer spriteRend;

  public GameObject info_upgradeNum;
  public SpriteRenderer totalNum_spriteRend;

  public GameObject upgradeAlert;
  public SpriteRenderer spriteRend_UpgradeAlert;
  public bool showUpgradeAlert;

  public GameObject upgradeSlots;

  public GameObject currentSlotUpgrading;
  public UI_Part_UpgradeSlot ugpradeSlotScript;

  public GameObject upgradeSlot1;
  private UI_Part_UpgradeSlot ugpradeSlot1Script;
  private bool upgrade1;

  public GameObject upgradeSlot2;
  private UI_Part_UpgradeSlot ugpradeSlot2Script;
  private bool upgrade2;

  public GameObject upgradeSlot3;
  private UI_Part_UpgradeSlot ugpradeSlot3Script;
  private bool upgrade3;

  public GameObject upgradeSlot4;
  private UI_Part_UpgradeSlot ugpradeSlot4Script;
  private bool upgrade4;

  public GameObject xButton;
  public OnClickExitWindow xButtonScript;

  // Start is called before the first frame update
  void Start()
  {
    parentObject = transform.parent.gameObject;
    parentScript = parentObject.GetComponent<UI_Part_Select>();
    Reset();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Reset()
  {
    spriteRend = GetComponent<SpriteRenderer>();

    spriteRend_UpgradeAlert = upgradeAlert.GetComponent<SpriteRenderer>();
    spriteRend_UpgradeAlert.enabled = false;

    xButtonScript = xButton.GetComponent<OnClickExitWindow>();

    totalNum_spriteRend = info_upgradeNum.GetComponent<SpriteRenderer>();

    transform.localScale = new Vector3(1, 1, 1);

    ugpradeSlot1Script = upgradeSlot1.GetComponent<UI_Part_UpgradeSlot>();
    ugpradeSlot2Script = upgradeSlot2.GetComponent<UI_Part_UpgradeSlot>();
    ugpradeSlot3Script = upgradeSlot3.GetComponent<UI_Part_UpgradeSlot>();
    ugpradeSlot4Script = upgradeSlot4.GetComponent<UI_Part_UpgradeSlot>();

    xButtonScript.Invoke("Reset", 0.01f);

    Invoke("SetCurrentPartProperties", 0.5f);
    OpenInfo();
  }

  public void HideUpgradeAlert()
  {
    if (!ugpradeSlot1Script.isHighlighted && !ugpradeSlot2Script.isHighlighted && !ugpradeSlot3Script.isHighlighted && !ugpradeSlot4Script.isHighlighted)
    {
      spriteRend_UpgradeAlert = upgradeAlert.GetComponent<SpriteRenderer>();
      spriteRend_UpgradeAlert.enabled = false;
    }
  }
  public void ShowUpgradeAlert()
  {
    spriteRend_UpgradeAlert = upgradeAlert.GetComponent<SpriteRenderer>();
    spriteRend_UpgradeAlert.enabled = true;
  }

  public void CheckUpgradeSlotsToShow()
  {
    ugpradeSlot1Script = upgradeSlot1.GetComponent<UI_Part_UpgradeSlot>();
    ugpradeSlot2Script = upgradeSlot2.GetComponent<UI_Part_UpgradeSlot>();
    ugpradeSlot3Script = upgradeSlot3.GetComponent<UI_Part_UpgradeSlot>();
    ugpradeSlot4Script = upgradeSlot4.GetComponent<UI_Part_UpgradeSlot>();
    if (!ugpradeSlot1Script.isActivated)
    {
      ugpradeSlot1Script.HighlightSlot();
    }
    if (!ugpradeSlot2Script.isActivated)
    {
      ugpradeSlot2Script.HighlightSlot();
    }
    if (!ugpradeSlot3Script.isActivated)
    {
      ugpradeSlot3Script.HighlightSlot();
    }
    if (!ugpradeSlot4Script.isActivated)
    {
      ugpradeSlot4Script.HighlightSlot();
    }
  }

  public void SetCurrentPartProperties()
  {
    upgrade1Progress = parentScript.progress1;
    upgrade2Progress = parentScript.progress2;
    upgradeNum = parentScript.progressNum;

    if (storagePart == 0)
    {
      if (partType == 0)
      {
        PlayerManager.current.head_progress1 = upgrade1Progress;
        PlayerManager.current.head_progress2 = upgrade2Progress;
        PlayerManager.current.temp_head_progress1 = upgrade1Progress;
        PlayerManager.current.temp_head_progress2 = upgrade2Progress;
        RefreshUpgradeSlotNum();
        PlayerManager.current.CheckAndRefreshUpgrades("Head");
      }
      else if (partType == 1)
      {
        PlayerManager.current.body_progress1 = upgrade1Progress;
        PlayerManager.current.body_progress2 = upgrade2Progress;
        PlayerManager.current.temp_body_progress1 = upgrade1Progress;
        PlayerManager.current.temp_body_progress2 = upgrade2Progress;
        RefreshUpgradeSlotNum();
        PlayerManager.current.CheckAndRefreshUpgrades("Body");
      }
      else if (partType == 2)
      {
        PlayerManager.current.drill_progress1 = upgrade1Progress;
        PlayerManager.current.drill_progress2 = upgrade2Progress;
        PlayerManager.current.temp_drill_progress1 = upgrade1Progress;
        PlayerManager.current.temp_drill_progress2 = upgrade2Progress;
        RefreshUpgradeSlotNum();
        PlayerManager.current.CheckAndRefreshUpgrades("RightArm");
      }
      else if (partType == 3)
      {
        PlayerManager.current.gun_progress1 = upgrade1Progress;
        PlayerManager.current.gun_progress2 = upgrade2Progress;
        PlayerManager.current.temp_gun_progress1 = upgrade1Progress;
        PlayerManager.current.temp_gun_progress2 = upgrade2Progress;
        RefreshUpgradeSlotNum();
        PlayerManager.current.CheckAndRefreshUpgrades("LeftArm");
      }
      else if (partType == 4)
      {
        PlayerManager.current.legs_progress1 = upgrade1Progress;
        PlayerManager.current.legs_progress2 = upgrade2Progress;
        PlayerManager.current.temp_legs_progress1 = upgrade1Progress;
        PlayerManager.current.temp_legs_progress2 = upgrade2Progress;
        RefreshUpgradeSlotNum();
        PlayerManager.current.CheckAndRefreshUpgrades("Legs");
      }
    }
    else if (storagePart == 1)
    {
      PlayerManager.current.storage1Pickup_progress1 = upgrade1Progress;
      PlayerManager.current.storage1Pickup_progress2 = upgrade2Progress;
      RefreshUpgradeSlotNum();
    }
    else if (storagePart == 2)
    {
      PlayerManager.current.storage2Pickup_progress1 = upgrade1Progress;
      PlayerManager.current.storage2Pickup_progress2 = upgrade2Progress;
      RefreshUpgradeSlotNum();
    }
  }

  public void IncreaseUpgradeNum()
  {
    ugpradeSlotScript = currentSlotUpgrading.GetComponent<UI_Part_UpgradeSlot>();

    if (ugpradeSlotScript.slotNum == 0)
    {
      upgrade1Progress += 1;
    }
    if (ugpradeSlotScript.slotNum == 1)
    {
      upgrade1Progress += 1;
    }
    if (ugpradeSlotScript.slotNum == 2)
    {
      upgrade2Progress += 1;

      if (partType == 0)
      {
        PlayerManager.current.RepairDurability(1);
      }
    }
    if (ugpradeSlotScript.slotNum == 3)
    {
      upgrade2Progress += 1;

      if (partType == 0)
      {
        PlayerManager.current.RepairDurability(1);
      }
    }
    upgradeNum = upgrade1Progress + upgrade2Progress;

    parentScript.progress1 = upgrade1Progress;
    parentScript.progress2 = upgrade2Progress;
    parentScript.progressNum = upgradeNum;

    SetCurrentPartProperties();
    parentScript.CheckUpgrades();
    CheckUpgradeSlotsToShow();
  }

  public void RefreshUpgradeSlotNum()
  {
    if (upgradeNum == 0)
    {
      totalNum_spriteRend.sprite = sprite_num_0;
    }
    else if (upgradeNum == 1)
    {
      totalNum_spriteRend.sprite = sprite_num_1;
    }
    else if (upgradeNum == 2)
    {
      totalNum_spriteRend.sprite = sprite_num_2;
    }
    else if (upgradeNum == 3)
    {
      totalNum_spriteRend.sprite = sprite_num_3;
    }
    else if (upgradeNum == 4)
    {
      totalNum_spriteRend.sprite = sprite_num_4;
    }
  }

  public void RefreshUpgrades()
  {
    RefreshUpgradeSlotNum();
    if (upgrade1Progress == 0)
    {
      upgrade1 = false;
      upgrade2 = false;
      ugpradeSlot1Script.Invoke("ResetSlot", 0.01f);
      ugpradeSlot2Script.Invoke("ResetSlot", 0.01f);
    }
    else if (upgrade1Progress == 1)
    {
      upgrade1 = true;
      ugpradeSlot1Script.Invoke("FillSlot", 0.01f);
      ugpradeSlot2Script.Invoke("ResetSlot", 0.01f);
    }
    else if (upgrade1Progress == 2)
    {
      upgrade2 = true;
      ugpradeSlot1Script.Invoke("FillSlot", 0.01f);
      ugpradeSlot2Script.Invoke("FillSlot", 0.01f);
    }

    if (upgrade2Progress == 0)
    {
      upgrade3 = false;
      upgrade4 = false;
      ugpradeSlot3Script.Invoke("ResetSlot", 0.01f);
      ugpradeSlot4Script.Invoke("ResetSlot", 0.01f);
    }
    else if (upgrade2Progress == 1)
    {
      upgrade3 = true;
      ugpradeSlot3Script.Invoke("FillSlot", 0.01f);
      ugpradeSlot4Script.Invoke("ResetSlot", 0.01f);
    }
    else if (upgrade2Progress == 2)
    {
      upgrade4 = true;
      ugpradeSlot3Script.Invoke("FillSlot", 0.01f);
      ugpradeSlot4Script.Invoke("FillSlot", 0.01f);
    }

    CheckIfUpgradesAvailable();
  }

  public void CheckIfUpgradesAvailable()
  {

  }

  public void OpenInfo()
  {
    RefreshUpgradeSlotNum();
    RefreshUpgrades();
    Invoke("CheckUpgradeSlotsToShow", 0.1f);
  }

  public void CloseAllWindows()
  {
    parentScript.Invoke("CloseAllWindows", 0.01f);
    MenuManager.current.isMouseOver = false;
  }

  private void OnMouseEnter()
  {
    MenuManager.current.isMouseOver = true;
    GameController.current.ChangeMouseCursorDefault();
  }
  private void OnMouseExit()
  {
    MenuManager.current.isMouseOver = false;
    GameController.current.CheckCursor();
  }
}
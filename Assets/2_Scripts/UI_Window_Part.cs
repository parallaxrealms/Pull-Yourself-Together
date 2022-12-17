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

  public int currentTab;//0=info,1=upgrade

  public int upgrade1Progress;
  public int upgrade2Progress;
  public int upgradeNum;

  public Sprite sprite_num_0;
  public Sprite sprite_num_1;
  public Sprite sprite_num_2;
  public Sprite sprite_num_3;
  public Sprite sprite_num_4;
  public Sprite sprite_num_5;
  public Sprite sprite_num_6;


  public SpriteRenderer spriteRend;
  public Sprite sprite_info;
  public Sprite sprite_upgrade;
  
  public GameObject infoTab;
  public UI_Part_Window_Tab infoTabScript;

  public GameObject upgradeNums;

  public GameObject info_upgradeNum;
  public SpriteRenderer totalNum_spriteRend;
  public GameObject info_upgrade1Num;
  public SpriteRenderer upgrade1Num_spriteRend;
  public GameObject info_upgrade2Num;
  public SpriteRenderer upgrade2Num_spriteRend;
  
  public GameObject upgradeTab;
  public UI_Part_Window_Tab upgradeTabScript;

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

  public GameObject upgradeSlot5;
  private UI_Part_UpgradeSlot ugpradeSlot5Script;
  private bool upgrade5;

  public GameObject upgradeSlot6;
  private UI_Part_UpgradeSlot ugpradeSlot6Script;
  private bool upgrade6;

  public GameObject xButton;
  public OnClickExitWindow xButtonScript;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<UI_Part_Select>();

        spriteRend = GetComponent<SpriteRenderer>();
        upgradeSlots.SetActive(false);

        xButtonScript = xButton.GetComponent<OnClickExitWindow>();

        infoTabScript = infoTab.GetComponent<UI_Part_Window_Tab>();
        upgradeTabScript = upgradeTab.GetComponent<UI_Part_Window_Tab>();

        totalNum_spriteRend = info_upgradeNum.GetComponent<SpriteRenderer>();
        upgrade1Num_spriteRend = info_upgrade1Num.GetComponent<SpriteRenderer>();
        upgrade2Num_spriteRend = info_upgrade2Num.GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3(1,1,1);

        ugpradeSlot1Script = upgradeSlot1.GetComponent<UI_Part_UpgradeSlot>();
        ugpradeSlot2Script = upgradeSlot2.GetComponent<UI_Part_UpgradeSlot>();
        ugpradeSlot3Script = upgradeSlot3.GetComponent<UI_Part_UpgradeSlot>();
        ugpradeSlot4Script = upgradeSlot4.GetComponent<UI_Part_UpgradeSlot>();
        ugpradeSlot5Script = upgradeSlot5.GetComponent<UI_Part_UpgradeSlot>();
        ugpradeSlot6Script = upgradeSlot6.GetComponent<UI_Part_UpgradeSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
      xButtonScript.Invoke("Reset",0.01f);
      GetCurrentPartProperties();
      OpenInfo();
    }

    public void GetCurrentPartProperties(){
      upgrade1Progress = parentScript.progress1;
      upgrade2Progress = parentScript.progress2;
      upgradeNum = parentScript.progressNum;

      RefreshUpgradeSlotNum();
    }

    public void IncreaseUpgradeNum(){
      ugpradeSlotScript = currentSlotUpgrading.GetComponent<UI_Part_UpgradeSlot>();

      if(ugpradeSlotScript.slotNum == 0){
        upgrade1Progress += 1;
      }
      if(ugpradeSlotScript.slotNum == 1){
        upgrade1Progress += 1;
      }
      if(ugpradeSlotScript.slotNum == 2){
        upgrade1Progress += 1;
      }
      if(ugpradeSlotScript.slotNum == 3){
        upgrade2Progress += 1;
      }
      if(ugpradeSlotScript.slotNum == 4){
        upgrade2Progress += 1;
      }
      if(ugpradeSlotScript.slotNum == 5){
        upgrade2Progress += 1;
      }
      upgradeNum = upgrade1Progress + upgrade2Progress;

      parentScript.progress1 = upgrade1Progress;
      parentScript.progress2 = upgrade2Progress;
      parentScript.progressNum = upgradeNum;

      GetCurrentPartProperties();
    }

    public void RefreshUpgradeSlotNum(){
      if(upgradeNum == 0){
        totalNum_spriteRend.sprite = sprite_num_0;
      }
      else if(upgradeNum == 1){
        totalNum_spriteRend.sprite = sprite_num_1;
      }
      else if(upgradeNum == 2){
        totalNum_spriteRend.sprite = sprite_num_2;
      }
      else if(upgradeNum == 3){
        totalNum_spriteRend.sprite = sprite_num_3;
      }
      else if(upgradeNum == 4){
        totalNum_spriteRend.sprite = sprite_num_4;
      }
      else if(upgradeNum == 5){
        totalNum_spriteRend.sprite = sprite_num_5;
      }
      else if(upgradeNum == 6){
        totalNum_spriteRend.sprite = sprite_num_6;
      }

      if(upgrade1Progress == 0){
        upgrade1Num_spriteRend.sprite = sprite_num_0;
      }
      else if(upgrade1Progress == 1){
        upgrade1Num_spriteRend.sprite = sprite_num_1;
      }
      else if(upgrade1Progress == 2){
        upgrade1Num_spriteRend.sprite = sprite_num_2;
      }
      else if(upgrade1Progress == 3){
        upgrade1Num_spriteRend.sprite = sprite_num_3;
      }

      if(upgrade2Progress == 0){
        upgrade2Num_spriteRend.sprite = sprite_num_0;
      }
      else if(upgrade2Progress == 1){
        upgrade2Num_spriteRend.sprite = sprite_num_1;
      }
      else if(upgrade2Progress == 2){
        upgrade2Num_spriteRend.sprite = sprite_num_2;
      }
      else if(upgrade2Progress == 3){
        upgrade2Num_spriteRend.sprite = sprite_num_3;
      }
    }

    public void RefreshUpgrades(){
      RefreshUpgradeSlotNum();
      if(upgrade1Progress == 0){
        upgrade1 = false;
        upgrade2 = false;
        upgrade3 = false;
        ugpradeSlot1Script.Invoke("ResetSlot",0.01f);
        ugpradeSlot2Script.Invoke("ResetSlot",0.01f);
        ugpradeSlot3Script.Invoke("ResetSlot",0.01f);
      }
      else if(upgrade1Progress == 1){
        upgrade1 = true;
        ugpradeSlot1Script.Invoke("FillSlot",0.01f);
        ugpradeSlot2Script.Invoke("ResetSlot",0.01f);
        ugpradeSlot3Script.Invoke("ResetSlot",0.01f);
      }
      else if(upgrade1Progress == 2){
        upgrade2 = true;
        ugpradeSlot1Script.Invoke("FillSlot",0.01f);
        ugpradeSlot2Script.Invoke("FillSlot",0.01f);
        ugpradeSlot3Script.Invoke("ResetSlot",0.01f);
      }
      else if(upgrade1Progress == 3){
        upgrade3 = true;
        ugpradeSlot1Script.Invoke("FillSlot",0.01f);
        ugpradeSlot2Script.Invoke("FillSlot",0.01f);
        ugpradeSlot3Script.Invoke("FillSlot",0.01f);
      }
      if(upgrade2Progress == 0){
        upgrade4 = false;
        upgrade5 = false;
        upgrade6 = false;
        ugpradeSlot4Script.Invoke("ResetSlot",0.01f);
        ugpradeSlot5Script.Invoke("ResetSlot",0.01f);
        ugpradeSlot6Script.Invoke("ResetSlot",0.01f);
      }
      else if(upgrade2Progress == 1){
        upgrade4 = true;
        ugpradeSlot4Script.Invoke("FillSlot",0.01f);
        ugpradeSlot5Script.Invoke("ResetSlot",0.01f);
        ugpradeSlot6Script.Invoke("ResetSlot",0.01f);
      }
      else if(upgrade2Progress == 2){
        upgrade5 = true;
        ugpradeSlot4Script.Invoke("FillSlot",0.01f);
        ugpradeSlot5Script.Invoke("FillSlot",0.01f);
        ugpradeSlot6Script.Invoke("ResetSlot",0.01f);
      }
      else if(upgrade2Progress == 3){
        upgrade6 = true;
        ugpradeSlot4Script.Invoke("FillSlot",0.01f);
        ugpradeSlot5Script.Invoke("FillSlot",0.01f);
        ugpradeSlot6Script.Invoke("FillSlot",0.01f);
      }
    }
    
    public void SwitchTabs(){
      if(currentTab == 0){
        OpenUpgrades();
      }
      else if(currentTab == 1){
        OpenInfo();
      }
    }

    public void OpenInfo(){
      currentTab = 0;
      spriteRend.sprite = sprite_info;

      infoTabScript.Invoke("OpenTab",0.01f);
      upgradeTabScript.Invoke("CloseTab",0.01f);

      upgradeSlots.SetActive(false);
      upgradeNums.SetActive(true);
      RefreshUpgradeSlotNum();
    }

    public void OpenUpgrades(){
      currentTab = 1;
      spriteRend.sprite = sprite_upgrade;
      
      infoTabScript.Invoke("CloseTab",0.01f);
      upgradeTabScript.Invoke("OpenTab",0.01f);

      upgradeSlots.SetActive(true);
      upgradeNums.SetActive(false);
      RefreshUpgrades();
    }

    public void CloseAllWindows(){
      parentScript.Invoke("CloseAllWindows",0.01f);
        MenuManager.current.isMouseOver = false;
    }

    private void OnMouseEnter() {
      MenuManager.current.isMouseOver = true;
    }
    private void OnMouseExit() {
      MenuManager.current.isMouseOver = false;
    }
}
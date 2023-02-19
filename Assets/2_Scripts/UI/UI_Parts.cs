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

    //press E to insta-place parts
    if (isEnabled)
    {
      if (PlayerManager.current.interactionAvailable)
      {
        if (Input.GetButtonDown("Interact"))
        {
          if (PlayerManager.current.playerHoldingPart)
          {
            if (PlayerManager.current.holdingPartNum == 9)
            {   //QUICK attach Head
              if (!PlayerManager.current.hasHead)
              {
                headPartScript.QuickAttach(0, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(0, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(0, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(0, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }

            if (PlayerManager.current.holdingPartNum == 1)
            { //QUICK attach Body
              if (!PlayerManager.current.hasBody)
              {
                bodyPartScript.QuickAttach(1, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(1, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(1, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(1, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }

            if (PlayerManager.current.holdingPartNum == 2)
            {
              if (!PlayerManager.current.hasDrill)
              {
                rightArmScript.QuickAttach(2, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(2, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(2, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(2, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }

            if (PlayerManager.current.holdingPartNum == 3)
            {
              if (!PlayerManager.current.hasGun)
              {
                leftArmScript.QuickAttach(3, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(3, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(3, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(3, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }

            if (PlayerManager.current.holdingPartNum == 4)
            {
              if (!PlayerManager.current.hasGun)
              {
                rightArmScript.QuickAttach(4, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(4, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(4, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(4, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }

            if (PlayerManager.current.holdingPartNum == 5)
            {
              if (!PlayerManager.current.hasGun)
              {
                rightArmScript.QuickAttach(5, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(5, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(5, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(5, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }

            if (PlayerManager.current.holdingPartNum == 6)
            {
              if (!PlayerManager.current.hasLegs)
              {
                legsPartScript.QuickAttach(6, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(6, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(6, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(6, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }

            if (PlayerManager.current.holdingPartNum == 7)
            {
              if (!PlayerManager.current.hasLegs)
              {
                legsPartScript.QuickAttach(7, 0);
              }
              else
              {
                if (PlayerManager.current.currentUpgrade_extraSlots == 1)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(7, 1);
                  }
                }
                else if (PlayerManager.current.currentUpgrade_extraSlots == 2)
                {
                  if (!PlayerManager.current.hasStorage1)
                  {
                    storage1Script.QuickAttach(7, 1);
                  }
                  else if (!PlayerManager.current.hasStorage2)
                  {
                    storage2Script.QuickAttach(7, 2);
                  }
                }
                else
                {
                  AudioManager.current.currentSFXTrack = 1;
                  AudioManager.current.PlaySfx();
                }
              }
            }
            GameController.current.CheckCursor();
          }
        }
      }
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

    // headPartScript.OpenWindow();
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
    bodyPartScript.HideShieldsReset();
  }

  public void CheckStorageSlots()
  {
    if (PlayerManager.current.currentUpgrade_extraSlots == 0)
    {
      spriteRend.sprite = sprite_inactive0;
      storage1Script.DropPartDisableUI();
      storage2Script.DropPartDisableUI();
    }
    if (PlayerManager.current.currentUpgrade_extraSlots == 1)
    {
      spriteRend.sprite = sprite_inactive1;
      storage2Script.DropPartDisableUI();
    }
    if (PlayerManager.current.currentUpgrade_extraSlots == 2)
    {
      spriteRend.sprite = sprite_inactive2;
    }
  }
  public void ResetStorageSlots()
  {
    spriteRend.sprite = sprite_inactive0;
    storage1Script.DropPartDisableUI();
    storage2Script.DropPartDisableUI();

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
    if (PlayerManager.current.currentUpgrade_extraSlots == 1)
    {
      storage1Script.CloseWindow();
    }
    if (PlayerManager.current.currentUpgrade_extraSlots == 2)
    {
      storage1Script.CloseWindow();
      storage2Script.CloseWindow();
    }
    MenuManager.current.isMouseOver = false;
  }

  public void TakeHitHead()
  {
    headPartScript.TakeDurabilityHit();
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
      if (PlayerManager.current.holdingPartNum == 9)
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

  public void ResetSelectionUI()
  {
    headPartScript.ResetSelectionUI();
    leftArmScript.ResetSelectionUI();
    rightArmScript.ResetSelectionUI();
    bodyPartScript.ResetSelectionUI();
    legsPartScript.ResetSelectionUI();
    if (PlayerManager.current.currentUpgrade_extraSlots == 1)
    {
      storage1Script.ResetSelectionUI();
    }
    if (PlayerManager.current.currentUpgrade_extraSlots == 2)
    {
      storage1Script.ResetSelectionUI();
      storage2Script.ResetSelectionUI();
    }
  }

  public void GainWorkerHead(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      headPartScript.progress1 = temp_progress1;
      headPartScript.progress2 = temp_progress2;
      headPartScript.progressNum = temp_progressNum;
      headPartScript.GainWorkerHead();
      if (openWindow)
      {
        headPartScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainWorkerHead();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainWorkerHead();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
  public void GainWorkerBody(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      bodyPartScript.progress1 = temp_progress1;
      bodyPartScript.progress2 = temp_progress2;
      bodyPartScript.progressNum = temp_progressNum;
      bodyPartScript.GainWorkerBody();
      if (openWindow)
      {
        bodyPartScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainWorkerBody();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainWorkerBody();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
  public void GainWorkerDrill(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      rightArmScript.progress1 = temp_progress1;
      rightArmScript.progress2 = temp_progress2;
      rightArmScript.progressNum = temp_progressNum;
      rightArmScript.GainWorkerDrill();
      if (openWindow)
      {
        rightArmScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainWorkerDrill();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainWorkerDrill();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
  public void GainBlaster(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      leftArmScript.progress1 = temp_progress1;
      leftArmScript.progress2 = temp_progress2;
      leftArmScript.progressNum = temp_progressNum;
      leftArmScript.GainBlaster();
      if (openWindow)
      {
        leftArmScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainBlaster();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainBlaster();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
  public void GainMissileLauncher(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      leftArmScript.progress1 = temp_progress1;
      leftArmScript.progress2 = temp_progress2;
      leftArmScript.progressNum = temp_progressNum;
      leftArmScript.GainMissileLauncher();
      if (openWindow)
      {
        leftArmScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainMissileLauncher();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainMissileLauncher();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
  public void GainLaserBeam(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      leftArmScript.progress1 = temp_progress1;
      leftArmScript.progress2 = temp_progress2;
      leftArmScript.progressNum = temp_progressNum;
      leftArmScript.GainLaserBeam();
      if (openWindow)
      {
        leftArmScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainLaserBeam();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainLaserBeam();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
  public void GainWorkerBoots(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      legsPartScript.progress1 = temp_progress1;
      legsPartScript.progress2 = temp_progress2;
      legsPartScript.progressNum = temp_progressNum;
      legsPartScript.GainWorkerBoots();
      if (openWindow)
      {
        legsPartScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainWorkerBoots();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainWorkerBoots();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
  public void GainJumpBoots(int attatchStyle, bool openWindow)
  {
    CloseAllWindows();
    if (attatchStyle == 0)
    {
      legsPartScript.progress1 = temp_progress1;
      legsPartScript.progress2 = temp_progress2;
      legsPartScript.progressNum = temp_progressNum;
      legsPartScript.GainJumpBoots();
      if (openWindow)
      {
        legsPartScript.OpenWindow();
      }
    }
    else if (attatchStyle == 1)
    {
      storage1Script.progress1 = temp_progress1;
      storage1Script.progress2 = temp_progress2;
      storage1Script.progressNum = temp_progressNum;
      storage1Script.GainJumpBoots();
      if (openWindow)
      {
        storage1Script.OpenWindow();
      }
    }
    else if (attatchStyle == 2)
    {
      storage2Script.progress1 = temp_progress1;
      storage2Script.progress2 = temp_progress2;
      storage2Script.progressNum = temp_progressNum;
      storage2Script.GainJumpBoots();
      if (openWindow)
      {
        storage2Script.OpenWindow();
      }
    }
  }
}
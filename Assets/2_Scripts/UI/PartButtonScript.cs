using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartButtonScript : MonoBehaviour
{
  public int buttonType; //0 = self-destruct, 1 = repair, 2= drop part, 3 = confirm yes, 4 = confirm yes 
  public Sprite spriteInactive;
  public Sprite spriteActive;

  private SpriteRenderer spriteRend;

  public UI_Part_Select parentScript;
  public ConfirmSelfDestruct parent2Script;

  public UI_CrystalManager crystalManagerScript;
  // Start is called before the first frame update
  void Start()
  {
    spriteRend = GetComponent<SpriteRenderer>();
    spriteRend.sprite = spriteInactive;

    if (buttonType == 0)
    {
      parentScript = transform.parent.GetComponent<UI_Part_Select>();
    }

    if (buttonType == 1)
    {
      parentScript = transform.parent.GetComponent<UI_Part_Select>();
      crystalManagerScript = GameObject.Find("CrystalManager").GetComponent<UI_CrystalManager>();
    }

    if (buttonType == 2)
    {
      parentScript = transform.parent.GetComponent<UI_Part_Select>();
    }

    if (buttonType == 3 || buttonType == 4)
    {
      parent2Script = transform.parent.GetComponent<ConfirmSelfDestruct>();
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Reset()
  {
    spriteRend.sprite = spriteInactive;
  }

  void OnMouseOver()
  {
    if (!PlayerManager.current.playerHoldingPart)
    {
      spriteRend.sprite = spriteActive;
      MenuManager.current.isMouseOver = true;
      GameController.current.ChangeMouseCursorDefault();
      if (!AudioManager.current.UIHover)
      {
        AudioManager.current.currentSFXTrack = 3;
        AudioManager.current.PlaySfx();
        AudioManager.current.UIHover = true;
      }
    }
  }

  void OnMouseExit()
  {
    if (!PlayerManager.current.playerHoldingPart)
    {
      spriteRend.sprite = spriteInactive;
      MenuManager.current.isMouseOver = false;
      GameController.current.CheckCursor();
      AudioManager.current.UIHover = false;
    }
  }

  public void OnMouseDown()
  {
    if (!PlayerManager.current.playerHoldingPart)
    {
      if (buttonType == 0)
      {
        parentScript.OpenConfirmation();
        MenuManager.current.isMouseOver = false;

        AudioManager.current.currentSFXTrack = 0;
        AudioManager.current.PlaySfx();
      }
      else if (buttonType == 1)
      {
        if (PlayerManager.current.numOfCorite > 0)
        {//Has Enough Corite
          parentScript.Invoke("RepairSelf", 0.01f);
          MenuManager.current.isMouseOver = false;

          crystalManagerScript.temp_corite = 1;
          crystalManagerScript.SubtractFromUpgrading();

          if (PlayerManager.current.head_progress2 > 0)
          {
            if (PlayerManager.current.currentDurability > PlayerManager.current.maxDurability)
            {
              PlayerManager.current.currentDurability = PlayerManager.current.maxDurability;
              parentScript.HideRepairButton();
            }
            if (PlayerManager.current.currentDurability == PlayerManager.current.maxDurability)
            {
              parentScript.HideRepairButton();
            }
          }

          AudioManager.current.currentSFXTrack = 0;
          AudioManager.current.PlaySfx();
        }
        else
        {//Not Enough Corite
          AudioManager.current.currentSFXTrack = 1;
          AudioManager.current.PlaySfx();
        }
      }
      else if (buttonType == 2)
      {
        parentScript.DropPart();
        MenuManager.current.isMouseOver = false;

        AudioManager.current.currentSFXTrack = 0;
        AudioManager.current.PlaySfx();
      }
      else if (buttonType == 3)
      {
        parent2Script.SelfDestruct();
        parent2Script.ConfirmClosed();
      }
      else if (buttonType == 4)
      {
        parent2Script.ConfirmClosed();
      }
    }
  }
}

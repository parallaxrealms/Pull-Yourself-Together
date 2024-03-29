using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_Window_Tab : MonoBehaviour
{
  public GameObject parentObject;
  public UI_Window_Part parentScript;

  public int tabNum;

  public SpriteRenderer spriteRend;
  public Sprite sprite_inactive;
  public Sprite sprite_active;
  public Sprite sprite_inactiveHover;

  public bool isActive;
  public bool isHovering;

  // Start is called before the first frame update
  void Start()
  {
    parentObject = transform.parent.gameObject;
    parentScript = parentObject.GetComponent<UI_Window_Part>();
    spriteRend = GetComponent<SpriteRenderer>();
    if (tabNum == 0)
    {
      isActive = true;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OpenTab()
  {
    spriteRend.sprite = sprite_active;
    isActive = true;
  }

  public void CloseTab()
  {
    spriteRend.sprite = sprite_inactive;
    isActive = false;
  }

  void OnMouseOver()
  {
    if (!PlayerManager.current.playerHoldingPart)
    {
      if (!isActive)
      {
        spriteRend.sprite = sprite_inactiveHover;
        isHovering = true;
      }
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
      if (!isActive)
      {
        spriteRend.sprite = sprite_inactive;
        isHovering = false;
      }
      MenuManager.current.isMouseOver = false;
      GameController.current.CheckCursor();
      AudioManager.current.UIHover = false;
    }
  }

  public void OnMouseDown()
  {
    if (!PlayerManager.current.playerHoldingPart)
    {
      if (!isActive)
      {
        parentScript.Invoke("SwitchTabs", 0.01f);
        AudioManager.current.currentSFXTrack = 0;
        AudioManager.current.PlaySfx();
      }
    }
  }
}

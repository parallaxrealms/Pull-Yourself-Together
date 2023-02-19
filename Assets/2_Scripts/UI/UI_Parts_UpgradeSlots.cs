using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parts_UpgradeSlots : MonoBehaviour
{

  public int partType;
  public int slotNum;

  private GameObject parentObj;
  public UI_Part_Select parentScript;

  public int progress1;
  public int progress2;

  public SpriteRenderer spriteRend;
  public Sprite sprite_progress_empty;
  public Sprite sprite_progress_1;
  public Sprite sprite_progress_2;
  // Start is called before the first frame update
  void Awake()
  {
    parentObj = transform.parent.gameObject;
    parentScript = parentObj.GetComponent<UI_Part_Select>();

    spriteRend = GetComponent<SpriteRenderer>();
    sprite_progress_empty = spriteRend.sprite;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void CheckCurrentUpgrades()
  {
    parentScript = parentObj.GetComponent<UI_Part_Select>();

    progress1 = parentScript.progress1;
    progress2 = parentScript.progress2;

    if (slotNum == 1)
    {
      if (progress1 == 0)
      {
        spriteRend.sprite = sprite_progress_empty;
      }
      else if (progress1 == 1)
      {
        spriteRend.sprite = sprite_progress_1;
      }
      else if (progress1 == 2)
      {
        spriteRend.sprite = sprite_progress_2;
      }
    }

    if (slotNum == 2)
    {
      if (progress2 == 0)
      {
        spriteRend.sprite = sprite_progress_empty;
      }
      else if (progress2 == 1)
      {
        spriteRend.sprite = sprite_progress_1;
      }
      else if (progress2 == 2)
      {
        spriteRend.sprite = sprite_progress_2;
      }
    }

  }
}

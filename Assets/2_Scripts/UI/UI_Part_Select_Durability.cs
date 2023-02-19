using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_Select_Durability : MonoBehaviour
{
  public GameObject parentObj;
  public UI_Part_Select parentScript;

  public SpriteRenderer spriteRend;

  public Sprite sprite_1;
  public Sprite sprite_2;
  public Sprite sprite_3;

  // Start is called before the first frame update
  void Start()
  {
    parentObj = transform.parent.gameObject;
    parentScript = parentObj.GetComponent<UI_Part_Select>();

    spriteRend = GetComponent<SpriteRenderer>();

    sprite_1 = spriteRend.sprite;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void ChangeStatusOnUpgrade()
  {
    if (PlayerManager.current.maxDurability == 1)
    {
      spriteRend.sprite = sprite_1;
    }
    if (PlayerManager.current.maxDurability == 2)
    {
      spriteRend.sprite = sprite_2;
    }
    if (PlayerManager.current.maxDurability == 3)
    {
      spriteRend.sprite = sprite_3;
    }
  }

  public void ChangeStatusOnHit()
  {
    if (PlayerManager.current.currentDurability == 1)
    {
      spriteRend.sprite = sprite_1;
    }
    if (PlayerManager.current.currentDurability == 2)
    {
      spriteRend.sprite = sprite_2;
    }
    if (PlayerManager.current.currentDurability == 3)
    {
      spriteRend.sprite = sprite_3;
    }
  }
}

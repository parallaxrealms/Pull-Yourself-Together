using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartButtonScript : MonoBehaviour
{
    public int buttonType; //0 = drop/self-destruct, 1 = repair
    public Sprite spriteInactive;
    public Sprite spriteActive;

    private SpriteRenderer spriteRend;

    public UI_Part_Select parentScript;

    public UI_CrystalManager crystalManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();

        parentScript = transform.parent.GetComponent<UI_Part_Select>();
        spriteRend.sprite = spriteInactive;

        if (buttonType == 1)
        {
            crystalManagerScript = GameObject.Find("CrystalManager").GetComponent<UI_CrystalManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
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

    void OnMouseExit()
    {
        spriteRend.sprite = spriteInactive;
        MenuManager.current.isMouseOver = false;
        GameController.current.ChangeMouseCursorBack();
        AudioManager.current.UIHover = false;
    }

    public void OnMouseDown()
    {
        if (buttonType == 0)
        {
            parentScript.Invoke("DropPart", 0.01f);
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
    }
}

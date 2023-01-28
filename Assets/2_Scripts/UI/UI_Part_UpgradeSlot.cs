using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_UpgradeSlot : MonoBehaviour
{
    public GameObject UI_CrystalManager;
    public UI_CrystalManager crystalManagerScript;

    public int slotNum;
    private GameObject parentObject;
    private GameObject parentParentObject;
    private UI_Window_Part parentScript;
    public bool isActivated = false;

    public GameObject hoverWhite;
    private SpriteRenderer hoverRend;

    private SpriteRenderer spriteRend;
    private Sprite greenFill;

    public int req_corite;
    public int req_velrite;
    public int req_nymrite;
    public int req_zyrite;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentParentObject = parentObject.transform.parent.gameObject;
        parentScript = parentParentObject.GetComponent<UI_Window_Part>();

        UI_CrystalManager = GameObject.Find("CrystalManager");
        crystalManagerScript = UI_CrystalManager.GetComponent<UI_CrystalManager>();

        hoverWhite.SetActive(false);
        spriteRend = GetComponent<SpriteRenderer>();
        greenFill = spriteRend.sprite;
        spriteRend.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (!isActivated)
        {
            hoverWhite.SetActive(true);
        }
        MenuManager.current.isMouseOver = true;
        if (!AudioManager.current.UIHover)
        {
            AudioManager.current.currentSFXTrack = 3;
            AudioManager.current.PlaySfx();
            AudioManager.current.UIHover = true;
        }
    }

    void OnMouseExit()
    {
        if (!isActivated)
        {
            hoverWhite.SetActive(false);
        }
        MenuManager.current.isMouseOver = false;
        AudioManager.current.UIHover = false;
    }

    void OnMouseDown()
    {
        if (!isActivated)
        {
            if (PlayerManager.current.numOfCorite >= req_corite && PlayerManager.current.numOfVelrite >= req_velrite && PlayerManager.current.numOfNymrite >= req_nymrite && PlayerManager.current.numOfZyrite >= req_zyrite)
            {
                if (slotNum == 0 || slotNum == 2)
                {
                    ActivateSlot();
                }
                else
                {
                    if (slotNum == 1)
                    {
                        if (parentScript.upgrade1Progress == 1)
                        {
                            ActivateSlot();
                        }
                        else
                        {
                            AudioManager.current.currentSFXTrack = 1;
                            AudioManager.current.PlaySfx();
                        }
                    }
                    if (slotNum == 3)
                    {
                        if (parentScript.upgrade2Progress == 1)
                        {
                            ActivateSlot();
                        }
                        else
                        {
                            AudioManager.current.currentSFXTrack = 1;
                            AudioManager.current.PlaySfx();
                        }
                    }
                }
            }
            else
            {
                AudioManager.current.currentSFXTrack = 1;
                AudioManager.current.PlaySfx();
            }
        }
    }

    private void ActivateSlot()
    {
        spriteRend.sprite = greenFill;
        hoverWhite.SetActive(false);

        crystalManagerScript.temp_corite = req_corite;
        crystalManagerScript.temp_velrite = req_velrite;
        crystalManagerScript.temp_nymrite = req_nymrite;
        crystalManagerScript.temp_zyrite = req_zyrite;
        crystalManagerScript.SubtractFromUpgrading();

        parentScript.currentSlotUpgrading = gameObject;
        parentScript.Invoke("IncreaseUpgradeNum", 0.01f);
        isActivated = true;

        AudioManager.current.currentSFXTrack = 2;
        AudioManager.current.PlaySfx();
    }

    public void ResetSlot()
    {
        spriteRend.sprite = null;
        isActivated = false;
    }

    public void FillSlot()
    {
        spriteRend.sprite = greenFill;
        hoverWhite.SetActive(false);
        isActivated = true;
    }
}

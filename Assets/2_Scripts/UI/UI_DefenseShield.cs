using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DefenseShield : MonoBehaviour
{
    public Animator anim;

    public SpriteRenderer spriteRend;

    public int defShieldNum;

    public bool isFilling;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeHit()
    {
        anim.SetBool("isHit", true);
        PlayerManager.current.currentDefenseShield -= 1;
    }
    public void StartFilling()
    {
        anim.SetBool("startFill", true);
        anim.SetBool("isHit", false);
        isFilling = true;
    }
    public void DoneFilling()
    {
        anim.SetBool("startFill", false);
        isFilling = false;
        if (PlayerManager.current.maxDefenseShield >= 1 && PlayerManager.current.maxDefenseShield <= 2)
        {
            {
                if (PlayerManager.current.currentDefenseShield < 2)
                {
                    PlayerManager.current.currentDefenseShield += 1;
                    PlayerManager.current.shieldActive = true;
                    PlayerManager.current.setShieldActiveUI();
                }
            }
            if (PlayerManager.current.currentDefenseShield > 2)
            {
                PlayerManager.current.currentDefenseShield = 2;
            }
        }
    }

    public void ShowUI()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = true;
    }

    public void HideUI()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealthManager : MonoBehaviour
{
    public GameObject UI_head;
    public GameObject UI_body;
    public GameObject UI_drillArm;
    public GameObject UI_gunArm;
    public GameObject UI_legs;

    public UI_Part_Health partScript_head;
    public UI_Part_Health partScript_body;
    public UI_Part_Health partScript_drillArm;
    public UI_Part_Health partScript_gunArm;
    public UI_Part_Health partScript_legs;

    public GameObject backedUp_UI;
    public SpriteRenderer backedUpSpriteRend;
    public Sprite sprite_BackedUp_No;
    public Sprite sprite_BackedUp_Yes;

    public GameObject shield_UI;
    public SpriteRenderer shieldSpriteRend;
    public Sprite sprite_shield_No;
    public Sprite sprite_shield_Yes;

    // Start is called before the first frame update
    void Start()
    {
        partScript_head = UI_head.GetComponent<UI_Part_Health>();
        partScript_body = UI_body.GetComponent<UI_Part_Health>();
        partScript_drillArm = UI_drillArm.GetComponent<UI_Part_Health>();
        partScript_gunArm = UI_gunArm.GetComponent<UI_Part_Health>();
        partScript_legs = UI_legs.GetComponent<UI_Part_Health>();

        backedUpSpriteRend = backedUp_UI.GetComponent<SpriteRenderer>();
        shieldSpriteRend = shield_UI.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GainHead()
    {
        partScript_head.Invoke("GainWorkerHead", 0.1f);
    }
    public void LoseHead()
    {
        partScript_head.Invoke("PartEmpty", 0.1f);
    }

    public void GainBody()
    {
        partScript_body.Invoke("GainWorkerBody", 0.1f);
    }
    public void LoseBody()
    {
        partScript_body.Invoke("PartEmpty", 0.1f);
    }

    public void GainDrillArm()
    {
        partScript_drillArm.Invoke("GainWorkerDrill", 0.1f);
    }
    public void LoseDrillArm()
    {
        partScript_drillArm.Invoke("PartEmpty", 0.1f);
    }

    public void GainBlasterGun()
    {
        partScript_gunArm.Invoke("GainBlasterGun", 0.1f);
    }
    public void LoseBlasterGun()
    {
        partScript_gunArm.Invoke("PartEmpty", 0.1f);
    }

    public void GainMissileLauncher()
    {
        partScript_gunArm.Invoke("GainMissileLauncher", 0.1f);
    }
    public void LoseMissileLauncher()
    {
        partScript_gunArm.Invoke("PartEmpty", 0.1f);
    }

    public void GainLaserBeam()
    {
        partScript_gunArm.Invoke("GainLaserBeam", 0.1f);
    }
    public void LoseLaserBeam()
    {
        partScript_gunArm.Invoke("PartEmpty", 0.1f);
    }

    public void GainAutoBlaster()
    {
        partScript_gunArm.Invoke("GainAutoBlaster", 0.1f);
    }
    public void LoseAutoBlaster()
    {
        partScript_gunArm.Invoke("PartEmpty", 0.1f);
    }

    public void GainWorkerBoots()
    {
        partScript_legs.Invoke("GainWorkerBoots", 0.1f);
    }
    public void LoseWorkerBoots()
    {
        partScript_legs.Invoke("PartEmpty", 0.1f);
    }

    public void GainJumpBoots()
    {
        partScript_legs.Invoke("GainJumpBoots", 0.1f);
    }
    public void LoseJumpBoots()
    {
        partScript_legs.Invoke("PartEmpty", 0.1f);
    }

    public void BackedUp()
    {
        backedUpSpriteRend.sprite = sprite_BackedUp_Yes;
    }
    public void NotBackedUp()
    {
        backedUpSpriteRend.sprite = sprite_BackedUp_No;
    }

    public void ShieldActive()
    {
        shieldSpriteRend.sprite = sprite_shield_Yes;
    }
    public void ShieldInactive()
    {
        shieldSpriteRend.sprite = sprite_shield_No;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Part_Health : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Sprite emptyObject;
    public Sprite workerHeadObject;
    public Sprite workerBodyObject;
    public Sprite workerDrillObject;
    public Sprite workerBootsObject;
    public Sprite jumpBootsObject;
    public Sprite blasterObject;
    public Sprite missileObject;
    public Sprite beamObject;
    public Sprite autogunObject;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        PartEmpty();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Set Part to Empty
    public void PartEmpty(){
        spriteRend.sprite = emptyObject;
    }


    //Heads
    public void GainWorkerHead(){
        spriteRend.sprite = workerHeadObject;
    }

    //Bodies
    public void GainWorkerBody(){
        spriteRend.sprite = workerBodyObject;
    }

    //Drills
    public void GainWorkerDrill(){
        spriteRend.sprite = workerDrillObject;
    }
    public void GainSoldierDrill(){
        
    }

    //Guns
    public void GainBlasterGun(){
        spriteRend.sprite = blasterObject;
    }
    public void GainMissileLauncher(){
        spriteRend.sprite = missileObject;
    }
    public void GainLaserBeam(){
        spriteRend.sprite = beamObject;
    }
    public void GainAutoBlaster(){
        spriteRend.sprite = autogunObject;
    }

    //Legs
    public void GainWorkerBoots(){
        spriteRend.sprite = workerBootsObject;
    }
    public void GainJumpBoots(){
        spriteRend.sprite = jumpBootsObject;
    }

}

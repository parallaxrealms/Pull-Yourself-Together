using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberMantisAnim : MonoBehaviour
{

  public GameObject parentObject;
  public CyberMantisScript parentScript;

  // Start is called before the first frame update
  void Start()
  {
    parentObject = transform.parent.gameObject;
    parentScript = parentObject.GetComponent<CyberMantisScript>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Active()
  {
    parentScript.Active();
  }

  public void LungeMode()
  {
    parentScript.Lunging();

    AudioManager.current.currentSFXTrack = 128;
    AudioManager.current.PlaySfx();
  }
  public void LungeEnd()
  {
    parentScript.LungeEnd();
  }

  public void CrashSound()
  {
    AudioManager.current.currentSFXTrack = 123;
    AudioManager.current.PlaySfx();
  }

  public void EnableAttackCollider()
  {
    parentScript.EnableAttackCollider();
    AudioManager.current.currentSFXTrack = 121;
    AudioManager.current.PlaySfx();
  }
  public void DisableAttackCollider()
  {
    parentScript.DisableAttackCollider();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Player_Bullet")
    {
      if (!parentScript.isHit)
      {
        if (!parentScript.isDead)
        {
          BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
          parentScript.damageTaken = PlayerManager.current.currentDamage_blaster;
          parentScript.TakeHit();
        }
      }
    }
    if (other.gameObject.tag == "MissileAOE")
    {
      if (!parentScript.isHit)
      {
        if (!parentScript.isDead)
        {
          MissileAOE missileAOEscript = other.gameObject.GetComponent<MissileAOE>();
          parentScript.damageTaken = missileAOEscript.damage;
          parentScript.TakeHit();
        }
      }
    }
    if (other.gameObject.tag == "Player_EnergyBeam")
    {
      if (!parentScript.isHit)
      {
        if (!parentScript.isDead)
        {
          BulletPhysics bulletScript = other.gameObject.GetComponent<BulletPhysics>();
          parentScript.damageTaken = PlayerManager.current.currentDamage_energyBeam;
          parentScript.TakeHit();
        }
      }
    }
  }


  private void OnTriggerStay(Collider other)
  {
    if (other.gameObject.tag == "PlayerDrill")
    {
      if (!parentScript.isHit)
      {
        if (!parentScript.isDead)
        {
          parentScript.damageTaken = PlayerManager.current.currentDamage_workerDrill;
          parentScript.TakeHit();
        }
      }
    }
  }


  public void DestroySelf()
  {
    Destroy(gameObject);
  }
}

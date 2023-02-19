using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
  private GameObject parentObject;
  private PickUpScript pickUpScript;

  public float spawnTimer = .7f;
  public bool isSpawning = true;

  // Start is called before the first frame update
  void Start()
  {
    parentObject = transform.parent.gameObject;
    pickUpScript = parentObject.GetComponent<PickUpScript>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  void FixedUpdate()
  {
    if (isSpawning)
    {
      if (spawnTimer > 0)
      {
        spawnTimer -= Time.deltaTime;
      }
      else
      {
        isSpawning = false;
        spawnTimer = .7f;
      }
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (!isSpawning)
    {
      if (other.gameObject.tag == "Player")
      {
        if (!PlayerManager.current.pickupSelectableActive)
        {
          if (!PlayerManager.current.playerHoldingPart)
          {
            if (!pickUpScript.nameActivated)
            {
              if (pickUpScript.pickupType == 1)
              { //If this pickup is Body
                if (PlayerManager.current.hasHead)
                {
                  if (!PlayerManager.current.hasBody)
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.EnablePickupParticles();
                  }
                  else
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.DisablePickupParticles();
                  }
                }
              }
              else if (pickUpScript.pickupType == 2)
              { //If this pickup is Drill
                if (PlayerManager.current.hasBody)
                {
                  if (!PlayerManager.current.hasDrill)
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.EnablePickupParticles();
                  }
                  else
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.DisablePickupParticles();
                  }
                }
              }
              else if (pickUpScript.pickupType == 3)
              { //If this pickup is Gun
                if (PlayerManager.current.hasBody)
                {
                  if (!PlayerManager.current.hasGun)
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.EnablePickupParticles();
                  }
                  else
                  {
                    if (pickUpScript.gunType != PlayerManager.current.gunType)
                    {
                      pickUpScript.activated = true;
                      pickUpScript.DefaultText();
                      pickUpScript.DrawOutline();
                      pickUpScript.DisablePickupParticles();
                    }
                    else
                    {
                      pickUpScript.activated = true;
                      pickUpScript.DefaultText();
                      pickUpScript.DrawOutline();
                      pickUpScript.DisablePickupParticles();
                    }
                  }
                }
              }
              else if (pickUpScript.pickupType == 4)
              { //If this pickup is Legs
                if (PlayerManager.current.hasBody)
                {
                  if (!PlayerManager.current.hasLegs)
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.EnablePickupParticles();
                  }
                  else
                  {
                    if (pickUpScript.legType != PlayerManager.current.legType)
                    {
                      pickUpScript.activated = true;
                      pickUpScript.DefaultText();
                      pickUpScript.DrawOutline();
                      pickUpScript.DisablePickupParticles();
                    }
                    else
                    {
                      pickUpScript.activated = true;
                      pickUpScript.DefaultText();
                      pickUpScript.DrawOutline();
                      pickUpScript.DisablePickupParticles();
                    }
                  }
                }

              }
              else if (pickUpScript.pickupType == 9)
              { //If this pickup is Backup
                if (!PlayerManager.current.backedUp)
                {
                  if (!pickUpScript.isUsed)
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.EnablePickupParticles();
                  }
                }
                else
                {
                  if (!pickUpScript.isUsed)
                  {
                    pickUpScript.activated = true;
                    pickUpScript.DefaultText();
                    pickUpScript.DrawOutline();
                    pickUpScript.DisablePickupParticles();
                  }
                  else
                  {
                    //Show PickupBotHead Text
                    pickUpScript.activated = true;
                    pickUpScript.EraseText();
                    pickUpScript.DrawOutline();
                    pickUpScript.DisablePickupParticles();
                  }
                }
              }
            }
          }
          else
          {
            if (pickUpScript.nameActivated)
            {
              pickUpScript.activated = false;
              pickUpScript.EraseText();
              pickUpScript.DrawOutline();
              pickUpScript.DisablePickupParticles();
            }
          }
          PlayerManager.current.pickupSelectableActive = true;
        }
        else
        {
          if (PlayerManager.current.playerHoldingPart)
          {
            if (pickUpScript.nameActivated)
            {
              pickUpScript.activated = false;
              pickUpScript.EraseText();
              pickUpScript.DrawOutline();
              pickUpScript.DisablePickupParticles();
            }
          }
        }
      }
    }
  }
  private void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == "Player")
    {
      if (PlayerManager.current.pickupSelectableActive)
      {
        if (pickUpScript.nameActivated)
        {
          if (pickUpScript.pickupType == 1)
          { //If this pickup is Body
            if (PlayerManager.current.hasHead)
            {
              if (!PlayerManager.current.hasBody)
              {
                pickUpScript.activated = false;
                pickUpScript.EraseText();
                pickUpScript.EnablePickupParticles();
              }
              else
              {
                pickUpScript.activated = false;
                pickUpScript.EraseText();
                pickUpScript.DisablePickupParticles();
                pickUpScript.DrawOutline();
              }
            }
          }
          else if (pickUpScript.pickupType == 2)
          { //If this pickup is Drill
            if (PlayerManager.current.hasBody)
            {
              if (!PlayerManager.current.hasDrill)
              {
                pickUpScript.activated = false;
                pickUpScript.EraseText();
                pickUpScript.EnablePickupParticles();
              }
              else
              {
                pickUpScript.activated = false;
                pickUpScript.EraseText();
                pickUpScript.DisablePickupParticles();
                pickUpScript.DrawOutline();
              }
            }
          }
          else if (pickUpScript.pickupType == 3)
          { //If this pickup is Gun
            if (PlayerManager.current.hasBody)
            {
              if (!PlayerManager.current.hasGun)
              {
                pickUpScript.activated = false;
                pickUpScript.EraseText();
                pickUpScript.EnablePickupParticles();
              }
              else
              {
                if (pickUpScript.gunType != PlayerManager.current.gunType)
                {
                  pickUpScript.activated = false;
                  pickUpScript.EraseText();
                  pickUpScript.DisablePickupParticles();
                  pickUpScript.DrawOutline();
                }
                else
                {
                  pickUpScript.activated = false;
                  pickUpScript.EraseText();
                  pickUpScript.DisablePickupParticles();
                  pickUpScript.DrawOutline();
                }
              }
            }
          }
          else if (pickUpScript.pickupType == 4)
          { //If this pickup is Legs
            if (PlayerManager.current.hasBody)
            {
              if (!PlayerManager.current.hasLegs)
              {
                pickUpScript.activated = false;
                pickUpScript.EraseText();
                pickUpScript.EnablePickupParticles();
              }
              else
              {
                if (pickUpScript.legType != PlayerManager.current.legType)
                {
                  pickUpScript.activated = false;
                  pickUpScript.EraseText();
                  pickUpScript.DisablePickupParticles();
                  pickUpScript.DrawOutline();
                }
                else
                {
                  pickUpScript.activated = false;
                  pickUpScript.EraseText();
                  pickUpScript.DisablePickupParticles();
                  pickUpScript.DrawOutline();
                }
              }
            }
          }
          else if (pickUpScript.pickupType == 9)
          { //If this pickup is Backup
            if (!PlayerManager.current.backedUp)
            {
              if (!pickUpScript.isUsed)
              {
                pickUpScript.activated = false;
                pickUpScript.EraseText();
                pickUpScript.EnablePickupParticles();
              }
            }
            else
            {
              pickUpScript.activated = false;
              pickUpScript.EraseText();
              pickUpScript.DisablePickupParticles();
              pickUpScript.DrawOutline();
            }
          }
        }
        PlayerManager.current.pickupSelectableActive = false;
      }


    }
  }
}

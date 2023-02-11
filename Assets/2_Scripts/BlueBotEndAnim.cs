using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBotEndAnim : MonoBehaviour
{
  public GameObject blueBotObject;
  public GameObject blueBot;
  public BlueBotScript bluebotScript;
  private Animator anim;

  public void Start()
  {
    anim = GetComponent<Animator>();
    bluebotScript = blueBotObject.GetComponent<BlueBotScript>();
  }
  public void StartEndDialoge()
  {
    blueBot = Instantiate(blueBotObject, new Vector3(32.5f, -13.5f, 0f), Quaternion.identity) as GameObject;
    blueBot.transform.localScale = new Vector3(-1f, 1f, 1f);
    bluebotScript.ActivateDialogue();
    Destroy(gameObject);
  }

  public void PlayPickupSFX()
  {
    AudioManager.current.currentSFXTrack = 19;
    AudioManager.current.PlaySfx();
  }
}
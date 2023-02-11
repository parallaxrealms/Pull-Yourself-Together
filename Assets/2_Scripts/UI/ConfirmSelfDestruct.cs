using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmSelfDestruct : MonoBehaviour
{
  private GameObject parentObj;
  private UI_Part_Select parentScript;

  public GameObject backup_active;
  public GameObject backup_inactive;

  public GameObject button_yes;
  public GameObject button_no;
  private PartButtonScript yesButtonScript;
  private PartButtonScript noButtonScript;

  // Start is called before the first frame update
  void Start()
  {
    parentObj = transform.parent.gameObject;
    parentScript = parentObj.GetComponent<UI_Part_Select>();

    yesButtonScript = button_yes.GetComponent<PartButtonScript>();

    noButtonScript = button_no.GetComponent<PartButtonScript>();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void OpenConfirmation()
  {
    yesButtonScript.Reset();
    noButtonScript.Reset();
    if (PlayerManager.current.backedUp)
    {
      backup_active.SetActive(true);
      backup_inactive.SetActive(false);
    }
    else
    {
      backup_active.SetActive(false);
      backup_inactive.SetActive(true);
    }
  }

  public void SelfDestruct()
  {
    parentScript.Invoke("DropPart", 0.01f);
    parentScript.CloseConfirmation();
  }

  public void ConfirmClosed()
  {
    parentScript.CloseConfirmation();
  }
}

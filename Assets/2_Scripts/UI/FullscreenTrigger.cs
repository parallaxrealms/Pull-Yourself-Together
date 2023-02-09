using UnityEngine;
using System.Collections;

public class FullscreenTrigger : MonoBehaviour
{
  [SerializeField] private BoxCollider boxCollider;

  void Start()
  {
    boxCollider = GetComponent<BoxCollider>();
    DisableCollider();
  }

  public void DisableCollider()
  {
    boxCollider.enabled = false;
  }
  public void EnableCollider()
  {
    boxCollider.enabled = true;
  }

  void OnMouseDown()
  {
    if (PlayerManager.current.playerHoldingPart)
    {
      PlayerManager.current.DropHeldObject(PlayerManager.current.holdingPartNum);
      DisableCollider();
    }
  }
}
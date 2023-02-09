using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D currentMouseCursor;
    [SerializeField] private Texture2D crosshairs1;
    [SerializeField] private Texture2D mouseDefault;
    [SerializeField] private Texture2D partAdd;

    public GameObject fullScreenObject;
    public FullscreenTrigger fullscreenTriggerScript;
    public GameObject mouseCursor;
    public StickToMouse mouseCursorScript;

    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        currentMouseCursor = mouseDefault;
        Cursor.SetCursor(currentMouseCursor, hotSpot, cursorMode);
        mouseCursorScript = mouseCursor.GetComponent<StickToMouse>();

        fullscreenTriggerScript = fullScreenObject.GetComponent<FullscreenTrigger>();
    }

    private void Update()
    {

    }

    public void ChangeCursorToGun()
    {
        if (!PlayerManager.current.playerHoldingPart)
        {
            currentMouseCursor = crosshairs1;
            Cursor.SetCursor(currentMouseCursor, hotSpot, cursorMode);
            mouseCursorScript.HideCursorObject();
            fullscreenTriggerScript.DisableCollider();
        }
    }
    public void ChangeCursorToDefault()
    {
        if (!PlayerManager.current.playerHoldingPart)
        {
            currentMouseCursor = mouseDefault;
            Cursor.SetCursor(currentMouseCursor, hotSpot, cursorMode);
            mouseCursorScript.HideCursorObject();
            fullscreenTriggerScript.DisableCollider();
        }
    }
    public void ChangeCursorToSelectedPart(int partNum)
    {
        currentMouseCursor = partAdd;
        mouseCursorScript.ChangeCursorTo(partNum);
        Cursor.SetCursor(currentMouseCursor, hotSpot, cursorMode);
        fullscreenTriggerScript.EnableCollider();
    }
    public void HideCursorSelectedPart()
    {
        if (!PlayerManager.current.playerHoldingPart)
        {
            if (mouseCursorScript.isActive)
            {
                mouseCursorScript.HideCursorObject();
            }
        }
    }
}
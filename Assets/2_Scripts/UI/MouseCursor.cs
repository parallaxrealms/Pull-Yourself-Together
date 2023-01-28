using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D currentMouseCursor;
    [SerializeField] private Texture2D crosshairs1;
    [SerializeField] private Texture2D mouseDefault;

    [SerializeField] public Texture2D cursor_HeadObject;
    [SerializeField] public Texture2D cursor_BodyObject;
    [SerializeField] public Texture2D cursor_DrillObject;
    [SerializeField] public Texture2D cursor_BlasterObject;
    [SerializeField] public Texture2D cursor_MissileObject;
    [SerializeField] public Texture2D cursor_LaserObject;
    [SerializeField] public Texture2D cursor_LegsObject;
    [SerializeField] public Texture2D cursor_LegsJumpObject;


    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        currentMouseCursor = mouseDefault;
        Cursor.SetCursor(currentMouseCursor, hotSpot, cursorMode);

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
        }
    }
    public void ChangeCursorToDefault()
    {
        if (!PlayerManager.current.playerHoldingPart)
        {
            currentMouseCursor = mouseDefault;
            Cursor.SetCursor(currentMouseCursor, hotSpot, cursorMode);
        }
    }
    public void ChangeCursorToSelectedPart(int partNum)
    {
        if (partNum == 0) //Worker Head
        {
            currentMouseCursor = cursor_HeadObject;
        }
        if (partNum == 1) //Worker Body
        {
            currentMouseCursor = cursor_BodyObject;
        }
        if (partNum == 2) //Worker Drill
        {
            currentMouseCursor = cursor_DrillObject;
        }
        if (partNum == 3) //Blaster Gun
        {
            currentMouseCursor = cursor_BlasterObject;
        }
        if (partNum == 4) //Missile Launcher
        {
            currentMouseCursor = cursor_MissileObject;
        }
        if (partNum == 5) //Energy Beam
        {
            currentMouseCursor = cursor_LaserObject;
        }
        if (partNum == 6) //Worker Legs
        {
            currentMouseCursor = cursor_LegsJumpObject;
        }
        if (partNum == 7) //Jump Legs
        {
            currentMouseCursor = cursor_LegsObject;
        }
        Cursor.SetCursor(currentMouseCursor, hotSpot, cursorMode);
    }
}
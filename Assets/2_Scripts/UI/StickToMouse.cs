using UnityEngine;
using System.Collections;

public class StickToMouse : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private Sprite cursor_HeadObject;
    [SerializeField] private Sprite cursor_BodyObject;
    [SerializeField] private Sprite cursor_DrillObject;
    [SerializeField] private Sprite cursor_BlasterObject;
    [SerializeField] private Sprite cursor_MissileObject;
    [SerializeField] private Sprite cursor_LaserObject;
    [SerializeField] private Sprite cursor_LegsObject;
    [SerializeField] private Sprite cursor_LegsJumpObject;

    public bool isActive = false;

    void Start()
    {
        DontDestroyOnLoad(transform);
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = false;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = new Vector3(objectPosition.x, objectPosition.y, 0);
    }

    public void ChangeCursorTo(int partNum)
    {
        if (partNum == 0) //Worker Head
        {
            spriteRend.sprite = cursor_HeadObject;
        }
        if (partNum == 1) //Worker Body
        {
            spriteRend.sprite = cursor_BodyObject;
        }
        if (partNum == 2) //Worker Drill
        {
            spriteRend.sprite = cursor_DrillObject;
        }
        if (partNum == 3) //Blaster Gun
        {
            spriteRend.sprite = cursor_BlasterObject;
        }
        if (partNum == 4) //Missile Launcher
        {
            spriteRend.sprite = cursor_MissileObject;
        }
        if (partNum == 5) //Energy Beam
        {
            spriteRend.sprite = cursor_LaserObject;
        }
        if (partNum == 6) //Worker Legs
        {
            spriteRend.sprite = cursor_LegsObject;
        }
        if (partNum == 7) //Jump Legs
        {
            spriteRend.sprite = cursor_LegsJumpObject;
        }
        spriteRend.enabled = true;
        isActive = true;
    }
    public void HideCursorObject()
    {
        spriteRend.enabled = false;
        isActive = false;
    }
}
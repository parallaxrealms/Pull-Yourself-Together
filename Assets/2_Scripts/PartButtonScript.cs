using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartButtonScript : MonoBehaviour
{

    public Sprite spriteInactive;
    public Sprite spriteActive;

    private SpriteRenderer spriteRend;

    public UI_Part_Select parentScript;
    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();

        parentScript = transform.parent.GetComponent<UI_Part_Select>();
        spriteRend.sprite = spriteInactive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver(){
        spriteRend.sprite = spriteActive;
        MenuManager.current.isMouseOver = true;
    }

    void OnMouseExit(){
        spriteRend.sprite = spriteInactive;
        MenuManager.current.isMouseOver = false;
    }

    public void OnMouseDown(){
        parentScript.Invoke("DropPart",0.01f);
    }
}

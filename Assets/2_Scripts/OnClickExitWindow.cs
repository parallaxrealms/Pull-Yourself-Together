using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickExitWindow : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Sprite origSprite;
    public Sprite hoverSprite;

    public GameObject parentObject;
    public UI_Window_Part parentScript;
    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        parentObject = transform.parent.gameObject;
        parentScript = parentObject.GetComponent<UI_Window_Part>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        spriteRend.sprite = origSprite;
    }

    private void OnMouseOver(){
        spriteRend.sprite = hoverSprite;
        MenuManager.current.isMouseOver = true;
    }

    private void OnMouseExit(){
        spriteRend.sprite = origSprite;
        MenuManager.current.isMouseOver = false;
    }

    private void OnMouseDown() {
        MenuManager.current.isMouseOver = true;
        spriteRend.sprite = origSprite;
        parentScript.Invoke("CloseAllWindows",0.1f);
        MenuManager.current.isMouseOver = false;
    }
}

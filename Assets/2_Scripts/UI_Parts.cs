using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Parts : MonoBehaviour
{

    public bool isEnabled = false;
    public SpriteRenderer spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePartsUI(){
        spriteRend.enabled = true;
    }

    public void DisablePartsUI(){
        spriteRend.enabled = false;
    }
}

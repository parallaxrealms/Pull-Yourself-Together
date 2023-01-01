using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_HoverUpAndDown : MonoBehaviour
{
    
    private float timer = 2.00f;
    private bool moveUp = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!moveUp){
            if(timer >= 0){
                timer -= Time.deltaTime;
                transform.Translate(Vector3.up * 0.5f * Time.deltaTime, Space.Self);
            }
            else{
                timer = 2.00f;
                moveUp = true;
            }
        }

        if(moveUp){
            if(timer >= 0){
                timer -= Time.deltaTime;
                transform.Translate(Vector3.up * -0.5f * Time.deltaTime, Space.Self);
            }
            else{
                timer = 2.00f;
                moveUp = false;
            }
        }
    }


}

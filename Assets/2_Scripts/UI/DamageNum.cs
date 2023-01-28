using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class DamageNum : MonoBehaviour
{
    private GameObject parentObject;
    private Vector3 spawnPos;
    private Vector3 currentPos;

    public TMP_Text numText;
    public float damageNum;

    private float timer;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0f){
            timer -= Time.deltaTime;

            // move the game object up on the Y axis by its local position
            transform.Translate(Vector3.up * 1.0f * Time.deltaTime, Space.Self);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void DamageInit(){
        numText = GetComponent<TMP_Text>();
        numText.GetComponent<TextMeshProUGUI>().DOFade(0, 1.45f);
        numText.text = damageNum.ToString();
        timer = 1.45f;
    }
}

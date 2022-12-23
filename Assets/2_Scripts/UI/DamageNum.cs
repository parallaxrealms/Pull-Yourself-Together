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
    public int damageNum = 0;

    private float timer = 1.45f;

    
    // Start is called before the first frame update
    void Start()
    {
        numText = GetComponent<TMP_Text>();
        numText.text = damageNum.ToString();

        numText.GetComponent<TextMeshProUGUI>().DOFade(0, 1.45f);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0f){
            timer -= Time.deltaTime;

            // move the game object up on the Y axis by its local position
            transform.Translate(Vector3.up * 1.0f * Time.deltaTime, Space.Self);
        }
        else{
            Destroy(gameObject);
        }
    }
}

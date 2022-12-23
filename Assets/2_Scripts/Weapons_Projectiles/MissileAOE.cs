using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAOE : MonoBehaviour
{
    public float damage;
    public int bulletType = 1;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}

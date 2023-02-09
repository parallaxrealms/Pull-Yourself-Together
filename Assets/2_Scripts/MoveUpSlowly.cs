using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpSlowly : MonoBehaviour
{
    public bool moving = false;
    private float speed = 0.25f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimDirection : MonoBehaviour
{
    private GameObject parentObject;
    private GameObject parentParentObject;
    // Start is called before the first frame update
    void Start()
    {
        parentObject = transform.parent.gameObject;
        parentParentObject = parentObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter(){
        PlayerManager.current.gunFacing = false;
    }

     public void OnMouseExit(){
        PlayerManager.current.gunFacing = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjects : MonoBehaviour
{
    private GameObject playerObject;
    public PlayerControl playerControlScript;

    public GameObject slot_head;
    public PickUpScript slot_headScript;
    public GameObject headObject;

    public GameObject slot_body;
    public PickUpScript slot_bodyScript;
    public GameObject bodyObject;

    public GameObject slot_l_arm;
    public PickUpScript slot_l_armScript;
    public GameObject lArmObject;

    public GameObject slot_r_arm;
    public PickUpScript slot_r_armScript;
    public GameObject rArmObject;

    public GameObject slot_legs;
    public PickUpScript slot_legsScript;
    public GameObject legsObject;

    public GameObject slot_storage1;
    public PickUpScript slot_storage1Script;
    public GameObject storage1Object;

    public GameObject slot_storage2;
    public PickUpScript slot_storage2Script;
    public GameObject storage2Object;

    public GameObject slot_currentlyHeld;
    public PickUpScript slot_heldScript;
    public GameObject currentlyHeldObject;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddGameObject(int slotNum, GameObject pickupObj)
    {
        PickUpScript pickupScript = pickupObj.GetComponent<PickUpScript>();
        if (slotNum == 0)
        {
            if (pickupScript.pickupType == 9)
            {
                GameObject newObj = Instantiate(pickupObj, new Vector3(-9000f, -9000f, -900f), Quaternion.identity) as GameObject;
            }
        }
        if (slotNum == 1)
        {

        }
        if (slotNum == 2)
        {

        }
        if (slotNum == 3)
        {

        }
        if (slotNum == 4)
        {

        }
        if (slotNum == 5)
        {

        }
        if (slotNum == 6)
        {

        }
    }

    public void InactivateChildren()
    {
        Transform[] children = slot_head.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.Inactive();
                }
            }
        }
    }
}
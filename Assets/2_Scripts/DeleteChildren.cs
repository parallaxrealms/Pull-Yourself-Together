using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteChildren : MonoBehaviour
{
    void Start()
    {
        // Call the DeleteChildren function after 0.1 seconds
        Invoke("DeleteAllChildren", 0.1f);
    }

    public void DeleteAllChildren()
    {
        // Get all the children of the GameObject
        Transform[] children = GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
             // Get the script component of the child GameObject
            PickUpScript childScript = children[i].GetComponent<PickUpScript>();

            // Make sure the script component is not null before accessing it
            if (childScript != null)
            {
                Debug.Log(childScript.id);
                childScript.DestroySelf();
            }
        }
    }
}
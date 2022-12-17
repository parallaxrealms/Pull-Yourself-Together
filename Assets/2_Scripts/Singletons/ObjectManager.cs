using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager current;

    public int currentLevelID;
    public List<GameObject> Objects_Temp = new List<GameObject>();

    public List<GameObject> Objects_CoL_0 = new List<GameObject>();
    public List<GameObject> Objects_CoL_1 = new List<GameObject>();
    public List<GameObject> Objects_CoL_2 = new List<GameObject>();

    public List<GameObject> Objects_Abyss_0 = new List<GameObject>();
    public List<GameObject> Objects_Abyss_1 = new List<GameObject>();
    public List<GameObject> Objects_Abyss_Boss = new List<GameObject>();

    private void Awake(){
        DontDestroyOnLoad(this);
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SaveCurrentObjects(){
        Objects_Temp.Clear();
        Objects_Temp = new List<GameObject>(GameController.current.ListPickups);
        IterateThroughObjects(currentLevelID);
    }

    public void IterateThroughObjects(int levelID){
        if(levelID == 1){
            foreach (GameObject obj in Objects_Temp)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();

                GameObject newObj = Instantiate(obj, new Vector3(1000f,1000f,1000f), Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                newObjScript.Invoke("RemoveFromList",0.01f);
                Rigidbody rb = newObj.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                SphereCollider collider = newObj.GetComponent<SphereCollider>();

                newObj.transform.parent = transform;
                newObjScript.scenePos = objScript.scenePos;
                Objects_CoL_0.Add(newObj);
            }
        }
        else if(levelID == 2){
            foreach (GameObject obj in Objects_Temp)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();

                GameObject newObj = Instantiate(obj, new Vector3(1000f,1000f,1000f), Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                newObjScript.Invoke("RemoveFromList",0.01f);
                Rigidbody rb = newObj.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                SphereCollider collider = newObj.GetComponent<SphereCollider>();

                newObj.transform.parent = transform;
                newObjScript.scenePos = objScript.scenePos;
                Objects_CoL_1.Add(newObj);
            }
        }
        else if(levelID == 3){
            foreach (GameObject obj in Objects_Temp)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();

                GameObject newObj = Instantiate(obj, new Vector3(1000f,1000f,1000f), Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                newObjScript.Invoke("RemoveFromList",0.01f);
                Rigidbody rb = newObj.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                SphereCollider collider = newObj.GetComponent<SphereCollider>();

                newObj.transform.parent = transform;
                newObjScript.scenePos = objScript.scenePos;
                Objects_CoL_2.Add(newObj);
            }
        }
        else if(levelID == 4){
            foreach (GameObject obj in Objects_Temp)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();

                GameObject newObj = Instantiate(obj, new Vector3(1000f,1000f,1000f), Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                newObjScript.Invoke("RemoveFromList",0.01f);
                Rigidbody rb = newObj.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                SphereCollider collider = newObj.GetComponent<SphereCollider>();

                newObj.transform.parent = transform;
                newObjScript.scenePos = objScript.scenePos;
                Objects_Abyss_0.Add(newObj);
            }
        }
        else if(levelID == 5){
            foreach (GameObject obj in Objects_Temp)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();

                GameObject newObj = Instantiate(obj, new Vector3(1000f,1000f,1000f), Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                newObjScript.Invoke("RemoveFromList",0.01f);
                Rigidbody rb = newObj.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                SphereCollider collider = newObj.GetComponent<SphereCollider>();

                newObj.transform.parent = transform;
                newObjScript.scenePos = objScript.scenePos;
                Objects_Abyss_1.Add(newObj);
            }
        }
        else if(levelID == 6){
            foreach (GameObject obj in Objects_Temp)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();

                GameObject newObj = Instantiate(obj, new Vector3(1000f,1000f,1000f), Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                newObjScript.Invoke("RemoveFromList",0.01f);
                Rigidbody rb = newObj.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                SphereCollider collider = newObj.GetComponent<SphereCollider>();

                newObj.transform.parent = transform;
                newObjScript.scenePos = objScript.scenePos;
                Objects_Abyss_Boss.Add(newObj);
            }
        }
        Objects_Temp.Clear();
    }

    public void SpawnCachedPickups(){
        if(currentLevelID == 1){
            foreach (GameObject obj in Objects_CoL_0)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();
                Vector3 spawnPos = objScript.scenePos;

                GameObject newObj = Instantiate(obj, spawnPos, Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                Debug.Log("spawned: " + newObj + " at: " + spawnPos);
            }
        }
        else if(currentLevelID == 2){
            foreach (GameObject obj in Objects_CoL_1)
            {
                PickUpScript objScript = obj.GetComponent<PickUpScript>();
                Vector3 spawnPos = objScript.scenePos;

                GameObject newObj = Instantiate(obj, spawnPos, Quaternion.identity) as GameObject;
                PickUpScript newObjScript = newObj.GetComponent<PickUpScript>();
                Debug.Log("spawned: " + newObj + " at: " + spawnPos);
            }
        }
        else if(currentLevelID == 3){

        }
        else if(currentLevelID == 4){

        }
        else if(currentLevelID == 5){

        }
        else if(currentLevelID == 6){

        }
        ClearCurrentObjects();
    }

    public void ClearCurrentObjects(){
        if(currentLevelID == 1){
            Objects_CoL_0.Clear();
        }
        else if(currentLevelID == 2){
            Objects_CoL_1.Clear();
        }
        else if(currentLevelID == 3){
            Objects_CoL_2.Clear();
        }
        else if(currentLevelID == 4){
            Objects_Abyss_0.Clear();
        }
        else if(currentLevelID == 5){
            Objects_Abyss_0.Clear();
        }
        else if(currentLevelID == 6){
            Objects_Abyss_Boss.Clear();
        }
        Objects_Temp.Clear();
    }
}

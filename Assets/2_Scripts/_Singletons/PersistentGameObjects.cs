using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PersistentGameObjects : MonoBehaviour
{
    public static PersistentGameObjects current;

    public GameObject CoL_0_GameObjects;
    public GameObject CoL_1_GameObjects;

   // Declare a static dictionary to store the list of PersistentObjects for each scene.
    // The dictionary will use the names of the scenes as keys and the PersistentObjects as values.
    [SerializeField]
    private static Dictionary<string, List<PersistentObject>> sceneGameObjects = new Dictionary<string, List<PersistentObject>>();

    [SerializeField]
    private List<GameObject> CoL_0_Objects = new List<GameObject>();
    [SerializeField]
    private List<GameObject> CoL_1_Objects = new List<GameObject>();

    // Declare a static integer to store the next available ID for a game object.
    public int nextID = 0;

    [System.Serializable]
    public class PersistentObject{
        // Declare fields for the ID and game object.
        public int ID;
        public GameObject gameObject;

        // Constructor for the PersistentObject class.
        public PersistentObject(int ID, GameObject gameObject)
        {
            this.ID = ID;
            this.gameObject = gameObject;
        }
    }

    private void Awake()
    {
        // Don't destroy this game object when a new scene is loaded
        DontDestroyOnLoad(gameObject);
        current = this;

        nextID++;
    }

    public void RegisterNewGameObject(GameObject gameObject){
        // Create a new PersistentObject for the game object.
        PersistentObject persistentObject = new PersistentObject(nextID, gameObject);

        // Increment the next available ID.
        nextID++;

        Debug.Log("gameObject Reg " + gameObject.name);
    }

     public void AddGameObject(GameObject gameObject, string sceneName){
        // Get the name of the current scene.
        string currentSceneName = SceneManager.GetActiveScene().name;

        if(sceneName == currentSceneName){
            // Create a new PersistentObject for the game object.
            PersistentObject persistentObject = new PersistentObject(nextID, gameObject);

            // Increment the next available ID.
            nextID++;
            Debug.Log("gameObject " + gameObject.name);

            // Get the list of PersistentObjects for the current scene.
            List<PersistentObject> gameObjects = GetSceneGameObjects(sceneName);

            // Add the PersistentObject to the list.
            gameObjects.Add(persistentObject);

            // Get the pickup script of the PersistentObject
            PickUpScript objSript = persistentObject.gameObject.GetComponent<PickUpScript>();
            objSript.SetScenePos();

            // Make PersistentObject a child
            if(sceneName == "CoL_0"){
                persistentObject.gameObject.transform.SetParent(CoL_0_GameObjects.transform);
            }
            if(sceneName == "CoL_1"){
                persistentObject.gameObject.transform.SetParent(CoL_1_GameObjects.transform);
            }

            Debug.Log("Add Game Object: " + gameObject + " " + nextID);
        }
    }

    public static GameObject GetGameObject(int ID)
    {
        // Get the name of the current scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Get the list of PersistentObjects for the current scene.
        List<PersistentObject> gameObjects = GetSceneGameObjects(sceneName);

        // Find the PersistentObject with the specified ID in the list.
        PersistentObject persistentObject = gameObjects.Find(obj => obj.ID == ID);

        // Return the game object from the PersistentObject, or null if it doesn't exist.
        return persistentObject != null ? persistentObject.gameObject : null;
    }

    public void RemoveGameObject(int ID)
    {
        // Get the name of the current scene.
        string sceneName = SceneManager.GetActiveScene().name;
        
        // Get the list of PersistentObjects for the current scene.
        List<PersistentObject> gameObjects = GetSceneGameObjects(sceneName);

        // Find the PersistentObject with the specified ID in the list.

        PersistentObject persistentObject = gameObjects.Find(obj => obj.ID == ID);
        // Remove the PersistentObject from the list.
        gameObjects.Remove(persistentObject);

        Debug.Log("Remove Game Object: " + gameObject + " " + ID);
    }

    // This function returns the list of PersistentObjects for the specified scene.
    // If the list doesn't exist, it creates a new one and adds it to the dictionary.
    private static List<PersistentObject> GetSceneGameObjects(string sceneName)
    {
        if (!sceneGameObjects.ContainsKey(sceneName))
        {
            sceneGameObjects[sceneName] = new List<PersistentObject>();
        }
        return sceneGameObjects[sceneName];
    }

    // This function will be called automatically by Unity when a new scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the name of the loaded scene.
        string sceneName = scene.name;

        // Check if the scene is new
        if(GameController.current.newScene){
            // Get the list of PersistentObjects for the loaded scene.
            List<PersistentObject> gameObjects = GetSceneGameObjects(sceneName);

            // Iterate through the list of PersistentObjects in reverse order.
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                // Get the PersistentObject at the current index.
                PersistentObject persistentObject = gameObjects[i];

                // Use the GameObject.Find function to check if a game object with the same ID already exists in the scene.
                GameObject gameObject = GameObject.Find("ID:" + persistentObject.ID);

                if (gameObject == null)
                {
                    // Instantiate the game object.
                    gameObject = Instantiate(persistentObject.gameObject);
                    // Set the ID of the game object.
                    gameObject.GetComponent<PickUpScript>().id = persistentObject.ID;

                    if(persistentObject.ID == 0){
                        gameObject.GetComponent<PickUpScript>().DestroySelf();
                    }
                }
                else{
                    gameObject.GetComponent<PickUpScript>().DestroySelf();
                }
            }
        }
        else if(!GameController.current.newScene){
            // Iterate through the list of PersistentObjects in reverse order.
            if(sceneName == "CoL_0"){
                for (int i = CoL_0_Objects.Count - 1; i >= 0; i--){
                    GameObject persistentObject = CoL_0_Objects[i];
                    PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                    objScript.Active();
                    Debug.Log("objScript active: " + objScript.id);
                }
            }
            else if(sceneName == "CoL_1"){
                for (int i = CoL_1_Objects.Count - 1; i >= 0; i--){
                    GameObject persistentObject = CoL_1_Objects[i];
                    PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                    objScript.Active();
                    Debug.Log("objScript active: " + objScript.id);
                }
            }
        }
        DeleteAllChildrenWithID0();
    }

    // This function will be called when the script is enabled.
    private void OnEnable()
    {
        // Register the OnSceneLoaded function as a callback for the sceneLoaded event.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // This function will be called when the script is disabled.
    private void OnDisable()
    {
        // Unregister the OnSceneLoaded function as a callback for the sceneLoaded event.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TransferPersistentObjects()
    {
        Debug.Log("TransferPersistentObjects");
        // Get the name of the current scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Iterate through the list of PersistentObjects and add them to seperate List based on Scene
        if(sceneName == "CoL_0"){
            // Get all the children of the GameObjects
            Transform[] children = CoL_0_GameObjects.GetComponentsInChildren<Transform>();

            CoL_0_Objects.Clear();
            for (int i = 0; i < children.Length; i++){
                PickUpScript childScript = children[i].GetComponent<PickUpScript>();

                if(childScript != null){
                    childScript.Inactive();
                    CoL_0_Objects.Add(children[i].gameObject);
                }
            }
        }
        else if(sceneName == "CoL_1"){
            // Get all the children of the GameObjects
            Transform[] children = CoL_1_GameObjects.GetComponentsInChildren<Transform>();

            CoL_1_Objects.Clear();
              for (int i = 0; i < children.Length; i++){
                PickUpScript childScript = children[i].GetComponent<PickUpScript>();

                if(childScript != null){
                    childScript.Inactive();
                    CoL_1_Objects.Add(children[i].gameObject);
                }
            }
        }
        sceneGameObjects[sceneName].Clear();
    }

    // This function removes any persistent game objects with an ID of 0.
    public void DeleteAllChildrenWithID0()
    {
        // Get the name of the current scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Get all the children of the GameObjects
        if(sceneName == "CoL_0"){
            Debug.Log(CoL_0_GameObjects);
            Transform[] children = CoL_0_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++){
                // Get the script component of the child GameObject
                PickUpScript childScript = children[i].GetComponent<PickUpScript>();

                // Make sure the script component is not null before accessing it
                if (childScript != null)
                {
                    if(childScript.id == 0){
                        Debug.Log("DELETED: " + childScript.id);
                        childScript.DestroySelf();
                    }
                }
            }
        }
        else if(sceneName == "CoL_1"){
            Transform[] children = CoL_1_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++){
                // Get the script component of the child GameObject
                PickUpScript childScript = children[i].GetComponent<PickUpScript>();

                // Make sure the script component is not null before accessing it
                if (childScript != null)
                {
                    if(childScript.id == 0){
                        Debug.Log("DELETED: " + childScript.id);
                        childScript.DestroySelf();
                    }
                }
            }
        }
        Debug.Log("DeleteAllChildrenWithID0");
    }
}
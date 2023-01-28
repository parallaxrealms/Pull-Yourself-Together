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
    public GameObject CoL_2_GameObjects;
    public GameObject Abyss_0_GameObjects;
    public GameObject Abyss_1_GameObjects;
    public GameObject Abyss_Boss_GameObjects;

    public GameObject backupObjectsContainer;

    // Declare a static dictionary to store the list of PersistentObjects for each scene.
    // The dictionary will use the names of the scenes as keys and the PersistentObjects as values.
    [SerializeField]
    private static Dictionary<string, List<PersistentObject>> sceneGameObjects = new Dictionary<string, List<PersistentObject>>();

    [SerializeField]
    private List<GameObject> CoL_0_Objects = new List<GameObject>();
    [SerializeField]
    private List<GameObject> CoL_1_Objects = new List<GameObject>();
    [SerializeField]
    private List<GameObject> CoL_2_Objects = new List<GameObject>();
    [SerializeField]
    private List<GameObject> Abyss_1_Objects = new List<GameObject>();
    [SerializeField]
    private List<GameObject> Abyss_0_Objects = new List<GameObject>();
    [SerializeField]
    private List<GameObject> Abyss_Boss_Objects = new List<GameObject>();

    // Declare a static integer to store the next available ID for a game object.
    public int nextID = 0;

    [System.Serializable]
    public class PersistentObject
    {
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

    public void RegisterNewGameObject(GameObject gameObject)
    {
        // Create a new PersistentObject for the game object.
        PersistentObject persistentObject = new PersistentObject(nextID, gameObject);

        // Increment the next available ID.
        nextID++;
    }

    public void AddGameObject(GameObject gameObject, string sceneName)
    {
        // Get the name of the current scene.
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (sceneName == currentSceneName)
        {
            // Create a new PersistentObject for the game object.
            PersistentObject persistentObject = new PersistentObject(nextID, gameObject);

            // Increment the next available ID.
            nextID++;

            // Get the list of PersistentObjects for the current scene.
            List<PersistentObject> gameObjects = GetSceneGameObjects(sceneName);

            // Add the PersistentObject to the list.
            gameObjects.Add(persistentObject);

            // Get the script of the PersistentObject
            if (persistentObject.gameObject.GetComponent<PickUpScript>() != null)
            {
                PickUpScript objSript = persistentObject.gameObject.GetComponent<PickUpScript>();
                objSript.SetScenePos();
            }
            if (persistentObject.gameObject.GetComponent<CrystalScript>() != null)
            {
                CrystalScript objSript = persistentObject.gameObject.GetComponent<CrystalScript>();
                objSript.SetScenePos();
            }

            // Make PersistentObject a child
            if (sceneName == "CoL_0")
            {
                persistentObject.gameObject.transform.SetParent(CoL_0_GameObjects.transform);
            }
            if (sceneName == "CoL_1")
            {
                persistentObject.gameObject.transform.SetParent(CoL_1_GameObjects.transform);
            }
            if (sceneName == "CoL_2")
            {
                persistentObject.gameObject.transform.SetParent(CoL_2_GameObjects.transform);
            }
            if (sceneName == "Abyss_0")
            {
                persistentObject.gameObject.transform.SetParent(Abyss_0_GameObjects.transform);
            }
            if (sceneName == "Abyss_1")
            {
                persistentObject.gameObject.transform.SetParent(Abyss_1_GameObjects.transform);
            }
            if (sceneName == "Abyss_Boss")
            {
                persistentObject.gameObject.transform.SetParent(Abyss_Boss_GameObjects.transform);
            }

            // Get the script of the PersistentObject
            if (persistentObject.gameObject.GetComponent<PickUpScript>() != null)
            {
                PickUpScript objScript = persistentObject.gameObject.GetComponent<PickUpScript>();
                objScript.SetOriginalParent();
            }
            if (persistentObject.gameObject.GetComponent<CrystalScript>() != null)
            {
                CrystalScript objScript = persistentObject.gameObject.GetComponent<CrystalScript>();
                objScript.SetOriginalParent();
            }

        }
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
    }

    // This function will be called automatically by Unity when a new scene is loaded.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the name of the loaded scene.
        string sceneName = scene.name;

        // Check if the scene is new
        if (GameController.current.newScene)
        {
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

                    // Get the script of the PersistentObject
                    if (gameObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objSript = gameObject.GetComponent<PickUpScript>();
                        // Set the ID of the game object.
                        objSript.id = persistentObject.ID;
                        if (persistentObject.ID == 0)
                        {
                            objSript.DestroySelf();
                        }
                    }
                    if (gameObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objSript = gameObject.GetComponent<CrystalScript>();
                        // Set the ID of the game object.
                        objSript.id = persistentObject.ID;
                        if (persistentObject.ID == 0)
                        {
                            objSript.DestroySelf();
                        }
                    }
                }
                else
                {
                    if (gameObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objSript = gameObject.GetComponent<PickUpScript>();
                        objSript.DestroySelf();
                    }
                    if (gameObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objSript = gameObject.GetComponent<CrystalScript>();
                        objSript.DestroySelf();
                    }
                }
            }
        }
        else if (!GameController.current.newScene)
        {
            // Iterate through the list of PersistentObjects and activate them
            if (sceneName == "CoL_0")
            {
                for (int i = CoL_0_Objects.Count - 1; i >= 0; i--)
                {
                    GameObject persistentObject = CoL_0_Objects[i];

                    if (persistentObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                        objScript.Active();
                    }
                    if (persistentObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objScript = persistentObject.GetComponent<CrystalScript>();
                        objScript.Active();
                    }
                }

                // Iterate through the list of backupObjects and activate them
                Transform[] children = backupObjectsContainer.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                        if (childScript != null)
                        {
                            if (childScript.sceneName == sceneName)
                            {
                                childScript.Active();
                            }
                        }
                    }
                }
            }
            else if (sceneName == "CoL_1")
            {
                for (int i = CoL_1_Objects.Count - 1; i >= 0; i--)
                {
                    GameObject persistentObject = CoL_1_Objects[i];

                    if (persistentObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                        objScript.Active();
                    }
                    if (persistentObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objScript = persistentObject.GetComponent<CrystalScript>();
                        objScript.Active();
                    }
                }
                // Iterate through the list of backupObjects and activate them
                Transform[] children = backupObjectsContainer.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                        if (childScript != null)
                        {
                            if (childScript.sceneName == sceneName)
                            {
                                childScript.Active();
                            }
                        }
                    }
                }
            }
            else if (sceneName == "CoL_2")
            {
                for (int i = CoL_2_Objects.Count - 1; i >= 0; i--)
                {
                    GameObject persistentObject = CoL_2_Objects[i];

                    if (persistentObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                        objScript.Active();
                    }
                    if (persistentObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objScript = persistentObject.GetComponent<CrystalScript>();
                        objScript.Active();
                    }
                }
                // Iterate through the list of backupObjects and activate them
                Transform[] children = backupObjectsContainer.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                        if (childScript != null)
                        {
                            if (childScript.sceneName == sceneName)
                            {
                                childScript.Active();
                            }
                        }
                    }
                }
            }
            else if (sceneName == "Abyss_0")
            {
                for (int i = Abyss_0_Objects.Count - 1; i >= 0; i--)
                {
                    GameObject persistentObject = Abyss_0_Objects[i]
                    ;
                    if (persistentObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                        objScript.Active();
                    }
                    if (persistentObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objScript = persistentObject.GetComponent<CrystalScript>();
                        objScript.Active();
                    }
                }
                // Iterate through the list of backupObjects and activate them
                Transform[] children = backupObjectsContainer.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                        if (childScript != null)
                        {
                            if (childScript.sceneName == sceneName)
                            {
                                childScript.Active();
                            }
                        }
                    }
                }
            }
            else if (sceneName == "Abyss_1")
            {
                for (int i = Abyss_1_Objects.Count - 1; i >= 0; i--)
                {
                    GameObject persistentObject = Abyss_1_Objects[i];

                    if (persistentObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                        objScript.Active();
                    }
                    if (persistentObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objScript = persistentObject.GetComponent<CrystalScript>();
                        objScript.Active();
                    }
                }
                // Iterate through the list of backupObjects and activate them
                Transform[] children = backupObjectsContainer.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                        if (childScript != null)
                        {
                            if (childScript.sceneName == sceneName)
                            {
                                childScript.Active();
                            }
                        }
                    }
                }
            }
            else if (sceneName == "Abyss_Boss")
            {
                for (int i = Abyss_Boss_Objects.Count - 1; i >= 0; i--)
                {
                    GameObject persistentObject = Abyss_Boss_Objects[i];

                    if (persistentObject.GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript objScript = persistentObject.GetComponent<PickUpScript>();
                        objScript.Active();
                    }
                    if (persistentObject.GetComponent<CrystalScript>() != null)
                    {
                        CrystalScript objScript = persistentObject.GetComponent<CrystalScript>();
                        objScript.Active();
                    }
                }
                // Iterate through the list of backupObjects and activate them
                Transform[] children = backupObjectsContainer.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].GetComponent<PickUpScript>() != null)
                    {
                        PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                        if (childScript != null)
                        {
                            if (childScript.sceneName == sceneName)
                            {
                                childScript.Active();
                            }
                        }
                    }
                }
            }
        }

        InactivateBackupChildren();
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
        // Get the name of the current scene.
        string sceneName = SceneManager.GetActiveScene().name;

        // Iterate through the list of PersistentObjects and add them to seperate List based on Scene
        if (sceneName == "CoL_0")
        {
            // Get all the children of the GameObjects
            Transform[] children = CoL_0_GameObjects.GetComponentsInChildren<Transform>();

            CoL_0_Objects.Clear();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        CoL_0_Objects.Add(children[i].gameObject);
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        CoL_0_Objects.Add(children[i].gameObject);
                    }
                }
            }
        }
        else if (sceneName == "CoL_1")
        {
            // Get all the children of the GameObjects
            Transform[] children = CoL_1_GameObjects.GetComponentsInChildren<Transform>();

            CoL_1_Objects.Clear();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        CoL_1_Objects.Add(children[i].gameObject);
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        CoL_1_Objects.Add(children[i].gameObject);
                    }
                }
            }
        }
        else if (sceneName == "CoL_2")
        {
            // Get all the children of the GameObjects
            Transform[] children = CoL_2_GameObjects.GetComponentsInChildren<Transform>();

            CoL_2_Objects.Clear();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        CoL_2_Objects.Add(children[i].gameObject);
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        CoL_2_Objects.Add(children[i].gameObject);
                    }
                }
            }
        }
        else if (sceneName == "Abyss_0")
        {
            // Get all the children of the GameObjects
            Transform[] children = Abyss_0_GameObjects.GetComponentsInChildren<Transform>();

            Abyss_0_Objects.Clear();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        Abyss_0_Objects.Add(children[i].gameObject);
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        Abyss_0_Objects.Add(children[i].gameObject);
                    }
                }
            }
        }
        else if (sceneName == "Abyss_1")
        {
            // Get all the children of the GameObjects
            Transform[] children = Abyss_1_GameObjects.GetComponentsInChildren<Transform>();

            Abyss_1_Objects.Clear();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        Abyss_1_Objects.Add(children[i].gameObject);
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        Abyss_1_Objects.Add(children[i].gameObject);
                    }
                }
            }
        }
        else if (sceneName == "Abyss_Boss")
        {
            // Get all the children of the GameObjects
            Transform[] children = Abyss_Boss_GameObjects.GetComponentsInChildren<Transform>();

            Abyss_Boss_Objects.Clear();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        Abyss_Boss_Objects.Add(children[i].gameObject);
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        childScript.Inactive();
                        Abyss_Boss_Objects.Add(children[i].gameObject);
                    }
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
        if (sceneName == "CoL_0")
        {
            Transform[] children = CoL_0_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
            }
        }
        else if (sceneName == "CoL_1")
        {
            Transform[] children = CoL_1_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
            }
        }
        else if (sceneName == "CoL_2")
        {
            Transform[] children = CoL_2_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
            }
        }
        else if (sceneName == "Abyss_0")
        {
            Transform[] children = Abyss_0_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
            }
        }
        else if (sceneName == "Abyss_1")
        {
            Transform[] children = Abyss_1_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
            }
        }
        else if (sceneName == "Abyss_Boss")
        {
            Transform[] children = Abyss_Boss_GameObjects.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].GetComponent<PickUpScript>() != null)
                {
                    PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
                if (children[i].GetComponent<CrystalScript>() != null)
                {
                    CrystalScript childScript = children[i].GetComponent<CrystalScript>();
                    if (childScript != null)
                    {
                        if (childScript.id == 0)
                        {
                            childScript.DestroySelf();
                        }
                    }
                }
            }
        }
    }

    public void DeleteAllChildren()
    {

        Transform[] children0 = CoL_0_GameObjects.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children0.Length; i++)
        {
            if (children0[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children0[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
            if (children0[i].GetComponent<CrystalScript>() != null)
            {
                CrystalScript childScript = children0[i].GetComponent<CrystalScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
        }

        Transform[] children1 = CoL_1_GameObjects.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children1.Length; i++)
        {
            if (children1[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children1[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
            if (children1[i].GetComponent<CrystalScript>() != null)
            {
                CrystalScript childScript = children1[i].GetComponent<CrystalScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
        }

        Transform[] children2 = CoL_2_GameObjects.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children2.Length; i++)
        {
            if (children2[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children2[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
            if (children2[i].GetComponent<CrystalScript>() != null)
            {
                CrystalScript childScript = children2[i].GetComponent<CrystalScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
        }

        Transform[] children3 = Abyss_0_GameObjects.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children3.Length; i++)
        {
            if (children3[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children3[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
            if (children3[i].GetComponent<CrystalScript>() != null)
            {
                CrystalScript childScript = children3[i].GetComponent<CrystalScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
        }

        Transform[] children4 = Abyss_1_GameObjects.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children4.Length; i++)
        {
            if (children4[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children4[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
            if (children4[i].GetComponent<CrystalScript>() != null)
            {
                CrystalScript childScript = children4[i].GetComponent<CrystalScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
        }

        Transform[] children5 = Abyss_Boss_GameObjects.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children5.Length; i++)
        {
            if (children5[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children5[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
            if (children5[i].GetComponent<CrystalScript>() != null)
            {
                CrystalScript childScript = children5[i].GetComponent<CrystalScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
        }

        Transform[] children6 = backupObjectsContainer.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children6.Length; i++)
        {
            if (children6[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children6[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    childScript.DestroySelf();
                }
            }
        }
    }

    public void InactivateBackupChildren()
    {
        // Get the name of the current scene.
        string sceneName = SceneManager.GetActiveScene().name;

        Transform[] children = backupObjectsContainer.GetComponentsInChildren<Transform>();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].GetComponent<PickUpScript>() != null)
            {
                PickUpScript childScript = children[i].GetComponent<PickUpScript>();
                if (childScript != null)
                {
                    if (childScript.sceneName != sceneName)
                    {
                        childScript.Inactive();
                    }
                }
            }
        }
    }

    public void ResetPersistentGameObjects()
    {
        nextID = 0;
        DeleteAllChildren();
        CoL_0_Objects.Clear();
        CoL_1_Objects.Clear();
        CoL_2_Objects.Clear();
        Abyss_0_Objects.Clear();
        Abyss_1_Objects.Clear();
        Abyss_Boss_Objects.Clear();
        sceneGameObjects.Clear();
    }
}
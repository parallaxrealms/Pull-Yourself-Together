using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public string triggerName;
    public string triggerConnectionName;
    public string sceneToName;
    public string playerWalkDirection;

    public bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        triggerName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!triggered)
            {
                if (triggerName == "Top_To_Abyss_1")
                {
                    if (GameObject.Find("CyberMantis") != null)
                    {
                        GameObject cyberMantis = GameObject.Find("CyberMantis");
                        CyberMantisScript bossScript = cyberMantis.GetComponent<CyberMantisScript>();
                        bossScript.SetLastPos();
                    }
                }
                PlayerManager.current.sceneDirection = playerWalkDirection;
                GameController.current.sceneChangeName = sceneToName;
                GameController.current.triggerSpawnName = triggerConnectionName;
                GameController.current.sceneChanging = true;
                MenuManager.current.ChangeSceneTo();
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }
}

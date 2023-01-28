using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToBossRoom : MonoBehaviour
{
    public GameObject playerObject;

    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = PlayerManager.current.currentPlayerObject;
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
                PlayerManager.current.sceneDirection = "Right";
                GameController.current.sceneChangeName = "CoL_2";
                GameController.current.triggerSpawnName = "Top_To_CoL_0";
                MenuManager.current.ChangeSceneTo();
                triggered = true;
            }
        }
    }
}

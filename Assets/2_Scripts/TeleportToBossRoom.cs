using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToBossRoom : MonoBehaviour
{
    public GameObject playerObject;  
      
    // Start is called before the first frame update
    void Start()
    {
        playerObject = PlayerManager.current.currentPlayerObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            GameController.current.Invoke("DestroyAllEnemyObjects", 0.1f);
            PlayerManager.current.Invoke("TransferPlayerProperties", 0.01f);
            SceneManager.LoadScene("BossFight");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CrystalManager : MonoBehaviour
{

    public GameObject UIObject_Corite;
    public SpriteRenderer rend_Corite;
    public Sprite UIObject_CoriteInactive;
    public Sprite UIObject_CoriteActive;
    public GameObject Num_Corite;
    public Text text_num_Corite;

    public GameObject UIObject_Nymrite;
    public SpriteRenderer rend_Nymrite;
    public Sprite UIObject_NymriteInactive;
    public Sprite UIObject_NymriteActive;
    public GameObject Num_Nymrite;
    public Text text_num_Nymrite;

    public GameObject UIObject_Velrite;
    public SpriteRenderer rend_Velrite;
    public Sprite UIObject_VelriteInactive;
    public Sprite UIObject_VelriteActive;
    public GameObject Num_Velrite;
    public Text text_num_Velrite;

    public GameObject UIObject_Zyrite;
    public SpriteRenderer rend_Zyrite;
    public Sprite UIObject_ZyriteInactive;
    public Sprite UIObject_ZyriteActive;
    public GameObject Num_Zyrite;
    public Text text_num_Zyrite;

    // Start is called before the first frame update
    void Start()
    {
        rend_Corite = UIObject_Corite.GetComponent<SpriteRenderer>();
        UIObject_CoriteInactive = rend_Corite.sprite;
        text_num_Corite = Num_Corite.GetComponent<Text>();

        rend_Nymrite = UIObject_Nymrite.GetComponent<SpriteRenderer>();
        UIObject_NymriteInactive = rend_Nymrite.sprite;
        text_num_Nymrite = Num_Nymrite.GetComponent<Text>();

        rend_Velrite = UIObject_Velrite.GetComponent<SpriteRenderer>();
        UIObject_VelriteInactive = rend_Velrite.sprite;
        text_num_Velrite = Num_Velrite.GetComponent<Text>();

        rend_Zyrite = UIObject_Zyrite.GetComponent<SpriteRenderer>();
        UIObject_ZyriteInactive = rend_Zyrite.sprite;
        text_num_Zyrite = Num_Zyrite.GetComponent<Text>();

        SyncCrystals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SyncCrystals(){
        text_num_Corite.text = PlayerManager.current.numOfCorite.ToString();
        text_num_Nymrite.text = PlayerManager.current.numOfNymrite.ToString();
        text_num_Velrite.text = PlayerManager.current.numOfVelrite.ToString();
        text_num_Zyrite.text = PlayerManager.current.numOfZyrite.ToString();
    }

    public void GainCorite(){
        PlayerManager.current.numOfCorite++;
        rend_Corite.sprite = UIObject_CoriteActive;
        text_num_Corite.text = PlayerManager.current.numOfCorite.ToString();
    }
    public void GainNymrite(){
        PlayerManager.current.numOfNymrite++;
        rend_Nymrite.sprite = UIObject_NymriteActive;
        text_num_Nymrite.text = PlayerManager.current.numOfNymrite.ToString();
    }
    public void GainVelrite(){
        PlayerManager.current.numOfVelrite++;
        rend_Velrite.sprite = UIObject_VelriteActive;
        text_num_Velrite.text = PlayerManager.current.numOfVelrite.ToString();
    }
    public void GainZyrite(){
        PlayerManager.current.numOfZyrite++;
        rend_Zyrite.sprite = UIObject_ZyriteActive;
        text_num_Zyrite.text = PlayerManager.current.numOfZyrite.ToString();
    }

    public void LoseCorite(){
        PlayerManager.current.numOfCorite--;
        if(PlayerManager.current.numOfCorite < 1){
            rend_Corite.sprite = UIObject_CoriteInactive;
        }
        text_num_Corite.text = PlayerManager.current.numOfCorite.ToString();
    }
    public void LoseNymrite(){
        PlayerManager.current.numOfNymrite--;
        if(PlayerManager.current.numOfNymrite < 1){
            rend_Nymrite.sprite = UIObject_NymriteInactive;
        }
        text_num_Nymrite.text = PlayerManager.current.numOfNymrite.ToString();
    }
    public void LoseVelrite(){
        PlayerManager.current.numOfVelrite--;
        if(PlayerManager.current.numOfVelrite < 1){
            rend_Velrite.sprite = UIObject_VelriteInactive;
        }
        text_num_Velrite.text = PlayerManager.current.numOfVelrite.ToString();
    }
    public void LoseZyrite(){
        PlayerManager.current.numOfZyrite--;
        if(PlayerManager.current.numOfZyrite < 1){
            rend_Zyrite.sprite = UIObject_ZyriteInactive;
        }
        text_num_Zyrite.text = PlayerManager.current.numOfZyrite.ToString();
    }

}

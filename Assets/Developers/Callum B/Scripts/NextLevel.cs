using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public GameObject guiObject;
    public string LevelToLoad;
    private DefaultInput input;

    // Start is called before the first frame update
    void Start()
    {
        guiObject.SetActive(false);
    }

    // Update is called once per frame
    void Awake()
    {
        input = new DefaultInput();
        input.Weapon.WeaponSlot1.started += e => Nextlevel();
        input.Enable();
    }

    void Nextlevel()
    {
        Application.LoadLevel(LevelToLoad);
    }

    void OnTriggerEnter(Collider other)
    {            
        if(other.gameObject.tag == "Player")
        {
            guiObject.SetActive(true);

            /*if (guiObject.activeInHierarchy == true && Input.GetButtonDown("Use"))
            {
                Debug.Log("loading level");
                Application.LoadLevel(LevelToLoad);
            }*/
        }
    }
    void OnTriggerExit()
    {
        guiObject.SetActive(false);
    }
}

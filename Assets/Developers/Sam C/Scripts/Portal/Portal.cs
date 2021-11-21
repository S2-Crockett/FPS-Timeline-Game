using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Portal : MonoBehaviour
{
    public Level_Manager levelManager;
    private Scene[] Scene;
    private Scene ActiveScene;

  
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (levelManager.active)
        {
            for (int i = 0; i < levelManager.levels.Length; i++)
            {
                if (levelManager.levelSelected == levelManager.levels[i].level)
                {
                    SceneManager.LoadScene(levelManager.levels[i].levelNumber);
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{

    public Levels[] levels;
    public int levelSelected = 0;
    public bool active = false;

    void Start()
    {
        
    }
    private void Update()
    {
        if(levelSelected != 0)
        {
            active = true;
        }
    }

}

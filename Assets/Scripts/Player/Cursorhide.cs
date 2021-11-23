using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Cursorhide : MonoBehaviour
{
        // Use this for initialization
        void Start()
        {
            //Set Cursor to not be visible
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}

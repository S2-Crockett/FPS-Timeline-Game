using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public bool doubleJump = false;
    public float power_up_duration = 10.0f;
    public float time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        doubleJump = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            doubleJump = true;
        }
      
    }


    private void Update()
    {
        if(doubleJump)
        {
            time += Time.deltaTime;
        }

        if (time >= power_up_duration)
        {
            doubleJump = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public DoubleJump doubleJump;

    public bool doubleJumpActiveText = false;

    public float power_up_duration = 10.0f;
    public float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        doubleJumpActiveText = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(doubleJump.doubleJump)
        {
            time += Time.deltaTime;
        }
        if (time >= power_up_duration)
        {
            doubleJump.doubleJump = false;
            doubleJumpActiveText = false;
        }
    }


}

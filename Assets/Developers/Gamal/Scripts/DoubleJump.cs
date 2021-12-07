using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public bool doubleJump = false;
    public bool doubleJumpText = false;
    public bool doubleJumpActiveText = false;
    private bool power_up_taken = false;
    public float power_up_duration = 10.0f;
    public float time = 0.0f;

    private DefaultInput defaultInput;

    private void Awake()
    {
        power_up_taken = false;
        doubleJump = false;
        doubleJumpText = false;
        doubleJumpActiveText = false;

        defaultInput = new DefaultInput();


        defaultInput.Player.DoubleJump.performed += e => activateDoubleJump();

        defaultInput.Enable();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            power_up_taken = true;
            doubleJumpText = true;
        }

    }

    void activateDoubleJump()
    {
        if (power_up_taken)
        {
            doubleJump = true;
            doubleJumpText = false;
            doubleJumpActiveText = true;
        }
    }


    private void Update()
    {
        if (doubleJump)
        {
            time += Time.deltaTime;
        }

        if (time >= power_up_duration)
        {
            doubleJump = false;
            doubleJumpActiveText = false;

        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public bool speedBooster = false;
    public bool speedBoosterText = false;
    public bool speedBoosterActiveText = false;
    public float power_up_duration = 10.0f;
    public float time = 0.0f;
    private bool power_up_taken = false;

    private DefaultInput defaultInput;

    // Start is called before the first frame update
    void Awake()
    {
        speedBooster = false;
        power_up_taken = false;
        speedBoosterText = false;
        speedBoosterActiveText = false;

        defaultInput = new DefaultInput();


        defaultInput.Player.SpeedBooster.performed += e => activateSpeedBoost();

        defaultInput.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            power_up_taken = true;
            speedBoosterText = true;
           
        }
    }

    void activateSpeedBoost()
    {
        if (power_up_taken)
        {
            speedBooster = true;
            speedBoosterText = false;
            speedBoosterActiveText = true;
           
        }
    }

    private void Update()
    {
        if (speedBooster)
        {
            time += Time.deltaTime;
        }

        if (time >= power_up_duration)
        {
            speedBooster = false;
            speedBoosterActiveText = false;

        }
    }
}
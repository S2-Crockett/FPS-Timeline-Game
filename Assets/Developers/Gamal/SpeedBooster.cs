using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public bool speedBooster = false;
    public float power_up_duration = 10.0f;
    public float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        speedBooster = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            speedBooster = true;
        }
    }
}

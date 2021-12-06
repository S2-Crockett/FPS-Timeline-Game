using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public bool flameThrower= false;
    public float power_up_duration = 10.0f;
    public float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        flameThrower = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            flameThrower = true;
        }
    }

    // Update is called once per frame

    private void Update()
    {
        if (flameThrower)
        {
            time += Time.deltaTime;
        }

        if (time >= power_up_duration)
        {
            flameThrower = false;
        }
    }
}

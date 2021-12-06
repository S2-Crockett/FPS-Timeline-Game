using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSystem : MonoBehaviour
{
    public bool shieldSystem = false;
    public float power_up_duration = 10.0f;
    public float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        shieldSystem = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            shieldSystem = true;
        }
    }

    // Update is called once per frame

    private void Update()
    {
        if (shieldSystem)
        {
            time += Time.deltaTime;
        }

        if (time >= power_up_duration)
        {
            shieldSystem = false;
        }
    }
}

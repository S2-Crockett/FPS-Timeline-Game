using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaHeal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HealthComponent>().AddHealth(25);
        }
    }
}

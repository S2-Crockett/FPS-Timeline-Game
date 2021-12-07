using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPowerUp : MonoBehaviour
{
    public RocketManager rc;

    //public GameObject pickupEffect;
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Pickup();
        }
    }

    void Pickup()
    {
        Debug.Log("RocketObtained");

        rc.rocketsPicked = true;

        Destroy(gameObject);
    }
 
}

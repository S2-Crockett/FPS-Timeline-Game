using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapons : MonoBehaviour
{
    private WeaponHandler weaponshandler;

    // Start is called before the first frame update
    void Start()
    {
        weaponshandler = GetComponent<WeaponHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("switching weapons");
            weaponshandler.weaponRefs[2].SetActive(true);
            weaponshandler.currentActiveIndex = 2;
            weaponshandler.currentWeapon = weaponshandler.weaponRefs[2].GetComponent<WeaponController>();
        }
    }*/
}

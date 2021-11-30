using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxInteract : MonoBehaviour
{
    public GameObject guiObject;
    private WeaponController weaponcontroller;

    // Start is called before the first frame update
    void Start()
    {
        weaponcontroller = GetComponent<WeaponController>();

        guiObject.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            guiObject.SetActive(true);

            weaponcontroller.equippedAmmo += 100;

            /*if (guiObject.activeInHierarchy == true && Input.GetKeyDown("E"))
            {
                Debug.Log("Ammo Replenished");
            }*/
        }
    }
    void OnTriggerExit()
    {
        guiObject.SetActive(false);
    }
}

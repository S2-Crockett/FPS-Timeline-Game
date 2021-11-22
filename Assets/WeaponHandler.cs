using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    private DefaultInput defaultInput;
    private bool shouldShoot;
    
    [Header("Weapon")] 
    private WeaponController currentWeapon;
    private int currentActiveIndex;
    private GameObject[] weaponRefs = new GameObject[5];
    public WeaponSlot[] weaponSlots;

    [Header("References")] 
    private PlayerController player;
    public Transform weaponHolder;

    private void Awake()
    {
        defaultInput = new DefaultInput();
        
        defaultInput.Weapon.Shoot.started += e => Shoot();
        defaultInput.Weapon.Shoot.canceled += e => Shoot();

        defaultInput.Weapon.WeaponSlot1.started += e => SwapWeapon(0); 
        defaultInput.Weapon.WeaponSlot2.started += e => SwapWeapon(1);
        
        defaultInput.Enable();
    }

    public void Initialise(PlayerController controller)
    {
        player = controller;
        InitiateBaseWeapons();
    }

    private void InitiateBaseWeapons()
    {
        int x = 0;
        foreach (var slot in weaponSlots)
        {
            GameObject obj = Instantiate(slot.weaponObject,weaponHolder.position, weaponHolder.rotation);
            obj.transform.parent = weaponHolder;
            obj.GetComponent<WeaponController>().Initialise(player);
            obj.SetActive(false);

            if (slot.weaponIndex == 0)
            {
                obj.SetActive(true);
                currentActiveIndex = 0;
                currentWeapon =  obj.GetComponent<WeaponController>();
            }
            
            weaponRefs[x] = obj;
            x++;
        }
    }

    private void SwapWeapon(int newIndex)
    {
        if (weaponRefs[newIndex] != null)
        {
             weaponRefs[currentActiveIndex].SetActive(false);
             weaponRefs[newIndex].SetActive(true);
             
             currentWeapon =  weaponRefs[newIndex].GetComponent<WeaponController>();
             currentActiveIndex = newIndex;
        }
    }

    private void Update()
    {
        if (shouldShoot)
        {
            if (currentWeapon)
            {
                currentWeapon.Shoot(Camera.main);
            }
        }
    }

    private void Shoot()
    {
        //fire the current weapon equipped? might need to check so you can do it whilst doing certain things?
        //apply offset of the camera based on the equipped weapons recoil amounts
        shouldShoot = !shouldShoot;
    }
}

[Serializable]
public class WeaponSlot
{
    public int weaponIndex;
    public GameObject weaponObject;
}

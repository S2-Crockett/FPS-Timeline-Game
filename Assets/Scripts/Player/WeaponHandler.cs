using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
// ReSharper disable All

public class WeaponHandler : MonoBehaviour
{
    private DefaultInput defaultInput;
    private bool shouldShoot;
    private bool isAiming;
    
    [Header("Weapon")]
    [HideInInspector]
    public WeaponController currentWeapon;
    private int currentActiveIndex;
    [HideInInspector]
    public GameObject[] weaponRefs;
    public WeaponSlot[] weaponSlots;

    [Header("References")] 
    private PlayerController player;
    private GameObject cameraHolder;
    public Transform weaponHolder;

    public ZoneChecker zoneCheck;

    public bool change = true;
    public int WeaponIndex = 0;

    private void Awake()
    {
        if (weaponRefs != null)
        {
            weaponRefs = new GameObject[5];
        }
        defaultInput = new DefaultInput();
        
        defaultInput.Weapon.Shoot.started += e => Shoot();
        defaultInput.Weapon.Shoot.canceled += e => Shoot();

        defaultInput.Weapon.WeaponSlot1.started += e => SwapWeapon(WeaponIndex); 
        defaultInput.Weapon.WeaponSlot2.started += e => SwapWeapon(WeaponIndex + 1);
        
        defaultInput.Weapon.Aim.started += e => AimingPressed();
        defaultInput.Weapon.Aim.canceled += e => AimingReleased();

        defaultInput.Weapon.Reload.performed += e => Reload();

        defaultInput.Enable();


        if (zoneCheck != null)
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (i <= 1)
                {
                    weaponSlots[i].weaponObject = zoneCheck.zone[0].weapons[i];
                }
                if (i >= 2)
                {
                    weaponSlots[i].weaponObject = zoneCheck.zone[1].weapons[i - 2];
                }
            }
        }

    }

    public void Initialise(PlayerController controller)
    {
        player = controller;
        InitiateBaseWeapons();
        StartCoroutine(InitiateBaseWeapons());
    }

    private IEnumerator InitiateBaseWeapons()
    {
        yield return new WaitForSeconds(0.2f);
        int x = 0;
        foreach (var slot in weaponSlots)
        {
            GameObject obj = Instantiate(slot.weaponObject,weaponHolder.position, weaponHolder.rotation);
            obj.transform.parent = weaponHolder;
            obj.GetComponent<WeaponController>().Initialise(player);
            obj.SetActive(false);
            weaponRefs[x] = obj;
            x++;
        }
        StartCoroutine(SpawnPrimaryWeapon());
    }

    IEnumerator SpawnPrimaryWeapon()
    {
        yield return new WaitForSeconds(0.1f);
        weaponRefs[0].SetActive(true);
        currentActiveIndex = 0;
        currentWeapon = weaponRefs[0].GetComponent<WeaponController>();
    }

    public void SwapWeapon(int newIndex)
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
        if(change)
        {
            defaultInput.Weapon.WeaponSlot1.started += e => SwapWeapon(WeaponIndex);
            defaultInput.Weapon.WeaponSlot2.started += e => SwapWeapon(WeaponIndex + 1);
            change = false;
        }

        CalculateAiming();
        
        if (shouldShoot)
        {
            if (currentWeapon)
            {
                currentWeapon.Shoot(player.camera.transform);
            }
        }
    }

    private void Shoot()
    {
        //fire the current weapon equipped? might need to check so you can do it whilst doing certain things?
        //apply offset of the camera based on the equipped weapons recoil amounts
        shouldShoot = !shouldShoot;
    }

    private void Reload()
    {
        StartCoroutine(currentWeapon.Reload());
    }

    private void CalculateAiming()
    {
        if (!currentWeapon)
        {
            return;
        }
        
        currentWeapon.isAiming = this.isAiming;
    }

    private void AimingPressed()
    {
        isAiming = true;
    }

    private void AimingReleased()
    {
        isAiming = false;
    }
}

[Serializable]
public class WeaponSlot
{
    public int weaponIndex;
    public GameObject weaponObject;
}

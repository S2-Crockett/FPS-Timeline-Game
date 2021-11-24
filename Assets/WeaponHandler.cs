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
    public WeaponController currentWeapon;
    public int currentActiveIndex;
    public GameObject[] weaponRefs = new GameObject[5];
    public WeaponSlot[] weaponSlots;

    [Header("References")] 
    private PlayerController player;
    private GameObject cameraHolder;
    public Transform weaponHolder;

    private void Awake()
    {
        defaultInput = new DefaultInput();
        
        defaultInput.Weapon.Shoot.started += e => Shoot();
        defaultInput.Weapon.Shoot.canceled += e => Shoot();

        defaultInput.Weapon.WeaponSlot1.started += e => SwapWeapon(0); 
        defaultInput.Weapon.WeaponSlot2.started += e => SwapWeapon(1);
        
        defaultInput.Weapon.Aim.started += e => AimingPressed();
        defaultInput.Weapon.Aim.canceled += e => AimingReleased();
        
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
        CalculateAiming();
        
        if (shouldShoot)
        {
            if (currentWeapon)
            {
                currentWeapon.Shoot(player.camera);
            }
        }
    }

    private void Shoot()
    {
        //fire the current weapon equipped? might need to check so you can do it whilst doing certain things?
        //apply offset of the camera based on the equipped weapons recoil amounts
        shouldShoot = !shouldShoot;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zone")
        {
            weaponRefs[2].SetActive(false);
            weaponRefs[3].SetActive(false);
            weaponRefs[0].SetActive(true);
            currentActiveIndex = 0;
            currentWeapon = weaponRefs[0].GetComponent<WeaponController>();
        }
        if (other.gameObject.tag == "Zone1")
        {
            weaponRefs[0].SetActive(false);
            weaponRefs[3].SetActive(false);
            weaponRefs[2].SetActive(true);
            currentActiveIndex = 2;
            currentWeapon = weaponRefs[2].GetComponent<WeaponController>();
        }
        if (other.gameObject.tag == "Zone2")
        {
            weaponRefs[0].SetActive(false);
            weaponRefs[2].SetActive(false);
            weaponRefs[3].SetActive(true);
            currentActiveIndex = 3;
            currentWeapon = weaponRefs[3].GetComponent<WeaponController>();
        }
    }
}

[Serializable]
public class WeaponSlot
{
    public int weaponIndex;
    public GameObject weaponObject;
}
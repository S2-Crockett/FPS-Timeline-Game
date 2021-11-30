using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Ammo")] 
    public int AddAmmoAmountPrimary;
    public int AddAmmoAmountSecondary;
    public AudioClip pickupSound;

    [Header("Floating")] 
    public float rotationPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset;
    private Vector3 tempPos;

    private WeaponHandler _weaponHandler;
    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotationPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _weaponHandler = other.GetComponent<WeaponHandler>();
            _weaponHandler.currentWeapon.AddAmmo(AddAmmoAmountPrimary);
           // _weaponHandler.weaponRefs[1].GetComponent<WeaponController>().AddAmmo(AddAmmoAmountSecondary);
            // this will need to be changed with the weapon switching stuff from the time mechanic
      
            Destroy(gameObject);
        }
    }
}

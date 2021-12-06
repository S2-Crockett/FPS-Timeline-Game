using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflameGun : MonoBehaviour
{
    public ParticleSystem fire;
    private DefaultInput defaultInput;

    // Start is called before the first frame update

    private void Awake()
    {
        
        defaultInput = new DefaultInput();
        defaultInput.Weapon.Shoot.performed += e => fireFlameThrower();
        defaultInput.Weapon.Shoot.canceled += e => StopFlameThrower();
        
        defaultInput.Enable();
    }
    private void Update()
    {
        
       
    }
    // Update is called once per frame
    void fireFlameThrower()
    {
        fire.Play();
    }
    void StopFlameThrower()
    {
        fire.Stop();
    }
}

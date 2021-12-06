using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerGun : MonoBehaviour
{
    public Flamethrower active;
    public GameObject flameGun;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(active.flameThrower)
        {
            flameGun.SetActive(true);
        }
        if(!active.flameThrower)
        {
            flameGun.SetActive(false);
        }
    }
}

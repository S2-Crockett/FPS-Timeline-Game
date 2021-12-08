using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketprojectile : MonoBehaviour
{
    public RocketManager rockets;

    
    public GameObject projectile;
    public float launchVelocity = 700f;

    public int rocketsShot = 4;

    private DefaultInput defaultInput;


    // Start is called before the first frame update
    private void Awake()
    {
        defaultInput = new DefaultInput();

        
        defaultInput.Player.Rocket.performed += e => shootRocket();
        

        defaultInput.Enable();  
    }

    void shootRocket()
    {
        if (rockets.rocketsPicked)
        {
            rocketsShot += 1;

            if (rocketsShot <= 3)
            {
                RocketProjectile();
            }
        }
    }

    private void Update()
    {
        if(rocketsShot >= 3)
        {
            rockets.rocketsPicked = false;
        }
    }
    void RocketProjectile()
    {
            GameObject ball = Instantiate(projectile, transform.position,
                                                         transform.rotation);

            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3
                                                  (0, launchVelocity, 0));
    }
}

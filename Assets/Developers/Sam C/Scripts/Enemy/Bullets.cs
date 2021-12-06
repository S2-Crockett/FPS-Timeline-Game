using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed;
    public float offset;
    public float fireRate;
    public float range;
    public int damage;
    public int clipSize;
    public float reloadSpeed;
    public bool hit = false;
    public bool hitWall = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hit = true;
        }
        else
        {
            if (other.gameObject.tag != "Weapon")
            {
                hitWall = true;
            }
        }
    }
}

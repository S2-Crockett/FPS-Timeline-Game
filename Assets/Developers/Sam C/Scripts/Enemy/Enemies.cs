using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemies : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemy;

    [Header("Weapon")]
    public GameObject bullet;
    public Bullets bulletInfo;

    [Header("Information")]
    public bool dead;


    private void Start()
    {
        bulletInfo = bullet.GetComponent<Bullets>();
    }
}

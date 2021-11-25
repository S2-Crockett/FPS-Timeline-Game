using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zones", menuName = "Levels/Zone", order = 1)]
public class Zone : ScriptableObject
{
    [Header("Environment Details")]
    public Material timezone1;
    public GameObject[] floor1;

    [Header("Player")]
    public GameObject Player;

    [Header("Enemy")]
    public Material material;

    [Header("Weapons")]
    public GameObject weapon;
}

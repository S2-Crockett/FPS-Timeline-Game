using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Zones", menuName = "Levels/Zone", order = 1)]
public class Zone : ScriptableObject
{
    [Header("Environment Details")]
    public GameObject timezone1;

    [Header("Player")]
    public GameObject Player;

    [Header("Enemy")]
    public GameObject material;

    [Header("Weapons")]
    public GameObject[] weapons;
    public int weaponIndex;


}

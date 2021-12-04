using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{

    enum Stages
    {
        SEARCHING,
        IN_RANGE,
        FOUND
    }

    private ZoneChecker LevelManager;
    private GameObject player;

    [Header("Weapon")]
    private WeaponController currentWeapon;
    private int currentActiveIndex;
    private GameObject[] weaponRefs = new GameObject[5];
    public WeaponSlot_[] weaponSlots;
    public Transform weaponHolder;


    private Stages stages;
    private Vector3 targetDir;
    private float rotation;
    private float distance;


    public int speed = 2;

    void Start()
    {
        LevelManager = GameObject.Find("LevelManager").GetComponent<ZoneChecker>();
        player = GameObject.Find("Player");
        stages = Stages.SEARCHING;
    }

    private void Awake()
    {
        Initialise();
    
        for(int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].weaponObject.gameObject.transform.localScale = new Vector3(1,1,1);
        }

    }

    void Update()
    {
        switch (stages)
        {
            case Stages.SEARCHING:
                {
                    if (Searching())
                    {
                        stages = Stages.IN_RANGE;
                    }
                    break;
                }
            case Stages.IN_RANGE:
                {
                    inRange();
                    break;
                }
            case Stages.FOUND:
                {
                    Vector3 target = player.transform.position - weaponHolder.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(target, Vector3.forward);
                    weaponHolder.transform.rotation = rotation;
                    if (currentWeapon)
                    {
                        currentWeapon.Shoot(weaponHolder.transform);
                    }
                    break;
                }
        }
    }

    private bool Searching()
    {
        targetDir = player.transform.position - transform.position;

        rotation = Vector3.Angle(targetDir, transform.forward);
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (rotation < 45 && distance < 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void inRange()
    {
        Vector3 target = player.transform.position - weaponHolder.transform.position;

        Quaternion rotation = Quaternion.LookRotation(target, Vector3.forward);
        weaponHolder.transform.rotation = rotation;

        RaycastHit hit;
        if (Physics.Raycast(weaponHolder.transform.position, weaponHolder.transform.forward, out hit, Mathf.Infinity, 1))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                //print("hit");
                stages = Stages.FOUND;
            }
        }
        if (!Searching())
        {
            stages = Stages.SEARCHING;
        }
    }





    public void Initialise()
    {
        InitiateBaseWeapons();
    }



    private void InitiateBaseWeapons()
    {
        int x = 0;
        foreach (var slot in weaponSlots)
        {
            GameObject obj = Instantiate(slot.weaponObject, weaponHolder.position, weaponHolder.rotation);
            obj.transform.parent = weaponHolder;
            obj.GetComponent<WeaponController>().InitialiseEnemy();
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

}
[Serializable]
public class WeaponSlot_
{
    public int weaponIndex;
    public GameObject weaponObject;
}

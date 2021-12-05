using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZoneChecker : MonoBehaviour
{

    [Header("Zones")]
    public Zone[] zone;

    private GameObject player;

    [Header("Enemies")]
    public GameObject[] enemies;

    [Header("Environment")]
    public GameObject[] environment;

    [Header("Floor")]
    public GameObject floor;

    [Header("Materials")]
    public Material Glitch;


    private WeaponHandler weaponHandler;


    public int index = 0;
    private int prevIndex = 0;
    private float random = 0;

    private bool change = false;

    GameObject[] newEnvironmentObject;
    public EnemyInfo[] enemyObjectZone1;
    public EnemyInfo[] enemyObjectZone2;
    GameObject newFloorObject;

    bool[] changable;

    Vector3[] newEnemyPos;
    Quaternion[] newEnemyRot;



    RaycastHit hit;

    private int layerMask = 1;

    void Start()
    {
        newEnvironmentObject = new GameObject[environment.Length];
        newEnemyPos = new Vector3[enemies.Length];
        newEnemyRot = new Quaternion[enemies.Length];

        changable = new bool[enemies.Length];

        prevIndex = index;

        player = GameObject.Find("Player");
        weaponHandler = player.GetComponent<WeaponHandler>();

        newFloorObject = Instantiate(zone[index].timezone1, floor.transform.position, floor.transform.rotation);

        for(int i = 0; i < zone.Length; i++)
        {
            zone[i].timezone1.tag = zone[0].name;
        }
        for (int i = 0; i < environment.Length; i++)
        {
            newEnvironmentObject[i] = Instantiate(zone[index].environment, environment[i].transform.position, environment[i].transform.rotation);
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyObjectZone1[i].enemyObj = zone[0].enemy;
            enemyObjectZone2[i].enemyObj = zone[1].enemy;
            enemyObjectZone1[i].enemyObj = Instantiate(zone[0].enemy, enemies[i].transform.position, enemies[i].transform.rotation);
            enemyObjectZone2[i].enemyObj = Instantiate(zone[1].enemy, enemies[i].transform.position, enemies[i].transform.rotation);
            enemyObjectZone2[i].enemyObj.SetActive(false);
            newEnemyPos[i] = enemyObjectZone1[i].enemyObj.transform.position;
            newEnemyRot[i] = enemyObjectZone1[i].enemyObj.transform.rotation;
        }

        for (int i = 0; i < changable.Length; i++)
        {
            changable[i] = false;
        }

        weaponHandler.WeaponIndex = zone[index].weaponIndex;
        weaponHandler.change = true;
        weaponHandler.SwapWeapon(zone[index].weaponIndex);
    }


    // Update is called once per frame
    void Update()
    {
        if(prevIndex != index)
        {
            prevIndex = index;
            change = true;
        }
        if(Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            ChangeObjects();
        }
    }

    private void ChangeObjects()
    {
        CheckZone();
        CreateNewObjects();
        CheckEnemy();
    }

    IEnumerator ChangeEnvironmentObj(int index_)
    {
        ChangeMat(environment, newEnvironmentObject, index_);

        yield return new WaitForSeconds(random);

         Destroy(newEnvironmentObject[index_]);
         newEnvironmentObject[index_] = Instantiate(zone[index].environment, environment[index_].transform.position, environment[index_].transform.rotation);
    }

    IEnumerator ChangeEnemyObj(int index_)
    {
        if (!enemyObjectZone1[index_].enemyObj.activeSelf)
        {
            newEnemyPos[index_] = enemyObjectZone2[index_].enemyObj.transform.position;
            if (!enemyObjectZone2[index_].dead)
            {
                newEnemyRot[index_] = enemyObjectZone2[index_].enemyObj.transform.rotation;
            }
            enemyObjectZone1[index_].enemyObj.SetActive(true);
            enemyObjectZone2[index_].enemyObj.SetActive(false);
            enemyObjectZone1[index_].enemyObj.transform.position = newEnemyPos[index_];
            enemyObjectZone1[index_].enemyObj.transform.rotation = newEnemyRot[index_];
        }
        else if (!enemyObjectZone2[index_].enemyObj.activeSelf)
        {
            newEnemyPos[index_] = enemyObjectZone1[index_].enemyObj.transform.position;
            if (!enemyObjectZone1[index_].dead)
            {
                newEnemyRot[index_] = enemyObjectZone1[index_].enemyObj.transform.rotation;
            }
            enemyObjectZone2[index_].enemyObj.SetActive(true);
            enemyObjectZone1[index_].enemyObj.SetActive(false);
            enemyObjectZone2[index_].enemyObj.transform.position = newEnemyPos[index_];
            enemyObjectZone2[index_].enemyObj.transform.rotation = newEnemyRot[index_];
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator ChangeFloor()
    {
        random = Random.Range(0.3f, 1f);
        newFloorObject.GetComponent<MeshRenderer>().material = Glitch;

        yield return new WaitForSeconds(random);

        Destroy(newFloorObject);
        newFloorObject = Instantiate(zone[index].timezone1, floor.transform.position, floor.transform.rotation);
    }
    private void ChangeMat(GameObject[] enemy_, GameObject[] newObj, int index)
    {
        for (int c = 0; c < newObj[index].transform.childCount; c++)
        {
            newObj[index].transform.GetChild(c).GetComponent<MeshRenderer>().material = Glitch;
            random = Random.Range(0.3f, 1f);
        }
    }
    private void CreateNewObjects()
    {
        if (change)
        {
            for (int i = 0; i < environment.Length; i++)
            {
                StartCoroutine(ChangeEnvironmentObj(i));
            }
            for(int i = 0; i < enemies.Length; i++)
            {
                StartCoroutine(ChangeEnemyObj(i));
            }
            StartCoroutine(ChangeFloor());

            weaponHandler.WeaponIndex = zone[index].weaponIndex;
            weaponHandler.change = true;
            weaponHandler.SwapWeapon(zone[index].weaponIndex);

            change = false;
        }
    }
    private void CheckZone()
    {
        for (int i = 0; i < zone.Length; i++)
        {
            if(hit.transform.gameObject.tag == zone[i].name)
            {
                index = i;
            }
        }
    }

    private void CheckEnemy()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemyObjectZone1[i].enemyObj.GetComponent<ShootingTarget>().dead)
            {
                enemyObjectZone1[i].enemyObj.GetComponent<LineOfSight>().enabled = false;
                enemyObjectZone1[i].enemyObj.GetComponent<Movement>().enabled = false;
                Quaternion dead = new Quaternion();
                dead.Set( 0, 90, 90, 0);
                enemyObjectZone1[i].enemyObj.transform.rotation = dead;
                enemyObjectZone1[i].dead = true;
            }
            if (enemyObjectZone2[i].enemyObj.GetComponent<ShootingTarget>().dead)
            {
                enemyObjectZone2[i].dead = true;
            }
        }
    }
}

[Serializable]
public class EnemyInfo
{
    public bool dead;
    public GameObject enemyObj;
}

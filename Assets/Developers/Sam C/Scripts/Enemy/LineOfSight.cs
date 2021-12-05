using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public GameObject weaponObject;
    public Transform weaponHolder;


    private Enemies enemy;


    private Stages stages;
    private Vector3 targetDir;
    private float rotation;
    private float distance;

    public float timer = 1.0f;
    public float offset = 5.0f;

    void Start()
    {
        LevelManager = GameObject.Find("LevelManager").GetComponent<ZoneChecker>();
        player = GameObject.Find("Player");
        stages = Stages.SEARCHING;
        enemy = GetComponent<Enemies>();
    }

    private void Awake()
    {
        GameObject obj = Instantiate(weaponObject, weaponHolder.position, weaponHolder.rotation);
        obj.transform.parent = weaponHolder;
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
                    Found();
                    break;
                }
        }
    }
    private bool Searching()
    {
        targetDir = player.transform.position - transform.position;

        rotation = Vector3.Angle(targetDir, transform.forward);
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (rotation < 45 && distance < 15)
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
                stages = Stages.FOUND;
            }
        }
        if (!Searching())
        {
            stages = Stages.SEARCHING;
        }
    }
    private void Shoot()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Quaternion rotation = Quaternion.Euler(Random.Range(-offset, offset), Random.Range(-offset, offset), 0);
            GameObject bullet = Instantiate(enemy.bullet, weaponHolder.position, weaponHolder.rotation);
            float speed = enemy.bulletInfo.speed;
            bullet.GetComponent<Rigidbody>().AddForce(rotation * weaponHolder.transform.forward * speed);
            timer = 0.5f;
        }
    }
    private void Found()
    {
        Vector3 target = player.transform.position - weaponHolder.transform.position;
        Quaternion rotation = Quaternion.LookRotation(target, Vector3.forward);
        weaponHolder.transform.rotation = rotation;

        RaycastHit hit;
        if (Physics.Raycast(weaponHolder.transform.position, weaponHolder.transform.forward, out hit, Mathf.Infinity, 1))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                stages = Stages.FOUND;
            }
            else
            {
                stages = Stages.SEARCHING;
            }
        }
        Shoot();
    }


}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LineOfSight : MonoBehaviour
{

    public enum Stages
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


    public Stages stages;
    private Vector3 targetDir;
    private Vector3 pos;
    private float rotation;
    private float distance;

    public float timer = 1.0f;
    public float offset = 5.0f;


    public List<GameObject> bullets;

    private Bullets bulletInfo;

    int clipSize;
    float reloadSpeed;

    void Start()
    {
        LevelManager = GameObject.Find("LevelManager").GetComponent<ZoneChecker>();
        player = GameObject.Find("Player");
        stages = Stages.SEARCHING;
        enemy = GetComponent<Enemies>();
        bulletInfo = enemy.bullet.GetComponent<Bullets>();
        clipSize = bulletInfo.clipSize;
        reloadSpeed = bulletInfo.reloadSpeed;
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
                    weaponHolder.transform.rotation = transform.rotation;
                    GetComponent<Move>().enabled = true;
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
                    GetComponent<Move>().enabled = false;
                    transform.position = pos;
                    Vector3 rot = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles;
                    rot.x = rot.z = 0;
                    transform.rotation = Quaternion.Euler(rot);
                    Found();

                    break;
                }
        }

        CheckBullets(bullets);
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
        else if (distance < 5)
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
                pos = transform.position;
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
        if (clipSize > 0)
        {
            if (timer <= 0)
            {
                Quaternion rotation = Quaternion.Euler(Random.Range(-bulletInfo.offset, bulletInfo.offset), Random.Range(-bulletInfo.offset, bulletInfo.offset), 0);
                GameObject bullet = Instantiate(enemy.bullet, weaponHolder.position, weaponHolder.rotation);
                bullets.Add(bullet);
                clipSize -= 1;
                float speed = bulletInfo.speed;
                bullet.GetComponent<Rigidbody>().AddForce(rotation * weaponHolder.transform.forward * speed);
                timer = bulletInfo.fireRate;
            }
            reloadSpeed = bulletInfo.reloadSpeed;
        }
        else
        {
            reloadSpeed -= Time.deltaTime;
            if(reloadSpeed <= 0)
            {
                timer = bulletInfo.fireRate;
                clipSize = bulletInfo.clipSize;
            }
        }
    }

    private void CheckBullets(List<GameObject> bullets)
    {
        float distance = bulletInfo.range;
        if (bullets.Count > 0)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                float tempDistance = Vector3.Distance(transform.position, bullets[i].gameObject.transform.position);
                if (tempDistance > distance)
                {
                    Destroy(bullets[i].gameObject);
                    bullets.Remove(bullets[i]);
                }
                if (bullets[i].GetComponent<Bullets>().hit)
                {
                    Destroy(bullets[i].gameObject);
                    bullets.Remove(bullets[i]);
                    player.GetComponent<HealthComponent>().Damage(bulletInfo.damage);
                }
                if (bullets[i].GetComponent<Bullets>().hitWall)
                {
                    Destroy(bullets[i].gameObject);
                    bullets.Remove(bullets[i]);
                }
            }
        }
    }
    private void Found()
    {
        Vector3 target = player.transform.position - weaponHolder.transform.position;
        Quaternion rotation = Quaternion.LookRotation(target, Vector3.forward);
        weaponHolder.transform.rotation = rotation;
        transform.position = pos;


        inRange();
        Shoot();
    }


}


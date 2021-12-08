using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LineOfSight : MonoBehaviour
{
    public class Bullets
    {
        public float time = 0.0f;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    public enum Stages
    {
        SEARCHING,
        IN_RANGE,
        FOUND
    }

    public GameObject player;

    [Header("Weapon")]
    public GameObject weaponObject;
    public Transform weaponHolder;
    public float speed = 1000.0f;
    public float offset = 0.01f;
    public float bulletDrop = 0.0f;
    
    [Header("Weapon Effects")]
    public TrailRenderer tracer;
    public GameObject HitParticle;
 
    public float fireRate;
    public float range;
    public int damage;
    public int clipSize;
    public float reloadSpeed;
    public bool hit = false;
    public bool hitWall = false;
    
    private Enemies enemy;
    private List<Bullets> spawnedBullets = new List<Bullets>();
    
    public Stages stages;
    private Vector3 targetDir;
    private Vector3 pos;
    private float rotation;
    private float distance;

    public float timer = 1.0f;

    public AudioClip hitSound;
    private AudioSource sound;

    public List<GameObject> bullets;

    private Bullets bulletInfo;
    private bool isNotified = false;
    void Start()
    {
        stages = Stages.SEARCHING;
        enemy = GetComponent<Enemies>();
    }

    private void Awake()
    {
        //spawn at the idle location rotation
        GameObject obj = Instantiate(weaponObject, weaponHolder.position, weaponHolder.rotation);
        obj.transform.parent = weaponHolder;
        enemy = GetComponent<Enemies>();
    }
    
    Vector3 GetBulletPosition(Bullets bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) +
               (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullets CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullets bullet = new Bullets();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracer, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        Destroy(bullet.tracer.gameObject, bullet.tracer.time);
        return bullet;
    }

    void Update()
    {
        switch (stages)
        {
            case Stages.SEARCHING:
                {
                    if (isNotified)
                    {
                        stages = Stages.FOUND;
                    }
                    
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
                    if (isNotified)
                    {
                        stages = Stages.FOUND;
                    }
                    
                    inRange();
                    break;
                }
            case Stages.FOUND:
                {
                    // stage needs to be set if we shoot and enemy..
                    // sphere trace in a radius around itself it trigger other enemies into being found.

                    //weaponHolder.transform.localRotation = weaponHolder.transform.rotation;
                    //GetComponent<Move>().enabled = false;
                    //transform.position = pos;
                    
                    Vector3 rot = Quaternion.LookRotation(player.transform.position - transform.position).eulerAngles;
                    rot.x = rot.z = 0;
                    transform.rotation = Quaternion.Euler(rot);
                    Found();
                    break;
                }
        }

        // do the bullet check here
        UpdateBullets(Time.deltaTime);
    }
    private bool Searching()
    {
        targetDir = player.transform.position - transform.position;

        rotation = Vector3.Angle(targetDir, transform.forward);
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (rotation < 45 && distance < 50)
        {
            return true;
        }
        else if (distance < 25)
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
        if (Physics.Raycast(weaponHolder.transform.position, weaponHolder.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                pos = transform.position;
                stages = Stages.FOUND;
            }
        }
        else
        {
            stages = Stages.FOUND;
        }
    }
    private void Shoot()
    {
        timer -= Time.deltaTime;
        if (clipSize > 0)
        {
            if (timer <= 0)
            {
                RaycastHit hit;
                Physics.Raycast(weaponHolder.position, player.transform.position - weaponHolder.transform.position, out hit, Mathf.Infinity);

                Vector3 direction = GetShotDirection();
                Vector3 hitpoint = weaponHolder.transform.TransformPoint(direction * 100.0f);
                Vector3 velocity = (hitpoint - weaponHolder.transform.position).normalized * speed;
                            var bullets = CreateBullet(weaponHolder.transform.position, velocity);
                spawnedBullets.Add(bullets);
                
                clipSize -= 1;
                timer = fireRate;
            }

            reloadSpeed = 1.5f;
        }
        else
        {
            reloadSpeed -= Time.deltaTime;
            if(reloadSpeed <= 0)
            {
                timer = fireRate;
                clipSize = 10;
            }
        }
    }

    private void CheckBullets(List<GameObject> bullets)
    {
        float distance = range;
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
                
                /*
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
                */
            }
        }
    }
    public void Found()
    {
        Vector3 target = player.transform.position - weaponHolder.transform.position;
        Quaternion rotation = Quaternion.LookRotation(target, Vector3.forward);
        //weaponHolder.transform.rotation
        transform.rotation = new Quaternion(transform.rotation.x, rotation.y, transform.rotation.z, 0);
        weaponHolder.transform.rotation = new Quaternion(weaponHolder.transform.rotation.x, rotation.y, weaponHolder.transform.rotation.z, 0);
        //transform.position = pos;
        
        // spawn a sphere here to alert other enemies... in a vinicity

        
        inRange();
        Shoot();
    }

    public void SendFoundAlert()
    {
        isNotified = true;

        Collider[] objectsAroundPlayer = Physics.OverlapSphere(transform.position, 10.0f);
        foreach (Collider obj in objectsAroundPlayer)
        {
            if (obj.CompareTag("Enemy")) //checks list for zombies
            {
                Debug.Log("found?");
                LineOfSight los = obj.gameObject.GetComponent<LineOfSight>();
                los.ReceiveFoundDirection();
            }
        }
    }
    public void ReceiveFoundDirection()
    {
        isNotified = true;
        StartCoroutine(DelayFound());
    }

    IEnumerator DelayFound()
    {
        yield return new WaitForSeconds(1.0f);
        stages = Stages.FOUND;
    }
    
     private Vector3 GetShotDirection()
    {
        Vector3 direction = Vector3.forward;
        direction += new Vector3(
                Random.Range(-offset, offset),
                Random.Range(-offset, offset),
                Random.Range(-offset, offset));
            direction.Normalize();
            return direction;
    }

    public void UpdateBullets(float delta)
    {
        SimulateBullets(delta);
        DestroyBullets();
    }

    private void SimulateBullets(float deltaTime)
    {
        spawnedBullets.ForEach(bullets =>
        {
            Vector3 p0 = GetBulletPosition(bullets);
            bullets.time += deltaTime;
            Vector3 p1 = GetBulletPosition(bullets);
            RaycastSegment(p0, p1, bullets);
        });
    }

    private void DestroyBullets()
    {
        spawnedBullets.RemoveAll(bullet => bullet.time >= 0.5f);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullets bullet)
    {
        float distance = (end - start).magnitude;
        RaycastHit hit;
        if (Physics.Raycast(start, end - start, out hit, distance))
        {
            var target = hit.transform.gameObject.GetComponent<HealthComponent>();
            
            if (bullet.tracer)
            {
               bullet.tracer.transform.position = hit.point; 
            }
            
            if (target != null && hit.transform.tag == "Player")
            {
                hit.transform.gameObject.GetComponent<HealthComponent>().Damage(damage);
            }
            
            bullet.time = 0.5f;
            
            var ImpactObject = Instantiate(HitParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactObject, 0.4f);
        }
        else
        {
            if (bullet.tracer)
            {
               bullet.tracer.transform.position = end; 
            }
        }
    }


}


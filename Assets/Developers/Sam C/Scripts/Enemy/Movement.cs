using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{

    Vector3 startingPosition;
    Vector3 roamPosition;

    public Transform target;
    NavMeshAgent agent;
    
    public float lookRadius = 10f;
    public float moveSpeed = 5f;
    public float minRoamDistance = 1f;
    public float maxRoamDistance = 20f;

    [Header ("Movement Timing")]
    private float lastTimeChecked = 0;
    Vector3 lastCheckPos;
    public float seconds = 3.0f;
    public float yMuch = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        roamPosition = GetRoamingPosition();
    }


    private void Awake()
    {
        Random.InitState(DateTime.Now.Millisecond);
        startingPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        

        if((Time.time - lastTimeChecked) > seconds)
        {
            if ((transform.position.x - lastCheckPos.x) <= yMuch)
            {
                if((transform.position.z  - lastCheckPos.z)<= yMuch)
                {
                    roamPosition = GetRoamingPosition();
                    Roam();
                }
            }
            lastCheckPos = transform.position;
            lastTimeChecked = Time.time;
        }

        if(distance <= lookRadius)
        {
            agent.SetDestination(transform.position);

            if(distance <= agent.stoppingDistance)
            {
                FaceTarget();
                //GetComponent<Rigidbody>().AddForce(transform.forward * moveSpeed);
            }
            
        }
        else
        {
            Roam();
        }
        
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 hit = startingPosition + GetRandomDir() * Random.Range(minRoamDistance, maxRoamDistance);
        return hit;
    }
    private void Roam()
    {
        agent.SetDestination(roamPosition);

        if (Vector3.Distance(roamPosition, transform.position) <= 5f)
        {
            roamPosition = GetRoamingPosition();
        }
    }
    private Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1f)).normalized;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
    }

}

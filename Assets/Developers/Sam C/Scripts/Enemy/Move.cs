using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    public float range = 10.0f;
    public bool moving = false;
    public float speed = 50;

    NavMeshAgent agent;
    Vector3 result;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void RandomPoint(Vector3 center, float range)
    {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    moving = true;
                }
    }

    void Update()
    {
        if (!moving)
        {
            RandomPoint(transform.position, range);
            Debug.DrawRay(result, Vector3.up, Color.blue, 1.0f);
        }
        else
        {
            agent.destination = result;
        }

        if (Vector3.Distance(transform.position, result) < 1f)
        {
            moving = false;
        }
    }
}

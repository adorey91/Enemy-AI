using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform[] goal;
    public int goalNumber = 0;
    NavMeshAgent agent;
    float distanceThreshold = 0.1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = goal[goalNumber].position;
        
        if (Vector3.Distance(agent.transform.position, goal[goalNumber].position) <= distanceThreshold)
        {
            goalNumber++;
            if (goalNumber == goal.Length)
            {
                goalNumber = 0;
            }
        }
    }
}
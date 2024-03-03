using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    private Transform moveTarget;

    private NavMeshAgent agent;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void LookTowards(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void MoveToTarget(Transform target)
    {
        moveTarget = target;
        MoveToPosition(target.position);
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
        LookTowards(position);
    }

    public void StopMovement()
    {
        agent.isStopped=true;
        moveTarget = null;
    }
}

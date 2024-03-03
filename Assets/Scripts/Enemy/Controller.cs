using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour
{
    private float moveToUpdateRate = 1.0f;
    private float lastMoveToUpdate;
    private Transform moveTarget;

    private NavMeshAgent agent;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //public void Update()
    //{
    //    if(moveTarget != null && Time.time - lastMoveToUpdate > moveToUpdateRate)
    //    {
    //        lastMoveToUpdate = Time.time;
    //        MoveToPosition(moveTarget.position);
    //    }
    //}

    public void LookTowards(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public void MoveToTarget(Transform target)
    {
        moveTarget = target;
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

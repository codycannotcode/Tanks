using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BasicMover : MonoBehaviour
{
    private Vector3 target;
    private bool moving = false;
    private Tank tank;
    private NavMeshAgent agent;
    
    void SetTarget(Vector3 destination)
    {
        target = destination;
        agent.SetDestination(target);
        moving = true;
    }

    bool RandomPoint(Vector3 center, out Vector3 result)
    {
        float range = 10.0f;

        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            randomPoint.y = 0;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    void Start()
    {

        tank = GetComponent<Tank>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3;
        agent.angularSpeed = 0;
        agent.updateRotation = false;
        target = transform.position;
        target.x = 9;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            Vector3 distanceFromTarget = transform.position - target;
            distanceFromTarget.y = 0;
            if (distanceFromTarget == Vector3.zero) {
                moving = false;
                Debug.Log("finished moving");
            }
            else {
                tank.SetVelocity(agent.velocity);
            }
        }
        
        if (Input.GetMouseButtonDown(1)) {
            Vector3 result;
            RandomPoint(transform.position, out result);
            Debug.Log(result);
            SetTarget(result);
        }
    }
}

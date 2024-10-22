using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//moves to random spots within a 10 unit range
public class BasicMover : MonoBehaviour
{
    private Vector3 target;
    private bool moving = false;
    private float[] moveDelay = {0f, 1f};
    private Tank tank;
    private NavMeshAgent agent;
    
    void SetTarget(Vector3 destination)
    {
        target = destination;
        agent.isStopped = false;
        agent.SetDestination(target);
        moving = true;
    }

    public static bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
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

    IEnumerator ChooseNewTarget() {
        Vector3 result;
        RandomPoint(transform.position, 10f, out result);
        SetTarget(result);

        yield return new WaitUntil(() => !moving);
        yield return new WaitForSeconds(Random.Range(moveDelay[0], moveDelay[1]));
        StartCoroutine(ChooseNewTarget());
    }

    void Start()
    {
        tank = GetComponent<Tank>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3;
        agent.angularSpeed = 0;
        agent.updateRotation = false;

        StartCoroutine(ChooseNewTarget());
    }

    void Update()
    {
        if (moving) {
            Vector3 distanceFromTarget = transform.position - target;
            distanceFromTarget.y = 0;
            if (distanceFromTarget.sqrMagnitude < 0.05f) {
                moving = false;
                agent.isStopped = true;
            }
            else {
                tank.SetVelocity(agent.velocity);
            }
        }
        else {
            tank.SetMovementDirection(Vector3.zero);
        }
    }
}

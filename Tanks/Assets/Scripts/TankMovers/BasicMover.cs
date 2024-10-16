using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BasicMover : MonoBehaviour
{
    private Tank tank;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {

        tank = GetComponent<Tank>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3;
        agent.angularSpeed = 0;
        agent.updateRotation = false;
        Vector3 target = transform.position;
        target.x = 9;
        
        float range = 10f;
        Vector3 randomPoint = tank.transform.position + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
            Debug.Log(hit.position);
        }

        agent.SetDestination(target);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        tank.SetVelocity(agent.velocity);
    }
}

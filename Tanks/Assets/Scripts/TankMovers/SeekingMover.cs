using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeekingMover : MonoBehaviour
{
    private bool canSeePlayer;
    private int layerMask;
    private Vector3 target;
    private bool moving = false;
    private GameObject player;
    private Tank tank;
    private NavMeshAgent agent;

    void SetTarget(Vector3 destination)
    {
        target = destination;
        agent.isStopped = false;
        agent.SetDestination(target);
        moving = true;
    }

    IEnumerator TrackPlayer() {
        yield return null;
    }

    void Start()
    {
        layerMask = LayerMask.GetMask("Tanks", "Walls");

        player = GameObject.Find("Player");
        tank = GetComponent<Tank>();
        
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3;
        agent.angularSpeed = 0;
        agent.updateRotation = false;


    }

    void Update()
    {
        RaycastHit hit;
        bool didHit = Physics.Raycast(transform.position, transform.forward, out hit, 20f, layerMask);
        canSeePlayer = didHit && hit.collider.gameObject == player;
    }
}

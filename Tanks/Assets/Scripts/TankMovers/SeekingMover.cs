using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//drives toward player if it can't see it, then starts juking for a short time, then tries to drive toward player again if it can't see
public class SeekingMover : MonoBehaviour
{
    private bool canSeePlayer;
    private int layerMask;
    private Vector3 target;
    private bool moving = false;
    private float[] moveDelay = {0f, 0.5f};
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
        while (!canSeePlayer) {
            SetTarget(player.transform.position);
            yield return null;
        }
        yield return new WaitForSeconds(Random.Range(moveDelay[0], moveDelay[1]));
        StartCoroutine(ChooseRandomTargets());
    }

    IEnumerator ChooseRandomTargets() {
        int minimumSpots = Random.Range(1, 5);
        //randomly move around for 1-5 cycles, and then begin to track player once we can't see them anymore
        for (int i = minimumSpots; i >= 0 || canSeePlayer; i--) {
            Vector3 result;
            BasicMover.RandomPoint(transform.position, 3f, out result);
            SetTarget(result);

            yield return new WaitUntil(() => !moving);
            Debug.Log("1");
            yield return new WaitForSeconds(Random.Range(moveDelay[0], moveDelay[1]));
            Debug.Log("2");
        }
        StartCoroutine(TrackPlayer());
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

        StartCoroutine(TrackPlayer());
    }

    void Update()
    {
        if (player != null) {
            RaycastHit hit;
            bool didHit = Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 20f, layerMask);
            canSeePlayer = didHit && hit.collider.gameObject == player;
        }

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

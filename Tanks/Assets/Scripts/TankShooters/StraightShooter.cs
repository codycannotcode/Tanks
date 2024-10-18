using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StraightShooter : MonoBehaviour
{
    private bool canSeePlayer = false;
    private int layerMask;

    private GameObject player;
    private Tank tank;
    private GameObject tankHead;

    void Start()
    {
        player = GameObject.Find("Player");
        tank = GetComponent<Tank>();
        layerMask = LayerMask.GetMask("Tanks", "Walls");
        tankHead = transform.Find("Head").gameObject;

        StartCoroutine(WaitToFire());
    }

    IEnumerator WaitToFire() {
        while (true) {
            yield return new WaitUntil(() => tank.reloaded && canSeePlayer);
            tank.QueueProjectile();
        }
    }

    void Update()
    {
        if (player == null) {
            return;
        }
        Vector3 aimDirection = player.transform.position - transform.position;
        tank.AimInDirection(aimDirection);

        if (tank.reloaded) {
            Vector3 origin = tankHead.transform.position + tankHead.transform.forward;
            Vector3 direction = tankHead.transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, 20f, layerMask)) {
                canSeePlayer = hit.collider.gameObject == player;
            }
        }
    }
}

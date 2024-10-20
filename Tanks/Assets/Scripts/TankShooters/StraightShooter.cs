using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StraightShooter : MonoBehaviour
{
    [SerializeField]
    private bool calculateRicochet;
    [SerializeField]
    private float angleVariation;
    private bool canSeePlayer = false;
    private int layerMask;
    private float bulletThickness = 0.125f;
    private int bounces;
    private float[] aimDelay = {0.25f, 0.75f};
    private float[] shootDelay = {0f, 1f};

    private GameObject player;
    private Tank tank;
    private GameObject tankHead;

    void Start()
    {
        player = GameObject.Find("Player");
        tank = GetComponent<Tank>();
        layerMask = LayerMask.GetMask("Tanks", "Walls");
        tankHead = transform.Find("Head").gameObject;
        bounces = tank.Projectile.GetComponent<Bullet>().Bounces;

        StartCoroutine(WaitToFire());
        StartCoroutine(ReAim());
    }

    IEnumerator WaitToFire() {
        while (true) {
            yield return new WaitUntil(() => tank.reloaded);
            yield return new WaitForSeconds(Random.Range(shootDelay[0], shootDelay[1]));
            yield return new WaitUntil(() => canSeePlayer);
            tank.FireProjectile();
        }
    }

    void Update()
    {
        if (tank.reloaded && player != null) {
            canSeePlayer = false;
            Vector3 origin = tankHead.transform.position + tankHead.transform.forward;
            Vector3 direction = tankHead.transform.forward;
            RaycastHit hit;
            for (int b = calculateRicochet ? 0 : bounces; b <= bounces; b++) {
                bool didHit = Physics.SphereCast(origin, bulletThickness, direction, out hit, 20, layerMask);
                //Debug.DrawLine(origin, hit.point, Color.red);
                if (didHit && hit.collider.gameObject == player) {
                    canSeePlayer = true;
                    break;
                }
                origin = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
            }
        }
    }

    IEnumerator ReAim() {
        while (player != null) {
            float angleVariation = Random.Range(-this.angleVariation, this.angleVariation);
            Vector3 aimDirection = Quaternion.Euler(0, angleVariation, 0) * (player.transform.position - transform.position);
            
            tank.AimInDirection(aimDirection);
            yield return new WaitForSeconds(Random.Range(aimDelay[0], aimDelay[1]));
        }
    }
}

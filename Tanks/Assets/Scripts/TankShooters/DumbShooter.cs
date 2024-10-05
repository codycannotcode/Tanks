using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbShooter : MonoBehaviour
{
    private Tank tank;
    private int aimDelay = 2;
    private float chanceOfShooting = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
        Invoke("ReAim", 1);
    }

    void ReAim() {
        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        tank.AimInDirection(new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)));
        if (Random.value < chanceOfShooting) {
            tank.QueueProjectile();
        }
        float waitTime = Random.Range(aimDelay, aimDelay + 3);
        Invoke("ReAim", waitTime);
    }
}

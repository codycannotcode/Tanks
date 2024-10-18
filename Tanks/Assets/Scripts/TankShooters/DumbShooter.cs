using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbShooter : MonoBehaviour
{
    private Tank tank;
    private float[] aimDelay = {2, 5};
    private float[] shootDelay = {0.5f, 1.5f};
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
            float shootWait = Random.Range(shootDelay[0], shootDelay[1]);
            Invoke("TryToFire", shootWait);
        }
        float waitTime = Random.Range(aimDelay[0], aimDelay[1]);
        Invoke("ReAim", waitTime);
    }

    void TryToFire() {
        tank.QueueProjectile();
    }
}

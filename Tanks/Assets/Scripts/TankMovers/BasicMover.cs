using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMover : MonoBehaviour
{
    private static int wallLayerMask;

    private Tank tank;
    private Vector3 movementDirection;
    private static float rotationRange = 30 * Mathf.Deg2Rad; 
    private static float[] changeDirectionTime = {0.2f, 0.4f};
    private float currentWait = 0;
    private float waitTime = changeDirectionTime[0];

    void Start()
    {
        wallLayerMask = LayerMask.GetMask("Walls");

        tank = GetComponent<Tank>();
        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        movementDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
    }

    void Update() {
        currentWait += Time.deltaTime;

        Collider[] collidingWalls = Physics.OverlapSphere(tank.transform.position, 1.2f, wallLayerMask);

        if (collidingWalls.Length > 0) {
            Debug.Log(collidingWalls[0]);
            Vector3 awayFromWall = tank.transform.position - collidingWalls[0].ClosestPoint(tank.transform.position);
            //copied and pasted, might want to make this code reusable later
            float rotateAngle = Random.Range(-60 * Mathf.Deg2Rad, 60 * Mathf.Deg2Rad);
            float newDirectionX = awayFromWall.x * Mathf.Cos(rotateAngle) - awayFromWall.z * Mathf.Sin(rotateAngle);
            float newDirectionZ = awayFromWall.x * Mathf.Sin(rotateAngle) + awayFromWall.z * Mathf.Cos(rotateAngle);
            movementDirection = new Vector3(newDirectionX, 0, newDirectionZ);
            tank.SetMovementDirection(movementDirection);
        }

        if (currentWait >= waitTime) {
            waitTime = Random.Range(changeDirectionTime[0], changeDirectionTime[1]);
            currentWait = 0;

            float rotateAngle = Random.Range(-rotationRange, rotationRange);
            float newDirectionX = movementDirection.x * Mathf.Cos(rotateAngle) - movementDirection.z * Mathf.Sin(rotateAngle);
            float newDirectionZ = movementDirection.x * Mathf.Sin(rotateAngle) + movementDirection.z * Mathf.Cos(rotateAngle);
            movementDirection = new Vector3(newDirectionX, 0, newDirectionZ);
            tank.SetMovementDirection(movementDirection);
        }   
    }


}

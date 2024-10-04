using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float speed = 3;
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Quaternion targetRotation;
    private Quaternion targetAim;
    

    // private GameObject tankBase;
    private GameObject tankHead;
    
    void Start()
    {
        // tankBase = transform.Find("Base").gameObject;
        tankHead = transform.Find("Head").gameObject;
    }

    public void MoveInDirection(Vector3 direction) {
        direction.y = 0;
        targetVelocity = direction.normalized * speed;

        if (direction != Vector3.zero) {
            Vector3 lookDirection = Vector3.Angle(direction, transform.forward) < 105 ? direction : direction * -1;

            targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
    }

    public void AimInDirection(Vector3 direction) {
        direction.y = 0;
        targetAim = Quaternion.LookRotation(direction);
    }

    public void SetAimDirection(Vector3 direction) {
        direction.y = 0;
        Quaternion quat = Quaternion.LookRotation(direction);
        targetAim = quat;
        tankHead.transform.rotation = quat;
    }

    public void FireProjectile() {
        GameObject projectile = Instantiate<GameObject>(projectilePrefab);
        projectile.transform.position = tankHead.transform.position + tankHead.transform.forward;
        projectile.transform.rotation = Quaternion.LookRotation(tankHead.transform.forward);
    }

    public void DestroyTank()
    {
        Destroy(gameObject);
    }

    void Update() {
        if (velocity != targetVelocity) {
            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * 10);
        }
        if (targetVelocity != Vector3.zero) {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 7);
        }
        transform.position = transform.position + (velocity * Time.deltaTime);

        if (targetAim != tankHead.transform.rotation) {
            tankHead.transform.rotation = Quaternion.RotateTowards(tankHead.transform.rotation, targetAim, Time.deltaTime * 360);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tank : MonoBehaviour, Hittable
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    public float speed = 3;
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private bool overrideVelocity = false;
    private Quaternion targetRotation;
    private Quaternion targetAim;
    private bool projectileQueued;
    

    // private GameObject tankBase;
    private GameObject tankHead;
    
    void Start()
    {
        // tankBase = transform.Find("Base").gameObject;
        tankHead = transform.Find("Head").gameObject;
    }

    public void SetMovementDirection(Vector3 direction) {
        direction.y = 0;
        targetVelocity = direction.normalized * speed;

        SetWheelRotation(direction);   
    }

    // manually override velocity. allows for NavMeshAgent to override movement
    public void SetVelocity(Vector3 velocity) {
        velocity.y = 0;
        this.velocity = velocity;
        overrideVelocity = true;

        SetWheelRotation(velocity.normalized * speed);
    }

    public void SetWheelRotation(Vector3 direction) {
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

    // Queue a projectile to be shot after turning
    public void QueueProjectile() {
        projectileQueued = true;
    }

    public void OnHit() {
        Destroy(gameObject);
    }

    void Update() {
        if (!overrideVelocity && velocity != targetVelocity) {
            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * 10);
        }
        if (velocity != Vector3.zero) {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 7);
        }
        if (!overrideVelocity) {
            transform.position = transform.position + (velocity * Time.deltaTime);
        }   

        if (targetAim != tankHead.transform.rotation) {
            tankHead.transform.rotation = Quaternion.RotateTowards(tankHead.transform.rotation, targetAim, Time.deltaTime * 360);
        }
        else if (projectileQueued) {
            FireProjectile();
            projectileQueued = false;
        }
    }
}

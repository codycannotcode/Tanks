using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tank : MonoBehaviour, Hittable
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private GameObject bombPrefab;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float reloadTime;
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private bool overrideVelocity = false;
    private Quaternion targetRotation;
    private Quaternion targetAim;
    private bool projectileQueued; // used to fire projectiles after the tank is finished rotating
    public bool reloaded { get; private set; }

    public GameObject Projectile { get {return projectilePrefab;} }

    // private GameObject tankBase;
    private GameObject tankHead;
    
    void Start()
    {
        reloaded = true;
        tankHead = transform.Find("Head").gameObject;
    }

    public void SetMovementDirection(Vector3 direction) {
        direction.y = 0;
        targetVelocity = direction.normalized * speed;
        overrideVelocity = false;

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

    public bool FireProjectile() {
        if (!reloaded) {
            return false;
        }
        Vector3 projectilePosition = tankHead.transform.position + tankHead.transform.forward * 0.85f;
        if (!Bullet.CanSpawnAtPosition(projectilePosition)) {
            return false;
        }

        GameObject projectile = Instantiate<GameObject>(projectilePrefab);
        projectile.transform.position = projectilePosition;
        projectile.transform.rotation = Quaternion.LookRotation(tankHead.transform.forward);
        reloaded = false;
        StartCoroutine(Reload());
        return true;
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(reloadTime);
        reloaded = true;
    }

    // Queue a projectile to be shot after turning
    public void QueueProjectile() {
        projectileQueued = true;
    }

    public void PlaceBomb() {
        GameObject bomb = Instantiate(bombPrefab);
        Vector3 position = transform.position;
        position.y = 0;
        bomb.transform.position = position;
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

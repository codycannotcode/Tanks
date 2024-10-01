using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tank : MonoBehaviour
{

    [SerializeField]
    private float speed = 10;
    private Vector3 velocity;
    private Vector3 targetVelocity;
    private Quaternion targetRotation;
    private Quaternion targetAim;

    private GameObject tankBase;
    private GameObject tankHead;
    
    void Start()
    {
        tankBase = transform.Find("Base").gameObject;
        tankHead = transform.Find("Head").gameObject;
    }

    public void MoveInDirection(Vector3 direction) {
        direction.y = 0;
        targetVelocity = direction.normalized * speed;

        if (direction != Vector3.zero) {
            Vector3 lookDirection = Vector3.Angle(direction, tankBase.transform.forward) < 105 ? direction : direction * -1;

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

    void Update() {
        if (velocity != targetVelocity) {
            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * 10);
        }
        if (targetVelocity != Vector3.zero) {
            tankBase.transform.rotation = Quaternion.Lerp(tankBase.transform.rotation, targetRotation, Time.deltaTime * 7);
        }
        transform.position = transform.position + (velocity * Time.deltaTime);

        if (targetAim != tankHead.transform.rotation) {
            tankHead.transform.rotation = Quaternion.RotateTowards(tankHead.transform.rotation, targetAim, Time.deltaTime * 360);
        }
    }
}
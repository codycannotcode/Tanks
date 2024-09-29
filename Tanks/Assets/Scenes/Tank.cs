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
    }

    void Update() {
        if (velocity != targetVelocity) {
            velocity = Vector3.Lerp(velocity, targetVelocity, 0.02f);
        }

        transform.position = transform.position + (velocity * Time.deltaTime);
    }
}

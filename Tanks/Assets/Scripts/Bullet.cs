using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) 
    {
        Debug.Log(collision.gameObject);
        Tank tank = collision.gameObject.GetComponent<Tank>();
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        Debug.Log(bullet);
        if (tank != null) {
            tank.DestroyTank();
            Destroy(gameObject);
        }
        else if (bullet != null) {
            Destroy(gameObject);
        }
        else {
            
            transform.rotation = Quaternion.LookRotation(transform.forward * -1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hittable
{
    void OnHit();
}

public class Bullet : MonoBehaviour, Hittable
{
    [SerializeField]
    private float speed;
    private int bounces = 1;

    public void OnHit() {}

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) 
    {
        Hittable hittable = collision.gameObject.GetComponent<Hittable>();
        if (hittable != null) {
            hittable.OnHit();
            Destroy(gameObject);
        }
        else {
            // If the collision is not a hittable, then we know we hit a wall
            if (bounces <= 0) {
                Destroy(gameObject);
            }
            else {
                // Reflect bullet across normal of the surface it hit. 
                // https://math.stackexchange.com/questions/13261/how-to-get-a-reflection-vector
                ContactPoint contact = collision.GetContact(0);
                Vector3 reflection = transform.forward - 2 * Vector3.Dot(transform.forward, contact.normal) * contact.normal;

                transform.rotation = Quaternion.LookRotation(reflection);
                bounces--;
            }
        }
    }
}

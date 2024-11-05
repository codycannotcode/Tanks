using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hittable
{
    void OnHit();
}

public class Bullet : MonoBehaviour, Hittable
{
    private AudioSource audioSource;
    
    [SerializeField]
    private AudioClip bounceSound;
    [SerializeField]
    private AudioClip brokeSound;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int bounces;
    public int Bounces { get {return bounces;} }
    private LevelPlayer levelPlayer;
    
    private static int layerMask;
    private static float thickness = 0.125f;

    public void OnHit() {}
    void Start() {
        audioSource = Camera.main.GetComponent<AudioSource>();
        layerMask = LayerMask.GetMask("Walls");
        levelPlayer = FindAnyObjectByType<LevelPlayer>();
    }

    void Update()
    {
        if (levelPlayer.currentLevel.Complete) {
            enabled = false;
        }
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) 
    {
        Hittable hittable = collision.gameObject.GetComponent<Hittable>();
        if (hittable != null) {
            hittable.OnHit();
            audioSource.PlayOneShot(brokeSound, 0.1f);
            Destroy(gameObject);
        }
        else {
            // If the collision is not a hittable, then we know we hit a wall
            if (bounces <= 0) {
                audioSource.PlayOneShot(brokeSound, 0.1f);
                Destroy(gameObject);
            }
            else {
                // Reflect bullet across normal of the surface it hit. 
                ContactPoint contact = collision.GetContact(0);
                Vector3 reflection = Vector3.Reflect(transform.forward, contact.normal);

                transform.rotation = Quaternion.LookRotation(reflection);
                bounces--;

                audioSource.PlayOneShot(bounceSound, 0.3f);
            }
        }
    }

    public static bool CanSpawnAtPosition(Vector3 position) {
        return !Physics.CheckSphere(position, thickness, layerMask);
    }
}

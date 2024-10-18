using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, Hittable
{
    [SerializeField]
    private GameObject explosionObject;
    private float explosionTime = 10; // seconds until the bomb explodes
    private float flashTime = 3; // seconds before explosion when bomb starts flashing
    
    void Start()
    {
        StartCoroutine(PrepareToExplode());
    }

    IEnumerator PrepareToExplode() {
        yield return new WaitForSeconds(explosionTime - flashTime);
        Debug.Log("start flashing");
        yield return new WaitForSeconds(flashTime);
        Explode();
    }

    void Explode() {
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionObject);
        explosion.transform.position = transform.position;
    }

    public void OnHit() {
        Explode();
    }
}

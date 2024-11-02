using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, Hittable
{
    [SerializeField]
    private GameObject explosionObject;
    private float explosionTime = 5; // seconds until the bomb explodes
    private float flashTime = 1.5f; // seconds before explosion when bomb starts flashing
    private int layerMask;
    
    void Start()
    {
        layerMask = LayerMask.GetMask("Tanks", "Bombs", "Walls");
        StartCoroutine(PrepareToExplode());
    }

    IEnumerator PrepareToExplode() {
        yield return new WaitForSeconds(explosionTime - flashTime);
        StartCoroutine(Flash());
        yield return new WaitForSeconds(flashTime);
        Explode();
    }

    IEnumerator Flash() {
        bool flash = false;
        Material material = GetComponent<Renderer>().material;
        Color regular = material.color;
        Color flashColor = Color.red;
        while (true) {
            flash = !flash;
            material.color = flash ? flashColor : regular;
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Explode() {
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject);

        GameObject explosion = Instantiate(explosionObject);
        explosion.transform.position = transform.position;

        Vector3 point0 = transform.position;
        Vector3 point1 = point0 + new Vector3(0, 4, 0);
        float radius = explosion.transform.localScale.x / 2 * 0.9f;

        Collider[] explosionOverlap = Physics.OverlapCapsule(point0, point1, radius, layerMask);

        foreach (Collider collider in explosionOverlap) {
            Hittable hittable = collider.GetComponent<Hittable>();

            if (hittable != null) {
                hittable.OnHit();
            }
            else if (collider.gameObject.tag == "ExplodableWall") {
                Destroy(collider.gameObject);
            }
        }
    }

    public void OnHit() {
        Explode();
    }
}

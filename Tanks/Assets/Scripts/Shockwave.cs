using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private Animator sphere;
    private Animator cylinder;
    // Start is called before the first frame update
    void Start()
    {
        sphere = transform.Find("Sphere").GetComponent<Animator>();
        cylinder = transform.Find("Cylinder").GetComponent<Animator>();

        StartCoroutine(SphereFinish());
        StartCoroutine(CylinderFinish());
    }

    IEnumerator SphereFinish() {
        yield return new WaitUntil(() => sphere.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        Destroy(sphere.gameObject);
        Destroy(gameObject);
    }

    IEnumerator CylinderFinish() {
        yield return new WaitUntil(() => cylinder.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        Destroy(cylinder.gameObject);
    }
}

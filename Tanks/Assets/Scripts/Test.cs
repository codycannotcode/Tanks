using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        FlipAlongAxis(transform, false, true);
    }

    static void FlipAlongAxis(Transform transform, bool xAxis, bool zAxis) {
        Vector3 flipVector = new Vector3(xAxis ? -1 : 1, 1, zAxis ? -1 : 1);
        foreach (Transform child in transform) {
            child.transform.position = Vector3.Scale(child.transform.position, flipVector);
        }
    }
}

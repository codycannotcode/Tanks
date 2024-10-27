using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpawn : MonoBehaviour
{
    void Start() {
        GetComponent<Renderer>().enabled = false;
    }
}

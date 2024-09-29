using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrive : MonoBehaviour
{
    private Tank tank;
    private Vector3 movementDirection;
    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 m = new Vector3(horizontal, 0, vertical);
        if (m != movementDirection) {
            movementDirection = m;
            tank.MoveInDirection(movementDirection);
        }
    }
}

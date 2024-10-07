using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
            tank.SetMovementDirection(movementDirection);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo)) {
            Vector3 aimDirection = hitInfo.point - transform.position;
            aimDirection.y = 0;
            tank.SetAimDirection(aimDirection);
        }

        if (Input.GetMouseButtonDown(0)) {
            tank.FireProjectile();
        }
    }
}

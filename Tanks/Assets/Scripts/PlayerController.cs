using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float RaycastPlaneHeight = 0.69f;
    private Tank tank;
    private Vector3 movementDirection;
    private Plane raycastPlane;
    // Start is called before the first frame update
    void Start()
    {
        raycastPlane = new Plane(Vector3.up, -RaycastPlaneHeight);
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
        if (raycastPlane.Raycast(ray, out float enter)) {
            Vector3 target = ray.origin + ray.direction * enter;
            Vector3 aimDirection = target - transform.position;
            aimDirection.y = 0;
            tank.SetAimDirection(aimDirection);
        }

        if (Input.GetMouseButtonDown(0)) {
            tank.FireProjectile();
        }
        if (Input.GetMouseButtonDown(1)) {
            tank.PlaceBomb();
        }
    }
}

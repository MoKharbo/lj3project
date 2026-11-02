using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : Rotator
{
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            LookAt(worldPosition);
        }
    }
}
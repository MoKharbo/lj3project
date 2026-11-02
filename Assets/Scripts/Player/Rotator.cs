using System.Runtime.CompilerServices;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    protected void LookAt(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Gun gun;
    [SerializeField] private int health = 100;
    private Vector2 move;
    private bool isDead;

    
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gun.Shoot();
        }
    }
    private void Update()
    {
        if (isDead)
        {
            return;
        }

        movePlayer();
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
        {
            return;
        }

        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject, 0.5f);
        }
    }
    private void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}

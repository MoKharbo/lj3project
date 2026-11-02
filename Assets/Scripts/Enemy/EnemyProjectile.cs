using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private bool hasHit;

    public void Initialize(int projectileDamage)
    {
        damage = projectileDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        DamagePlayer(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamagePlayer(collision.collider);
    }

    private void DamagePlayer(Collider target)
    {
        if (hasHit)
        {
            return;
        }

        PlayerController player = target.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            hasHit = true;
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class AllianceProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BoundaryEdge _))
        {
            GameManager.Instance.StageManager.projectiles.Remove(this);
            PoolManager.Despawn(gameObject);
        }
        else
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                GameManager.Instance.StageManager.projectiles.Remove(this);
                PoolManager.Despawn(gameObject);
            }
        }
    }
}

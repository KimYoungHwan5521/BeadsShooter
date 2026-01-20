using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BoundaryEdge _))
        {
            GameManager.Instance.StageManager.projectiles.Remove(this);
            PoolManager.Despawn(gameObject);
        }
        else
        {
            Bar bar = collision.GetComponentInParent<Bar>();
            if (bar != null)
            {
                GameManager.Instance.StageManager.projectiles.Remove(this);
                PoolManager.Despawn(gameObject);
                bar.Shrink(0.1f);
            }
        }
    }
}

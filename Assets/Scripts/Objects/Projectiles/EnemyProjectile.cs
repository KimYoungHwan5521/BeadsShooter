using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField] bool slow;
    [SerializeField] float slowMagnitude;
    [SerializeField] float slowTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BoundaryEdge _) || collision.TryGetComponent(out IceBlock _))
        {
            GameManager.Instance.StageManager.projectiles.Remove(this);
            PoolManager.Despawn(gameObject);
        }
        else if (collision.CompareTag("Aura")) return;
        else
        {
            Bar bar = collision.GetComponentInParent<Bar>();
            if (bar != null)
            {
                GameManager.Instance.StageManager.projectiles.Remove(this);
                PoolManager.Despawn(gameObject);
                bar.Shrink(0.1f);
                if (slow)
                {
                    bar.timeLimitedSpeedMagnification = slowMagnitude;
                    bar.timeLimitedSpeedMagnificationTime = slowTime;
                }
            }
        }
    }
}

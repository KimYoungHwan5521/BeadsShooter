using UnityEngine;

public class AllianceProjectile : Projectile
{
    bool explosible;
    float explosionRange;
    bool burnable;
    float burnDamage;

    public void SetProjectile(float damage, float speed, bool explosible, float explosionRange, bool burnable, float burnDamage)
    {
        SetProjectile(damage, speed);
        this.explosible = explosible;
        this.explosionRange = explosionRange;
        this.burnable = burnable;
        this.burnDamage = burnDamage;
    }

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
                if(explosible)
                {

                }
                else
                {
                    enemy.TakeDamage(damage);
                    if (burnable) enemy.Burn(burnDamage);
                }
                GameManager.Instance.StageManager.projectiles.Remove(this);
                PoolManager.Despawn(gameObject);
            }
        }
    }
}

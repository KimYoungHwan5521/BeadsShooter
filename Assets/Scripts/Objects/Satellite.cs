using UnityEngine;

public class Satellite : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyProjectile projectile))
        {
            PoolManager.Despawn(projectile.gameObject);
        }
    }
}

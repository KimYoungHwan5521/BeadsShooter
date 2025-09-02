using UnityEngine;

public class Boundary : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Projectile projectile) && projectile.gameObject.activeSelf)
        {
            PoolManager.Despawn(projectile.gameObject);
        }
    }
}

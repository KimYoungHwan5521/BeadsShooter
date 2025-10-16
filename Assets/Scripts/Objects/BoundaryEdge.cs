using UnityEngine;

public class BoundaryEdge : CustomObject
{
    // edgeType - 0: Bottom, 1: Top, 2: Left, 3: Right
    public int edgeType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Projectile projectile) && projectile.gameObject.activeSelf)
        {
            if (edgeType == 0)
            {
                projectile.activated = false;
                PoolManager.Despawn(projectile.gameObject);
                return;
            }
            else if (edgeType == 1)
            {
                projectile.SetDirection(new(projectile.Direction.x, - Mathf.Abs(projectile.Direction.y)));
            }
            else if (edgeType == 2)
            {
                projectile.SetDirection(new(Mathf.Abs(projectile.Direction.x), projectile.Direction.y));
            }
            else if (edgeType == 3)
            {
                projectile.SetDirection(new(- Mathf.Abs(projectile.Direction.x), projectile.Direction.y));
            }
        }
    }
}

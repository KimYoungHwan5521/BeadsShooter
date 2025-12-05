using UnityEngine;

public class BoundaryEdge : CustomObject
{
    // edgeType - 0: Bottom, 1: Top, 2: Left, 3: Right
    public int edgeType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bead projectile) && projectile.gameObject.activeSelf)
        {
            if (edgeType == 0)
            {
                projectile.activated = false;
                GameManager.Instance.StageManager.beads.Remove(projectile);
                PoolManager.Despawn(projectile.gameObject);
                if (!projectile.IsFake)
                {
                    Bead newBead = PoolManager.Spawn(ResourceEnum.Prefab.NormalBead, GameManager.Instance.StageManager.bar.transform.position + new Vector3(1, 0.5f, 0)).GetComponent<Bead>();
                    newBead.Initialize(1, 20, 0, 0, new());
                    newBead.activated = false;
                    GameManager.Instance.StageManager.beads.Add(newBead);
                    GameManager.Instance.StageManager.bar.grabbedBeads.Add(newBead);
                }
                return;
            }
            else if (edgeType == 1)
            {
                projectile.SetDirection(new(projectile.Direction.x, - Mathf.Abs(projectile.Direction.y)));
                if(!projectile.IsFake) GameManager.Instance.StageManager.FeverHalf = true;
            }
            else if (edgeType == 2)
            {
                projectile.SetDirection(new(Mathf.Abs(projectile.Direction.x), projectile.Direction.y));
                if (!projectile.IsFake) GameManager.Instance.StageManager.FeverHalf = true;
            }
            else if (edgeType == 3)
            {
                projectile.SetDirection(new(- Mathf.Abs(projectile.Direction.x), projectile.Direction.y));
                if (!projectile.IsFake) GameManager.Instance.StageManager.FeverHalf = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Bead projectile) && projectile.gameObject.activeSelf && !projectile.IsFake)
        {
            GameManager.Instance.StageManager.FeverHalf = true;
        }
    }
}

using UnityEngine;

public class BoundaryEdge : CustomObject
{
    // edgeType - 0: Bottom, 1: Top, 2: Left, 3: Right
    public int edgeType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bead bead) && bead.gameObject.activeSelf)
        {
            if (edgeType == 0)
            {
                bead.activated = false;
                GameManager.Instance.StageManager.beads.Remove(bead);
                PoolManager.Despawn(bead.gameObject);
                if (!bead.IsFake)
                {
                    GameManager.Instance.StageManager.Life--;
                    GameManager.Instance.StageManager.beadRefill = true;
                }
                return;
            }
            else if (edgeType == 1)
            {
                bead.SetDirection(new(bead.Direction.x, -Mathf.Abs(bead.Direction.y)));
                if (!bead.IsFake) GameManager.Instance.StageManager.FeverHalf = true;
            }
            else if (edgeType == 2)
            {
                bead.SetDirection(new(Mathf.Abs(bead.Direction.x), bead.Direction.y));
                if (!bead.IsFake) GameManager.Instance.StageManager.FeverHalf = true;
            }
            else if (edgeType == 3)
            {
                bead.SetDirection(new(-Mathf.Abs(bead.Direction.x), bead.Direction.y));
                if (!bead.IsFake) GameManager.Instance.StageManager.FeverHalf = true;
            }
        }
        else
        {
            Coin coin = collision.GetComponentInParent<Coin>();
            if (coin != null)
            {
                PoolManager.Despawn(coin.gameObject);
                GameManager.Instance.StageManager.coins.Remove(coin);
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

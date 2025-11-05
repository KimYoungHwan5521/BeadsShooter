using UnityEngine;

public class IllusionBlock : Block
{
    [SerializeField] Projectile entered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Projectile projectile))
        {
            entered = projectile;
            if (GameManager.Instance.StageManager.currentStageEnemies.Contains(this)) GameManager.Instance.StageManager.currentStageEnemies.Remove(this);
            else if (GameManager.Instance.StageManager.nextStageEnemies.Contains(this)) GameManager.Instance.StageManager.nextStageEnemies.Remove(this);
            GameManager.Instance.StageManager.StageClearCheck();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Projectile projectile) && projectile == entered)
        {
            entered.DuplicateFakeBead(entered.Direction.Rotate(-30));
            entered.DuplicateFakeBead(entered.Direction.Rotate(30));
            entered = null;
            PoolManager.Despawn(gameObject);

        }
    }
}

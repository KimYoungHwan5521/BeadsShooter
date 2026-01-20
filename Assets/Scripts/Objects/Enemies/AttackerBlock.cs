using UnityEngine;

public class AttackerBlock : Block
{
    [SerializeField] float attackCool;
    float curAttackCool;

    override protected void OnEnable()
    {
        base.OnEnable();
        curAttackCool = 0;
    }

    public override void MyUpdateOnlyCurrentStage(float deltaTime)
    {
        if (!gameObject.activeSelf) return;

        curAttackCool += deltaTime;
        if(curAttackCool > attackCool )
        {
            Projectile projectile = PoolManager.Spawn(ResourceEnum.Prefab.NormalProjectile, transform.position).GetComponent<Projectile>();
            projectile.SetDirection(Vector2.down);
            curAttackCool = 0;
        }
    }
}

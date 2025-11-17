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

    protected override void MyUpdate()
    {
        if (!gameObject.activeSelf) return;

        curAttackCool += Time.deltaTime;
        if(curAttackCool > attackCool )
        {
            PoolManager.Spawn(ResourceEnum.Prefab.NormalProjectile, transform.position);
            curAttackCool = 0;
        }
    }
}

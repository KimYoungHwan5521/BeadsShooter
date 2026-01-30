using UnityEngine;

public class Boss2Split : Enemy
{
    Rigidbody2D rigid;
    [SerializeField] Boss2 owner;
    // part : 0 = full body, 1~2 = half body, 3~6 = quarter body
    [SerializeField] int part;
    public override float CurHP
    {
        get => base.CurHP;
        set
        {
            curHP = Mathf.Max(0, value);
            if (curHP <= 0)
            {
                isDead = true;
                if (part == 0)
                {
                    owner.FullBodySplit();
                }
                else if(part == 1)
                {
                    owner.HalfBody1Split();
                }
                else if(part == 2)
                {
                    owner.HalfBody2Split();
                }
                else
                {
                    gameObject.SetActive(false);
                    owner.CheckIsDead();
                }
                return;
            }
            owner.SetHPBar();
        }
    }
    float originalScale;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 curMoveDirection;

    [SerializeField] ResourceEnum.Prefab dripping;
    [SerializeField] float drippingCool;
    [SerializeField] float curDrippingCool;
    [SerializeField] float attackCool;
    [SerializeField] float curAttackCool;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected override void MyStart()
    {
        rigid.linearVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        originalScale = transform.localScale.x;
    }

    public override void MyUpdateOnlyCurrentStage(float deltaTime)
    {
        if (!gameObject.activeSelf || GameManager.Instance.StageManager.currentStage != stage) return;
        rigid.linearVelocity = rigid.linearVelocity.normalized * moveSpeed;
        Debug.Log(rigid.linearVelocity);
        if (rigid.linearVelocityX > 0) transform.localScale = new(-originalScale, originalScale, originalScale);
        else transform.localScale = new(originalScale, originalScale, originalScale);

        curDrippingCool += deltaTime;
        if (curDrippingCool > drippingCool)
        {
            curDrippingCool = 0;
            PoolManager.Spawn(dripping, transform.position);
        }

        if(part < 3)
        {
            curAttackCool += deltaTime;
            if (curAttackCool > attackCool)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        Projectile projectile;
        if(part == 0)
        {
            projectile = PoolManager.Spawn(ResourceEnum.Prefab.Boss2Attack1, transform.position).GetComponent<Projectile>();
        }
        else
        {
            projectile = PoolManager.Spawn(ResourceEnum.Prefab.Boss2Attack2, transform.position).GetComponent<Projectile>();
        }
        projectile.SetDirection(Vector2.down);
        curAttackCool = 0;
    }
}

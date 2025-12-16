using UnityEngine;

public class Enemy : CustomObject
{
    //Animator animator;
    //[SerializeField] Image hpBar;
    protected int stage;
    //[SerializeField] Vector2 moveDirection = Vector2.down;
    protected bool isDead;
    public bool IsDead
    {
        get => isDead;
        set
        {
            //animator.SetTrigger("Dead");
            if(value && isDead != value)
            {
                GameManager.Instance.StageManager.currentStageEnemies.Remove(this);
                GameManager.Instance.StageManager.nextStageEnemies.Remove(this);
                GameManager.Instance.StageManager.currentStageWalls.Remove(gameObject);
                PoolManager.Despawn(gameObject);
                GameManager.Instance.StageManager.StageClearCheck();

                for(int i=0; i<coins; i++)
                {
                    Rigidbody2D coinRigid = PoolManager.Spawn(ResourceEnum.Prefab.Coin, transform.position).GetComponent<Rigidbody2D>();
                    coinRigid.AddForce((new Vector2(Random.Range(0, 1f), Random.Range(0, 1f))).normalized * 100);
                    GameManager.Instance.StageManager.coins.Add(coinRigid.GetComponent<Coin>());
                }
            }
            isDead = value;
        }
    }
    [SerializeField] protected float maxHP;
    [SerializeField] protected float curHP;
    public virtual float CurHP
    {
        get => curHP;
        set
        {
            curHP = value;
            //hpBar.fillAmount = curHP / maxHP;
            if (curHP <= 0) IsDead = true;
        }
    }
    protected int coins;

    protected override void Start()
    {
        //animator = GetComponent<Animator>();
        //animator.SetFloat("moveSpeed", moveSpeed);
        base.Start();
    }

    protected virtual void OnEnable()
    {
        IsDead = false;
        CurHP = maxHP;
    }

    public virtual void SetInfo(int stage, float maxHP, bool isWall = false)
    { 
        this.stage = stage;
        CurHP = this.maxHP = maxHP;
        if (!isWall) coins = (int)maxHP;
    }

    public virtual void TakeDamage(float damage)
    {
        CurHP -= damage;
    }
}

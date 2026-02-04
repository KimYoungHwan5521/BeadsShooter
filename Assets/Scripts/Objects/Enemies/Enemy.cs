using UnityEngine;
using UnityEngine.UI;

public class Enemy : CustomObject
{
    //Animator animator;
    [SerializeField] protected Image hpBar;
    [SerializeField] protected int stage;
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
                if (attaker != null) attaker.Strike++;
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
    public float MaxHP => maxHP;
    [SerializeField] protected float curHP;
    public virtual float CurHP
    {
        get => curHP;
        set
        {
            curHP = Mathf.Max(0, value);
            if(hpBar != null) hpBar.fillAmount = curHP / maxHP;
            if (curHP <= 0) IsDead = true;
        }
    }
    protected int coins;
    public bool isInvincible;
    protected Bead attaker;

    float burnDamage;
    float curBurnCool;
    float burnLeft;

    protected virtual void Start()
    {
        //animator = GetComponent<Animator>();
        //animator.SetFloat("moveSpeed", moveSpeed);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(GameManager.Instance.StageManager.currentStage == stage)
        {
            GameManager.Instance.ObjectUpdate -= MyUpdateOnlyCurrentStage;
            GameManager.Instance.ObjectUpdate += MyUpdateOnlyCurrentStage;
        }
        IsDead = false;
        CurHP = maxHP;
        curBurnCool = 0;
    }

    public override void MyUpdate(float deltaTime)
    {
        if(burnLeft > 0)
        {
            burnLeft -= deltaTime;
            curBurnCool += deltaTime;
            if(curBurnCool >= 1)
            {
                TakeDamage(burnDamage);
                curBurnCool = 0;
            }
        }
    }

    public virtual void SetInfo(int stage, float maxHP, bool isWall = false, bool isInvincible = false)
    { 
        this.stage = stage;
        CurHP = this.maxHP = maxHP;
        if (!isWall) coins = (int)maxHP;
        this.isInvincible = isInvincible;
    }

    public virtual void TakeDamage(float damage)
    {
        TakeDamage(damage, null);
    }

    public virtual void TakeDamage(float damage, Bead attaker)
    {
        if (isInvincible) return;
        this.attaker = attaker;
        CurHP -= damage;
    }

    public virtual void Burn(float burnDamage)
    {
        this.burnDamage = burnDamage;
        burnLeft = 3f;
    }
}

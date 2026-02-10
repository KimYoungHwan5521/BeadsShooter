using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : CustomObject
{
    [SerializeField] Animator burnAnim;
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
    const float electricChainTerm = 0.06f;
    float curElectricChainTerm;
    bool electricReserved;
    float electricDamage;
    Vector3 electricFrom;
    int leftChain;
    List<Enemy> alreadyChained = new();

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
                if (burnAnim != null) burnAnim.SetTrigger("Burn");
                else Debug.LogWarning($"No burn animation : {gameObject.name}");
                TakeDamage(burnDamage);
                curBurnCool = 0;
            }
        }

        if(electricReserved)
        {
            curElectricChainTerm += deltaTime;
            if(curElectricChainTerm > electricChainTerm)
            {
                ElectricChain(electricDamage, electricFrom, leftChain, alreadyChained);
                electricReserved = false;
                curElectricChainTerm = 0;
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

    public void ReserveElectricChain(float damage, Vector3 from, int leftChain, List<Enemy> alreadyChained)
    {
        electricReserved = true;
        electricDamage = damage;
        electricFrom = from;
        this.leftChain = leftChain;
        this.alreadyChained = alreadyChained;
    }

    public virtual void ElectricChain(float damage, Vector3 from, int leftChain, List<Enemy> alreadyChained)
    {
        LineRenderer electric = PoolManager.Spawn(ResourceEnum.Prefab.Electric, transform.position).GetComponent<LineRenderer>();
        electric.SetPositions(new Vector3[] { from, GetComponent<Collider2D>().bounds.center });
        electric.startWidth = 0.1f * (leftChain + 1);
        electric.endWidth = 0.1f * leftChain;
        TakeDamage(damage);
        if(leftChain > 0)
        {
            List<Enemy> targets = new();
            for (int i = 0; i < 3; i++)
            {
                Enemy target = FindElectricChainTarget(from, ref alreadyChained);
                if (target == null)
                {
                    break;
                }
                targets.Add(target);
            }
            foreach(var target in targets) target.ReserveElectricChain(damage * 0.5f, GetComponentInChildren<Collider2D>().bounds.center, leftChain - 1, alreadyChained);
        }
    }

    protected virtual Enemy FindElectricChainTarget(Vector3 from, ref List<Enemy> alreadyChained)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(GetComponentInChildren<Collider2D>().bounds.center, 1.5f);
        List<Enemy> candidates = new();
        foreach(var hit in hits)
        {
            Enemy candidate = hit.GetComponentInParent<Enemy>();
            if (candidate != null && !alreadyChained.Contains(candidate))
            {
                candidates.Add(candidate);
            }
        }
        if (candidates.Count > 0)
        {
            Enemy nearest = candidates[0];
            float minDistance = Vector2.Distance(from, GetComponentInChildren<Collider2D>().bounds.center);
            foreach(var candidate in candidates)
            {
                float distance = Vector2.Distance(from, candidate.GetComponentInChildren<Collider2D>().bounds.center);
                if (distance < minDistance)
                {
                    nearest = candidate;
                    minDistance = distance;
                }
            }
            alreadyChained.Add(nearest);
            return nearest;
        }
        else return null;
    }
}

using UnityEngine;

public class SplitBlock : Block
{
    Collider2D col;
    [SerializeField] SplitterBlock owner;
    // 0 : fullBody, 1 : halfBody1, 2 : halfBody2
    [SerializeField] int part;
    bool isInvincible;
    [SerializeField] float invincibleTime;
    [SerializeField] float curInvincibleTime;

    public override float CurHP 
    { 
        get => base.CurHP; 
        set
        {
            curHP = value;
            if(curHP <= 0)
            {
                isDead = true;
                if (part == 0)
                {
                    owner.Split();
                }
                else
                {
                    gameObject.SetActive(false);
                    owner.CheckIsDead();
                }
                return;
            }
            if (crack != null)
            {
                crack.gameObject.SetActive(curHP != maxHP);
                crack.localScale = new(Mathf.Max(0.25f * (4 + curHP - maxHP), 0), Mathf.Max(0.25f * (4 + curHP - maxHP), 0));
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();
    }

    protected override void MyUpdate()
    {
        if(isInvincible)
        {
            curInvincibleTime += Time.deltaTime;
            if(curInvincibleTime > invincibleTime)
            {
                col.enabled = true;
                isInvincible = false;
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(part > 0)
        {
            curInvincibleTime = 0;
            col.enabled = false;
            isInvincible = true;
        }
    }
}

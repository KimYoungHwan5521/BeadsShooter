using UnityEngine;

public class Enemy : CustomObject
{
    //Animator animator;
    //[SerializeField] Image hpBar;
    int stage;
    [SerializeField] Vector2 moveDirection = Vector2.down;
    bool isDead;
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
                PoolManager.Despawn(gameObject);
                GameManager.Instance.StageManager.StageClearCheck();
            }
            isDead = value;
        }
    }
    [SerializeField]float maxHP;
    [SerializeField]float curHP;
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

    protected override void Start()
    {
        //animator = GetComponent<Animator>();
        //animator.SetFloat("moveSpeed", moveSpeed);
        base.Start();
    }

    private void OnEnable()
    {
        IsDead = false;
        CurHP = maxHP;
    }

    public virtual void SetInfo(int stage, float maxHP)
    { 
        this.stage = stage;
        CurHP = this.maxHP = maxHP;
    }

    public virtual void TakeDamage(float damage)
    {
        CurHP -= damage;
    }
}

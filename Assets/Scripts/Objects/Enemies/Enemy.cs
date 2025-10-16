using UnityEngine;

public class Enemy : CustomObject
{
    //Animator animator;
    //[SerializeField] Image hpBar;
    int stage;
    [SerializeField] Vector2 moveDirection = Vector2.down;
    [SerializeField] float moveSpeed = 1f;
    bool isDead;
    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            //animator.SetTrigger("Dead");
            if(value)
            {
                GameManager.Instance.StageManager.currentStageEnemies.Remove(this);
                GameManager.Instance.StageManager.nextStageEnemies.Remove(this);
                PoolManager.Despawn(gameObject);
                GameManager.Instance.StageManager.StageClearCheck();
            }
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

    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
            //animator.SetFloat("moveSpeed", value);
        }
    }

    protected override void Start()
    {
        //animator = GetComponent<Animator>();
        //animator.SetFloat("moveSpeed", moveSpeed);
        base.Start();
    }

    protected override void MyUpdate()
    {
        if (IsDead) return;
        if (stage <= GameManager.Instance.StageManager.currentStage)
        {
            transform.position += moveSpeed * Time.deltaTime * (Vector3)moveDirection.normalized;
        }
    }

    private void OnEnable()
    {
        IsDead = false;
        CurHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boderline"))
        {
            IsDead = true;
            PoolManager.Despawn(gameObject);
        }
    }

    public virtual void SetInfo(int stage, float maxHP, float moveSpeed = 0.1f)
    { 
        this.stage = stage;
        CurHP = this.maxHP = maxHP;
        MoveSpeed = moveSpeed;
    }

    public virtual void TakeDamage(float damage)
    {
        CurHP -= damage;
    }
}

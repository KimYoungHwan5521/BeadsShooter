using UnityEngine;
using UnityEngine.UI;

public class Enemy : CustomObject
{
    Animator animator;
    [SerializeField] Image hpBar;

    [SerializeField] Vector2 moveDirection = Vector2.down;
    [SerializeField] float moveSpeed = 1f;
    bool isDead;
    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            animator.SetTrigger("Dead");
        }
    }
    [SerializeField]float maxHP;
    [SerializeField]float curHP;
    public float CurHP
    {
        get => curHP;
        set
        {
            curHP = value;
            hpBar.fillAmount = curHP / maxHP;
        }
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
            animator.SetFloat("moveSpeed", value);
        }
    }

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("moveSpeed", moveSpeed);
        base.Start();
    }

    protected override void MyUpdate()
    {
        if (IsDead) return;
        transform.position += moveSpeed * Time.deltaTime * (Vector3)moveDirection.normalized;
    }

    private void OnEnable()
    {
        CurHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boderline"))
        {
            PoolManager.Despawn(gameObject);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        CurHP -= damage;
        if (curHP <= 0) IsDead = true;
    }
}

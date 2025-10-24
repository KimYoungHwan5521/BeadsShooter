using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] int penetrationNumber;
    [SerializeField] float criticalRate;
    [SerializeField] Vector2 direction;

    Rigidbody2D rigidBody;
    public Vector2 Direction => direction;
    public bool activated;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    protected override void MyStart()
    {
        base.MyStart();
    }

    public void Initialize(float damage, float speed, int penetrationNumber, float criticalRate, Vector2 direction)
    {
        this.damage = damage;
        this.speed = speed;
        this.penetrationNumber = penetrationNumber;
        this.criticalRate = criticalRate;
        this.direction = direction;
        activated = true;
    }

    private void FixedUpdate()
    {
        if (!activated || GameManager.Instance.phase != GameManager.Phase.BattlePhase) return;
        rigidBody.linearVelocity = rigidBody.linearVelocity.normalized * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.phase != GameManager.Phase.BattlePhase || !activated) return;
        if(collision.collider.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }
        else if (collision.gameObject.TryGetComponent(out Bar bar) && !bar.grabbedBeads.Contains(this))
        {
            SetDirection(transform.position - bar.transform.position);
        }
    }

    public void SetDirection(Vector2 wantDirection)
    {
        rigidBody.linearVelocity = wantDirection.normalized * speed;
    }
}

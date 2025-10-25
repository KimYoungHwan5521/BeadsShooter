using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float defaultSpeed;
    public float speedMagnification = 1;
    public float speedCorrection = 0;
    float CurrentSpeed => (defaultSpeed * speedMagnification) + speedCorrection;
    public bool stop = false;
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
        defaultSpeed = speed;
        this.penetrationNumber = penetrationNumber;
        this.criticalRate = criticalRate;
        this.direction = direction;
        activated = true;
    }

    private void OnEnable()
    {
        speedMagnification = 1f;
        speedCorrection = 0f;
    }

    private void FixedUpdate()
    {
        if (!activated || GameManager.Instance.phase != GameManager.Phase.BattlePhase) return;
        if (!stop) rigidBody.linearVelocity = rigidBody.linearVelocity.normalized * CurrentSpeed;
        else rigidBody.linearVelocity = Vector2.zero;
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
        rigidBody.linearVelocity = wantDirection.normalized * CurrentSpeed;
    }
}

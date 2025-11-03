using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float defaultSpeed;
    public float speedMagnification = 1;
    public float temporarySpeedMagnification = 1;
    public float speedCorrection = 0;
    float CurrentSpeed => (defaultSpeed * speedMagnification * temporarySpeedMagnification) + speedCorrection;
    public bool stop = false;
    public TrailRenderer trail;
    [SerializeField] int penetrationNumber;
    [SerializeField] float criticalRate;
    [SerializeField] Vector2 direction;

    Rigidbody2D rigidBody;
    public Vector2 Direction => direction;
    public bool activated;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
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
        trail.Clear();
        activated = true;
    }

    private void OnEnable()
    {
        speedMagnification = 1f;
        temporarySpeedMagnification = 1f;
        speedCorrection = 0f;
    }

    private void FixedUpdate()
    {
        trail.enabled = activated;
        if (!activated || GameManager.Instance.phase != GameManager.Phase.BattlePhase) return;
        if (!stop)
        {
            // 수평 최소각
            if(Vector2.Angle(Vector2.right, rigidBody.linearVelocity) < 3)
            {
                if (Vector2.SignedAngle(Vector2.right, rigidBody.linearVelocity) < 3) SetDirection(new(15, 1));
                else if (Vector2.SignedAngle(Vector2.right, rigidBody.linearVelocity) > -3) SetDirection(new(15, -1));
            }
            else if (Vector2.Angle(Vector2.left, rigidBody.linearVelocity) < 3)
            {
                if (Vector2.SignedAngle(Vector2.left, rigidBody.linearVelocity) < 3) SetDirection(new(-15, -1));
                else if (Vector2.SignedAngle(Vector2.left, rigidBody.linearVelocity) > -3) SetDirection(new(-15, 1));
            }
            rigidBody.linearVelocity = rigidBody.linearVelocity.normalized * CurrentSpeed;
        }
        else
        {
            rigidBody.linearVelocity = Vector2.zero;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.phase != GameManager.Phase.BattlePhase || !activated) return;
        if(collision.collider.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            GameManager.Instance.StageManager.FeverGauge++;
        }
        else if (collision.gameObject.TryGetComponent(out Bar bar) && !bar.grabbedBeads.Contains(this))
        {
            temporarySpeedMagnification = 1;
            SetDirection(transform.position - bar.transform.position);
            GameManager.Instance.StageManager.FeverHalf = true;
        }
    }

    public void SetDirection(Vector2 wantDirection)
    {
        rigidBody.linearVelocity = wantDirection.normalized * CurrentSpeed;
    }
}

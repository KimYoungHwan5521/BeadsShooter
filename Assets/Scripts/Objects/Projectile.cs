using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float defaultSpeed;
    public float speedMagnification = 1;
    public float timeLimitedSpeedMagnification = 1;
    public float timeLimitedSpeedMagnificationTime;
    public float temporarySpeedMagnification = 1;
    public float speedCorrection = 0;
    float CurrentSpeed => (defaultSpeed * speedMagnification * timeLimitedSpeedMagnification * temporarySpeedMagnification) + speedCorrection;
    public bool stop = false;

    Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trail;
    
    [SerializeField] int penetrationNumber;
    [SerializeField] float criticalRate;
    public Vector2 Direction => rigidBody.linearVelocity;

    public bool activated;
    [SerializeField] bool isFake;
    public bool IsFake
    {
        get => isFake;
        set
        {
            isFake = value;
            if (value) gameObject.layer = LayerMask.NameToLayer("Fake Projectile");
            else gameObject.layer = LayerMask.NameToLayer("Projectile");
        }
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        SetDirection(direction);
        trail.Clear();
        IsFake = false;
        activated = true;
    }

    private void OnEnable()
    {
        speedMagnification = 1f;
        timeLimitedSpeedMagnification = 1f;
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

        if (timeLimitedSpeedMagnificationTime > 0)
        {
            timeLimitedSpeedMagnificationTime -= Time.deltaTime;
            timeLimitedSpeedMagnification = 1.3f;
        }
        else timeLimitedSpeedMagnification = 1f;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.phase != GameManager.Phase.BattlePhase || !activated) return;
        if(collision.collider.gameObject.TryGetComponent(out Enemy enemy))
        {
            if(!IsFake)
            {
                enemy.TakeDamage(damage);
                GameManager.Instance.StageManager.FeverGauge++;
            }
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

    public Projectile DuplicateFakeBead(Vector2 direction)
    {
        Projectile duplicated = PoolManager.Spawn(ResourceEnum.Prefab.NormalBead, transform.position).GetComponent<Projectile>();
        duplicated.Initialize(damage, defaultSpeed, penetrationNumber, criticalRate, direction);
        duplicated.IsFake = true;
        GameManager.Instance.StageManager.projectiles.Add(duplicated);

        return duplicated;
    }
}

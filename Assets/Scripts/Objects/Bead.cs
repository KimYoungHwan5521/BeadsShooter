using UnityEngine;
using System.Collections.Generic;

public class Bead : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float defaultSpeed;
    public float speedMagnification = 1;
    public float timeLimitedSpeedMagnification = 1;
    public float timeLimitedSpeedMagnificationTime;
    public float temporarySpeedMagnification = 1;
    List<Area> curInAreas = new();
    Vector2 lastDirection;
    public float SpeedMagnificationByArea
    {
        get
        {
            if (curInAreas.Count > 0) return curInAreas[0].speedMagnification;
            else return 1;
        }
    }
    public float speedCorrection = 0;
    float CurrentSpeed => (defaultSpeed * speedMagnification * timeLimitedSpeedMagnification * temporarySpeedMagnification * SpeedMagnificationByArea) + speedCorrection;
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
            if (value) gameObject.layer = LayerMask.NameToLayer("Fake Bead");
            else gameObject.layer = LayerMask.NameToLayer("Bead");
        }
    }
    int strike;
    public int Strike
    {
        get => strike;
        set
        {
            strike = value;
            QuestManager.Quest strikeQ = GameManager.Instance.StageManager.ongoingQuests.Find(x => x.conditionType == QuestManager.Condition.Strike);
            if (strikeQ != null && strikeQ.conditionQuantaty <= value)
            {
                GameManager.Instance.StageManager.ClearQuest(strikeQ);
            }
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

    protected override void OnEnable()
    {
        speedMagnification = 1f;
        timeLimitedSpeedMagnification = 1f;
        temporarySpeedMagnification = 1f;
        curInAreas = new();
        speedCorrection = 0f;
    }

    private void FixedUpdate()
    {
        trail.enabled = activated;
        //rigidBody.bodyType = activated ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
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
            lastDirection = rigidBody.linearVelocity;
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
        if (collision.collider.CompareTag("Wall")) return;
        Enemy enemy = collision.collider.GetComponentInParent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage, this);
        }
        else
        {
            Bar bar = collision.transform.GetComponentInParent<Bar>();
            if (bar != null && !bar.grabbedBeads.Contains(this))
            {
                temporarySpeedMagnification = 1;
                SetDirection(transform.position - bar.transform.position);
                Strike = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out SplitBlock split))
        {
            split.TakeDamage(damage, this);
        }
        else if(collision.TryGetComponent(out Area area))
        {
            curInAreas.Add(area);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Area area))
        {
            curInAreas.Remove(area);
        }
    }

    public void SetDirection(Vector2 wantDirection)
    {
        rigidBody.linearVelocity = wantDirection.normalized * CurrentSpeed;
    }

    public void SetDirectionToLastDirection()
    {
        SetDirection(lastDirection);
    }

    public Bead DuplicateFakeBead(Vector2 direction)
    {
        Bead duplicated = PoolManager.Spawn(ResourceEnum.Prefab.NormalBead, transform.position).GetComponent<Bead>();
        duplicated.Initialize(damage, defaultSpeed, penetrationNumber, criticalRate, direction);
        duplicated.IsFake = true;
        GameManager.Instance.StageManager.beads.Add(duplicated);

        return duplicated;
    }
}

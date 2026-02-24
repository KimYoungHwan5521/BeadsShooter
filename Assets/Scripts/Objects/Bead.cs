using System.Collections.Generic;
using UnityEngine;

public class Bead : CustomObject
{
    const float horizontalMinimumAngle = 20f;

    [SerializeField] float defaultDamage;
    public float damageMagnification = 1f;
    public float temporaryDamageCorrection = 0;
    public float Damage => (defaultDamage * damageMagnification) + temporaryDamageCorrection;
    public float defaultSpeed;
    public float speedMagnification = 1;
    public class TimeLimitedSpeedMagnification
    {
        public float magnification;
        public float time;

        public TimeLimitedSpeedMagnification(float magnification, float time)
        {
            this.magnification = magnification;
            this.time = time;
        }
    }
    List<TimeLimitedSpeedMagnification> timeLimitedSpeedMagnifications = new();
    float TotalTimeLimitedSpeedMagnification
    {
        get
        {
            float min = 1;
            foreach (var x in timeLimitedSpeedMagnifications) if (min > x.magnification) min = x.magnification;
            return min;
        }
    }
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
    float CurrentSpeed => (defaultSpeed * speedMagnification * TotalTimeLimitedSpeedMagnification * temporarySpeedMagnification * SpeedMagnificationByArea) + speedCorrection;
    //float CurrentSpeed => (defaultSpeed * speedMagnification * TotalTimeLimitedSpeedMagnification * temporarySpeedMagnification) + speedCorrection;
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

    [Header("Electric Ability")]
    [SerializeField] ParticleSystem electricChargeParticle;
    [SerializeField] GameObject electirChargedAnim;
    [SerializeField] bool electricCharged;
    public bool ElectricCharged
    {
        get => electricCharged;
        set
        {
            electricCharged = value;
            electirChargedAnim.SetActive(value);
            if (value)
            {
                //electricChargeParticle.Play();
                electricChainsLeft = GameManager.Instance.StageManager.bar.electricChainsCount;
                trail.startColor = Color.blue;
                trail.endColor = Color.blue;
            }
            else
            {
                //electricChargeParticle.Stop();
                trail.startColor = Color.white;
                trail.endColor = Color.white;
            }
        }
    }
    int electricChainsLeft;
    
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    public void Initialize(float damage, float speed, int penetrationNumber, float criticalRate, Vector2 direction)
    {
        defaultDamage = damage;
        defaultSpeed = speed;
        this.penetrationNumber = penetrationNumber;
        this.criticalRate = criticalRate;
        SetDirection(direction);
        trail.Clear();
        trail.emitting = true;
        IsFake = false;
        activated = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        damageMagnification = 1f;
        temporaryDamageCorrection = 0;
        speedMagnification = 1f;
        timeLimitedSpeedMagnifications.Clear();
        temporarySpeedMagnification = 1f;
        //curInAreas = new();
        speedCorrection = 0f;
        ElectricCharged = false;
    }

    public override void MyUpdate(float deltaTime)
    {
        trail.enabled = activated;
        //rigidBody.bodyType = activated ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        if (!activated || GameManager.Instance.phase != GameManager.Phase.BattlePhase || stop)
        {
            rigidBody.linearVelocity = Vector2.zero;
            return;
        }
        // 수평 최소각
        Vector2 v = rigidBody.linearVelocity;
        float speed = CurrentSpeed;

        if (v.sqrMagnitude > 0.0001f)
        {
            float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;   // -180~180
            float min = horizontalMinimumAngle;                  // deg

            // 오른쪽(ang≈0) / 왼쪽(ang≈180 or -180) 각각에서 "수평에 너무 가까우면" 보정
            if (Mathf.Abs(ang) < min) // right side near 0 deg
            {
                ang = Mathf.Sign(ang == 0 ? v.y : ang) * min; // 0일 때는 기존 y부호로 위/아래 결정
            }
            else if (Mathf.Abs(Mathf.DeltaAngle(ang, 180f)) < min) // left side near 180 deg
            {
                float sign = Mathf.Sign(Mathf.DeltaAngle(ang, 180f)); // 180 기준 위/아래
                if (sign == 0) sign = Mathf.Sign(v.y);
                ang = 180f + sign * min;
            }

            Vector2 dir = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad));
            rigidBody.linearVelocity = dir * speed;
            lastDirection = rigidBody.linearVelocity;
        }

        List<TimeLimitedSpeedMagnification> toBeDeleted = new();
        foreach(var tlsm in timeLimitedSpeedMagnifications)
        {
            tlsm.time -= deltaTime;
            if(tlsm.time < 0) toBeDeleted.Add(tlsm);
            if(toBeDeleted.Count == timeLimitedSpeedMagnifications.Count)
            {
                trail.startColor = Color.white;
                trail.endColor = Color.white;
            }
        }
        foreach (var tlsm in toBeDeleted) timeLimitedSpeedMagnifications.Remove(tlsm);
        
    }

    void GiveDamage(Enemy target, float damage)
    {
        target.TakeDamage(damage, this);
        if(electricCharged)
        {
            List<Enemy> chains = new() { target };
            target.ElectricChain(GameManager.Instance.StageManager.bar.electricDamage, transform.position, GameManager.Instance.StageManager.bar.electricChainsCount, chains);
            if (GameManager.Instance.StageManager.bar.gotElectrostaticInduction) target.redischargeReserved = true;
            electricChainsLeft--;
            Debug.Log($"electric chains left : {electricChainsLeft}");
            if(electricChainsLeft == 0) ElectricCharged = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.phase != GameManager.Phase.BattlePhase || !activated) return;
        if (collision.collider.CompareTag("Wall"))
        {
            temporaryDamageCorrection = 0;
            if(!ElectricCharged)
            {
                trail.startColor = Color.white;
                trail.endColor= Color.white;
            }
            return;
        }
        Enemy enemy = collision.collider.GetComponentInParent<Enemy>();
        if(enemy != null)
        {
            GiveDamage(enemy, Damage);
            temporaryDamageCorrection = 0;
            trail.startColor = Color.white;
            trail.endColor = Color.white;
            if (enemy is DrippingBlock dripper)
            {
                timeLimitedSpeedMagnifications.Add(new(dripper.slowRate, 1f));
                trail.startColor = Color.green;
                trail.endColor = Color.green;
            }
            else if(enemy is Boss2Split boss2)
            {
                timeLimitedSpeedMagnifications.Add(new(boss2.slowRate, 1f));
                trail.startColor = Color.green;
                trail.endColor = Color.green;
            }
        }
        else
        {
            Bar bar = collision.transform.GetComponentInParent<Bar>();
            if (bar != null && !bar.grabbedBeads.Contains(this))
            {
                temporarySpeedMagnification = 1;
                SetDirection(transform.position - bar.transform.position);
                Strike = 0;

                if (bar.ElectricCharged)
                {
                    ElectricCharged = true;
                    bar.ElectricCharged = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out SplitBlock split))
        {
            GiveDamage(split, Damage);
        }
        else if (collision.TryGetComponent(out Area area))
        {
            if(area.sticky)
            {
                timeLimitedSpeedMagnifications.Add(new(area.speedMagnification, 1f));
                trail.startColor = Color.green;
                trail.endColor = Color.green;
            }
            else curInAreas.Add(area);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Area area))
        {
            if(area.sticky)
            {
                timeLimitedSpeedMagnifications.Add(new(area.speedMagnification, 1f));
                trail.startColor = Color.green;
                trail.endColor = Color.green;
            }
            else curInAreas.Remove(area);
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
        duplicated.Initialize(defaultDamage, defaultSpeed, penetrationNumber, criticalRate, direction);
        duplicated.IsFake = true;
        GameManager.Instance.StageManager.beads.Add(duplicated);

        return duplicated;
    }
}

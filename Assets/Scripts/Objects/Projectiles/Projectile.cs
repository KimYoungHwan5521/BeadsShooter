using UnityEngine;

public class Projectile : CustomObject
{
    Rigidbody2D rigid;
    [Tooltip("lookAngle : Right = 0, Up = -90, Down = 90, Left = 180")]
    [SerializeField] float lookAngle;
    [SerializeField] protected float damage;
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;
    [SerializeField] public float shotDelay;
    float curShotDelay;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.StageManager.projectiles.Add(this);
        curShotDelay = 0;
    }

    public override void MyUpdate(float deltaTime)
    {
        if (!gameObject.activeSelf) return;
        if(curShotDelay > shotDelay)
        {
            transform.position += deltaTime * speed * (Vector3)direction.normalized;

            if (direction.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + lookAngle);
            }
        }
        else
        {
            curShotDelay += deltaTime;
        }
    }

    public void SetProjectile(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

    public void SetDirection(Vector2 wantDirection)
    {
        direction = wantDirection;
    }
}

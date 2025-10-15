using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] int penetrationNumber;
    [SerializeField] float criticalRate;
    [SerializeField] Vector2 direction;
    public Vector2 Direction => direction;
    public bool activated;

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

    protected override void MyUpdate()
    {
        if (!activated) return;
        transform.position += (Vector3)direction.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated) return;
        if (collision.TryGetComponent(out Enemy enemy))
        {
            Vector2 normalVector = collision.ClosestPoint(transform.position) - (Vector2)transform.position;
            SetDirection(Vector2.Reflect(direction, normalVector.normalized));
            enemy.TakeDamage(damage);
            //PoolManager.Despawn(gameObject);
            //activated = false;
        }
        if (collision.TryGetComponent(out Bar bar))
        {
            SetDirection(transform.position - bar.transform.position);
        }
    }

    public void SetDirection(Vector2 wantDirection)
    {
        direction = wantDirection;
    }
}

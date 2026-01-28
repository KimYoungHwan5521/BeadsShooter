using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] protected float damage;
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;
    [SerializeField] public float shotDelay;
    float curShotDelay;

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

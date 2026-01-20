using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] protected float damage;
    [SerializeField] float speed;
    [SerializeField] Vector2 direction;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.StageManager.projectiles.Add(this);
    }

    public override void MyUpdate(float deltaTime)
    {
        if (!gameObject.activeSelf) return;
        transform.position += deltaTime * speed * (Vector3)direction.normalized;
    }

    public void SetDirection(Vector2 wantDirection)
    {
        direction = wantDirection;
    }
}

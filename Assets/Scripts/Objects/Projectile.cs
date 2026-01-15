using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float speed;

    private void OnEnable()
    {
        GameManager.Instance.StageManager.projectiles.Add(this);
    }

    protected override void MyUpdate(float deltaTime)
    {
        if (!gameObject.activeSelf) return;
        transform.position += speed * Vector3.down * deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BoundaryEdge _))
        {
            GameManager.Instance.StageManager.projectiles.Remove(this);
            PoolManager.Despawn(gameObject);
        }
        else
        {
            Bar bar = collision.GetComponentInParent<Bar>();
            if (bar != null)
            {
                GameManager.Instance.StageManager.projectiles.Remove(this);
                PoolManager.Despawn(gameObject);
                bar.Shrink(0.1f);
            }
        }
    }
}

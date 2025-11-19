using UnityEngine;

public class Projectile : CustomObject
{
    [SerializeField] float damage;
    [SerializeField] float speed;

    protected override void MyUpdate()
    {
        transform.position += speed * Vector3.down * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out BoundaryEdge boundary) || collision.TryGetComponent(out Bar bar))
        {
            PoolManager.Despawn(gameObject);
        }
    }
}

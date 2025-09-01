using UnityEngine;

public class Enemy : CustomObject
{
    [SerializeField] Vector2 moveDirection = Vector2.down;
    [SerializeField] float moveSpeed = 1f;
    
    protected override void MyUpdate()
    {
        transform.position += moveSpeed * Time.deltaTime * (Vector3)moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boderline"))
        {
            PoolManager.Despawn(gameObject);
        }
    }
}

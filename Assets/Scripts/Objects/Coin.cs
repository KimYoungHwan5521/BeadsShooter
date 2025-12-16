using UnityEngine;

public class Coin : CustomObject
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bar bar = collision.transform.GetComponentInParent<Bar>();
        if (bar != null)
        {
            GameManager.Instance.StageManager.Coin++;
            PoolManager.Despawn(gameObject);
        }
    }
}

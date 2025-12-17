using UnityEngine;

public class Coin : CustomObject
{
    public void BeCollected()
    {
        GameManager.Instance.StageManager.Coin++;
        PoolManager.Despawn(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bar bar = collision.transform.GetComponentInParent<Bar>();
        if (bar != null)
        {
            BeCollected();
        }
    }
}

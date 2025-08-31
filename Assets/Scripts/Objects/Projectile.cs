using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] int penetrationNumber;
    [SerializeField] float criticalRate;
    [SerializeField] Vector2 direction;
    bool initiated;

    public void Initialize(float damage, float speed, int penetrationNumber, float criticalRate, Vector2 direction)
    {
        this.damage = damage;
        this.speed = speed;
        this.penetrationNumber = penetrationNumber;
        this.criticalRate = criticalRate;
        this.direction = direction;
        initiated = true;
    }

    private void Update()
    {
        if (!initiated) return;
        transform.position += (Vector3)direction.normalized * speed;
    }
}

using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField] bool slow;
    [SerializeField] float slowMagnitude;
    [SerializeField] float slowTime;

    public override void MyUpdate(float deltaTime)
    {
        if (!gameObject.activeSelf) return;

        // ===== 핵심: direction을 쓰지 않고, 현재 velocity 기준으로만 제어 =====
        Vector2 v = rigid.linearVelocity;

        if (v.sqrMagnitude > 0.0001f)
        {
            // (1) 속도 크기만 유지, 방향은 물리(충돌)가 정한 방향 사용
            rigid.linearVelocity = v.normalized * speed;

            // (2) 머리 회전도 실제 이동 방향 기준
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + lookAngle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out BoundaryEdge boundary) && boundary.edgeType == 0 || collision.collider.TryGetComponent(out IceBlock _))
        {
            GameManager.Instance.StageManager.projectiles.Remove(this);
            PoolManager.Despawn(gameObject);
        }
        else if (collision.collider.CompareTag("Aura")) return;
        else
        {
            Bar bar = collision.collider.GetComponentInParent<Bar>();
            if (bar != null)
            {
                GameManager.Instance.StageManager.projectiles.Remove(this);
                PoolManager.Despawn(gameObject);
                bar.Shrink(0.1f);
                if (slow)
                {
                    bar.timeLimitedSpeedMagnification = slowMagnitude;
                    bar.timeLimitedSpeedMagnificationTime = slowTime;
                }
            }
        }
    }

    public override void SetDirection(Vector2 wantDirection)
    {
        direction = wantDirection;
        rigid.linearVelocity = direction;
    }
}

using UnityEngine;

public class MovableBlock : Block
{
    Rigidbody2D rigid;
    public enum MovePattern { Random, UpDown, LeftRight }
    public MovePattern movePattern;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 curMoveDirection;

    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        rigid.bodyType = RigidbodyType2D.Kinematic;
        GameManager.Instance.ObjectStart += () => { rigid.bodyType = RigidbodyType2D.Dynamic; };
    }

    protected override void MyStart()
    {
        if(movePattern == MovePattern.Random)
        {
            curMoveDirection = (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
        }
        else if(movePattern == MovePattern.UpDown)
        {
            curMoveDirection = Vector2.up;
        }
        else if( movePattern == MovePattern.LeftRight)
        {
            curMoveDirection = Vector2.right;
        }
        GameManager.Instance.ObjectStart += () => { rigid.bodyType = RigidbodyType2D.Dynamic; };
    }

    protected override void MyUpdate(float deltaTime)
    {
        if (IsDead) return;
        if (GameManager.Instance.phase != GameManager.Phase.BattlePhase) rigid.linearVelocity = Vector2.zero;
        else rigid.linearVelocity = curMoveDirection * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bead _)) return;

        if (movePattern == MovePattern.Random)
        {
            ChangeDirection();
        }
        else if (movePattern == MovePattern.UpDown)
        {
            if (Mathf.Abs(collision.contacts[0].normal.x) < Mathf.Abs(collision.contacts[0].normal.y))
                ChangeDirection();
        }
        else if (movePattern == MovePattern.LeftRight)
        {
            if (Mathf.Abs(collision.contacts[0].normal.x) > Mathf.Abs(collision.contacts[0].normal.y))
                ChangeDirection();
        }

    }

    void ChangeDirection()
    {
        curMoveDirection *= -1;
    }
}

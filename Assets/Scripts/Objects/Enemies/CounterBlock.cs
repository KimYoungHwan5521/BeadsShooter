using UnityEngine;

public class CounterBlock : Block
{
    [SerializeField] GameObject activatedSquare;
    BoxCollider2D col;
    //[SerializeField] bool counterActivated;
    [SerializeField] Projectile caughted;
    [SerializeField] float caughtTime = 1f;
    [SerializeField] float curCaughtTime;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //counterActivated = true;
        col.isTrigger = true;
        activatedSquare.SetActive(true);
    }

    protected override void MyUpdate()
    {
        if(caughted != null)
        {
            curCaughtTime += Time.deltaTime;
            if(curCaughtTime > caughtTime)
            {
                caughted.SetDirection(Vector2.down + Vector2.right * Random.Range(-1f, 1f));
                caughted.speedMagnification *= 1.3f;
                caughted.stop = false;
                curCaughtTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Projectile projectile))
        {
            caughted = projectile;
            caughted.transform.position = transform.position;
            caughted.stop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Projectile projectile) && projectile == caughted)
        {
            //counterActivated = false;
            col.isTrigger = false;
            activatedSquare.SetActive(false);
            caughted = null;
        }
    }
}

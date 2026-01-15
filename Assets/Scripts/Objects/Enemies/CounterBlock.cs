using UnityEngine;

public class CounterBlock : Block
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite activedSprite;
    [SerializeField] Sprite caughtSprite;
    [SerializeField] Sprite inactivedSprite;
    BoxCollider2D col;
    //[SerializeField] bool counterActivated;
    [SerializeField] Bead caughted;
    [SerializeField] float counterSpeed = 2f;
    [SerializeField] float caughtTime = 1f;
    [SerializeField] float curCaughtTime;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //counterActivated = true;
        col.isTrigger = true;
        spriteRenderer.sprite = activedSprite;
    }

    protected override void MyUpdate(float deltaTime)
    {
        if(caughted != null)
        {
            curCaughtTime += deltaTime;
            if(curCaughtTime > caughtTime)
            {
                spriteRenderer.sprite = inactivedSprite;
                caughted.SetDirection(Vector2.down + Vector2.right * Random.Range(-1f, 1f));
                caughted.temporarySpeedMagnification *= counterSpeed;
                caughted.stop = false;
                curCaughtTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Bead bead))
        {
            spriteRenderer.sprite = caughtSprite;
            caughted = bead;
            caughted.transform.position = transform.position;
            caughted.stop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Bead bead) && bead == caughted)
        {
            //counterActivated = false;
            col.isTrigger = false;
            caughted = null;
        }
    }
}

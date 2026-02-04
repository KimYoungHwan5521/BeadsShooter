using UnityEngine;

public class Boss1 : Enemy
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite activedSprite;
    [SerializeField] Sprite inactivedSprite;
    [SerializeField] GameObject shield;
    Collider2D col;
    [SerializeField] Bead caughted;
    [SerializeField] float counterSpeed = 2f;
    [SerializeField] float caughtTime = 1f;
    [SerializeField] float curCaughtTime;
    bool shieldTemporaryInactivated;
    [SerializeField] float temporaryInactiveTime = 1f;
    [SerializeField] float curTemporaryInactiveTime;
    float counterCool;
    [SerializeField] float attackCool;
    [SerializeField] float curAttackCool;

    public override float CurHP
    {
        get => curHP;
        set
        {
            base.CurHP = value;
            if (!shield.activeSelf && CurHP < maxHP * 0.5f) shield.SetActive(true);
            if(value < maxHP * counterCool)
            {
                counterCool -= 0.2f;
                col.isTrigger = true;
                spriteRenderer.sprite = activedSprite;
            }
        }
    }

    protected void Awake()
    {
        col = GetComponentInChildren<Collider2D>(true);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        col.isTrigger = true;
        spriteRenderer.sprite = activedSprite;
        curTemporaryInactiveTime = temporaryInactiveTime;
        shield.SetActive(false);
        counterCool = 0.8f;
        curAttackCool = 0;
    }

    public override void MyUpdateOnlyCurrentStage(float deltaTime)
    {
        if (!gameObject.activeSelf || GameManager.Instance.StageManager.bar.grabbedBeads.Count > 0) return;
        if (caughted != null)
        {
            curCaughtTime += deltaTime;
            if (curCaughtTime > caughtTime)
            {
                spriteRenderer.sprite = inactivedSprite;
                caughted.SetDirection(Vector2.down + Vector2.right * Random.Range(-1f, 1f));
                caughted.temporarySpeedMagnification *= counterSpeed;
                caughted.stop = false;
                curCaughtTime = 0;
                TemporaryShiledInactive();
            }
        }

        if(shieldTemporaryInactivated)
        {
            curTemporaryInactiveTime += deltaTime;
            if(curTemporaryInactiveTime > temporaryInactiveTime)
            {
                ShieldReactive();
            }
        }

        curAttackCool += deltaTime;
        if(curAttackCool > attackCool)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bead bead))
        {
            caughted = bead;
            caughted.transform.position = transform.position;
            caughted.stop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bead bead) && bead == caughted)
        {
            col.isTrigger = false;
            caughted = null;
        }
    }

    void TemporaryShiledInactive()
    {
        shieldTemporaryInactivated = true;
        curTemporaryInactiveTime = 0;
        Color currentColor = shield.GetComponent<SpriteRenderer>().color;
        currentColor.a = 0.5f;
        shield.GetComponent<SpriteRenderer>().color = currentColor;
        shield.GetComponent<BoxCollider2D>().enabled = false;
    }

    void ShieldReactive()
    {
        shieldTemporaryInactivated = false;
        Color currentColor = shield.GetComponent<SpriteRenderer>().color;
        currentColor.a = 1f;
        shield.GetComponent<SpriteRenderer>().color = currentColor;
        shield.GetComponent<BoxCollider2D>().enabled = true;
    }

    void Attack()
    {
        //int rand = Random.Range(-5, 6);
        //Vector3 spawnPos = new(transform.position.x + rand, transform.position.y);
        //Projectile projectile = PoolManager.Spawn(ResourceEnum.Prefab.NormalProjectile, spawnPos).GetComponent<Projectile>();
        
        for(int i=0; i<2; i++)
        {
            Projectile projectile = PoolManager.Spawn(ResourceEnum.Prefab.NormalProjectile, transform.position).GetComponent<Projectile>();
            Vector2 direction = new(Random.Range(-1f, 1f), -1);
            projectile.SetDirection(direction);
        }
        curAttackCool = 0;
    }
}

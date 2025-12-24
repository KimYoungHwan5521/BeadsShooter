using UnityEngine;

public class Boss1 : Enemy
{
    [SerializeField] GameObject activatedSquare;
    [SerializeField] GameObject shield;
    BoxCollider2D col;
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
                activatedSquare.SetActive(true);
            }
        }
    }

    protected void Awake()
    {
        col = GetComponentInChildren<BoxCollider2D>(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        col.isTrigger = true;
        activatedSquare.SetActive(true);
        curTemporaryInactiveTime = temporaryInactiveTime;
        shield.SetActive(false);
        counterCool = 0.8f;
        curAttackCool = 0;
    }

    protected override void MyUpdate()
    {
        if (caughted != null)
        {
            curCaughtTime += Time.deltaTime;
            if (curCaughtTime > caughtTime)
            {
                caughted.SetDirection(Vector2.down + Vector2.right * Random.Range(-1f, 1f));
                caughted.temporarySpeedMagnification *= counterSpeed;
                caughted.stop = false;
                curCaughtTime = 0;
                TemporaryShiledInactive();
            }
        }

        if(shieldTemporaryInactivated)
        {
            curTemporaryInactiveTime += Time.deltaTime;
            if(curTemporaryInactiveTime > temporaryInactiveTime)
            {
                ShieldReactive();
            }
        }

        curAttackCool += Time.deltaTime;
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
            activatedSquare.SetActive(false);
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
        int rand = Random.Range(-5, 6);
        Vector3 spawnPos = new(transform.position.x + rand, transform.position.y);
        PoolManager.Spawn(ResourceEnum.Prefab.NormalProjectile, spawnPos);
        curAttackCool = 0;
    }
}

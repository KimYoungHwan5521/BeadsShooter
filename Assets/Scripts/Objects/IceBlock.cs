using UnityEngine;

public class IceBlock : CustomObject
{
    public bool isActived;
    public float regenerateCool = 10f;
    [SerializeField] float curRegenerateCool;
    public int durability = 1;
    int curDurability = 1;
    int CurDurability
    {
        get => curDurability;
        set
        {
            curDurability = value;
            if(curDurability <= 0)
            {
                gameObject.SetActive(false);
                curDurability = durability;
                spawnIcicles = true;
            }
        }
    }
    public int icicleCount = 0;
    int curSpawnIcicles;
    bool spawnIcicles;
    const float icicleSpawnTerm = 0.5f;
    float curIcicleSpawnTerm = icicleSpawnTerm;

    public override void MyUpdate(float deltaTime)
    {
        if(spawnIcicles)
        {
            curIcicleSpawnTerm += deltaTime;
            if(curIcicleSpawnTerm >  icicleSpawnTerm)
            {
                curIcicleSpawnTerm = 0;
                curSpawnIcicles++;
                if(curSpawnIcicles > icicleCount)
                {
                    spawnIcicles = false;
                    curSpawnIcicles = 0;
                    curIcicleSpawnTerm = icicleSpawnTerm;
                    return;
                }
                Projectile icicle = PoolManager.Spawn(ResourceEnum.Prefab.Icicle, transform.position).GetComponent<Projectile>();
                icicle.SetDirection(Vector2.up);
            }
        }

        if(isActived && !gameObject.activeSelf)
        {
            curRegenerateCool += deltaTime;
            if(curRegenerateCool > regenerateCool)
            {
                gameObject.SetActive(true);
                curRegenerateCool = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent(out Bead bead))
        {
            CurDurability--;
        }
    }

    public void ResetObject()
    {
        gameObject.SetActive(isActived);
        curRegenerateCool = 0;
        CurDurability = durability;
        spawnIcicles = false;
    }
}

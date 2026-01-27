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
                for(int i=0; i<icicleCount; i++)
                {
                    Projectile icicle = PoolManager.Spawn(ResourceEnum.Prefab.Icicle, transform.position).GetComponent<Projectile>();
                    icicle.SetDirection(Vector2.up);
                    icicle.shotDelay = i * 0.5f;
                }
            }
        }
    }
    public int icicleCount = 0;

    public override void MyUpdate(float deltaTime)
    {
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
    }
}

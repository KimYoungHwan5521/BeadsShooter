using UnityEngine;

public class Block : Enemy
{
    SpriteRenderer sprite;
    [SerializeField] protected Transform crack;
    public SpriteMask spriteMask;
    public SpriteRenderer crackSprite;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        spriteMask = GetComponent<SpriteMask>();
        
        if(crack != null) crackSprite = crack.GetComponent<SpriteRenderer>();
    }

    public override float CurHP 
    { 
        get => base.CurHP; 
        set
        {
            base.CurHP = value;
            if (CurHP <= 0)
            {
                IsDead = true;
                return;
            }

            if (crack != null) 
            {
                crack.gameObject.SetActive(curHP != maxHP);
                crack.localScale = new(Mathf.Max(0.25f * (4 + curHP - maxHP), 0), Mathf.Max(0.25f * (4 + curHP - maxHP), 0));
            }
        }
    }

    public virtual void SetMaskLayer(int layerNumber)
    {
        if (spriteMask == null) return;
        spriteMask.frontSortingOrder = (layerNumber + 1) * 10;
        spriteMask.backSortingOrder = layerNumber * 10;
        crackSprite.sortingOrder = layerNumber * 10 + 1;
    }
}

using UnityEngine;

public class Block : Enemy
{
    SpriteRenderer sprite;
    [SerializeField] protected Transform crack;
    public SpriteRenderer crackSprite;

    protected override void Awake()
    {
        base.Awake();
        sprite = GetComponent<SpriteRenderer>();
        
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
                //Debug.Log($"crack local scale : {crack.localScale}");
            }
        }
    }

    public override void SetMaskLayer(int layerNumber)
    {
        base.SetMaskLayer(layerNumber);
        if (crackSprite == null) return;
        crackSprite.sortingOrder = layerNumber * 10 + 2;
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}

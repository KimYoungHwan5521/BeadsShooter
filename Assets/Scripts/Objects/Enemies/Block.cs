using UnityEngine;

public class Block : Enemy
{
    SpriteRenderer sprite;
    [SerializeField] Transform crack;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
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
                crack.localScale = new(curHP / maxHP, curHP / maxHP);
            }
        }
    }
}

using UnityEngine;

public class Block : Enemy
{
    SpriteRenderer sprite;

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

            if ((int)CurHP < ColorInfo.color.Length)
            {
                sprite.color = ColorInfo.color[(int)CurHP];
            }
            else sprite.color = ColorInfo.color[^1];
        }
    }
}

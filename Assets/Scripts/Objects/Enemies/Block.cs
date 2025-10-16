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

            if ((int)value < ColorInfo.color.Length && (int)value >= 0)
            {
                sprite.color = ColorInfo.color[(int)value];
            }
            else sprite.color = ColorInfo.color[^1];
        }
    }
}

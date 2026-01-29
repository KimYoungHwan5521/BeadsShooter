using System.Collections.Generic;
using UnityEngine;

public class Explosion : CustomObject
{
    SpriteRenderer spriteRenderer;
    Animator animator;

    float range;
    float damage;
    float burnDamage;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetExplosion(float range, float damage, float burnDamage = 0)
    {
        this.range = range;
        this.damage = damage;
        this.burnDamage = burnDamage;

        spriteRenderer.size = new(range * 2, range * 2);
    }

    public void Explode()
    {
        animator.SetTrigger("Explode");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, range);

        HashSet<Enemy> damaged = new HashSet<Enemy>();

        foreach (Collider2D col in cols)
        {
            Enemy enemy = col.GetComponentInParent<Enemy>();
            if (enemy != null && damaged.Add(enemy))
            {
                enemy.TakeDamage(damage);
                enemy.Burn(burnDamage);
            }
        }
    }
}

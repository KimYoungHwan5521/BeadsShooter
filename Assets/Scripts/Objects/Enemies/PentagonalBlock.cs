using UnityEngine;

public class PentagonalBlock : Block
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Projectile projectile))
        {
            anim.SetTrigger("Active");
        }
    }

    public void AddFeverGauge()
    {
        GameManager.Instance.StageManager.FeverGauge++;
    }
}

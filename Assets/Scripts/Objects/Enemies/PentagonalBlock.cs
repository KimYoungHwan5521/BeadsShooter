using UnityEngine;

public class PentagonalBlock : Block
{
    Animator anim;
    [SerializeField] Bead caughted;
    Vector2 caughtedsLastVector;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Bead projectile) && caughted == null && !projectile.IsFake)
        {
            anim.SetTrigger("Active");
            caughted = projectile;
            caughtedsLastVector = caughted.Direction;
            caughted.transform.position = transform.position;
            caughted.stop = true;
            caughted.spriteRenderer.enabled = false;

            if (GameManager.Instance.StageManager.currentStageEnemies.Contains(this)) GameManager.Instance.StageManager.currentStageEnemies.Remove(this);
            else if (GameManager.Instance.StageManager.nextStageEnemies.Contains(this)) GameManager.Instance.StageManager.nextStageEnemies.Remove(this);
            GameManager.Instance.StageManager.StageClearCheck();
        }
    }

    public void AddFeverGauge()
    {
        GameManager.Instance.StageManager.FeverGauge++;
    }

    public void Release()
    {
        caughted.spriteRenderer.enabled = true;
        caughted.stop = false;
        caughted.SetDirection(caughtedsLastVector);
        caughted = null;
    }
}

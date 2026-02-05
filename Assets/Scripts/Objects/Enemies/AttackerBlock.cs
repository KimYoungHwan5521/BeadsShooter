using UnityEngine;

public class AttackerBlock : Block
{
    ParticleSystem particle;
    [SerializeField] float attackCool;
    float curAttackCool;

    protected override void Awake()
    {
        base.Awake();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    override protected void OnEnable()
    {
        base.OnEnable();
        curAttackCool = 0;
        particle.Stop();
    }

    public override void MyUpdateOnlyCurrentStage(float deltaTime)
    {
        if (!gameObject.activeSelf || GameManager.Instance.StageManager.currentStage != stage)
        {
            particle.Stop();
            return;
        }
        if (GameManager.Instance.StageManager.bar.grabbedBeads.Count > 0) return;

        curAttackCool += deltaTime;
        if(attackCool - curAttackCool < 4f && !particle.isPlaying)
        {
            particle.Play();
        }
        if(curAttackCool > attackCool)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Projectile projectile = PoolManager.Spawn(ResourceEnum.Prefab.NormalProjectile, transform.position).GetComponent<Projectile>();
            projectile.SetDirection(Vector2.down);
            curAttackCool = 0;
        }
    }

    public void PlayParticle()
    {
        particle.Play();
    }
}

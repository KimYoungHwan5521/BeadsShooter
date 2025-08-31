using UnityEngine;

public class Barrel : CustomObject
{
    [SerializeField] Transform muzzle;
    [SerializeField] float shotCool;
    [SerializeField] float curShotCool;

    protected override void MyUpdate()
    {
        SpawnProjectiles();
    }

    void SpawnProjectiles()
    {
        curShotCool += Time.deltaTime;
        if(curShotCool > shotCool)
        {
            curShotCool = 0;
            GameObject projectile = PoolManager.Spawn(ResourceEnum.Prefab.NormalBullet, muzzle.transform.position);
            projectile.GetComponent<Projectile>().Initialize(10, 0.1f, 0, 0, muzzle.transform.position - transform.position);
        }
    }
}

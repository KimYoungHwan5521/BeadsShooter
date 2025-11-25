using UnityEngine;

public class DrippingBlock : MovableBlock
{
    [SerializeField] ResourceEnum.Prefab dripping;
    [SerializeField] float drippingCool;
    [SerializeField] float curDrippingCool;

    protected override void MyUpdate()
    {
        if(IsDead) return;
        base.MyUpdate();
        curDrippingCool += Time.deltaTime;
        if(curDrippingCool > drippingCool)
        {
            curDrippingCool = 0;
            PoolManager.Spawn(dripping, transform.position);
        }
    }
}

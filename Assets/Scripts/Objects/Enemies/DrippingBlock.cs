using UnityEngine;

public class DrippingBlock : MovableBlock
{
    [SerializeField] ResourceEnum.Prefab dripping;
    [SerializeField] float drippingCool;
    [SerializeField] float curDrippingCool;

    public override void MyUpdate(float deltaTime)
    {
        if(IsDead) return;
        base.MyUpdate(deltaTime);
        curDrippingCool += deltaTime;
        if(curDrippingCool > drippingCool)
        {
            curDrippingCool = 0;
            PoolManager.Spawn(dripping, transform.position);
        }
    }
}

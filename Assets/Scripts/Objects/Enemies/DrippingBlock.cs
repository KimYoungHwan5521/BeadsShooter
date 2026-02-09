using UnityEngine;

public class DrippingBlock : MovableBlock
{
    [SerializeField] ResourceEnum.Prefab dripping;
    [SerializeField] float drippingCool;
    [SerializeField] float curDrippingCool;
    public float slowRate = 1f;

    public override void MyUpdate(float deltaTime)
    {
        if(IsDead) return;
        base.MyUpdate(deltaTime);
        curDrippingCool += deltaTime;
        if(curDrippingCool > drippingCool)
        {
            curDrippingCool = 0;
            GameObject area = PoolManager.Spawn(dripping, transform.position);
            if(!GameManager.Instance.StageManager.areas.Contains(area)) GameManager.Instance.StageManager.areas.Add(area);
        }
    }
}

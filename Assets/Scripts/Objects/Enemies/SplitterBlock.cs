using UnityEngine;

public class SplitterBlock : Block
{
    [SerializeField] SplitBlock fullBody;
    [SerializeField] SplitBlock halfBody1;
    [SerializeField] SplitBlock halfBody2;
    bool isSplit;
    [SerializeField] float reunionTime;
    [SerializeField] float curReunionTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        isSplit = false;
        fullBody.gameObject.SetActive(true);
        curReunionTime = 0;
    }

    protected override void MyUpdate()
    {
        if(isSplit && !halfBody1.IsDead && !halfBody2.IsDead)
        {
            curReunionTime += Time.deltaTime;
            if(curReunionTime > reunionTime)
            {
                Reunion();
            }
        }
    }

    public void CheckIsDead()
    {
        IsDead = halfBody1.IsDead && halfBody2.IsDead;
    }

    public override void SetMaskLayer(int layerNumber)
    {
        fullBody.spriteMask.frontSortingOrder = (layerNumber + 1) * 10;
        fullBody.spriteMask.backSortingOrder = layerNumber * 10;
        fullBody.crackSprite.sortingOrder = layerNumber * 10 + 1;

        halfBody1.spriteMask.frontSortingOrder = (layerNumber + 1) * 10 - 5;
        halfBody1.spriteMask.backSortingOrder = layerNumber * 10;
        halfBody1.crackSprite.sortingOrder = layerNumber * 10 + 1;

        halfBody2.spriteMask.frontSortingOrder = (layerNumber + 1) * 10;
        halfBody2.spriteMask.backSortingOrder = layerNumber * 10 + 5;
        halfBody2.crackSprite.sortingOrder = layerNumber * 10 + 6;
    }

    public override void SetInfo(int stage, float maxHP)
    {
        base.SetInfo(stage, maxHP);
        fullBody.SetInfo(stage, maxHP);
    }

    public void Split()
    {
        fullBody.gameObject.SetActive(false);
        halfBody1.gameObject.SetActive(true);
        halfBody2.gameObject.SetActive(true);
        halfBody1.SetInfo(stage, maxHP / 2);
        halfBody2.SetInfo(stage, maxHP / 2);
        isSplit = true;
    }

    void Reunion()
    {
        isSplit = false;
        fullBody.gameObject.SetActive(true);
        halfBody1.gameObject.SetActive(false);
        halfBody2.gameObject.SetActive(false);
        fullBody.SetInfo(stage, halfBody1.CurHP + halfBody2.CurHP);
    }
}

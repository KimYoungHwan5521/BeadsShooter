using UnityEngine;

public class Boss2 : Enemy
{
    [SerializeField] Boss2Split fullBody;
    [SerializeField] Boss2Split halfBody1;
    [SerializeField] Boss2Split halfBody2;
    [SerializeField] Boss2Split quarterBody1;
    [SerializeField] Boss2Split quarterBody2;
    [SerializeField] Boss2Split quarterBody3;
    [SerializeField] Boss2Split quarterBody4;

    protected override void OnEnable()
    {
        base.OnEnable();
        fullBody.gameObject.SetActive(true);
        halfBody1.gameObject.SetActive(false);
        halfBody2.gameObject.SetActive(false);
        quarterBody1.gameObject.SetActive(false);
        quarterBody2.gameObject.SetActive(false);
        quarterBody3.gameObject.SetActive(false);
        quarterBody4.gameObject.SetActive(false);
    }

    public void CheckIsDead()
    {
        IsDead = quarterBody1.IsDead && quarterBody2.IsDead && quarterBody3.IsDead && quarterBody4.IsDead;
    }

    public override void SetInfo(int stage, float maxHP, bool isWall = false, bool isInvincible = false)
    {
        base.SetInfo(stage, maxHP);
        fullBody.SetInfo(stage, maxHP);
    }

    public void Split()
    {
        fullBody.gameObject.SetActive(false);
        halfBody1.transform.localPosition = new(-1, 0, 0);
        halfBody2.transform.localPosition = new(1, 0, 0);
        halfBody1.gameObject.SetActive(true);
        halfBody2.gameObject.SetActive(true);
        halfBody1.SetInfo(stage, 1, false, true);
        halfBody2.SetInfo(stage, 1, false, true);
    }
}

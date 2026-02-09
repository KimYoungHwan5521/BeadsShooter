using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy
{
    public Boss2Split fullBody;
    [SerializeField] Boss2Split halfBody1;
    [SerializeField] Boss2Split halfBody2;
    [SerializeField] Boss2Split quarterBody1;
    [SerializeField] Boss2Split quarterBody2;
    [SerializeField] Boss2Split quarterBody3;
    [SerializeField] Boss2Split quarterBody4;

    public override float CurHP
    {
        get => curHP;
        set
        {
            curHP = value;
            if (curHP <= 0) IsDead = true;
        }
    }

    public void SetHPBar()
    {
        if (hpBar != null) hpBar.fillAmount = (fullBody.CurHP + halfBody1.CurHP + halfBody2.CurHP + quarterBody1.CurHP + quarterBody2.CurHP + quarterBody3.CurHP + quarterBody4.CurHP) / (fullBody.MaxHP + halfBody1.MaxHP + halfBody2.MaxHP + quarterBody1.MaxHP + quarterBody2.MaxHP + quarterBody3.MaxHP + quarterBody4.MaxHP);
    }

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
        Debug.Log(stage);
        base.SetInfo(stage, maxHP);
        fullBody.SetInfo(stage, maxHP);
    }

    public void FullBodySplit()
    {
        fullBody.gameObject.SetActive(false);
        halfBody1.transform.localPosition = new(fullBody.transform.position.x - 1, 0, 0);
        halfBody2.transform.localPosition = new(fullBody.transform.position.x + 1, 0, 0);
        halfBody1.gameObject.SetActive(true);
        halfBody2.gameObject.SetActive(true);
        halfBody1.SetInfo(stage, maxHP / 2, false, false);
        halfBody2.SetInfo(stage, maxHP / 2, false, false);
        GameManager.Instance.ObjectUpdate -= halfBody1.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate += halfBody1.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate -= halfBody2.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate += halfBody2.MyUpdateOnlyCurrentStage;
    }

    public void HalfBody1Split()
    {
        halfBody1.gameObject.SetActive(false);
        quarterBody1.transform.localPosition = new(halfBody1.transform.position.x - 1, 0, 0);
        quarterBody2.transform.localPosition = new(halfBody1.transform.position.x + 1, 0, 0);
        quarterBody1.gameObject.SetActive(true);
        quarterBody2.gameObject.SetActive(true);
        quarterBody1.SetInfo(stage, maxHP / 4, false, false);
        quarterBody2.SetInfo(stage, maxHP / 4, false, false);
        GameManager.Instance.ObjectUpdate -= quarterBody1.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate += quarterBody1.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate -= quarterBody2.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate += quarterBody2.MyUpdateOnlyCurrentStage;
    }

    public void HalfBody2Split()
    {
        halfBody2.gameObject.SetActive(false);
        quarterBody3.transform.localPosition = new(halfBody2.transform.position.x - 1, 0, 0);
        quarterBody4.transform.localPosition = new(halfBody2.transform.position.x + 1, 0, 0);
        quarterBody3.gameObject.SetActive(true);
        quarterBody4.gameObject.SetActive(true);
        quarterBody3.SetInfo(stage, maxHP / 4, false, false);
        quarterBody4.SetInfo(stage, maxHP / 4, false, false);
        GameManager.Instance.ObjectUpdate -= quarterBody3.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate += quarterBody3.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate -= quarterBody4.MyUpdateOnlyCurrentStage;
        GameManager.Instance.ObjectUpdate += quarterBody4.MyUpdateOnlyCurrentStage;
    }

    public override void TakeDamage(float damage)
    {
        TakeDamage(damage, null);
    }

    public override void TakeDamage(float damage, Bead attaker)
    {
        this.attaker = attaker;
        if (!fullBody.IsDead)
        {
            fullBody.TakeDamage(damage, attaker);
        }
        else
        {
            List<Boss2Split> candidates = new();
            if (!halfBody1.IsDead) candidates.Add(halfBody1);
            if (!halfBody2.IsDead) candidates.Add(halfBody2);
            if (!quarterBody1.IsDead) candidates.Add(quarterBody1);
            if (!quarterBody2.IsDead) candidates.Add(quarterBody2);
            if (!quarterBody3.IsDead) candidates.Add(quarterBody3);
            if (!quarterBody4.IsDead) candidates.Add(quarterBody4);
            
            if (candidates.Count == 0)
            {
                targetPos = GameManager.Instance.StageManager.bar.transform.position;
                return;
            }
            int target = Random.Range(0, candidates.Count);
            candidates[target].TakeDamage(damage, attaker);
            targetPos = candidates[target].transform.position;
        }
    }

    Vector3 targetPos;
    public Vector3 GetTargetPos()
    {
        return targetPos;
    }
}

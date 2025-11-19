using UnityEngine;

public class SplitBlock : Block
{
    [SerializeField] SplitterBlock owner;
    // 0 : fullBody, 1 : halfBody1, 2 : halfBody2
    [SerializeField] int part;

    public override float CurHP 
    { 
        get => base.CurHP; 
        set
        {
            curHP = value;
            if(curHP <= 0)
            {
                if (part == 0)
                {
                    owner.Split();
                }
                else
                {
                    isDead = true;
                    GameManager.Instance.StageManager.StageClearCheck();
                    gameObject.SetActive(false);
                }
                if (crack != null)
                {
                    crack.gameObject.SetActive(curHP != maxHP);
                    crack.localScale = new(Mathf.Max(0.25f * (4 + curHP - maxHP), 0), Mathf.Max(0.25f * (4 + curHP - maxHP), 0));
                }
            }
        }
    }
}

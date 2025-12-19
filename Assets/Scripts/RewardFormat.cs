using UnityEngine;

public enum RewardType { None, AttackDamage, PoisonDamage, AreaDamage, Reroll, Life }

public class RewardFormat
{
    public RewardType rewardType;
    public float value;
    public Vector2 area;
    public string Explain
    {
        get
        {
            return rewardType switch
            {
                RewardType.None => "",
                RewardType.AttackDamage => $"Ball's attack damage + {value}",
                RewardType.PoisonDamage => $"Blocks hit by the ball take {value} damage per second.",
                RewardType.AreaDamage => $"When damaging a block, deals {value} damage to the surrounding {area.x}x{area.y} area.",
                RewardType.Reroll => $"Reroll + {value}.",
                RewardType.Life => $"Life + {value}.",
                _ => ""
            };
        }
    }


    public RewardFormat(RewardType rewardType, float value)
    {
        this.rewardType = rewardType;
        this.value = value;
    }

    public RewardFormat(RewardType rewardType, float value, Vector2 area)
    {
        this.rewardType = rewardType;
        this.value = value;
        this.area = area;
    }
}

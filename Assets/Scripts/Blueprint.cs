using UnityEngine;

public enum RewardType { NotValid, AttackDamage, PoisonDamage, AreaDamage }

public class RewardFormat
{
    public RewardType rewardType;
    public float value;
    public Vector2 area;

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

public class Blueprint
{
    public int[,] blueprint;
    public RewardFormat reward;

    public Blueprint(int[,] blueprint, RewardFormat reward)
    {
        this.blueprint = blueprint;
        this.reward = reward;
    }
}

public static class MaterialsColor
{
    public static Color[] colors = { new(1, 1, 1, 0), new(1, 0, 0), new(0, 1, 0), new(0, 0, 1), new(1, 1, 0), new(1, 0, 1), new(0, 1, 1), new(0, 0, 0) };
}
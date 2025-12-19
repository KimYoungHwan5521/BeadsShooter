using UnityEngine;

public class Blueprint
{
    public static int ColumnCount = 3;
    public static int RowCount = 3;
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
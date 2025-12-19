using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager
{
    public static List<Blueprint> blueprints = new();
    
    public IEnumerator Initiate()
    {
        blueprints.Add(new Blueprint(new int[,] { { 1, 2 }, { 0, 3 } }, new(RewardType.AttackDamage, 1f)));
        blueprints.Add(new Blueprint(new int[,] { { 2, 2, 2 } }, new(RewardType.PoisonDamage, 0.5f)));
        blueprints.Add(new Blueprint(new int[,] { { 1, 1, 4 }, { 2, 2, 0 } }, new(RewardType.AreaDamage, 1f, new Vector2(4, 4))));
        
        yield return null;
    }
}

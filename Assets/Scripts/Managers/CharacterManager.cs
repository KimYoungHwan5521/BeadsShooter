using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager
{
    public List<CharacterData> characters = new();
    public IEnumerator Initiate()
    {
        List<Blueprint> blueprints = new() 
        {
            new Blueprint(new int[,] { { 1, 2 }, { 0, 3 } }, new(RewardType.AttackDamage, 1f)),
            new Blueprint(new int[,] { { 2, 2, 2 } }, new(RewardType.PoisonDamage, 0.5f)),
            new Blueprint(new int[,] { { 1, 1, 5 }, { 2, 2, 0 } }, new(RewardType.AreaDamage, 1f, new Vector2(4, 4))),
        };
        characters.Add(new("Ball Handler", 20, blueprints));
        yield return null;
    }
}

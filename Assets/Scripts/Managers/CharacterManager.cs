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
            BlueprintManager.blueprints[0],
            BlueprintManager.blueprints[1],
            BlueprintManager.blueprints[2],
        };
        characters.Add(new("Laser man", 20, blueprints, FeverManager.FeverName.Laser));
        yield return null;
    }
}

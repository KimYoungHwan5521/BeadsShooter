using System.Collections.Generic;
using UnityEngine.Events;

public class CharacterData
{
    public string characterName;
    public float moveSpeed;
    public List<Blueprint> blueprints = new();
    public FeverAction fever;

    public CharacterData(string characterName, float moveSpeed, List<Blueprint> blueprints, FeverManager.FeverName fever)
    {
        this.characterName = characterName;
        this.moveSpeed = moveSpeed;
        this.blueprints = blueprints;
        this.fever = GameManager.Instance.FeverManager.Fevers[fever];
    }
}

using System.Collections.Generic;

public class CharacterData
{
    public string characterName;
    public float moveSpeed;
    public List<Blueprint> blueprints = new();

    public CharacterData(string characterName, float moveSpeed, List<Blueprint> blueprints)
    {
        this.characterName = characterName;
        this.moveSpeed = moveSpeed;
        this.blueprints = blueprints;
    }
}

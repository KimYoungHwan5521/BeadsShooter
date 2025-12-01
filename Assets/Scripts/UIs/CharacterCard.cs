using TMPro;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Sprite characterSprite;
    [SerializeField] BlueprintDrawer[] blueprintDrawers;
    [SerializeField] TextMeshProUGUI detailText;

    public void SetInfo(CharacterData character)
    {
        characterNameText.text = character.characterName;
        for(int i=0; i< blueprintDrawers.Length; i++)
        {
            blueprintDrawers[i].SetBlueprint(character.blueprints[i]);
        }
    }

    public void SetInfoDetail(CharacterData character)
    {
        SetInfo(character);
        detailText.text = $"Move speed : {character.moveSpeed:0}";
    }
}

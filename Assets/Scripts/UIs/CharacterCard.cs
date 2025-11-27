using TMPro;
using UnityEngine;

public class CharacterCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Sprite characterSprite;
    [SerializeField] TextMeshProUGUI detailText;

    public void SetInfo(string characterName)
    {
        characterNameText.text = characterName;
    }

    public void SetInfo(string characterName, float moveSpeed)
    {
        characterNameText.text = characterName;
        detailText.text = $"{moveSpeed:0}";
    }
}

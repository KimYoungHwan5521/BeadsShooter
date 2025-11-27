using UnityEngine;
using System.Collections.Generic;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] List<CharacterCard> characterCards;
    [SerializeField] CharacterCard largeCharacterCard;
    private void Start()
    {
        GameManager.Instance.ManagerStart += CharacterSetting;
    }

    void CharacterSetting()
    {
        List<CharacterData> charactersData = GameManager.Instance.CharacterManager.characters;
        for(int i=0; i<characterCards.Count; i++)
        {
            if(i < charactersData.Count)
            {
                characterCards[i].gameObject.SetActive(true);
                characterCards[i].SetInfo(charactersData[i].characterName);
            }
            else
            {
                characterCards[i].gameObject.SetActive(false);
            }
        }
    }

    int currentSelectedCharacterIndex;
    public void OpenLargeCharacterCard(int index)
    {
        List<CharacterData> charactersData = GameManager.Instance.CharacterManager.characters;
        largeCharacterCard.SetInfo(charactersData[index].characterName, charactersData[index].moveSpeed);
        currentSelectedCharacterIndex = index;
    }

    public void Select()
    {
        GameManager.Instance.StageManager.bar.SetBar(GameManager.Instance.CharacterManager.characters[currentSelectedCharacterIndex]);
        GameManager.Instance.StartBattlePhase();
    }
}

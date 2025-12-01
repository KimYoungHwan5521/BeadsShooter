using UnityEngine;
using System.Collections.Generic;

public class SelectCharacter : MonoBehaviour
{
    [SerializeField] List<CharacterCard> characterCards;
    [SerializeField] CharacterCard largeCharacterCard;
    bool showingBlueprintDetail;

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
                characterCards[i].SetInfo(charactersData[i]);
            }
            else
            {
                characterCards[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetShowBlueprint(bool value)
    {
        showingBlueprintDetail = value;
    }

    int currentSelectedCharacterIndex;
    public void OpenLargeCharacterCard(int index)
    {
        if (showingBlueprintDetail)
        {
            showingBlueprintDetail = false;
            return;
        }
        List<CharacterData> charactersData = GameManager.Instance.CharacterManager.characters;
        largeCharacterCard.gameObject.SetActive(true);
        largeCharacterCard.SetInfoDetail(charactersData[index]);
        currentSelectedCharacterIndex = index;
    }

    public void Select()
    {
        GameManager.Instance.StageManager.bar.SetBar(GameManager.Instance.CharacterManager.characters[currentSelectedCharacterIndex]);
        GameManager.Instance.StartBattlePhase();
    }
}

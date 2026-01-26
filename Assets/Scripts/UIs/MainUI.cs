using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public enum Tab { Market, Upgrade, Stage, Character, Setting }

    [Header("Main")]
    [SerializeField] GameObject market;
    [SerializeField] GameObject upgrade;
    [SerializeField] GameObject stage;
    [SerializeField] GameObject character;
    [SerializeField] GameObject setting;

    [Header("Stage")]
    int currentStageIndex;
    [SerializeField] Image stageCard;
    [SerializeField] Image stageBoss;
    [SerializeField] Sprite[] stageCards;
    [SerializeField] Sprite[] stageBosses;
    [SerializeField] TextMeshProUGUI stageName;
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;

    Tab selectedTab;
    public Tab SelectedTab
    {
        get => selectedTab;
        set
        {
            selectedTab = value;
            market.SetActive(value == Tab.Market);
            upgrade.SetActive(value == Tab.Upgrade);
            stage.SetActive(value == Tab.Stage);
            character.SetActive(value == Tab.Character);
            setting.SetActive(value == Tab.Setting);
        }
    }

    private void Start()
    {
        GameManager.Instance.ManagerStart += () => SetCurrentStage(0);
    }

    public void SetCurrentStage(int index)
    {
        if(index == 0) currentStageIndex = 0;
        else currentStageIndex += index;

        stageName.text = $"{((StageManager.Stage)currentStageIndex)} + Stage";
        stageCard.sprite = stageCards[currentStageIndex];
        stageBoss.sprite = stageBosses[currentStageIndex];
        leftArrow.interactable = currentStageIndex > 0;
        rightArrow.interactable = currentStageIndex < GameManager.Instance.StageManager.stages.Count - 1;
    }

    public void SelectStage()
    {
        StartCoroutine(nameof(ISelect));
        GameManager.Instance.mainUI.SetActive(false);
    }

    IEnumerator ISelect()
    {
        GameManager.ClaimLoadInfo("Setting stage");
        GameManager.Instance.StageManager.Initiate(currentStageIndex);
        GameManager.Instance.StageManager.bar.SetBar(GameManager.Instance.CharacterManager.characters[GameManager.Instance.StageManager.currentSelectedCharacterIndex]);
        GameManager.Instance.StageManager.currentStage = 0;
        GameManager.Instance.StartBattlePhase();
        GameManager.CloseLoadInfo();
        yield return null;
    }
}

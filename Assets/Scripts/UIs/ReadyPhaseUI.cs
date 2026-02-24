using System;
using System.Collections.Generic;
using UnityEngine;

public class ReadyPhaseUI : MonoBehaviour
{
    //const float promotionAppearRate = 0.15f;
    const float epicAppearRate = 0.1f;
    const float passiveAppearRate = 0.3f;
    [SerializeField] GameObject startNextStage;

    [Header("Select Options")]
    [SerializeField] AbilityOption[] abilityOptions;
    int choiceCount;

    bool isShop;
    public bool IsShop => isShop;

    private void Start()
    {
        GameManager.Instance.ManagerStart += () => {  };
    }

    public void SetReadyPhase(int choiceCount)
    {
        this.choiceCount = choiceCount;
        SetOptions();
    }

    public void StartNextStage()
    {
        if(isShop)
        {
            GameManager.Instance.readyPhaseWindow.SetActive(false);
        }
        else
        {
            GameManager.Instance.StartBattlePhase();
        }
    }
    
    //public void HideOrShowRewardOptions()
    //{
    //    rewardOptionsBox.SetActive(!rewardOptionsBox.activeSelf);
    //}

    void SetOptions()
    {
        StageManager stageManager = GameManager.Instance.StageManager;
        //foreach(RewardOption rewardOption in rewardOptions)
        //{
        //    rewardOption.SetOption("", UnityEngine.Random.Range(1, 5), GameManager.Instance.StageManager.GetRandomeReward());
        //}
        int a, b, c;
        a = b = c = 0;

        float rand = UnityEngine.Random.Range(0, 1f);
        // cardType : 0 = Rare, 1 = Epic, 2 = Promotion
        int cardType = 0;
        if (rand < epicAppearRate) cardType = 1;
        //else if (rand < epicAppearRate + promotionAppearRate) cardType = 2;
        bool isPassive = UnityEngine.Random.Range(0, 1f) < passiveAppearRate;

        List<AbilityManager.Ability> pool = new();
        // 진급가능한 카드 있으면 무조건 진급하나 포함
        pool = stageManager.possibleToAppearAbilities.FindAll(x => x.cardType == AbilityManager.CardType.Promotion);
        if (pool.Count == 0)
        {
            // 획득한 어빌리티가 세종류 이상이라면 한장은 기존에 가지고 있는 어빌리티중 확정
            if(stageManager.selectedRootAbilities.Count >= 3) pool = stageManager.possibleToAppearAbilities.FindAll(x => (int)x.cardType == cardType && x.isPassive == isPassive && stageManager.selectedRootAbilities.Find(y => y.abilityName == x.rootAbility) != null);
            else pool = stageManager.possibleToAppearAbilities.FindAll(x => (int)x.cardType == cardType && x.isPassive == isPassive);
        }
        AbilityManager.Ability ability = pool[UnityEngine.Random.Range(0, pool.Count)];
        a = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);

        for (int i = 0; i < 1000; i++)
        {
            isPassive = UnityEngine.Random.Range(0, 1f) < passiveAppearRate;
            pool = stageManager.possibleToAppearAbilities.FindAll(x => (int)x.cardType == cardType && x.isPassive == isPassive);
            ability = pool[UnityEngine.Random.Range(0, pool.Count)];
            b = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);

            if (b != a) break;
        }
        for (int i = 0; i < 1000; i++)
        {
            isPassive = UnityEngine.Random.Range(0, 1f) < passiveAppearRate;
            pool = stageManager.possibleToAppearAbilities.FindAll(x => x.isPassive == isPassive);
            ability = pool[UnityEngine.Random.Range(0, pool.Count)];
            c = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);

            if (c != a && c != b) break;
        }

        abilityOptions[0].SetOption(stageManager.possibleToAppearAbilities[a]);
        abilityOptions[1].SetOption(stageManager.possibleToAppearAbilities[b]);
        abilityOptions[2].SetOption(stageManager.possibleToAppearAbilities[c]);
    }

    public void SelectAbility(int index)
    {
        AbilityManager.Ability selectedAbility = abilityOptions[index].linkedAbility;
        GameManager.Instance.StageManager.GetAbility(selectedAbility);
        choiceCount--;
        if (choiceCount == 0) StartNextStage();
        else SetOptions();
    }

}

using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public MerchandiseOption[] merchandiseOptions;
    public void SetShop()
    {
        StageManager stageManager = GameManager.Instance.StageManager;
        foreach(MerchandiseOption option in merchandiseOptions)
        {
            option.gameObject.SetActive(true);
            option.SoldOut = false;
        }

        List<int> checkDup = new();
        int merchant0;
        int merchant1;
        int merchant2;
        int merchant3;
        int merchant4;
        int merchant5;
        float rand = UnityEngine.Random.Range(0, 1f);
        // cardType : 0 = Rare, 1 = Epic, 2 = Promotion
        int cardType = 2;
        bool isPassive = false;
        if (rand < ReadyPhaseUI.epicAppearRate) cardType = 1;

        List<AbilityManager.Ability> pool = new();
        AbilityManager.Ability ability;
        // merchant0 : 진급카드, 진급가능한 카드 없으면 null
        pool = stageManager.possibleToAppearAbilities.FindAll(x => x.cardType == AbilityManager.CardType.Promotion);
        if (pool.Count == 0)
        {
            // 획득한 어빌리티가 세종류 이상이라면 한장은 기존에 가지고 있는 어빌리티중 확정
            if (stageManager.selectedRootAbilities.Count >= 3) pool = stageManager.possibleToAppearAbilities.FindAll(x => (int)x.cardType == cardType && x.isPassive == isPassive && stageManager.selectedRootAbilities.Find(y => y.abilityName == x.rootAbility) != null);
            else pool = stageManager.possibleToAppearAbilities.FindAll(x => (int)x.cardType == cardType && x.isPassive == false);
        }
        if (pool.Count == 0) merchandiseOptions[0].SetOption(null, 0);
        else
        {
            ability = pool[UnityEngine.Random.Range(0, pool.Count)];
            merchant0 = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);
            merchandiseOptions[0].SetOption(ability, 150);
            checkDup.Add(merchant0);
        }

        // merchant1, 2 : 업그레이드 카드
        for (int i = 0; i < 1000; i++)
        {
            cardType = UnityEngine.Random.Range(0, 1f) < ReadyPhaseUI.epicAppearRate ? 1 : 0;
            isPassive = UnityEngine.Random.Range(0, 1f) < ReadyPhaseUI.passiveAppearRate;
            pool = stageManager.possibleToAppearAbilities.FindAll(x => (int)x.cardType == cardType && x.isPassive == isPassive && x.rootAbility != x.name);
            ability = pool[UnityEngine.Random.Range(0, pool.Count)];
            merchant1 = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);
            if (merchant1 == -1 || checkDup.Contains(merchant1)) continue;
            else
            {
                merchandiseOptions[1].SetOption(ability, cardType == 0 ? 100 : 200);
                checkDup.Add(merchant1);
            }
        }
        for (int i = 0; i < 1000; i++)
        {
            cardType = UnityEngine.Random.Range(0, 1f) < ReadyPhaseUI.epicAppearRate ? 1 : 0;
            isPassive = UnityEngine.Random.Range(0, 1f) < ReadyPhaseUI.passiveAppearRate;
            pool = stageManager.possibleToAppearAbilities.FindAll(x => (int)x.cardType == cardType && x.isPassive == isPassive && x.rootAbility != x.name);
            ability = pool[UnityEngine.Random.Range(0, pool.Count)];
            merchant2 = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);
            if (merchant2 == -1 || checkDup.Contains(merchant2)) continue;
            else
            {
                merchandiseOptions[2].SetOption(ability, cardType == 0 ? 100 : 200);
                checkDup.Add(merchant2);
            }
        }
        // merchant3, 4, 5 : 새 어빌리티, 3=에픽/4,5=레어
        pool = stageManager.possibleToAppearAbilities.FindAll(x => x.cardType == AbilityManager.CardType.Epic && x.isPassive == false && x.rootAbility == x.name);
        if(pool.Count == 0) merchandiseOptions[3].SetOption(null, 0);
        else
        {
            ability = pool[UnityEngine.Random.Range(0, pool.Count)];
            merchant3 = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);
            merchandiseOptions[3].SetOption(ability, 200);
            checkDup.Add(merchant3);
        }

        pool = stageManager.possibleToAppearAbilities.FindAll(x => x.cardType == AbilityManager.CardType.Rare && x.isPassive == false && x.rootAbility == x.name);
        ability = pool[UnityEngine.Random.Range(0, pool.Count)];
        merchant4 = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);
        if (merchant4 == -1) merchandiseOptions[4].SetOption(null, 0);
        else
        {
            merchandiseOptions[4].SetOption(ability, 100);
            checkDup.Add(merchant4);
        }

        for(int i=0; i<1000; i++)
        {
            pool = stageManager.possibleToAppearAbilities.FindAll(x => x.cardType == AbilityManager.CardType.Rare && x.isPassive == false && x.rootAbility == x.name);
            if(pool.Count == 0)
            {
                merchandiseOptions[4].SetOption(null, 0);
                break;
            }
            ability = pool[UnityEngine.Random.Range(0, pool.Count)];
            merchant5 = stageManager.possibleToAppearAbilities.FindIndex(x => x.name == ability.name);
            if (merchant5 == -1 || checkDup.Contains(merchant5)) continue;
            else
            {
                merchandiseOptions[4].SetOption(ability, 100);
                checkDup.Add(merchant4);
            }
        }
    }

    public void Reroll()
    {
        if (GameManager.Instance.StageManager.Coin < GameManager.Instance.StageManager.RerollCost) return;
        GameManager.Instance.StageManager.Coin -= GameManager.Instance.StageManager.RerollCost;
        SetShop();
    }

    public void ExitShop()
    {
        GameManager.Instance.StageManager.currentStage++;
        GameManager.Instance.readyPhaseUI.GetComponent<ReadyPhaseUI>().StartNextStage();
    }
}

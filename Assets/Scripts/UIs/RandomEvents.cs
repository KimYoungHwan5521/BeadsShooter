using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomEvents : MonoBehaviour
{
    public enum EventType { Quest, Rest, Merchant, GetRandomStat, }
    public EventType type;

    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] MerchandiseOption merchandiseOption;
    List<RewardFormat> rewards;

    QuestManager.Quest quest;

    private void OnEnable()
    {
        switch(type)
        {
            case EventType.Quest:
                SetRandomQuest();
                break;
            case EventType.Rest:
                Rest();
                break;
            case EventType.Merchant:
                SetRandomMerchandise();
                break;
            case EventType.GetRandomStat:
                break;
        }
    }

    void SetRandomQuest()
    {
        quest = GameManager.Instance.QuestManager.GetRandomQuest();
        questText.text = quest.Explain;
        string rewardExplain = "";
        for(int i = 0; i < quest.rewards.Count; i++)
        {
            if(i != 0) rewardExplain += ", ";
            rewardExplain += quest.rewards[i].Explain;
        }
    }

    void Rest()
    {
        GameManager.Instance.StageManager.bar.BarLength = 1;
        GameManager.Instance.StageManager.Life++;
        questText.text = "Bar length has recovered, Life + 1";
    }

    void SetRandomMerchandise()
    {
        ShopManager.MerchandiseInfo merchandise = GameManager.Instance.ShopManager.rareMerchandises[Random.Range(0, GameManager.Instance.ShopManager.rareMerchandises.Count)];
        merchandiseOption.SetOption(merchandise);
        merchandiseOption.Soldout = false;
    }

    public void AcceptQuest()
    {
        GameManager.Instance.StageManager.AddQuest(quest);
    }

    public void ConfirmOrDecline()
    {
        gameObject.SetActive(false);
        GameManager.Instance.Shop.ExitShop();
    }
}

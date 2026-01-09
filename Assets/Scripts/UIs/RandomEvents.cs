using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomEvents : MonoBehaviour
{
    public enum EventType { Quest, Rest, Merchant, GetRandomStat, }
    public EventType type;

    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] MerchandiseOption merchandiseOption;
    [SerializeField] Button rerollButton;
    List<RewardFormat> rewards = new();

    QuestManager.Quest quest;

    private void OnEnable()
    {
        rewards.Clear();
        switch (type)
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
                SetRandomRewards();
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
        rewardText.text = rewardExplain;
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

    void SetRandomRewards()
    {
        questText.text = "You can sacrifice 0.1 bar length to gain 3 random stat rewards.";
        bool interactable = GameManager.Instance.StageManager.bar.BarLength >= GameManager.Instance.StageManager.bar.BarMinLength + 0.1;
        rerollButton.interactable = interactable;
    }

    public void RerollRewards()
    {
        GameManager.Instance.StageManager.bar.Shrink(0.1f);
        rewards.Clear();
        rewards.Add(GameManager.Instance.StageManager.GetRandomeReward());
        rewards.Add(GameManager.Instance.StageManager.GetRandomeReward());
        rewards.Add(GameManager.Instance.StageManager.GetRandomeReward());

        string rewardExplain = "";
        for (int i = 0; i < rewards.Count; i++)
        {
            if (i != 0) rewardExplain += ", ";
            rewardExplain += rewards[i].Explain;
        }
        questText.text = rewardExplain;
    }

    public void AcceptQuest()
    {
        GameManager.Instance.StageManager.AddQuest(quest);
        GameManager.Instance.Shop.ExitShop();
    }

    public void ConfirmOrDecline()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StageManager.ApplyRewards(rewards);
        GameManager.Instance.Shop.ExitShop();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    public enum Condition { Strike }

    public class Quest
    {
        public Condition conditionType;
        public int conditionQuantaty;
        public List<RewardFormat> rewards; 

        public string Explain
        {
            get
            {
                return conditionType switch
                {
                    Condition.Strike => $"Destroy {conditionQuantaty} blocks with one shot",
                    _ => "!Wrong quest condition!"
                };
            }
        }

        public Quest(Condition conditionType, int conditionQuantaty, List<RewardFormat> rewards)
        {
            this.conditionType = conditionType;
            this.conditionQuantaty = conditionQuantaty;
            this.rewards = rewards;
        }
    }
    public List<Quest> quests;

    public IEnumerator Initiate()
    {
        quests = new List<Quest>()
        {
            new(Condition.Strike, 5, new(){ new(RewardType.AttackDamage, 0.2f) }),
        };
        yield return null;
    }

    public Quest GetRandomQuest()
    {
        return quests[Random.Range(0, quests.Count)];
    }
}

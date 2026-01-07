using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    const int merchandiseCount = 4;
    public MerchandiseOption[] merchandiseOptions;

    public void SetShop()
    {
        List<int> checkDup = new();
        int checkLoop = 0;
        for(int i = 0; i < merchandiseCount; i++)
        {
            int rand = Random.Range(0, GameManager.Instance.ShopManager.merchandises.Count);
            if(checkDup.Contains(rand))
            {
                if(++checkLoop > 1000)
                {
                    Debug.LogWarning("Infinity loop has detected!");
                }
                else
                {
                    i--;
                    continue;
                }
            }
            merchandiseOptions[i].SetOption(GameManager.Instance.ShopManager.merchandises[rand]);
            checkDup.Add(rand);
            merchandiseOptions[i].Soldout = false;
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
        Time.timeScale = 0;
        GameManager.Instance.StageManager.currentStage++;
        GameManager.Instance.phase = GameManager.Phase.ReadyPhase;
        foreach (var shop in GameManager.Instance.StageManager.currentStageEnemies)
        {
            PoolManager.Despawn(shop.gameObject);
        }
        GameManager.Instance.StageManager.currentStageEnemies.Clear();
    }
}

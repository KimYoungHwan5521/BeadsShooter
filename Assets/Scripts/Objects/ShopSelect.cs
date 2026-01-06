using UnityEngine;

public class ShopSelect : MonoBehaviour
{
    // Type : 0 - Shop, 1 - Rest, 2 - Random Event
    [SerializeField] int type;

    void Rest()
    {
        GameManager.Instance.StageManager.bar.BarLength = 1;
        GameManager.Instance.readyPhaseWindow.SetActive(true);
        GameManager.Instance.readyPhaseUI.SetDisplace();
    }

    void RandomEvent()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(type == 0)
        {
            GameManager.Instance.OpenShop();
        }
        else if(type == 1)
        {
            Rest();
        }
        else
        {
            RandomEvent();
            return;
        }
        Time.timeScale = 0;
        GameManager.Instance.StageManager.currentStage++;
        GameManager.Instance.phase = GameManager.Phase.ReadyPhase;
        foreach(var shop in GameManager.Instance.StageManager.currentStageEnemies)
        {
            PoolManager.Despawn(shop.gameObject);
        }
        GameManager.Instance.StageManager.currentStageEnemies.Clear();
    }
}

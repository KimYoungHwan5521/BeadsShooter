using UnityEngine;

public class RestOptions : MonoBehaviour
{
    public void SelectOption(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Instance.StageManager.bar.BarLength = 1;
                break;
            case 1:
                GameManager.Instance.StageManager.Life++;
                break;
            case 2:
                GameManager.Instance.readyPhaseUI.SetDisplace();
                break;
            default:
                break;
        }
        GameManager.Instance.restOptions.SetActive(false);
        GameManager.Instance.Shop.ExitShop();
    }
}
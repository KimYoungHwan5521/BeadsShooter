using UnityEngine;

public class ShopSelect : MonoBehaviour
{
    // Type : 0 - Shop, 1 - Rest, 2 - Random Event
    [SerializeField] int type;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Time.timeScale = 0;
        if(type == 0)
        {
            GameManager.Instance.OpenShop();
        }
        else if(type == 1)
        {
            GameManager.Instance.restOptions.SetActive(true);
            return;
        }
        else
        {
            GameManager.Instance.RandomEvent();
            return;
        }
        GameManager.Instance.Shop.ExitShop();
    }
}
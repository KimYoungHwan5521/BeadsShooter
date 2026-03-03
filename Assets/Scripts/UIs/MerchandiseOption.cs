using TMPro;
using UnityEngine;

public class MerchandiseOption : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] AbilityOption abilityOption;
    [SerializeField] TextMeshProUGUI priceText;
    int price;
    [SerializeField] GameObject soldOutObject;
    bool soldOut;
    public bool SoldOut
    {
        get => soldOut;
        set
        {
            soldOut = value;
            soldOutObject.SetActive(value);
        }
    }

    public void SetOption(AbilityManager.Ability ability, int price)
    {
        if(ability == null)
        {
            gameObject.SetActive(false);
            return;
        }
        abilityOption.SetOption(ability);
        priceText.text = price.ToString();
        this.price = price;
        priceText.color = GameManager.Instance.StageManager.Coin < this.price ? Color.red : Color.black;
    }

    public void Buy()
    {
        if (GameManager.Instance.StageManager.Coin < price) return;
        GameManager.Instance.StageManager.Coin -= price;
        foreach(MerchandiseOption merchandise in shop.merchandiseOptions)
        {
            merchandise.priceText.color = GameManager.Instance.StageManager.Coin < merchandise.price ? Color.red : Color.black;
        }
        SoldOut = true;
    }
}

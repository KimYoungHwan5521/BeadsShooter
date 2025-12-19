using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchandiseOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI detailText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] GameObject soldOutUI;
    bool soldOut;
    public bool Soldout
    {
        get => soldOut;
        set
        {
            soldOut = value;
            soldOutUI.SetActive(value);
        }
    }
    ShopManager.MerchandiseInfo merchandiseInfo;

    public void SetOption(ShopManager.MerchandiseInfo merchandiseInfo)
    {
        this.merchandiseInfo = merchandiseInfo;
        nameText.text = merchandiseInfo.name;
        typeText.text = merchandiseInfo.type.ToString();
        switch (merchandiseInfo.type)
        {
            case ShopManager.MerchandiseType.Material:
            case ShopManager.MerchandiseType.Consumable:
                detailText.text = merchandiseInfo.reward.Explain;
                break;
            case ShopManager.MerchandiseType.Blueprint:
                detailText.text = merchandiseInfo.blueprint.reward.Explain;
                break;
        }
        priceText.text = merchandiseInfo.price.ToString();
        priceText.color = GameManager.Instance.StageManager.Coin < merchandiseInfo.price ? Color.red : Color.black;
    }

    public void Buy()
    {
        if (GameManager.Instance.StageManager.Coin < merchandiseInfo.price) return;
        GameManager.Instance.StageManager.Coin -= merchandiseInfo.price;
        Soldout = true;
    }
}

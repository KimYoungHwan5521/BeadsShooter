using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI optionNameText;
    [SerializeField] Image optionImage;
    public int materialType;
    public RewardFormat reward;
    [SerializeField] TextMeshProUGUI detailText;

    public void SetOption(string optionName, int materialType, RewardFormat reward)
    {
        optionNameText.text = optionName;
        this.materialType = materialType;
        optionImage.color = MaterialsColor.colors[materialType];
        this.reward = reward;
        detailText.text = reward.Explain;
    }
}

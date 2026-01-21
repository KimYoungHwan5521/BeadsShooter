using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoText;
    //[SerializeField] TextMeshProUGUI progressText;
    [SerializeField] Image progressBar;

    public void SetLoadInfo(string info, int numerator, int denominator)
    {
        infoText.text = info;
        //progressText.text = $"( {numerator} / {denominator} )";
        progressBar.fillAmount = numerator / denominator;
    }
}

using UnityEngine;

public class SafeAreaCover : MonoBehaviour
{
    enum CoverLocation { None, Top, Bottom }
    [SerializeField] CoverLocation wantCover;
    void Start()
    {
        if(wantCover == CoverLocation.Top) GameManager.Instance.ManagerStart += CoverTop;
        else if(wantCover == CoverLocation.Bottom) GameManager.Instance.ManagerStart += CoverBottom;
    }

    void CoverTop()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height - (Screen.safeArea.y + Screen.safeArea.height));
    }

    void CoverBottom()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height - (Screen.safeArea.y + Screen.safeArea.height));
    }
}

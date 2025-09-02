using UnityEngine;

public class SafeAreaCover : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.ManagerStart += Cover;
    }

    void Cover()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height - (Screen.safeArea.y + Screen.safeArea.height));
    }
}

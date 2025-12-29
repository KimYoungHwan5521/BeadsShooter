using UnityEngine;

public class TabToggle : MonoBehaviour
{
    [SerializeField] MainUI mainUI;
    [SerializeField] MainUI.Tab tab;

    public void Toggle(bool isOn)
    {
        mainUI.SelectedTab = tab;
    }
}

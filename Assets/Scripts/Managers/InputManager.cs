using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Bar bar;
    bool touch;

    private void Update()
    {
        if (!Camera.main.pixelRect.Contains(Input.mousePosition)) return;
        if(touch)
        {
            float xPos = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -8.82f, 8.82f);
            bar.MoveBar(xPos);
        }
    }

    void OnClick(InputValue value)
    {
        if(GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            touch = value.Get<float>() == 1;
            if(value.Get<float>() == 0) bar.ReleaseBeads();
        }
    }
}

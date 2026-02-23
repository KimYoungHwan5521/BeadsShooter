using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Bar bar;
    bool touch;

    private void Update()
    {
        if (!Camera.main.pixelRect.Contains(Input.mousePosition)) return;
        if (touch)
        {
            if (bar.grabbedBeads.Count == 0)
            {
                bar.MoveBar(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
            }
            else bar.DrawPredictionLine(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else bar.MoveBar(bar.transform.position.x);
    }

    void OnClick(InputValue value)
    {
        touch = value.Get<float>() == 1;
        if(GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            if(value.Get<float>() == 0)
            {
                bar.ReleaseBeads(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}

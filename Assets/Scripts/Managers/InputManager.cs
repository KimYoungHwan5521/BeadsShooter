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
            if (bar.grabbedBeads.Count == 0)
            {
                float xPos = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -8.82f, 8.82f);
                bar.MoveBar(xPos);
            }
            else bar.DrawPredictionLine();
        }
    }

    void OnClick(InputValue value)
    {
        touch = value.Get<float>() == 1;
        if(GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            if(value.Get<float>() == 0)
            {
                bar.ReleaseBeads(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (GameManager.Instance.StageManager.FeverCharged) GameManager.Instance.StageManager.Fever();
                else
                {
                    foreach(var sprite in bar.GetComponentsInChildren<SpriteRenderer>())
                    {
                        Color color = bar.feverColor;
                        color.a = 0.5f;
                        sprite.color = color;
                    }
                }
            }
            else
            {
                foreach (var sprite in bar.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = Color.white;
                }
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Bar bar;

    private void Update()
    {
        if (!Camera.main.pixelRect.Contains(Input.mousePosition)) return;
        //float xOffset = (Screen.width / 1170f) / 2;
        //float xPos = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Vector3.zero).x + (bar.barBody.localScale.x + 1) * bar.barLength / 2 + xOffset, Camera.main.ScreenToWorldPoint(new(1170, 0)).x - (bar.barBody.localScale.x + 1) * bar.barLength / 2 + xOffset);
        float xPos = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -8.82f, 8.82f);
        bar.MoveBar(xPos);
    }

    void OnClick(InputValue value)
    {
        if(GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            if(value.Get<float>() == 0) bar.ReleaseBeads();
        }
    }
}

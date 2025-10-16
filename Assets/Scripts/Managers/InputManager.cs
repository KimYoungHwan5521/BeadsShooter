using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] Bar bar;

    private void Update()
    {
        if (!Camera.main.pixelRect.Contains(Input.mousePosition)) return;
        float xPos = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Vector3.zero).x + bar.barLength / 2, Camera.main.ScreenToWorldPoint(new(Screen.width, 0)).x - bar.barLength / 2);
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

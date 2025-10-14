using UnityEngine;

public class Bar : CustomObject
{
    [SerializeField] float barLength = 1;

    protected override void MyStart()
    {
        base.MyStart();
        transform.localScale = new(barLength, 0.1f);
    }
    protected override void MyUpdate()
    {
        if (!Camera.main.pixelRect.Contains(Input.mousePosition)) return;
        float xPos = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Vector3.zero).x + barLength / 2, Camera.main.ScreenToWorldPoint(new(Screen.width, 0)).x - barLength / 2);
        transform.position = new(xPos, -3);
    }
}

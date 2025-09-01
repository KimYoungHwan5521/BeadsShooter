using UnityEngine;

public class Gun : CustomObject
{
    void Update()
    {
        if (!Camera.main.pixelRect.Contains(Input.mousePosition)) return;
        transform.up = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
}

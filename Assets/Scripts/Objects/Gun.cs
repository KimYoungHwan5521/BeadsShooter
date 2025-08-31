using UnityEngine;

public class Gun : MonoBehaviour
{
    void Update()
    {
        transform.up = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
}

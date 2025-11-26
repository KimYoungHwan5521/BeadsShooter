using UnityEngine;
using System.Collections.Generic;

public class Portal : CustomObject
{
    [SerializeField] Portal linkedPortal;
    [SerializeField] bool isVertical;
    List<GameObject> teleporteds = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.phase != GameManager.Phase.BattlePhase) return;
        if (teleporteds.Contains(collision.gameObject)) return;

        Vector2 offset = collision.transform.position - transform.position;

        // 만약 버티컬이면 : 오른쪽에서 충돌했다고 하면 - 출구에서 왼쪽에서 나오고, 왼쪽에서 충돌했다고 하면 - 출구에서 오른쪽에서 나옴
        // 호라이즌이면 : 위에서 충돌했으면 - 출구에서 아래에서 나오고, 아래에서 충돌했으면 - 출구에서 위에서 나옴
        if(isVertical)
        {
            collision.transform.position = linkedPortal.transform.position + new Vector3(-offset.x, offset.y);
        }
        else
        {
            collision.transform.position = linkedPortal.transform.position + new Vector3(offset.x, -offset.y);
        }

        if(collision.TryGetComponent(out TrailRenderer trail))
        {
            trail.Clear();
        }
        linkedPortal.teleporteds.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        teleporteds.Remove(collision.gameObject);
    }
}

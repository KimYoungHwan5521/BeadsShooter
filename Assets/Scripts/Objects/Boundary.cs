using UnityEngine;

public class Boundary : CustomObject
{
    EdgeCollider2D[] edges;

    protected override void MyStart()
    {
        base.MyStart();
        
        edges = GetComponentsInChildren<EdgeCollider2D>();
        Rect safe = Screen.safeArea;

        // SafeArea 4점
        Vector3 bl = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMin, safe.yMin, Camera.main.nearClipPlane)); // bottom-left
        Vector3 br = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMax, safe.yMin, Camera.main.nearClipPlane)); // bottom-right
        Vector3 tr = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMax, safe.yMax, Camera.main.nearClipPlane)); // top-right
        Vector3 tl = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMin, safe.yMax, Camera.main.nearClipPlane)); // top-left

        // 각 EdgeCollider를 SafeArea의 한 변으로 세팅
        edges[0].points = new Vector2[] { bl, br }; // bottom
        edges[1].points = new Vector2[] { br, tr }; // right
        edges[2].points = new Vector2[] { tr, tl }; // top
        edges[3].points = new Vector2[] { tl, bl }; // left

        edges[0].GetComponent<BoundaryEdge>().edgeType = 0;
        edges[1].GetComponent<BoundaryEdge>().edgeType = 2;
        edges[2].GetComponent<BoundaryEdge>().edgeType = 1;
        edges[3].GetComponent<BoundaryEdge>().edgeType = 2;
    }
}

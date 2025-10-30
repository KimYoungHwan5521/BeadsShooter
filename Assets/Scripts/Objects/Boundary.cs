using UnityEngine;

public class Boundary : CustomObject
{
    BoxCollider2D[] edges;

    protected override void MyStart()
    {
        base.MyStart();

        edges = GetComponentsInChildren<BoxCollider2D>();
        Rect safe = Screen.safeArea;
        // SafeArea 4Á¡
        Vector3 bl = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMin, safe.yMin, Camera.main.nearClipPlane)); // bottom-left
        Vector3 br = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMax, safe.yMin, Camera.main.nearClipPlane)); // bottom-right
        Vector3 tr = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMax, safe.yMax, Camera.main.nearClipPlane)); // top-right
        Vector3 tl = Camera.main.ScreenToWorldPoint(new Vector3(safe.xMin, safe.yMax, Camera.main.nearClipPlane)); // top-left
        float safeWidth = br.x - bl.x;
        float safeHeight = tr.y - br.y;

        // bottom
        edges[0].transform.position = new(0, - safeHeight / 2, 0);
        edges[0].size = new(safeHeight, 2);
        edges[0].offset = new(0, 0);
        // right
        edges[1].transform.position = new(safeWidth / 2, 0, 0);
        edges[1].size = new(2, safeHeight);
        edges[1].offset = new(1, 0);
        // top
        edges[2].transform.position = new(0, safeHeight / 2, 0);
        edges[2].size = new(safeHeight, 2);
        edges[2].offset = new(0, -2);
        // left
        edges[3].transform.position = new(-safeWidth / 2, 0, 0);
        edges[3].size = new(2, safeHeight);
        edges[3].offset = new(-1, 0);

        edges[0].GetComponent<BoundaryEdge>().edgeType = 0;
        edges[1].GetComponent<BoundaryEdge>().edgeType = 3;
        edges[2].GetComponent<BoundaryEdge>().edgeType = 1;
        edges[3].GetComponent<BoundaryEdge>().edgeType = 2;
    }
}

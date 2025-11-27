using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(GridLayoutGroup))]
public class DynamicGridLayout : MonoBehaviour
{
    GridLayoutGroup grid;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    private IEnumerator Start()
    {
        yield return null;
        float x = (GetComponent<RectTransform>().rect.width - (grid.padding.right + grid.padding.left + grid.spacing.x * (grid.constraintCount - 1))) / 2f - 0.001f;
        grid.cellSize = new Vector2(x, x * 1.5f);
    }
}

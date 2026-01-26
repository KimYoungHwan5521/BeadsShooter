using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class BlueprintDrawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    List<Image> materials = new();
    Blueprint blueprint;
    [SerializeField] TextMeshProUGUI detailText;
    GridLayoutGroup grid;

    [SerializeField] float rectWidth;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        float cellSize = rectWidth * 0.8f / Blueprint.ColumnCount;
        grid.cellSize = new(cellSize, cellSize);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = Blueprint.ColumnCount;

        int iter = Blueprint.RowCount * Blueprint.ColumnCount;
        for (int i=0; i<iter; i++)
        {
            GameObject material = new GameObject($"Material{i}", typeof(Image));
            material.transform.SetParent(transform, false);
            materials.Add(material.GetComponent<Image>());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.blueprintDetail.SetActive(true);
        GameManager.Instance.blueprintDetail.transform.position = transform.position + new Vector3(0, 10);
        GameManager.Instance.blueprintDetail.GetComponentInChildren<BlueprintDrawer>().SetBlueprint(blueprint, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.blueprintDetail.SetActive(false);
    }

    public void SetBlueprint(Blueprint blueprint, bool detail = false)
    {
        this.blueprint = blueprint;
        for(int i=0; i<Blueprint.RowCount; i++)
        {
            for(int j=0; j<Blueprint.ColumnCount; j++)
            {
                if (i >= blueprint.blueprint.GetLength(0) || j >= blueprint.blueprint.GetLength(1))
                {
                    materials[Blueprint.RowCount * i + j].color = MaterialsColor.colors[0];
                }
                else
                {
                    materials[Blueprint.RowCount * i + j].color = MaterialsColor.colors[blueprint.blueprint[i, j]];
                }
            }
        }

        string description = "";
        if(detail)
        {
            description = blueprint.reward.rewardType switch
            {
                RewardType.AttackDamage => $"Ball's attack damage + {blueprint.reward.value}",
                RewardType.PoisonDamage => $"Blocks attacked by the ball take {blueprint.reward.value} damage per second.",
                RewardType.AreaDamage => $"After the ball damages a block, it deals {blueprint.reward.value} damage to other blocks in the surrounding {blueprint.reward.area.x}x{blueprint.reward.area.y} tiles.",
                _ => "",
            };
        }
        if(detailText != null) detailText.text = description;
    }
}

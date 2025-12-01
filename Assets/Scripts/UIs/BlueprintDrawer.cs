using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlueprintDrawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] materials;
    Blueprint blueprint;
    [SerializeField] TextMeshProUGUI detailText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.blueprintDetail.SetActive(true);
        GameManager.Instance.blueprintDetail.transform.position = transform.position + new Vector3(0, 10);
        GameManager.Instance.blueprintDetail.GetComponentInChildren<BlueprintDrawer>().SetBlueprint(blueprint, true);
        GameManager.Instance.selectCharacter.SetShowBlueprint(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.blueprintDetail.SetActive(false);
    }

    public void SetBlueprint(Blueprint blueprint, bool detail = false)
    {
        this.blueprint = blueprint;
        for(int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                if (i >= blueprint.blueprint.GetLength(0) || j >= blueprint.blueprint.GetLength(1))
                {
                    materials[4 * i + j].color = MaterialsColor.colors[0];
                }
                else
                {
                    materials[4 * i + j].color = MaterialsColor.colors[blueprint.blueprint[i, j]];
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

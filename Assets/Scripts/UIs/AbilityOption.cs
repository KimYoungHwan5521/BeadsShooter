using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] Image abilityIcon;
    [SerializeField] TextMeshProUGUI abilityExplain;
    public AbilityManager.Ability linkedAbility;

    public void SetOption(AbilityManager.Ability ability)
    {
        linkedAbility = ability;
        abilityName.text = ability.name.ToString();
        abilityExplain.text = ability.explain;
    }
}

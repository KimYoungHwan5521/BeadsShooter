using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityOption : MonoBehaviour
{
    [SerializeField] Image cardBody;
    [SerializeField] Sprite[] cardBodyByRarity;
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] Image abilityIcon;
    AspectRatioFitter fitter;
    [SerializeField] TextMeshProUGUI abilityExplain;
    public AbilityManager.Ability linkedAbility;

    private void Awake()
    {
        fitter = GetComponentInChildren<AspectRatioFitter>();
    }

    public void SetOption(AbilityManager.Ability ability)
    {
        linkedAbility = ability;
        cardBody.sprite = cardBodyByRarity[(int)ability.cardType];
        abilityName.text = ability.name.ToString();
        Sprite sprite;
        if (Enum.TryParse(ability.name.ToString().Split("LV")[0], out ResourceEnum.Sprite spriteEnum))
        {
            sprite = ResourceManager.Get(spriteEnum);
        }
        else
        {
            sprite = ResourceManager.Get(ResourceEnum.Sprite.Unknown);
        }
        fitter.aspectRatio = (float) sprite.rect.width / sprite.rect.height;
        abilityIcon.sprite = sprite;
        abilityExplain.text = ability.explain;
    }
}

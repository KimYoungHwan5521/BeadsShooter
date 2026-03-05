using UnityEngine;
using UnityEngine.UI;

public class PickedCard : MonoBehaviour
{
    [SerializeField] AbilityOption abilityOption;
    [SerializeField] Image[] stars;
    [SerializeField] GameObject starBox;
    [SerializeField] Sprite notFilledStar;
    [SerializeField] Sprite filledStar;
    [SerializeField] Sprite notFilledPromotionStar;
    [SerializeField] Sprite filledPromotionStar;

    public void SetCard(AbilityManager.Ability ability, int level)
    {
        abilityOption.SetOption(ability);
        if(ability.isPassive)
        {
            starBox.SetActive(false);
        }
        else
        {
            starBox.SetActive(true);
            for(int i=0; i<5; i++)
            {
                if(i<4)
                {
                    stars[i].sprite = i < level ? filledStar : notFilledStar;
                }
                else
                {
                    stars[4].sprite = i < level ? filledPromotionStar : notFilledPromotionStar;
                }
            }
        }
    }
}

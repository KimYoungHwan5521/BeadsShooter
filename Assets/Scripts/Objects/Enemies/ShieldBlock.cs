using UnityEngine;

public class ShieldBlock : Block
{
    public GameObject downShield;
    public GameObject leftShield;
    public GameObject rightShield;
    public GameObject upShield;
    float shieldDurability;

    public void SetShield(bool left, bool right, bool down, bool up, float shieldDurability)
    {
        leftShield.SetActive(left);
        rightShield.SetActive(right);
        downShield.SetActive(down);
        upShield.SetActive(up);
        this.shieldDurability = shieldDurability;
    }

    public void ShieldDamage(float damage)
    {
        if(shieldDurability < damage)
        {
            leftShield.SetActive(false);
            rightShield.SetActive(false);
            downShield.SetActive(false);
            upShield.SetActive(false);
        }
    }
}

using UnityEngine;

public class HideBlock : Block
{
    public GameObject downShield;
    public GameObject leftShield;
    public GameObject rightShield;

    public void SetShield(bool left, bool right, bool down = true)
    {
        leftShield.SetActive(left);
        rightShield.SetActive(right);
        downShield.SetActive(down);
    }
}

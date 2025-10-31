using UnityEngine;

public class ShieldBlock : Block
{
    public GameObject downShield;
    public GameObject leftShield;
    public GameObject rightShield;
    public GameObject upShield;

    public void SetShield(bool left, bool right, bool down, bool up)
    {
        leftShield.SetActive(left);
        rightShield.SetActive(right);
        downShield.SetActive(down);
        upShield.SetActive(up);
    }
}

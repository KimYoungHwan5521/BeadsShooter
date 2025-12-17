using UnityEngine;

public class ShopSelect : MonoBehaviour
{
    // Type : 0 - Shop, 1 - Rest, 2 - Random Event
    [SerializeField] int type;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(type == 0)
        {

        }
        else if(type == 1)
        {

        }
        else
        {

        }
    }
}

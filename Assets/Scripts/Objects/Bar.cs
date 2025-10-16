using System.Collections.Generic;
using UnityEngine;

public class Bar : CustomObject
{
    public float barLength = 1;
    [SerializeField] List<Projectile> grabbedBeads;
    float yPos = -3.5f;

    protected override void MyStart()
    {
        base.MyStart();
        transform.localScale = new(barLength, 0.1f);
    }

    protected override void MyUpdate()
    {
    }

    public void MoveBar(float xPos)
    {
        float lastXPos = transform.position.x;
        transform.position = new(xPos, yPos);
        foreach(var bead in grabbedBeads)
        {
            bead.transform.position += new Vector3(transform.position.x - lastXPos, 0, 0);
        }
    }

    public void ReleaseBeads()
    {
        foreach(var bead in grabbedBeads)
        {
            bead.SetDirection(bead.transform.position - transform.position);
            bead.activated = true;
        }
        grabbedBeads.Clear();
    }
}

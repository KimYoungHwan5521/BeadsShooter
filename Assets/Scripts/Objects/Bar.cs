using System.Collections.Generic;
using UnityEngine;

public class Bar : CustomObject
{
    public float barLength = 1;
    [SerializeField] List<Projectile> grabbedBeads;
    float yPos;

    protected override void MyStart()
    {
        base.MyStart();
        transform.localScale = new(barLength, 0.1f);
        yPos = transform.position.y;
    }

    protected override void MyUpdate()
    {
    }

    public void MoveBar(float xPos)
    {
        transform.position = new(xPos, yPos);
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

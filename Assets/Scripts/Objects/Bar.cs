using System.Collections.Generic;
using UnityEngine;

public class Bar : CustomObject
{
    public Transform barBody;
    public float barLength = 1;
    public List<Bead> grabbedBeads;
    float yPos = -17.5f;

    protected override void Start()
    {
        base.Start();
        yPos = GameManager.Instance.barYPos;
        transform.position = new Vector3(0, yPos, 0);
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

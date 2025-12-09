using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bar : CustomObject
{
    public Transform barBody;
    public float barLength = 1;
    public List<Bead> grabbedBeads;
    float yPos = -17.5f;
    [SerializeField] float moveSpeed = 1;
    public List<Blueprint> blueprints;

    protected override void Start()
    {
        base.Start();
        yPos = GameManager.Instance.barYPos;
        transform.position = new Vector3(0, yPos, 0);
    }

    public void MoveBar(float xPos)
    {
        if (Mathf.Abs(xPos - transform.position.x) < 0.1f) return;
        Vector2 direction = new Vector2(xPos - transform.position.x, 0).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.unscaledDeltaTime;
        foreach(var bead in grabbedBeads)
        {
            bead.transform.position += (Vector3)direction * moveSpeed * Time.unscaledDeltaTime;
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

    public void SetBar(CharacterData characterData)
    {
        moveSpeed = characterData.moveSpeed;
        blueprints = characterData.blueprints.ToList();
    }
}

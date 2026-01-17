using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bar : CustomObject
{
    public Transform barBody;
    LineRenderer lineRenderer;
    [SerializeField] Sprite dottedLineSprite;
    [SerializeField] GameObject character;
    //[SerializeField] Animator anim;
    public FeverAction fever;
    public int feverLevel = 0;

    const float originalLength = 4;
    const float barMinLength = 0.3f;
    public float BarMinLength => barMinLength;
    float barLength = 1;
    public float BarLength
    {
        get => barLength;
        set
        {
            barLength = Mathf.Max(barMinLength, value);
            barBody.GetComponent<SpriteRenderer>().size = new(originalLength * barLength, 1f);
            barBody.GetComponent<BoxCollider2D>().size = new(originalLength * barLength, 1f);
        }
    }
    public List<Bead> grabbedBeads;
    float yPos = -17.5f;
    [SerializeField] float moveSpeed = 1;
    public List<Blueprint> blueprints;
    public Color feverColor;

    protected virtual void Start()
    {
        yPos = GameManager.Instance.barYPos;
        transform.position = new Vector3(0, yPos, 0);

        ColorUtility.TryParseHtmlString("#44CDCD", out feverColor);
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.material.mainTexture = dottedLineSprite.texture;
    }

    public void MoveBar(float xPos)
    {
        if (Mathf.Abs(xPos - transform.position.x) < 0.1f) return;
        //if (Mathf.Abs(xPos - transform.position.x) < 0.1f)
        //{
        //    anim.SetFloat("MoveSpeed", 0);
        //    return;
        //}
        //anim.SetFloat("MoveSpeed", moveSpeed);
        Vector2 direction = new Vector2(xPos - transform.position.x, 0).normalized;
        character.transform.localScale = new(-direction.x, 1);
        transform.position += (Vector3)direction * moveSpeed * Time.unscaledDeltaTime;
        foreach(var bead in grabbedBeads)
        {
            bead.transform.position += (Vector3)direction * moveSpeed * Time.unscaledDeltaTime;
        }
    }

    public void ReleaseBeads(Vector3 wantPos)
    {
        if (wantPos.y < transform.position.y + 1) wantPos = new(wantPos.x, transform.position.y + 1);
        foreach(var bead in grabbedBeads)
        {
            bead.SetDirection(wantPos - bead.transform.position);
            bead.activated = true;
        }
        grabbedBeads.Clear();
        lineRenderer.enabled = false;
    }

    public void SetBar(CharacterData characterData)
    {
        moveSpeed = characterData.moveSpeed;
        blueprints = characterData.blueprints.ToList();
        fever = characterData.fever;
        feverLevel = 0;
    }

    public void Shrink(float value)
    {
        BarLength -= value;
        //anim.SetTrigger("TakeDamage");
    }

    public void DrawPredictionLine(Vector3 shotPosition)
    {
        if (shotPosition.y < transform.position.y + 1) shotPosition = new(shotPosition.x, transform.position.y + 1);
        RaycastHit2D[] hits = Physics2D.RaycastAll(grabbedBeads[0].transform.position, shotPosition - grabbedBeads[0].transform.position, 50, LayerMask.GetMask("Border"));
        if(Vector2.Distance(grabbedBeads[0].transform.position, hits[0].point) > 20)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new Vector3[] { grabbedBeads[0].transform.position, grabbedBeads[0].transform.position + ((Vector3)hits[0].point - grabbedBeads[0].transform.position).normalized * 20 });
        }
        else
        {
            float leftLength = 20 - Vector2.Distance(grabbedBeads[0].transform.position, hits[0].point);
            Vector2 reflectedVector = hits[0].point - (Vector2)grabbedBeads[0].transform.position;
            reflectedVector.x = -reflectedVector.x;
            reflectedVector.Normalize();
            lineRenderer.positionCount = 3;
            lineRenderer.SetPositions(new Vector3[] { grabbedBeads[0].transform.position, hits[0].point, hits[0].point + reflectedVector * leftLength });
        }
        lineRenderer.enabled = true;
    }

    public void Fever()
    {
        //anim.SetTrigger("Fever");
        fever?.Invoke(feverLevel);
    }
}

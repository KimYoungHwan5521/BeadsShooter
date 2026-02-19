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

    const float barMoveLimit = 11.1f;
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
            electricChargedAnim.GetComponent<SpriteRenderer>().size = new(originalLength * barLength, 1f);
            for(int i = 0; i < iceBlocks.Length; i++)
            {
                iceBlocks[i].transform.localPosition = new(i % 2 == 0 ? - originalLength * barLength / 2 - 0.5f - i / 2 : originalLength * barLength / 2 + 0.5f + i / 2, 0, 0);
            }
        }
    }
    public List<Bead> grabbedBeads;
    float yPos = -17.5f;
    [SerializeField] float moveSpeed = 1;
    public float timeLimitedSpeedMagnification = 1;
    public float timeLimitedSpeedMagnificationTime;

    public List<Blueprint> blueprints;
    public Color feverColor;

    [Header("Ice Ability")]
    public IceBlock[] iceBlocks;
    [SerializeField] int activedIceBlock;
    public int ActivedIceBlock
    {
        get => activedIceBlock;
        set
        {
            activedIceBlock = value;
            for(int i = 0; i < iceBlocks.Length; i++)
            {
                iceBlocks[i].gameObject.SetActive(i < activedIceBlock);
                iceBlocks[i].isActived = i < activedIceBlock;
            }
        }
    }
    public Area chilingAura;

    [Header("Fire Ability")]
    [SerializeField] ParticleSystem fireBallCharging;
    public float fireBallCool;
    float curFireBallCool;
    public float fireBallDamage;
    public bool fireBallExplosion;
    public float fireBallExplosionRange;
    public bool fireBallBurn;
    public float fireBallBurnDamage;

    [Header("Laser Ability")]
    public float laserCool;
    float curLaserCool;
    public int laserCount;
    int firedLaserCount;
    const float laserDelay = 0.1f;
    float curLaserDelay;
    bool laserFire;

    [Header("Electric Ability")]
    [SerializeField] GameObject electricChargedAnim;
    public bool gotElectricAbility;
    public float electricDamage;
    public int electricChainsCount;
    public int electricDischargeCount;
    public bool gotElectrostaticInduction;
    public float electrostaticInductionDamage;
    bool electricCharged;
    public bool ElectricCharged
    {
        get => electricCharged;
        set
        {
            electricCharged = value;
            electricChargedAnim.SetActive(value);
        }
    }
    float electricChargeCool = 10f;
    float curElectricCharge;

    [Header("Telekinesis Ability")]
    public bool gotTelekinesisAbility;
    public float telekinesisCool;
    float curTelekinesisCool;
    public float telekinesisAdditionalDamage;
    public int controllableBallsCount;
    public bool controllableFireball;
    public bool controllableIcicles;

    protected virtual void Start()
    {
        yPos = GameManager.Instance.barYPos;
        transform.position = new Vector3(0, yPos, 0);

        ColorUtility.TryParseHtmlString("#44CDCD", out feverColor);
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.material.mainTexture = dottedLineSprite.texture;
        iceBlocks = GetComponentsInChildren<IceBlock>();
    }

    public override void MyUpdate(float deltaTime)
    {
        if (timeLimitedSpeedMagnificationTime > 0)
        {
            timeLimitedSpeedMagnificationTime -= Time.deltaTime;
        }
        else timeLimitedSpeedMagnification = 1f;

        if(grabbedBeads.Count == 0)
        {
            if(fireBallCool > 0)
            {
                curFireBallCool += deltaTime;
                if (fireBallCool - curFireBallCool < 3f && !fireBallCharging.isPlaying)
                {
                    fireBallCharging.Play();
                }
                if (curFireBallCool > fireBallCool)
                {
                    curFireBallCool = 0;
                    fireBallCharging.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    AllianceProjectile fireBall = PoolManager.Spawn(ResourceEnum.Prefab.FireBall, transform.position).GetComponent<AllianceProjectile>();
                    if (controllableFireball)
                    {
                        List<Enemy> enemies = GameManager.Instance.StageManager.currentStageEnemies;
                        if (enemies.Count > 0)
                        {
                            Enemy target = enemies[Random.Range(0, enemies.Count)];
                            fireBall.SetDirection(target.ColliderCenter - (Vector2)fireBall.transform.position);
                        }
                        else fireBall.SetDirection(Vector2.up);
                    }
                    else fireBall.SetDirection(Vector2.up);
                    fireBall.SetProjectile(fireBallDamage, 20f, fireBallExplosion, fireBallExplosionRange, fireBallBurn, fireBallBurnDamage);
                }
            }

            if(laserCount > 0)
            {
                curLaserCool += deltaTime;
                if(curLaserCool > laserCool)
                {
                    curLaserCool = 0;
                    laserFire = true;
                }
            }

            if(laserFire)
            {
                curLaserDelay += deltaTime;
                if(curLaserDelay > laserDelay)
                {
                    curLaserDelay = 0;
                    firedLaserCount++;
                    int rand = Random.Range(0, GameManager.Instance.StageManager.currentStageEnemies.Count);
                    Enemy target = GameManager.Instance.StageManager.currentStageEnemies[rand];
                    target.TakeDamage(1);
                    LineRenderer line = PoolManager.Spawn(ResourceEnum.Prefab.FeverLaser).GetComponent<LineRenderer>();
                    Vector3 targetPos;
                    if (target is SplitterBlock targetSplitter) targetPos = targetSplitter.GetTartgetPos();
                    else if (target is Boss2 boss2) targetPos = boss2.GetTargetPos();
                    else targetPos = target.transform.position;
                    line.SetPositions(new Vector3[] { GameManager.Instance.StageManager.bar.transform.position, targetPos });

                    if(firedLaserCount == laserCount)
                    {
                        laserFire = false;
                        firedLaserCount = 0;
                    }
                }
            }

            if(gotElectricAbility)
            {
                curElectricCharge += deltaTime;
                if (curElectricCharge > electricChargeCool)
                {
                    curElectricCharge = 0;
                    ElectricCharged = true;
                }
            }

            if (gotTelekinesisAbility)
            {
                curTelekinesisCool += deltaTime;
                if (curTelekinesisCool > telekinesisCool)
                {
                    curTelekinesisCool = 0;
                    List<Bead> beads = GameManager.Instance.StageManager.beads;
                    if (beads.Count > 0)
                    {
                        for(int i=0; i<controllableBallsCount; i++)
                        {
                            if (i > beads.Count) break;
                            Bead targetBead = beads[i];
                            List<Enemy> enemies = GameManager.Instance.StageManager.currentStageEnemies;
                            if(enemies.Count > 0)
                            {
                                targetBead.temporaryDamageCorrection += telekinesisAdditionalDamage;
                                targetBead.temporarySpeedMagnification *= 1.3f;
                                targetBead.trail.startColor = new Color(0.6f, 0.2f, 0.8f);
                                targetBead.trail.endColor = new Color(0.6f, 0.2f, 0.8f);
                                Enemy target = enemies[Random.Range(0, enemies.Count)];
                                targetBead.SetDirection(target.ColliderCenter - (Vector2)targetBead.transform.position);
                                Debug.Log($"target colider center : {target.ColliderCenter}");
                            }
                        }
                    }
                }
            }
        }
    }

    public void MoveBar(float xPos)
    {
        float offset = Mathf.Clamp(xPos, - barMoveLimit + originalLength * barLength * 0.5f - 0.5f, barMoveLimit - originalLength *  barLength * 0.5f + 0.5f);
        if (Mathf.Abs(offset - transform.position.x) < 0.5f) return;
        //if (Mathf.Abs(xPos - transform.position.x) < 0.1f)
        //{
        //    anim.SetFloat("MoveSpeed", 0);
        //    return;
        //}
        //anim.SetFloat("MoveSpeed", moveSpeed);
        Vector2 direction = new Vector2(offset - transform.position.x, 0).normalized;
        character.transform.localScale = new(-direction.x, 1);
        transform.position += (Vector3)direction * moveSpeed * timeLimitedSpeedMagnification * Time.unscaledDeltaTime;
        foreach(var bead in grabbedBeads)
        {
            bead.transform.position += (Vector3)direction * moveSpeed * timeLimitedSpeedMagnification * Time.unscaledDeltaTime;
        }
        if (timeLimitedSpeedMagnification < 1) barBody.GetComponent<SpriteRenderer>().color = Color.green;
        else barBody.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ReleaseBeads(Vector3 wantPos)
    {
        if (wantPos.y < transform.position.y + 1) wantPos = new(wantPos.x, transform.position.y + 1);
        foreach(var bead in grabbedBeads)
        {
            bead.SetDirection(wantPos - bead.transform.position);
            bead.activated = true;
            if (ElectricCharged) bead.ElectricCharged = true;
        }
        ElectricCharged = false;
        grabbedBeads.Clear();
        lineRenderer.enabled = false;
    }

    public void SetBar(CharacterData characterData)
    {
        moveSpeed = characterData.moveSpeed;
        timeLimitedSpeedMagnification = 1f;
        blueprints = characterData.blueprints.ToList();
        fever = characterData.fever;
        feverLevel = 0;
        BarLength = 1f;
        
        ActivedIceBlock = 0;
        chilingAura.gameObject.SetActive(false);
        fireBallCharging.Stop();
        fireBallCool = -1;
        fireBallExplosion = false;
        fireBallBurn = false;
        laserCount = 0;
        gotElectricAbility = false;
        gotElectrostaticInduction = false;
        gotTelekinesisAbility = false;
        ElectricCharged = false;
        controllableFireball = false;
        controllableIcicles = false;
    }

    public void RoundReset()
    {
        foreach (var iceBlock in iceBlocks) iceBlock.ResetObject();
        curFireBallCool = 0;
        curLaserCool = 0;
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

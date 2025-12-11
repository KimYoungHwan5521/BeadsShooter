using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    const int row = 14;
    const int column = 22;
    const int term = 6;

    [Header("Header UI")]
    [SerializeField] Image feverGaugeImage;
    [SerializeField] Sprite[] feverGaugeSprites;
    bool feverCharged;
    public bool FeverCharged
    {
        get => feverCharged;
        set
        {
            feverCharged = value;
            if(value)
            {
                foreach(var sprite in bar.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = bar.feverColor;
                }
            }
            else
            {
                foreach (var sprite in bar.GetComponentsInChildren<SpriteRenderer>())
                {
                    sprite.color = Color.white;
                }
            }
        }
    }
    int feverGauge;
    public int FeverGauge
    {
        get => feverGauge;
        set
        {
            if(value >= feverGaugeSprites.Length)
            {
                FeverCharged = true;
                return;
            }
            feverGauge = value;
            feverGaugeImage.sprite = feverGaugeSprites[value];
        }
    }
    bool feverHalf;
    public bool FeverHalf
    {
        get => feverHalf;
        set
        {
            if(value)
            {
                if (feverHalf)
                {
                    FeverGauge++;
                    feverHalf = false;
                }
                else feverHalf = true;
            }
            else feverHalf = false;
        }
    }

    [Header("Ingame")]
    [SerializeField] Transform board;
    [SerializeField] TextMeshProUGUI stageStartCountText;
    public int currentStage = 0;
    public List<Enemy> currentStageEnemies;
    public List<Enemy> nextStageEnemies;
    public List<GameObject> currentStageWalls;
    public List<GameObject> nextStageWalls;
    public Bar bar;
    public List<Bead> beads;

    readonly float stageStartCount = 3f;
    [SerializeField]float curStageStartCount;

    public enum BlockType { Normal, Wall, Shield, Counter, PentagonalBlock, SpeedUp, Illusion, Attacker, Splitter, MucusDripper }

    public class BlockForm
    {
        public BlockType blockType;
        public Vector2Int size;
        // MovableBlock
        public int moveType;

        public BlockForm(BlockType blockType)
        {
            this.blockType = blockType;
            size = new Vector2Int(1,1);
        }

        public BlockForm(BlockType blockType, Vector2Int size)
        {
            this.blockType = blockType;
            this.size = size;
        }

        public BlockForm(BlockType blockType, int moveType)
        {
            this.blockType = blockType;
            size = new Vector2Int(1, 1);
            this.moveType = moveType;
        }
    }

    public class EnemyArrangementInfo
    {
        public BlockType blockType;
        public Vector2Int position;
        public Vector2Int size;
        public float maxHP;
        // Shield block
        public int shieldPosition;

        public EnemyArrangementInfo(BlockType blockType, Vector2Int position, float maxHP)
        {
            this.blockType = blockType;
            this.position = position;
            size = new Vector2Int(1, 1);
            this.maxHP = maxHP;
        }

        public EnemyArrangementInfo(BlockType blockType, Vector2Int position, Vector2Int size)
        {
            this.blockType = blockType;
            this.position = position;
            this.size = size;
            maxHP = Mathf.Max(size.x * size.y / 2, 1);
        }

        public EnemyArrangementInfo(BlockType blockType, Vector2Int position, int shieldPos)
        {
            this.blockType = blockType;
            this.position = position;
            size = new Vector2Int(1, 1);
            maxHP = 1;
            shieldPosition = shieldPos;
        }
    }

    public class StageInfo
    {
        public List<EnemyArrangementInfo> enemyArrangementInfo;

        public StageInfo(EnemyArrangementInfo[] enemyArrangementInfo)
        {
            this.enemyArrangementInfo = enemyArrangementInfo.ToList();
        }
    }

    public StageInfo[] stageInfos;

    float wantDown;

    private void Start()
    {
        stageInfos = new[]
        {
            // Stage 0
            //RandomStageGenerate((new(BlockType.Attacker, new(2,1)), 20)),
            //RandomStageGenerate((new(BlockType.MucusDripper, 2), 5), (new(BlockType.Splitter, new Vector2Int(2,1)), 10)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),

            //RandomStageGenerate((new(BlockType.Normal, new(2,1)), 10), (new(BlockType.PentagonalBlock), 3), (new(BlockType.SpeedUp, new(1,2)), 3), (new(BlockType.Illusion, new(2,1)), 3)),
            // Stage 1
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2,1)), 8), (new(BlockType.Normal, new Vector2Int(1,2)), 8), (new(BlockType.Shield, new Vector2Int(2, 1)), 4)),
            // ...
            RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 6), (new(BlockType.Normal, new Vector2Int(1, 2)), 6), (new(BlockType.Normal, new Vector2Int(2, 2)), 6), (new(BlockType.Shield), 4),  (new(BlockType.Counter), 4)),
        };
    }

    private void Update()
    {
        if (wantDown > 0.1f)
        {
            board.transform.position += Vector3.down * 0.2f;
            foreach(var bead in beads)
            {
                if (bead.transform.position.y > bar.transform.position.y + 3)
                {
                    bead.transform.position += Vector3.down * 0.2f;
                    Vector3[] points = new Vector3[bead.trail.positionCount];
                    bead.trail.GetPositions(points);
                    for(int i=0; i<points.Length; i++) points[i] += Vector3.down * 0.2f;
                    bead.trail.SetPositions(points);
                }
            }
            wantDown -= 0.2f;
            if(wantDown < 0.1f)
            {
                if (currentStage > 0)
                {
                    curStageStartCount = stageStartCount;
                    stageStartCountText.gameObject.SetActive(true);
                }
                else
                    GameManager.Instance.BattlePhaseStart();
            }
        }
        if(curStageStartCount > 0)
        {
            curStageStartCount -= Time.unscaledDeltaTime;
            stageStartCountText.text = $"{(int)curStageStartCount + 1}";
            if(curStageStartCount <= 0)
            {
                stageStartCountText.gameObject.SetActive(false);
                foreach (var bead in beads)
                {
                    bead.trail.emitting = true;
                }
                GameManager.Instance.BattlePhaseStart();
            }
        }
    }

    public void StageSetting()
    {
        bool clearBothStage = nextStageEnemies.Count == 0;
        Debug.Log($"{clearBothStage} currentStage : {currentStage}");
        if (!clearBothStage)
        {
            currentStageEnemies = nextStageEnemies.ToList();
            nextStageEnemies = new();
        }

        StageInfo nextStageInfo;
        int wantStage;
        if (clearBothStage)
        {
            if (currentStage > 0)
            {
                currentStage++;
            }
            wantStage = currentStage;
        }
        else
        {
            wantStage = currentStage + 1;
        }
        if (wantStage >= stageInfos.Length) nextStageInfo = RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 6), (new(BlockType.Normal, new Vector2Int(1, 2)), 6), (new(BlockType.Normal, new Vector2Int(2, 2)), 6), (new(BlockType.Shield), 4), (new(BlockType.Counter), 4));
        else nextStageInfo = stageInfos[wantStage];
        SpawnBlocks(nextStageInfo, wantStage, clearBothStage, true);

        foreach(GameObject wall in currentStageWalls)
        {
            PoolManager.Despawn(wall);
        }
        if(!clearBothStage) currentStageWalls = nextStageWalls.ToList();
        else currentStageWalls.Clear();
        nextStageWalls.Clear();
        for(int i=-5; i<=5; i++)
        {
            Block wall = PoolManager.Spawn(ResourceEnum.Prefab.NormalBlock).GetComponent<Block>();
            if (currentStageEnemies.Contains(wall)) currentStageEnemies.Remove(wall);
            if (nextStageEnemies.Contains(wall)) nextStageEnemies.Remove(wall);
            if (currentStage == 0) wall.transform.position = new(i * 2, -0.25f + row + 1 + term + row);
            else wall.transform.position = new(i * 2, -0.25f + row + 1 + term + row);
            wall.transform.SetParent(board, true);
            wall.GetComponent<BoxCollider2D>().size = new(2,1);
            wall.GetComponent<SpriteRenderer>().size = new(2,1);
            wall.GetComponent<SpriteRenderer>().color = Color.gray;
            wall.SetInfo(currentStage, 3);
            if (clearBothStage) currentStageWalls.Add(wall.gameObject);
            else nextStageWalls.Add(wall.gameObject);
        }
        // 2스테이지 동시에 깬 경우 다다음 스테이지
        if(clearBothStage)
        {
            if (currentStage < stageInfos.Length) nextStageInfo = stageInfos[currentStage + 1];
            else nextStageInfo = RandomStageGenerate((new(BlockType.Normal, new Vector2Int(2, 1)), 6), (new(BlockType.Normal, new Vector2Int(1, 2)), 6), (new(BlockType.Normal, new Vector2Int(2, 2)), 6), (new(BlockType.Shield), 4), (new(BlockType.Counter), 4));
            SpawnBlocks(nextStageInfo, wantStage, clearBothStage, false);
            foreach (GameObject wall in nextStageWalls)
            {
                PoolManager.Despawn(wall);
            }
            nextStageWalls.Clear();
            for (int i = -5; i <= 5; i++)
            {
                Block wall = PoolManager.Spawn(ResourceEnum.Prefab.NormalBlock).GetComponent<Block>();
                wall.transform.position = new(i * 2, -0.25f + row + 1 + term + row + 1 + row);
                if (currentStageEnemies.Contains(wall)) currentStageEnemies.Remove(wall);
                if (nextStageEnemies.Contains(wall)) nextStageEnemies.Remove(wall);
                wall.transform.SetParent(board, true);
                wall.GetComponent<BoxCollider2D>().size = new(2, 1);
                wall.GetComponent<SpriteRenderer>().size = new(2, 1);
                wall.GetComponent<SpriteRenderer>().color = Color.gray;
                wall.SetInfo(currentStage + 1, 3);
                nextStageWalls.Add(wall.gameObject);
            }
        }

        wantDown = clearBothStage ? (row + 1) * 2 : row + 1;
    }

    void SpawnBlocks(StageInfo nextStageInfo, int wantStage, bool clearBothStage, bool frontStage)
    {
        int index = 0;
        foreach (EnemyArrangementInfo enemyArrangementInfo in nextStageInfo.enemyArrangementInfo)
        {
            Block block;
            if(enemyArrangementInfo.blockType == BlockType.Shield)
            {
                block = PoolManager.Spawn(ResourceEnum.Prefab.ShieldBlock).GetComponent<ShieldBlock>();
                bool leftShield = false;
                bool rightShield = false;
                bool downShield = false;
                bool upShield = false;
                if (enemyArrangementInfo.shieldPosition == 0) downShield = true;
                else if(enemyArrangementInfo.shieldPosition == 1) leftShield = true;
                else if(enemyArrangementInfo.shieldPosition == 2) rightShield = true;
                else if(enemyArrangementInfo.shieldPosition == 3) upShield = true;
                ((ShieldBlock)block).SetShield(leftShield, rightShield, downShield, upShield);
            }
            else if(enemyArrangementInfo.blockType == BlockType.Counter) block = PoolManager.Spawn(ResourceEnum.Prefab.CounterBlock).GetComponent<CounterBlock>();
            else if(enemyArrangementInfo.blockType == BlockType.PentagonalBlock) block = PoolManager.Spawn(ResourceEnum.Prefab.PentagonalBlock).GetComponent<PentagonalBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.SpeedUp) block = PoolManager.Spawn(ResourceEnum.Prefab.SpeedUpBlock).GetComponent<SpeedUpBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.Illusion) block = PoolManager.Spawn(ResourceEnum.Prefab.IllusionBlock).GetComponent<IllusionBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.Attacker) block = PoolManager.Spawn(ResourceEnum.Prefab.AttackerBlock).GetComponent<AttackerBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.Splitter) block = PoolManager.Spawn(ResourceEnum.Prefab.SplitterBlock).GetComponent<SplitterBlock>();
            else if(enemyArrangementInfo.blockType == BlockType.MucusDripper)
            {
                block = PoolManager.Spawn(ResourceEnum.Prefab.MucusDrippingBlock).GetComponent<DrippingBlock>();
                ((MovableBlock)block).movePattern = (MovableBlock.MovePattern)enemyArrangementInfo.shieldPosition;
            }
            else
            {
                block = PoolManager.Spawn(ResourceEnum.Prefab.NormalBlock).GetComponent<Block>();
                block.GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (frontStage)
            {
                block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 10.5f, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f - 0.25f + row + 1 + term);
            }
            else
            {
                block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 10.5f, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f - 0.25f + row + 1 + term + row + 1);
            }

            if(block.TryGetComponent(out BoxCollider2D bC))
            {
                foreach(var boxCollider in block.GetComponentsInChildren<BoxCollider2D>())
                {
                    boxCollider.size = enemyArrangementInfo.size;
                }
                block.GetComponent<SpriteRenderer>().size = enemyArrangementInfo.size;
            }
            block.transform.SetParent(board, true);
            if(frontStage)
            {
                block.SetInfo(wantStage, enemyArrangementInfo.maxHP);
                if (clearBothStage) currentStageEnemies.Add(block);
                else nextStageEnemies.Add(block);
            }
            else
            {
                block.SetInfo(currentStage + 1, enemyArrangementInfo.maxHP);
                nextStageEnemies.Add(block);
            }

            block.SetMaskLayer(index++);
        }
    }

    public void StageClearCheck()
    {
        if(currentStageEnemies.Count == 0 && GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            GameManager.Instance.ReadyPhase();
            foreach(var bead in beads)
            {
                bead.trail.emitting = false;
            }
            currentStage++;
        }
    }

    // Input : (Size Vector(x, y), count)
    StageInfo RandomStageGenerate(params (BlockForm, int)[] wantForms)
    {
        StageInfo result = null;
        int[,] stageBoard = new int[row, column];
        foreach(var form in wantForms)
        {
            int exit = 0;
            for(int i=0; i<form.Item2; i++)
            {
                if(++exit > 10000)
                {
                    Debug.LogWarning("Infinity loop has detected!");
                    break;
                }
                // 랜덤하게 배치 시도
                bool discrimination = true;
                int shieldPos = 0;
                int xPos;
                if(form.Item1.blockType == BlockType.Shield || form.Item1.blockType == BlockType.Splitter) xPos = xPos = Random.Range(1, column - form.Item1.size.x);
                else xPos = Random.Range(0, column - form.Item1.size.x + 1);
                int yPos;
                if (form.Item1.blockType == BlockType.Shield)
                {
                    yPos = Random.Range(1, (row - form.Item1.size.y + 1) * 2 / 3);
                    shieldPos = Random.Range(0, 4);
                }
                else if(form.Item1.blockType == BlockType.MucusDripper)
                {
                    yPos = Random.Range(0, row - form.Item1.size.y + 1);
                    shieldPos = form.Item1.moveType;
                }
                else if (form.Item1.blockType == BlockType.Counter) yPos = Random.Range((row - form.Item1.size.y + 1) / 2, row - form.Item1.size.y + 1);
                else yPos = Random.Range(0, row - form.Item1.size.y + 1);
                // 다른 블록이 이미 배치 되어있는지 판별
                for(int wantY = 0; wantY < form.Item1.size.y; wantY++)
                {
                    for(int wantX = 0; wantX < form.Item1.size.x; wantX++)
                    {
                        if (stageBoard[yPos + wantY, xPos + wantX] == 1)
                        {
                            discrimination = false;
                            break;
                        }
                    }
                    if (!discrimination) break;
                }
                // ShieldBlock은 쉴드 방향도 확인
                if(discrimination && form.Item1.blockType == BlockType.Shield)
                {
                    // 0 : down, 1 : left, 2 : right, 3 : up
                    if(shieldPos == 0)
                    {
                        if (stageBoard[yPos - 1, xPos - 1] == 1 || stageBoard[yPos - 1, xPos] == 1 || stageBoard[yPos - 1, xPos + 1] == 1)
                        {
                            discrimination = false;
                        }
                    }
                    else if(shieldPos == 1)
                    {
                        if (stageBoard[yPos - 1, xPos - 1] == 1 || stageBoard[yPos, xPos - 1] == 1 || stageBoard[yPos + 1, xPos - 1] == 1)
                        {
                            discrimination = false;
                        }
                    }
                    else if (shieldPos == 2)
                    {
                        if (stageBoard[yPos - 1, xPos + 1] == 1 || stageBoard[yPos, xPos + 1] == 1 || stageBoard[yPos + 1, xPos + 1] == 1)
                        {
                            discrimination = false;
                        }
                    }
                    else
                    {
                        if (stageBoard[yPos + 1, xPos - 1] == 1 || stageBoard[yPos + 1, xPos] == 1 || stageBoard[yPos + 1, xPos + 1] == 1)
                        {
                            discrimination = false;
                        }
                    }
                }
                // 이미 다른 블록이 있으면 다시
                if(!discrimination)
                {
                    i--;
                    continue;
                }
                // 배치
                EnemyArrangementInfo info;
                if (form.Item1.blockType == BlockType.Shield || form.Item1.blockType == BlockType.MucusDripper) info = new(form.Item1.blockType, new(xPos, yPos), shieldPos);
                else if (form.Item1.blockType == BlockType.Splitter) info = new(form.Item1.blockType, new(xPos,yPos), 4f);
                else info = new(form.Item1.blockType, new(xPos, yPos), form.Item1.size);
                
                if (result == null) result = new(new EnemyArrangementInfo[]{info});
                else result.enemyArrangementInfo.Add(info);
                for (int wantY = 0; wantY < form.Item1.size.y; wantY++)
                {
                    for (int wantX = 0; wantX < form.Item1.size.x; wantX++)
                    {
                        stageBoard[yPos + wantY, xPos + wantX] = 1;
                    }
                }
                if(form.Item1.blockType == BlockType.Shield)
                {
                    if (shieldPos == 0)
                    {
                        stageBoard[yPos - 1, xPos - 1] = 1;
                        stageBoard[yPos - 1, xPos] = 1;
                        stageBoard[yPos - 1, xPos + 1] = 1;
                    }
                    else if (shieldPos == 1)
                    {
                        stageBoard[yPos - 1, xPos - 1] = 1;
                        stageBoard[yPos, xPos - 1] = 1;
                        stageBoard[yPos + 1, xPos - 1] = 1;
                    }
                    else if (shieldPos == 2)
                    {
                        stageBoard[yPos - 1, xPos + 1] = 1;
                        stageBoard[yPos, xPos + 1] = 1;
                        stageBoard[yPos + 1, xPos + 1] = 1;
                    }
                    else
                    {
                        stageBoard[yPos + 1, xPos - 1] = 1;
                        stageBoard[yPos + 1, xPos] = 1;
                        stageBoard[yPos + 1, xPos + 1] = 1;
                    }
                }
            }
        }
        string log = "";
        for (int y = row - 1; y >= 0; y--)
        {
            string line = "";
            for (int x = 0; x < column; x++)
            {
                line += stageBoard[y, x].ToString() + " ";
            }
            log += line + "\n";
        }

        Debug.Log(log);
        return result;
    }

    public void Fever()
    {
        if (currentStageEnemies.Count == 0) return;
        int rand = Random.Range(0, currentStageEnemies.Count);
        Enemy target = currentStageEnemies[rand];
        LineRenderer line = PoolManager.Spawn(ResourceEnum.Prefab.FeverAttack).GetComponent<LineRenderer>();
        line.SetPositions(new Vector3[]{ bar.transform.position, target.transform.position });
        target.TakeDamage(1);
        FeverCharged = false;
        feverGauge = 0;
    }

}

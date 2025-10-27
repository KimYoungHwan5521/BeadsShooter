using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    const int row = 13;
    const int column = 23;

    [SerializeField] Transform board;
    [SerializeField] TextMeshProUGUI stageStartCountText;
    public int currentStage = 0;
    public List<Enemy> currentStageEnemies;
    public List<Enemy> nextStageEnemies;
    public List<GameObject> currentStageWalls;
    public List<GameObject> nextStageWalls;
    public Bar bar;
    public List<Projectile> projectiles;

    readonly float stageStartCount = 3f;
    [SerializeField]float curStageStartCount;

    public enum BlockType { Normal, Hide, Counter, Flower, SpeedUp, Illusion }

    public class BlockForm
    {
        public BlockType blockType;
        public Vector2Int size;

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
    }

    public class EnemyArrangementInfo
    {
        public BlockType blockType;
        public Vector2Int position;
        public Vector2Int size;
        public float maxHP;

        public EnemyArrangementInfo(BlockType blockType, Vector2Int position)
        {
            this.blockType = blockType;
            this.position = position;
            size = new Vector2Int(1, 1);
            maxHP = 1;
        }

        public EnemyArrangementInfo(BlockType blockType, Vector2Int position, Vector2Int size)
        {
            this.blockType = blockType;
            this.position = position;
            this.size = size;
            maxHP = size.x * size.y;
        }
    }

    public class StageInfo
    {
        // Stage 크기 : 가로 0~8, 세로 0~13
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
            RandomStageGenerate((new(BlockType.Normal, new(2,1)), 10), (new(BlockType.Counter), 5)),
            //RandomStageGenerate((new(BlockType.Normal, new(1,1)), 5), new(new(BlockType.Normal, new(2,1)), 5), (new(BlockType.Normal, new(1,2)), 5), (new(BlockType.Normal, new(2,2)), 5), (new(BlockType.Hide), 1)),
            // Stage 1
            RandomStageGenerate((new(BlockType.Normal, new(2,1)), 5), (new(BlockType.Normal, new(1,2)), 5), (new(BlockType.Normal, new(2,2)), 5), (new(BlockType.Hide), 5)),
        };
    }

    private void Update()
    {
        if (wantDown > 0)
        {
            board.transform.position += Vector3.down * 0.2f;
            foreach(var projectile in projectiles)
            {
                if (projectile.transform.position.y > bar.transform.position.y + 3)
                {
                    projectile.transform.position += Vector3.down * 0.2f;
                    Vector3[] points = new Vector3[projectile.trail.positionCount];
                    projectile.trail.GetPositions(points);
                    for(int i=0; i<points.Length; i++) points[i] += Vector3.down * 0.2f;
                    projectile.trail.SetPositions(points);
                }
            }
            wantDown -= 0.2f;
            if(wantDown <= 0)
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
                foreach (var projectile in projectiles) projectile.trail.emitting = true;
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
        if (wantStage >= stageInfos.Length) nextStageInfo = stageInfos[^1];
        else nextStageInfo = stageInfos[wantStage];
        SpawnBlocks(nextStageInfo, wantStage, clearBothStage, true);

        foreach(GameObject wall in currentStageWalls)
        {
            PoolManager.Despawn(wall);
        }
        if(!clearBothStage) currentStageWalls = nextStageWalls.ToList();
        else currentStageWalls.Clear();
        nextStageWalls.Clear();
        for(int i=-11; i<=11; i++)
        {
            Block block = PoolManager.Spawn(ResourceEnum.Prefab.NormalBlock).GetComponent<Block>();
            if (currentStage == 0) block.transform.position = new(i, 13 + 14 + 3);
            else block.transform.position = new(i, 3 + 30);
            block.transform.SetParent(board, true);
            block.SetInfo(currentStage, float.MaxValue);
            if (clearBothStage) currentStageWalls.Add(block.gameObject);
            else nextStageWalls.Add(block.gameObject);
        }
        // 2스테이지 동시에 깬 경우 다다음 스테이지
        if(clearBothStage)
        {
            if (currentStage < stageInfos.Length) nextStageInfo = stageInfos[currentStage + 1];
            else nextStageInfo = stageInfos[^1];
            SpawnBlocks(nextStageInfo, wantStage, clearBothStage, false);
            foreach (GameObject wall in nextStageWalls)
            {
                PoolManager.Despawn(wall);
            }
            nextStageWalls.Clear();
            for (int i = -11; i <= 11; i++)
            {
                Block block = PoolManager.Spawn(ResourceEnum.Prefab.NormalBlock).GetComponent<Block>();
                block.transform.position = new(i, 14 + 2 + 29);
                block.transform.SetParent(board, true);
                block.SetInfo(currentStage + 1, float.MaxValue);
                nextStageWalls.Add(block.gameObject);
            }
        }

        // 내려가는 애니메이션
        //if (!clearBothStage) board.transform.position += Vector3.down * 13;
        //else board.transform.position += Vector3.down * 26;
        wantDown = clearBothStage ? 28 : 14;
    }

    void SpawnBlocks(StageInfo nextStageInfo, int wantStage, bool clearBothStage, bool frontStage)
    {
        foreach (EnemyArrangementInfo enemyArrangementInfo in nextStageInfo.enemyArrangementInfo)
        {
            Block block;
            if(enemyArrangementInfo.blockType == BlockType.Hide)
            {
                block = PoolManager.Spawn(ResourceEnum.Prefab.HideBlock).GetComponent<HideBlock>();
                ((HideBlock)block).SetShield(enemyArrangementInfo.position.x >= column / 2 + 1, enemyArrangementInfo.position.x < column / 2);
            }
            else if(enemyArrangementInfo.blockType == BlockType.Counter) block = PoolManager.Spawn(ResourceEnum.Prefab.CounterBlock).GetComponent<CounterBlock>();
            else block = PoolManager.Spawn(ResourceEnum.Prefab.NormalBlock).GetComponent<Block>();
            if (frontStage)
            {
                if (currentStage == 0) block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 11, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f + 14 + 2);
                else block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 11, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f + 14 + 6);
            }
            else
            {
                block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 11, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f + 14 + 2 + 15);
            }
            block.GetComponent<BoxCollider2D>().size = enemyArrangementInfo.size;
            block.GetComponent<SpriteRenderer>().size = enemyArrangementInfo.size;
            block.transform.SetParent(board, true);
            if(frontStage)
            {
                if (currentStage > 0) block.SetInfo(wantStage, enemyArrangementInfo.maxHP);
                else block.SetInfo(wantStage, enemyArrangementInfo.maxHP);
                if (clearBothStage) currentStageEnemies.Add(block);
                else nextStageEnemies.Add(block);
            }
            else
            {
                block.SetInfo(currentStage + 1, enemyArrangementInfo.maxHP);
                nextStageEnemies.Add(block);
            }
        }
    }

    public void StageClearCheck()
    {
        if(currentStageEnemies.Count == 0 && GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            GameManager.Instance.ReadyPhase();
            foreach(var projectile in projectiles) projectile.trail.emitting = false;
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
                int xPos = Random.Range(0, column - form.Item1.size.x + 1);
                int yPos;
                if (form.Item1.blockType == BlockType.Hide) yPos = Random.Range(0, (row - form.Item1.size.y + 1) * 2 / 3);
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
                // HideBlock은 좌/우, 아래도 확인
                if(discrimination && form.Item1.blockType == BlockType.Hide)
                {
                    if(yPos > 0)
                    {
                        if(xPos <= column / 2 && stageBoard[yPos - 1, xPos + 1] == 1
                            || xPos > column / 2 + 1 && stageBoard[yPos - 1, xPos - 1] == 1
                            || stageBoard[yPos - 1, xPos] == 1)
                        {
                            discrimination = false;
                        }
                    }
                    else
                    {
                        //if (xPos >= 0 && xPos < column / 2 && stageBoard[yPos, xPos + 1] == 1
                        //    || xPos < column && xPos >= column / 2 + 1 && stageBoard[yPos, xPos - 1] == 1)
                        {
                            discrimination = false;
                        }
                    }
                    if(xPos <= column / 2 && stageBoard[yPos, xPos + 1] == 1
                            || xPos > column / 2 + 1 && stageBoard[yPos, xPos - 1] == 1)
                    {
                        discrimination = false;
                    }
                }
                // 이미 다른 블록이 있으면 다시
                if(!discrimination)
                {
                    i--;
                    continue;
                }
                // 배치
                if (result == null) result = new(new EnemyArrangementInfo[]{new(form.Item1.blockType, new(xPos, yPos), form.Item1.size)});
                else result.enemyArrangementInfo.Add(new(form.Item1.blockType, new(xPos, yPos), form.Item1.size));
                for (int wantY = 0; wantY < form.Item1.size.y; wantY++)
                {
                    for (int wantX = 0; wantX < form.Item1.size.x; wantX++)
                    {
                        stageBoard[yPos + wantY, xPos + wantX] = 1;
                    }
                }
                if(form.Item1.blockType == BlockType.Hide)
                {
                    if (xPos <= column / 2) stageBoard[yPos, xPos + 1] = 1;
                    else if (xPos > column / 2 + 1) stageBoard[yPos, xPos - 1] = 1;
                    if (yPos > 0)
                    {
                        stageBoard[yPos - 1, xPos] = 1;
                        if (xPos <= column / 2) stageBoard[yPos - 1, xPos + 1] = 1;
                        else if (xPos > column / 2 + 1) stageBoard[yPos - 1, xPos - 1] = 1;
                    }
                }
            }
        }
        string log = "";
        for (int y = row - 1; y > 0; y--)
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
}

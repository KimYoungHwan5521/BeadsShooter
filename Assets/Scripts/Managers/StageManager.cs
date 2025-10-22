using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] Transform board;
    public int currentStage = 0;
    public List<Enemy> currentStageEnemies;
    public List<Enemy> nextStageEnemies;
    public List<GameObject> currentStageWalls;
    public List<GameObject> nextStageWalls;
    public Bar bar;
    public List<Projectile> projectiles;

    public class EnemyArrangementInfo
    {
        public Vector2Int position;
        public Vector2Int size;
        public float maxHP;

        public EnemyArrangementInfo(Vector2Int position)
        {
            this.position = position;
            size = new Vector2Int(1, 1);
            maxHP = 1;
        }

        public EnemyArrangementInfo(Vector2Int position, Vector2Int size)
        {
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
            RandomStageGenerate((new(1,1), 10)),
            // Stage 1
            RandomStageGenerate((new(1,1), 10), (new(2, 1), 5), (new(1, 2), 5)),
        };
    }

    private void Update()
    {
        if (wantDown > 0)
        {
            board.transform.position += Vector3.down * 0.2f;
            wantDown -= 0.2f;
            if(wantDown <= 0) GameManager.Instance.BattlePhaseStart();
        }
    }

    public void StageSetting()
    {
        bool clearBothStage = nextStageEnemies.Count == 0;
        Debug.Log(clearBothStage);
        Debug.Log($"currentStage : {currentStage}");
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
        foreach (EnemyArrangementInfo enemyArrangementInfo in nextStageInfo.enemyArrangementInfo)
        {
            Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
            if (currentStage == 0) block.transform.position = new(enemyArrangementInfo.position.x - 11, enemyArrangementInfo.position.y + 14 + 2);
            else block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 10, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f + 14 + 6);
            block.GetComponent<BoxCollider2D>().size = enemyArrangementInfo.size;
            block.GetComponent<SpriteRenderer>().size = enemyArrangementInfo.size;
            block.transform.SetParent(board, true);
            if (currentStage > 0) block.SetInfo(wantStage, enemyArrangementInfo.maxHP);
            else block.SetInfo(wantStage, enemyArrangementInfo.maxHP);
            if (clearBothStage) currentStageEnemies.Add(block);
            else nextStageEnemies.Add(block);
        }

        foreach(GameObject wall in currentStageWalls)
        {
            PoolManager.Despawn(wall);
        }
        if(!clearBothStage) currentStageWalls = nextStageWalls.ToList();
        else currentStageWalls.Clear();
        nextStageWalls.Clear();
        for(int i=-11; i<=11; i++)
        {
            Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
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
            foreach (EnemyArrangementInfo enemyArrangementInfo in nextStageInfo.enemyArrangementInfo)
            {
                Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
                block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 11, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f + 14 + 2 + 15);
                block.GetComponent<BoxCollider2D>().size = enemyArrangementInfo.size;
                block.GetComponent<SpriteRenderer>().size = enemyArrangementInfo.size;
                block.transform.SetParent(board, true);
                block.SetInfo(currentStage + 1, enemyArrangementInfo.maxHP);
                nextStageEnemies.Add(block);
            }
            foreach (GameObject wall in nextStageWalls)
            {
                PoolManager.Despawn(wall);
            }
            nextStageWalls.Clear();
            for (int i = -11; i <= 11; i++)
            {
                Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
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

    public void StageClearCheck()
    {
        if(currentStageEnemies.Count == 0 && GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            GameManager.Instance.ReadyPhase();
            currentStage++;
        }
    }

    // Input : (Size Vector(x, y), count)
    StageInfo RandomStageGenerate(params (Vector2Int, int)[] wantForms)
    {
        StageInfo result = null;
        int[,] stageBoard = new int[13, 23];
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
                int xPos = Random.Range(0, stageBoard.GetLength(1) - form.Item1.x + 1);
                int yPos = Random.Range(0, stageBoard.GetLength(0) - form.Item1.y + 1);
                // 다른 블록이 이미 배치 되어있는지 판별
                for(int wantY = 0; wantY < form.Item1.y; wantY++)
                {
                    for(int wantX = 0; wantX < form.Item1.x; wantX++)
                    {
                        if (stageBoard[yPos + wantY, xPos + wantX] == 1)
                        {
                            discrimination = false;
                            break;
                        }
                    }
                    if (!discrimination) break;
                }
                // 이미 다른 블록이 있으면 다시
                if(!discrimination)
                {
                    i--;
                    continue;
                }
                // 배치
                if (result == null) result = new(new EnemyArrangementInfo[]{new(new(xPos, yPos), form.Item1)});
                else result.enemyArrangementInfo.Add(new(new(xPos, yPos), form.Item1));
                for (int wantY = 0; wantY < form.Item1.y; wantY++)
                {
                    for (int wantX = 0; wantX < form.Item1.x; wantX++)
                    {
                        stageBoard[yPos + wantY, xPos + wantX] = 1;
                    }
                }
            }
        }
        return result;
    }
}

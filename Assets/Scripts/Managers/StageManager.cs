using UnityEngine;
using System.Collections.Generic;

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
        public float maxHP;

        public EnemyArrangementInfo(Vector2Int position, float maxHP)
        {
            this.position = position;
            this.maxHP = maxHP;
        }
    }

    public class StageInfo
    {
        // Stage 크기 : 가로 0~8, 세로 0~13
        public EnemyArrangementInfo[] enemyArrangementInfo;

        public StageInfo(EnemyArrangementInfo[] enemyArrangementInfo)
        {
            this.enemyArrangementInfo = enemyArrangementInfo;
        }
    }

    public StageInfo[] stageInfos = {
        // Stage 0
        new(new EnemyArrangementInfo[] { new(new(0, 10), 1), new(new(1, 10), 1), new(new(2, 10), 1), new(new(3, 10), 1),new(new(4, 10), 1), new(new(5, 10), 1), new(new(6, 10), 1), new(new(7, 10), 1), new(new(8, 10), 1),}),
        // Stage 1
        new(new EnemyArrangementInfo[] { new(new(0, 7), 2), new(new(1, 7), 2), new(new(2, 7), 2), new(new(3, 7), 2),new(new(4, 7), 2), new(new(5, 7), 2), new(new(6, 7), 2), new(new(7, 7), 2), new(new(8, 7), 2),}),
        // Stage 2
        new(new EnemyArrangementInfo[] { new(new(0, 7), 3), new(new(1, 7), 3), new(new(2, 7), 3), new(new(3, 7), 3),new(new(4, 7), 3), new(new(5, 7), 3), new(new(6, 7), 3), new(new(7, 7), 3), new(new(8, 7), 3),}),
        // Stage 3
        new(new EnemyArrangementInfo[] { new(new(0, 7), 4), new(new(1, 7), 4), new(new(2, 7), 4), new(new(3, 7), 4),new(new(4, 7), 4), new(new(5, 7), 4), new(new(6, 7), 4), new(new(7, 7), 4), new(new(8, 7), 4),}),
    };

    float wantDown;

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
            currentStageEnemies = nextStageEnemies;
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
            block.transform.position = new(enemyArrangementInfo.position.x - 10, enemyArrangementInfo.position.y + 16);
            Debug.Log($"block : {block.transform.position}");
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
        if(!clearBothStage) currentStageWalls = nextStageWalls;
        else currentStageWalls.Clear();
        nextStageWalls.Clear();
        for(int i=-11; i<=11; i++)
        {
            Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
            block.transform.position = new(i, 30);
            Debug.Log($"wall : {block.transform.position}");
            block.transform.SetParent(board, true);
            block.SetInfo(currentStage, float.MaxValue);
            currentStageWalls.Add(block.gameObject);
        }
        // 2스테이지 동시에 깬 경우 다다음 스테이지
        if(clearBothStage)
        {
            if (currentStage < stageInfos.Length) nextStageInfo = stageInfos[currentStage + 1];
            else nextStageInfo = stageInfos[^1];
            foreach (EnemyArrangementInfo enemyArrangementInfo in nextStageInfo.enemyArrangementInfo)
            {
                Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
                block.transform.position = new(enemyArrangementInfo.position.x * 2 - 10, enemyArrangementInfo.position.y + 30);
                Debug.Log($"block : {block.transform.position}");
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
                block.transform.position = new(i, 45);
                Debug.Log($"wall : {block.transform.position}");
                block.transform.SetParent(board, true);
                block.SetInfo(currentStage + 1, float.MaxValue);
                nextStageWalls.Add(block.gameObject);
            }
        }

        // 내려가는 애니메이션
        //if (!clearBothStage) board.transform.position += Vector3.down * 13;
        //else board.transform.position += Vector3.down * 26;
        wantDown = clearBothStage ? 26 : 13;
    }

    public void StageClearCheck()
    {
        if(currentStageEnemies.Count == 0)
        {
            GameManager.Instance.ReadyPhase();
            currentStage++;
        }
    }
}

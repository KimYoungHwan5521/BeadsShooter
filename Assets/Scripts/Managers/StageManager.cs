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

    public class EnemyArrangementInfo
    {
        public Vector2Int position;
        public float maxHP;
        public float moveSpeed;

        public EnemyArrangementInfo(Vector2Int position, float maxHP, float moveSpeed = 0.1f)
        {
            this.position = position;
            this.maxHP = maxHP;
            this.moveSpeed = moveSpeed;
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
        new(new EnemyArrangementInfo[] { new(new(0, 12), 1), new(new(1, 12), 1), new(new(2, 12), 1), new(new(3, 12), 1),new(new(4, 12), 1), new(new(5, 12), 1), new(new(6, 12), 1), new(new(7, 12), 1), new(new(8, 12), 1),}),
        // Stage 1
        new(new EnemyArrangementInfo[] { new(new(0, 7), 2), new(new(1, 7), 2), new(new(2, 7), 2), new(new(3, 7), 2),new(new(4, 7), 2), new(new(5, 7), 2), new(new(6, 7), 2), new(new(7, 7), 2), new(new(8, 7), 2),}),
        // Stage 2
        new(new EnemyArrangementInfo[] { new(new(0, 7), 3), new(new(1, 7), 3), new(new(2, 7), 3), new(new(3, 7), 3),new(new(4, 7), 3), new(new(5, 7), 3), new(new(6, 7), 3), new(new(7, 7), 3), new(new(8, 7), 3),}),
        // Stage 3
        new(new EnemyArrangementInfo[] { new(new(0, 7), 4), new(new(1, 7), 4), new(new(2, 7), 4), new(new(3, 7), 4),new(new(4, 7), 4), new(new(5, 7), 4), new(new(6, 7), 4), new(new(7, 7), 4), new(new(8, 7), 4),}),
    };

    public void StageSetting()
    {
        bool clearBothStage = nextStageEnemies.Count == 0;
        if(!clearBothStage)
        {
            currentStageEnemies = nextStageEnemies;
            nextStageEnemies = new();
            currentStageWalls = nextStageWalls;
            nextStageWalls = new();
        }

        StageInfo nextStageInfo;
        if(clearBothStage && currentStage > 0) currentStage++;
        if (currentStage - 1 < stageInfos.Length) nextStageInfo = stageInfos[currentStage];
        else nextStageInfo = stageInfos[^1];
        foreach (EnemyArrangementInfo enemyArrangementInfo in nextStageInfo.enemyArrangementInfo)
        {
            Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
            block.transform.position = new(enemyArrangementInfo.position.x * 0.5f - 2, enemyArrangementInfo.position.y * 0.2f + 4.2f);
            block.transform.SetParent(board);
            block.SetInfo(currentStage, enemyArrangementInfo.maxHP, enemyArrangementInfo.moveSpeed);
            if (clearBothStage) currentStageEnemies.Add(block);
            else nextStageEnemies.Add(block);
        }
        foreach(GameObject wall in currentStageWalls)
        {
            PoolManager.Despawn(wall);
        }
        currentStageWalls.Clear();
        for(int i=0; i<9; i++)
        {
            if (clearBothStage && (i == 0 || i == 8)) continue;
            Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
            block.transform.position = new(i * 0.5f - 2, 7);
            block.transform.SetParent(board);
            block.SetInfo(currentStage, float.MaxValue, 0);
            if(clearBothStage || i == 0 || i == 8) currentStageWalls.Add(block.gameObject);
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
                block.transform.position = new(enemyArrangementInfo.position.x * 0.5f - 2, enemyArrangementInfo.position.y * 0.2f + 7.2f);
                block.transform.SetParent(board);
                block.SetInfo(currentStage + 1, enemyArrangementInfo.maxHP, enemyArrangementInfo.moveSpeed);
                nextStageEnemies.Add(block);
            }
            foreach (GameObject wall in nextStageWalls)
            {
                PoolManager.Despawn(wall);
            }
            nextStageWalls.Clear();
            for (int i = 0; i < 9; i++)
            {
                Block block = PoolManager.Spawn(ResourceEnum.Prefab.Block).GetComponent<Block>();
                block.transform.position = new(i * 0.5f - 2, 10);
                block.transform.SetParent(board);
                block.SetInfo(currentStage + 1, float.MaxValue, 0);
                if (i == 0 || i == 8) currentStageWalls.Add(block.gameObject);
                else nextStageWalls.Add(block.gameObject);
            }
        }
        if (!clearBothStage) board.transform.position += Vector3.down * 3;
        else board.transform.position += Vector3.down * 6;
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

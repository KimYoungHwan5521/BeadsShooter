using System.Collections;
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
    [SerializeField] TextMeshProUGUI currentStageText;

    [SerializeField] TextMeshProUGUI coinText;
    int coin;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            coinText.text = $"{coin}";
        }
    }

    [SerializeField] Image lifeImage;
    [SerializeField] Sprite[] lifeSprites;
    int life;
    public int Life
    {
        get => life;
        set
        {
            if (value >= lifeSprites.Length)
            {
                return;
            }
            else if(value < 0)
            {
                // GameOver();
                return;
            }
            life = value;
            lifeImage.sprite = lifeSprites[value];
        }
    }

    [Header("Ingame")]
    [SerializeField] Transform board;
    [SerializeField] TextMeshProUGUI stageStartCountText;
    int selectedStageIndex;
    public int currentStage = 0;
    public List<Enemy> currentStageEnemies;
    public List<Enemy> nextStageEnemies;
    public List<GameObject> currentStageWalls;
    public List<GameObject> nextStageWalls;
    public Bar bar;
    public List<Bead> beads;
    public List<Coin> coins = new();
    public List<Projectile> projectiles = new();
    [SerializeField] GameObject ongoingQuestsBox;
    [SerializeField] GameObject[] ongoingQuestsObject;
    [SerializeField] Image questHideButtonArrow;
    public List<QuestManager.Quest> ongoingQuests = new();

    [Header("Shop")]
    [SerializeField] TextMeshProUGUI shopFreeRerollText;
    [SerializeField] TextMeshProUGUI shopRerollCostText;
    int shopRerollStack;
    public int RerollCost => shopFreeReroll > 0 ? 0 : (shopRerollStack + 1) * (shopRerollStack + 2);

    int shopFreeReroll;
    public int ShopFreeReroll
    {
        get => shopFreeReroll;
        set
        {
            shopFreeReroll = Mathf.Max(value, 0);
            shopFreeRerollText.text = $"Free Reroll x {shopFreeReroll}";
            if (Coin < RerollCost) shopRerollCostText.color = Color.red;
            else shopRerollCostText.color = RerollCost > 0 ? Color.black : Color.green;
            shopRerollCostText.text = $"{RerollCost}";
        }
    }

    RewardFormat[] randomRewards = new RewardFormat[] { new(RewardType.AttackDamage, 0.2f) };

    [Header("Ability")]
    public List<AbilityManager.Ability> possibleToAppearAbilities;
    public List<AbilityManager.Ability> selectedAbilities;

    [SerializeField] GameObject pauseUI;

    public int currentSelectedCharacterIndex;

    const float stageStartCount = 3f;
    [SerializeField]float curStageStartCount;
    bool readyToReadyPhase;
    const float readyToReadyPhaseTime = 2f;
    float curReadyToReadyPhaseTime;
    public bool beadRefill;
    const float beadRefillTime = 1f;
    float curBeadRefillTime;

    bool stageClear;

    #region Stage Info
    public enum BlockType 
    { 
        Normal, Wall, Shield, Counter, PentagonalBlock, SpeedUp, Illusion, Attacker, Splitter, MucusDripper, Boss1, Boss2
    }

    public class BlockForm
    {
        public BlockType blockType;
        public Vector2Int size;
        // MovableBlock
        public int moveType;
        public float maxHP;

        public BlockForm(BlockType blockType)
        {
            this.blockType = blockType;
            size = new Vector2Int(1,1);
            maxHP = 1f;
        }

        public BlockForm(BlockType blockType, Vector2Int size)
        {
            this.blockType = blockType;
            this.size = size;
            maxHP = Mathf.Max(size.x * size.y / 2, 1);
        }

        public BlockForm(BlockType blockType, Vector2Int size, float maxHP)
        {
            this.blockType = blockType;
            this.size = size;
            this.maxHP = maxHP;
        }

        public BlockForm(BlockType blockType, int moveType)
        {
            this.blockType = blockType;
            size = new Vector2Int(1, 1);
            this.moveType = moveType;
            maxHP = 1f;
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

        public EnemyArrangementInfo(BlockType blockType, Vector2Int position, Vector2Int size, float maxHP)
        {
            this.blockType = blockType;
            this.position = position;
            this.size = size;
            this.maxHP = maxHP;
        }

        public EnemyArrangementInfo(BlockType blockType, Vector2Int position, int shieldPos)
        {
            this.blockType = blockType;
            this.position = position;
            size = new Vector2Int(4, 2);
            maxHP = 1;
            shieldPosition = shieldPos;
        }
    }

    public class StageInfo
    {
        public List<EnemyArrangementInfo> enemyArrangementInfo;
        // Stage type : 0 - normal, 1 - shop, 2 - boss
        public int stageType;

        public StageInfo(EnemyArrangementInfo[] enemyArrangementInfo, int stageType = 0)
        {
            this.enemyArrangementInfo = enemyArrangementInfo.ToList();
            this.stageType = stageType;
        }
    }

    public StageInfo[] selectedStageInfos;
    public enum Stage { Practice, Slime, Portal }
    public List<StageInfo[]> stages = new();
    #endregion
    float wantDown;

    private void Start()
    {
        SetStages();
    }

    void SetStages()
    {
        stages.Clear();
        StageInfo[] stageInfos = new[]
        {
            // Stage 0
            //GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            //GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            //GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            //GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            //GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 1)),
            //GenerateRandomStage((new(BlockType.Shield, new Vector2Int(4,2), 1), 10)),
            //GenerateRandomStage((new(BlockType.Normal, new Vector2Int(4,2), 1), 10)),
            //GenerateRandomStage((new(BlockType.Attacker, new Vector2Int(2,1), 10), 10)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 5)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 10)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 15)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 10), (new(BlockType.Normal, new Vector2Int(2, 2)), 10)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 10), (new(BlockType.Normal, new Vector2Int(2, 2)), 10), (new(BlockType.Shield, new Vector2Int(4, 2)), 5)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 10), (new(BlockType.Normal, new Vector2Int(2, 2)), 10), (new(BlockType.Normal, new Vector2Int(4, 2)), 5), (new(BlockType.Shield, new Vector2Int(4, 2)), 5)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(4, 2)), 10), (new(BlockType.Shield, new Vector2Int(4, 2)), 10)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(4, 2)), 10), (new(BlockType.Shield, new Vector2Int(4, 2)), 10), (new(BlockType.Counter, new Vector2Int(2,2), 1), 3)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(4, 2)), 10), (new(BlockType.Shield, new Vector2Int(4, 2)), 10), (new(BlockType.Counter, new Vector2Int(2,2), 1), 3), (new(BlockType.Attacker, new Vector2Int(2, 1)), 5)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(4, 2)), 10), (new(BlockType.Shield, new Vector2Int(4, 2)), 10), (new(BlockType.Counter, new Vector2Int(2,2), 1), 5), (new(BlockType.Attacker, new Vector2Int(2, 1)), 5)),
            //GenerateShopStage(),
            GenerateBossStage(BlockType.Boss1),

        };
        stages.Add(stageInfos);
        
        stageInfos = new StageInfo[]
        {
            // Stage 1
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 5), (new(BlockType.Normal, new Vector2Int(1, 2)), 5), (new(BlockType.MucusDripper, 2), 2)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 1)), 5), (new(BlockType.Normal, new Vector2Int(2, 2)), 5), (new(BlockType.MucusDripper, 2), 2)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 2)), 8), (new(BlockType.Shield, new Vector2Int(2, 1)), 8), (new(BlockType.MucusDripper, 2), 4)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 2)), 4), (new(BlockType.Shield, new Vector2Int(2, 1)), 8), (new(BlockType.Splitter, new Vector2Int(2,1)), 4), (new(BlockType.MucusDripper, 2), 4)),
            GenerateRandomStage((new(BlockType.Normal, new Vector2Int(2, 2)), 4), (new(BlockType.Shield, new Vector2Int(2, 1)), 8), (new(BlockType.Splitter, new Vector2Int(2,1)), 8), (new(BlockType.MucusDripper, 2), 4)),
            GenerateShopStage(),
            GenerateBossStage(BlockType.Boss2),
        };
        stages.Add(stageInfos);
    }

    private void Update()
    {
        if (wantDown > 0.1f)
        {
            board.transform.position += Vector3.down * 0.2f;
            if (selectedStageInfos[currentStage].stageType == 0)
            {
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
            }
            else
            {
                // 공 회수
                for (int i = 0; i < beads.Count; i++)
                {
                    if (!beads[i].activated) continue;
                    beads[i].trail.Clear();
                    int reverse = i % 2 == 0 ? 1 : -1;
                    Vector2 destination = new(bar.transform.position.x + reverse * ((i + 1) / 2), bar.transform.position.y + 0.51f);
                    if (Vector2.Distance(beads[i].transform.position, destination) < 0.3f) continue;
                    beads[i].transform.position += (Vector3)((destination - (Vector2)beads[i].transform.position)).normalized * 20 * Time.unscaledDeltaTime;
                    beads[i].Strike = 0;
                }
            }
            wantDown -= 0.2f;
            if(wantDown < 0.1f)
            {
                if (currentStage > 0)
                {
                    curStageStartCount = stageStartCount;
                    stageStartCountText.gameObject.SetActive(true);
                    if (selectedStageInfos[currentStage].stageType > 0)
                    {
                        foreach(Bead bead in beads)
                        {
                            bar.grabbedBeads.Add(bead);
                            bead.SetDirection(Vector2.zero);
                            bead.activated = false;
                        }
                    }
                }
                else
                {
                    GameManager.Instance.BattlePhaseStart();
                }
            }
        }
        if(curStageStartCount > 0 && !pauseUI.activeSelf)
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

        if(readyToReadyPhase)
        {
            List<Coin> collected = new();
            foreach(Coin coin in coins)
            {
                if(!coin.gameObject.activeSelf) continue;
                coin.transform.position += (Vector3)((Vector2)(bar.transform.position - coin.transform.position)).normalized * 20 * Time.unscaledDeltaTime;
                if(Vector2.Distance(coin.transform.position, bar.transform.position) < 0.2f) collected.Add(coin);
            }
            foreach(Coin coin in collected) coin.BeCollected();
            curReadyToReadyPhaseTime += Time.unscaledDeltaTime;
            if(curReadyToReadyPhaseTime > readyToReadyPhaseTime)
            {
                coins.Clear();
                if (stageClear)
                {
                    GameManager.Instance.StageClear();
                    stageClear = false;
                }
                else GameManager.Instance.ReadyPhase();
                readyToReadyPhase = false;
                curReadyToReadyPhaseTime = 0;
            }
        }

        if(beadRefill)
        {
            curBeadRefillTime += Time.deltaTime;
            if(curBeadRefillTime > beadRefillTime)
            {
                BeadRefill();
                curBeadRefillTime = 0;
                beadRefill = false;
            }
        }
    }

    public void Initiate(int currentStageIndex)
    {
        SetStages();
        board.transform.position = Vector3.zero;
        wantDown = 0;
        selectedStageIndex = currentStageIndex;
        selectedStageInfos = stages[currentStageIndex];
        currentStage = 0;
        Life = 3;
        Coin = 0;
        shopRerollStack = 0;
        ShopFreeReroll = 0;
        //GameManager.Instance.readyPhaseUI.StoreCapacity = 1;
        ongoingQuests.Clear();

        possibleToAppearAbilities = new();
        possibleToAppearAbilities.Add(AbilityManager.Abilities.Find(x => x.name == AbilityManager.AbilityName.Ice));
        possibleToAppearAbilities.Add(AbilityManager.Abilities.Find(x => x.name == AbilityManager.AbilityName.Fire));
        possibleToAppearAbilities.Add(AbilityManager.Abilities.Find(x => x.name == AbilityManager.AbilityName.Telekinesis));
        possibleToAppearAbilities.Add(AbilityManager.Abilities.Find(x => x.name == AbilityManager.AbilityName.Steel));

        selectedAbilities = new();
    }

    public void StageSetting()
    {
        bool clearBothStage = nextStageEnemies.Count == 0;
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
        nextStageInfo = null;
        if (wantStage < selectedStageInfos.Length) nextStageInfo = selectedStageInfos[wantStage];

        if(nextStageInfo != null)
        {
            if (nextStageInfo.stageType == 0) SpawnBlocks(nextStageInfo, wantStage, clearBothStage, true);
            else if (nextStageInfo.stageType == 1) SpawnShop(clearBothStage, true);
            else SpawnBoss(nextStageInfo, wantStage);
        }

        foreach (GameObject wall in currentStageWalls)
        {
            PoolManager.Despawn(wall);
        }

        currentStageText.text = $"Stage {selectedStageIndex} - {currentStage + 1}";
        if(!clearBothStage)
        {
            currentStageWalls = nextStageWalls.ToList();
            if(selectedStageInfos.Length > currentStage + 1 && selectedStageInfos[currentStage + 1].stageType == 0)
            {
                foreach(GameObject wall in currentStageWalls)
                {
                    wall.GetComponent<Block>().isInvincible = false;
                }
            }
        }
        else
        {
            currentStageWalls.Clear();
            foreach (GameObject wall in nextStageWalls)
            {
                PoolManager.Despawn(wall);
            }
        }
        nextStageWalls.Clear();
        if (selectedStageInfos.Length > currentStage + 1 && selectedStageInfos[currentStage + 1].stageType < 2)
        {
            for(int i=-5; i<=5; i++)
            {
                Block wall = PoolManager.Spawn(ResourceEnum.Prefab.Wall).GetComponent<Block>();
                if (currentStageEnemies.Contains(wall)) currentStageEnemies.Remove(wall);
                if (nextStageEnemies.Contains(wall)) nextStageEnemies.Remove(wall);
                if (currentStage == 0) wall.transform.position = new(i * 2, -0.25f + row + 1 + term + row);
                else wall.transform.position = new(i * 2, -0.25f + row + 1 + term + row);
                wall.transform.SetParent(board, true);
                wall.GetComponent<BoxCollider2D>().size = new(2,1);
                wall.GetComponent<SpriteRenderer>().size = new(2,1);
                wall.SetInfo(currentStage, 3, true, currentStage > 0);
                if (clearBothStage) currentStageWalls.Add(wall.gameObject);
                else nextStageWalls.Add(wall.gameObject);
            }
        }
        // 2스테이지 동시에 깬 경우 다다음 스테이지
        if(clearBothStage)
        {
            Debug.Log("Clear both stage");
            nextStageInfo = null;
            if (currentStage + 1 < selectedStageInfos.Length) nextStageInfo = selectedStageInfos[currentStage + 1];

            if (nextStageInfo != null)
            {
                if (nextStageInfo.stageType == 0) SpawnBlocks(nextStageInfo, wantStage, clearBothStage, false);
                else if (nextStageInfo.stageType == 1) SpawnShop(clearBothStage, false);
                else SpawnBoss(nextStageInfo, wantStage);

                for (int i = -5; i <= 5; i++)
                {
                    Block wall = PoolManager.Spawn(ResourceEnum.Prefab.Wall).GetComponent<Block>();
                    wall.transform.position = new(i * 2, -0.25f + row + 1 + term + row + 1 + row);
                    if (currentStageEnemies.Contains(wall)) currentStageEnemies.Remove(wall);
                    if (nextStageEnemies.Contains(wall)) nextStageEnemies.Remove(wall);
                    wall.transform.SetParent(board, true);
                    wall.GetComponent<BoxCollider2D>().size = new(2, 1);
                    wall.GetComponent<SpriteRenderer>().size = new(2, 1);
                    wall.SetInfo(currentStage + 1, 3, true, true);
                    nextStageWalls.Add(wall.gameObject);
                }
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
                else if (enemyArrangementInfo.shieldPosition == 1) leftShield = true;
                else if (enemyArrangementInfo.shieldPosition == 2) rightShield = true;
                else if (enemyArrangementInfo.shieldPosition == 3) upShield = true;
                ((ShieldBlock)block).SetShield(leftShield, rightShield, downShield, upShield);
            }
            else if (enemyArrangementInfo.blockType == BlockType.Counter) block = PoolManager.Spawn(ResourceEnum.Prefab.CounterBlock).GetComponent<CounterBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.PentagonalBlock) block = PoolManager.Spawn(ResourceEnum.Prefab.PentagonalBlock).GetComponent<PentagonalBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.SpeedUp) block = PoolManager.Spawn(ResourceEnum.Prefab.SpeedUpBlock).GetComponent<SpeedUpBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.Illusion) block = PoolManager.Spawn(ResourceEnum.Prefab.IllusionBlock).GetComponent<IllusionBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.Attacker) block = PoolManager.Spawn(ResourceEnum.Prefab.AttackerBlock).GetComponent<AttackerBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.Splitter) block = PoolManager.Spawn(ResourceEnum.Prefab.SplitterBlock).GetComponent<SplitterBlock>();
            else if (enemyArrangementInfo.blockType == BlockType.MucusDripper)
            {
                block = PoolManager.Spawn(ResourceEnum.Prefab.MucusDrippingBlock).GetComponent<DrippingBlock>();
                ((MovableBlock)block).movePattern = (MovableBlock.MovePattern)enemyArrangementInfo.shieldPosition;
            }
            else
            {
                block = PoolManager.Spawn(ResourceEnum.Prefab.NormalBlock).GetComponent<Block>();
            }

            if (frontStage)
            {
                block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 10.5f, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f - 0.25f + row + 1 + term);
            }
            else
            {
                block.transform.position = new(enemyArrangementInfo.position.x + (enemyArrangementInfo.size.x - 1) * 0.5f - 10.5f, enemyArrangementInfo.position.y + (enemyArrangementInfo.size.y - 1) * 0.5f - 0.25f + row + 1 + term + row + 1);
            }

            if(block.TryGetComponent(out BoxCollider2D _) && enemyArrangementInfo.blockType != BlockType.Shield)
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

    void SpawnShop(bool clearBothStage, bool frontStage)
    {
        Enemy shopStage = PoolManager.Spawn(ResourceEnum.Prefab.ShopStage).GetComponent<Enemy>();
        shopStage.transform.SetParent(board, true);

        if (frontStage)
        {
            shopStage.transform.position = new(0, - 0.75f + 10 + row + 1 + term);
            shopStage.SetInfo(currentStage, 1, false, true);
            if (clearBothStage) currentStageEnemies.Add(shopStage);
            else nextStageEnemies.Add(shopStage);
        }
        else
        {
            shopStage.transform.position = new(0, - 0.75f + 10 + row + 1 + term + row + 1);
            shopStage.SetInfo(currentStage + 1, 1, false, true);
            nextStageEnemies.Add(shopStage);
        }
    }

    void SpawnBoss(StageInfo stageInfo, int wantStage)
    {
        Enemy boss = null;
        switch (stageInfo.enemyArrangementInfo[0].blockType)
        {
            case BlockType.Boss1:
                boss = PoolManager.Spawn(ResourceEnum.Prefab.Boss1).GetComponent<Enemy>();
                boss.transform.position = new(0, -0.75f + 1 + row + 1 + term + row + 1);
                boss.SetInfo(wantStage, 100f);
                break;
            case BlockType.Boss2:
                boss = PoolManager.Spawn(ResourceEnum.Prefab.Boss2).GetComponent<Enemy>();
                boss.transform.position = new(0, -0.75f + 1 + row + 1 + term + row + 1);
                boss.SetInfo(wantStage, 60f);
                break;
            default:
                Debug.LogWarning("Wrong Boss!");
                break;
        }
        if(boss != null)
        {
            nextStageEnemies.Add(boss);
            boss.transform.SetParent(board, true);
        }
    }

    public void StageClearCheck()
    {
        if(currentStageEnemies.Count == 0 && GameManager.Instance.phase == GameManager.Phase.BattlePhase)
        {
            readyToReadyPhase = true;
            Time.timeScale = 0f;
            foreach(var bead in beads)
            {
                bead.trail.emitting = false;
                bead.SetDirectionToLastDirection();
            }
            if (++currentStage >= selectedStageInfos.Length) stageClear = true;
        }
    }

    public void Clear()
    {
        foreach (var bead in beads) PoolManager.Despawn(bead.gameObject);
        beads.Clear();
        foreach (var enemy in currentStageEnemies) PoolManager.Despawn(enemy.gameObject);
        foreach (var enemy in nextStageEnemies) PoolManager.Despawn(enemy.gameObject);
        currentStageEnemies.Clear();
        nextStageEnemies.Clear();
        foreach(var wall in currentStageWalls) PoolManager.Despawn(wall);
        foreach(var wall in nextStageWalls) PoolManager.Despawn(wall);
        currentStageWalls.Clear();
        nextStageWalls.Clear();
        foreach (var projectile in projectiles) PoolManager.Despawn(projectile.gameObject);
        projectiles.Clear();
        foreach(var coin in coins) PoolManager.Despawn(coin.gameObject);
        coins.Clear();
    }

    // Input : (Size Vector(x, y), count)
    StageInfo GenerateRandomStage(params (BlockForm, int)[] wantForms)
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
                if(form.Item1.blockType == BlockType.Shield || form.Item1.blockType == BlockType.Splitter) xPos = Random.Range(1, column - form.Item1.size.x);
                else xPos = Random.Range(0, column - form.Item1.size.x + 1);
                int yPos;
                if (form.Item1.blockType == BlockType.Shield)
                {
                    yPos = Random.Range(1, (row - form.Item1.size.y) * 2 / 3);
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
                        for(int x = xPos - 1; x <= xPos + form.Item1.size.x; x++)
                        {
                            if (stageBoard[yPos - 1, x] == 1)
                            {
                                discrimination = false;
                                break;
                            }
                        }
                    }
                    else if(shieldPos == 1)
                    {
                        for (int y = yPos - 1; y <= yPos + form.Item1.size.y; y++)
                        {
                            if (stageBoard[y, xPos - 1] == 1)
                            {
                                discrimination = false;
                                break;
                            }
                        }
                    }
                    else if (shieldPos == 2)
                    {
                        for (int y = yPos - 1; y <= yPos + form.Item1.size.y; y++)
                        {
                            if (stageBoard[y, xPos + form.Item1.size.x] == 1)
                            {
                                discrimination = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int x = xPos - 1; x <= xPos + form.Item1.size.x; x++)
                        {
                            if (stageBoard[yPos + form.Item1.size.y, x] == 1)
                            {
                                discrimination = false;
                                break;
                            }
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
                else info = new(form.Item1.blockType, new(xPos, yPos), form.Item1.size, form.Item1.maxHP);
                
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
                        for (int x = xPos - 1; x <= xPos + form.Item1.size.x; x++)
                        {
                            stageBoard[yPos - 1, x] = 1;
                        }
                    }
                    else if (shieldPos == 1)
                    {
                        for (int y = yPos - 1; y <= yPos + form.Item1.size.y; y++)
                        {
                            stageBoard[y, xPos - 1] = 1;
                        }
                    }
                    else if (shieldPos == 2)
                    {
                        for (int y = yPos - 1; y <= yPos + form.Item1.size.y; y++)
                        {
                            stageBoard[y, xPos + form.Item1.size.x] = 1;
                        }
                    }
                    else
                    {
                        for (int x = xPos - 1; x <= xPos + form.Item1.size.x; x++)
                        {
                            stageBoard[yPos + form.Item1.size.y, x] = 1;
                        }
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

    StageInfo GenerateShopStage()
    {
        return new(new EnemyArrangementInfo[] { }, 1);
    }

    StageInfo GenerateBossStage(BlockType wantBoss)
    {
        EnemyArrangementInfo enemyArrangementInfo = null;
        switch(wantBoss)
        {
            case BlockType.Boss1:
                enemyArrangementInfo = new(BlockType.Boss1, new(0, 0), 100f);
                break;
            case BlockType.Boss2:
                enemyArrangementInfo = new(BlockType.Boss2, new(0, 0), 60f);
                break;
        }
        if (enemyArrangementInfo == null) Debug.LogWarning("Null boss!");
        return new(new EnemyArrangementInfo[] { enemyArrangementInfo }, 2);
    }

    public void Fever()
    {
        if (currentStageEnemies.Count == 0) return;
        bar.Fever();
    }

    void BeadRefill()
    {
        Bead newBead = PoolManager.Spawn(ResourceEnum.Prefab.NormalBead, GameManager.Instance.StageManager.bar.transform.position + new Vector3(0, 0.51f, 0)).GetComponent<Bead>();
        newBead.Initialize(1, 20, 0, 0, new());
        newBead.activated = false;
        beads.Add(newBead);
        bar.grabbedBeads.Add(newBead);
    }

    public RewardFormat GetRandomeReward()
    {
        return randomRewards[Random.Range(0, randomRewards.Length)];
    }

    public void ApplyReward(RewardFormat reward)
    {
        switch (reward.rewardType)
        {
            case RewardType.AttackDamage:
                return;
            default:
                return;
        }
    }

    public void ApplyRewards(List<RewardFormat> rewards)
    {
        foreach(RewardFormat reward in rewards) ApplyReward(reward);
    }

    public void AddQuest(QuestManager.Quest quest)
    {
        ongoingQuests.Add(quest);
        ongoingQuestsBox.SetActive(true);
        for (int i = 0; i < ongoingQuestsObject.Length; i++)
        {
            ongoingQuestsObject[i].SetActive(i < ongoingQuests.Count);
        }
        ongoingQuestsObject[ongoingQuests.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = quest.Explain;
    }

    bool questHided;
    public void HideOrShowQuest()
    {
        if(questHided)
        {
            for (int i = 0; i < ongoingQuestsObject.Length; i++)
            {
                ongoingQuestsObject[i].SetActive(i < ongoingQuests.Count);
            }
            questHideButtonArrow.transform.localScale = new(-1, 1, 1);
        }
        else
        {
            for(int i = 0; i < ongoingQuestsObject.Length; i++)
            {
                ongoingQuestsObject[i].SetActive(false);
            }
            questHideButtonArrow.transform.localScale = Vector3.one;
        }
        questHided = !questHided;
    }

    public void ClearQuest(QuestManager.Quest quest)
    {
        ApplyRewards(quest.rewards);
        ongoingQuests.Remove(quest);
        if(ongoingQuests.Count == 0)
        {
            ongoingQuestsBox.SetActive(false);
        }
        else
        {
            for (int i = 0; i < ongoingQuestsObject.Length; i++)
            {
                ongoingQuestsObject[i].SetActive(i < ongoingQuests.Count);
                if (i < ongoingQuests.Count) ongoingQuestsObject[i].GetComponentInChildren<TextMeshProUGUI>().text = ongoingQuests[i].Explain;
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseUI.SetActive(!pauseUI.activeSelf);
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        stageStartCountText.gameObject.SetActive(true);
        curStageStartCount = 3f;
    }

    public void Restart()
    {
        pauseUI.SetActive(false);
        GameManager.Instance.shopCanvas.SetActive(false);
        GameManager.Instance.readyPhaseWindow.SetActive(false);
        StartCoroutine(nameof(IRestart));
        Time.timeScale = 1f;
    }

    IEnumerator IRestart()
    {
        GameManager.ClaimLoadInfo("Reset stage");
        Clear();
        Initiate(selectedStageIndex);
        bar.SetBar(GameManager.Instance.CharacterManager.characters[currentSelectedCharacterIndex]);
        GameManager.Instance.StageManager.currentStage = 0;
        GameManager.Instance.StartBattlePhase();
        GameManager.CloseLoadInfo();
        yield return null;
    }

    public void QuitStage()
    {
        pauseUI.SetActive(false);
        GameManager.Instance.shopCanvas.SetActive(false);
        GameManager.Instance.readyPhaseWindow.SetActive(false);
        Clear();
        GameManager.Instance.mainUI.SetActive(true);
    }

    public void GetAbility(AbilityManager.Ability selectedAbility)
    {
        GameManager.Instance.StageManager.selectedAbilities.Add(selectedAbility);
        GameManager.Instance.StageManager.possibleToAppearAbilities.Remove(selectedAbility);
        foreach (var ability in AbilityManager.Abilities.Find(x => x == selectedAbility).lowerAbilities)
        {
            bool unlock = true;
            foreach (var need in ability.upperAbilities)
            {
                if (!GameManager.Instance.StageManager.selectedAbilities.Contains(need))
                {
                    unlock = false; break;
                }
            }
            if (unlock) GameManager.Instance.StageManager.possibleToAppearAbilities.Add(ability);
        }
        ApplyAbility(selectedAbility);
    }

    void ApplyAbility(AbilityManager.Ability ability)
    {
        switch(ability.name)
        {
            case AbilityManager.AbilityName.Ice:
                bar.ActivedIceBlock = 2;
                foreach (var iceBlock in bar.iceBlocks)
                {
                    iceBlock.regenerateCool = 10f;
                    iceBlock.durability = 1;
                }
                break;
            case AbilityManager.AbilityName.FastFreezeLV1:
                foreach(var iceBlock in bar.iceBlocks) iceBlock.regenerateCool = 8f;
                break;
            case AbilityManager.AbilityName.FastFreezeLV2:
                foreach(var iceBlock in bar.iceBlocks) iceBlock.regenerateCool = 6f;
                break;
            case AbilityManager.AbilityName.FastFreezeLV3:
                foreach(var iceBlock in bar.iceBlocks) iceBlock.regenerateCool = 4f;
                break;
            case AbilityManager.AbilityName.IcicleBurstLV1:
                foreach (var iceBlock in bar.iceBlocks) iceBlock.icicleCount = 1;
                break;
            case AbilityManager.AbilityName.IcicleBurstLV2:
                foreach (var iceBlock in bar.iceBlocks) iceBlock.icicleCount = 2;
                break;
            case AbilityManager.AbilityName.IcicleBurstLV3:
                foreach (var iceBlock in bar.iceBlocks) iceBlock.icicleCount = 3;
                break;
            case AbilityManager.AbilityName.ChilingAuraLV1:
                bar.chilingAura.gameObject.SetActive(true);
                bar.chilingAura.speedMagnification = 0.8f;
                break;
            case AbilityManager.AbilityName.ChilingAuraLV2:
                bar.chilingAura.speedMagnification = 0.6f;
                break;
            case AbilityManager.AbilityName.ChilingAuraLV3:
                bar.chilingAura.speedMagnification = 0.4f;
                break;
            case AbilityManager.AbilityName.MultiLayerLV1:
                foreach (var iceBlock in bar.iceBlocks) iceBlock.durability = 2;
                break;
            case AbilityManager.AbilityName.MultiLayerLV2:
                foreach (var iceBlock in bar.iceBlocks) iceBlock.durability = 3;
                break;
            case AbilityManager.AbilityName.MultiLayerLV3:
                foreach (var iceBlock in bar.iceBlocks) iceBlock.durability = 4;
                break;
            case AbilityManager.AbilityName.FrostWideLV1:
                bar.ActivedIceBlock = 4;
                break;
            case AbilityManager.AbilityName.FrostWideLV2:
                bar.ActivedIceBlock = 6;
                break;
            case AbilityManager.AbilityName.FrostWideLV3:
                bar.ActivedIceBlock = 8;
                break;
            case AbilityManager.AbilityName.Fire:
                bar.fireBallCool = 9f;
                bar.fireBallDamage = 2f;
                break;
            case AbilityManager.AbilityName.QuickDrawLV1:
                bar.fireBallCool = 7f;
                break;
            case AbilityManager.AbilityName.QuickDrawLV2:
                bar.fireBallCool = 5f;
                break;
            case AbilityManager.AbilityName.QuickDrawLV3:
                bar.fireBallCool = 3f;
                break;
            case AbilityManager.AbilityName.HotterBallLV1:
                bar.fireBallDamage = 4f;
                break;
            case AbilityManager.AbilityName.HotterBallLV2:
                bar.fireBallDamage = 6f;
                break;
            case AbilityManager.AbilityName.HotterBallLV3:
                bar.fireBallDamage = 8f;
                break;
            case AbilityManager.AbilityName.ExplosionLV1:
                bar.fireBallExplosion = true;
                bar.fireBallExplosionRange = 1f;
                break;
            case AbilityManager.AbilityName.ExplosionLV2:
                bar.fireBallExplosionRange = 2f;
                break;
            case AbilityManager.AbilityName.ExplosionLV3:
                bar.fireBallExplosionRange = 3f;
                break;
            case AbilityManager.AbilityName.BurningLV1:
                bar.fireBallBurn = true;
                bar.fireBallBurnDamage = 1f;
                break;
            case AbilityManager.AbilityName.BurningLV2:
                bar.fireBallBurnDamage = 2f;
                break;
            case AbilityManager.AbilityName.BurningLV3:
                bar.fireBallBurnDamage = 3f;
                break;
        }
    }
}

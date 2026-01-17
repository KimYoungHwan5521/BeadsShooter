using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void CustomStart();
public delegate void CustomUpdate(float deltaTime);
public delegate void CustomDestroy();

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public static string gameVirsion = "1.0";

    public CustomStart ManagerStart;
    public CustomUpdate ManagerUpdate;

    public CustomStart ObjectStart;
    public CustomUpdate ObjectUpdate;
    public CustomDestroy ObjectDestroy;

    static GameManager instance;
    public static GameManager Instance => instance;
    ResourceManager resourceManager;
    public ResourceManager ResourceManager => resourceManager;
    SoundManager soundManager;
    public SoundManager SoundManager => soundManager;
    FeverManager feverManager;
    public FeverManager FeverManager => feverManager;
    PoolManager poolManager;
    public PoolManager PoolManager => poolManager;
    StageManager stageManager;
    public StageManager StageManager => stageManager;
    BlueprintManager blueprintManager;
    public BlueprintManager BlueprintManager => blueprintManager;
    CharacterManager characterManager;
    public CharacterManager CharacterManager => characterManager;
    ShopManager shopManager;
    public ShopManager ShopManager => shopManager;
    QuestManager questManager;
    public QuestManager QuestManager => questManager;
    //public LoadingCanvas loadingCanvas;

    public GameObject blueprintDetail;
    public GameObject shopCanvas;
    public GameObject mainUI;
    public GameObject characterSelectUI;
    Shop shop;
    public Shop Shop => shop;
    public SelectCharacter selectCharacter;
    public GameObject restOptions;
    public GameObject[] randomEvents;

    public float barYPos;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        stageManager = GetComponent<StageManager>();
        shop = shopCanvas.GetComponent<Shop>();
        //if (!SteamAPI.Init())
        //{
        //    Debug.LogError("SteamAPI 초기화 실패");
        //    //Application.Quit();
        //}
        //else
        //{
        //    Debug.Log("SteamAPI 초기화 성공");
        //}
    }

    //private void OnApplicationQuit()
    //{
    //    SteamAPI.Shutdown();
    //}

    bool gameReady;
    public IEnumerator Start()
    {
        resourceManager = new ResourceManager();
        yield return resourceManager.Initiate();
        soundManager = new SoundManager();
        yield return soundManager.Initiate();
        poolManager = new PoolManager();
        yield return poolManager.Initiate();
        feverManager = new FeverManager();
        yield return feverManager.Initiate();
        blueprintManager = new BlueprintManager();
        yield return blueprintManager.Initiate();
        characterManager = new CharacterManager();
        yield return characterManager.Initiate();
        shopManager = new ShopManager();
        yield return shopManager.Initiate();
        questManager = new QuestManager();
        yield return questManager.Initiate();

        gameReady = true;
        SetCameraAspect();
        //CloseLoadInfo();
    }

    void Update()
    {
        if (!gameReady) return;
        //SteamAPI.RunCallbacks(); // 필수!

        ManagerStart?.Invoke();
        ManagerStart = null;

        if (phase != Phase.BattlePhase) return;
        ObjectStart?.Invoke();
        ObjectStart = null;

        ManagerUpdate?.Invoke(Time.deltaTime);
        ObjectUpdate?.Invoke(Time.deltaTime);

        ObjectDestroy?.Invoke();
        ObjectDestroy = null;
    }

    public void Test()
    {
        stageManager.Coin += 1000;
    }

    public enum Phase { None, BattlePhase, ReadyPhase };
    public Phase phase = Phase.None;
    public ReadyPhaseUI readyPhaseUI;
    public GameObject readyPhaseWindow;

    void SetCameraAspect()
    {
        // 아이폰12 기준 비율 (가로/세로)
        float targetAspect = 1170f / 2532f; // ≈ 0.4620853f

        // 현재 기기 비율 (가로/세로)
        float currentAspect = (float)Screen.width / Screen.height;

        Camera cam = Camera.main;

        // "아이폰12 환경에서 작업하던 그대로의 orthographicSize"
        // 즉, 기준 구도일 때 카메라에 세팅돼 있던 값
        float baseOrtho = cam.orthographicSize;

        // 기본은 그대로 쓴다
        float newOrtho = baseOrtho;

        // 만약 현재 화면이 더 세로로 길어서 (currentAspect < targetAspect)
        // 좌우가 잘릴 위험이 있다면, 더 많이 보이도록 orthographicSize를 키운다.
        float worldScale = targetAspect / currentAspect;
        if (currentAspect > targetAspect)
        {
            newOrtho = baseOrtho / worldScale;
        }
        else
        {
            newOrtho = baseOrtho * worldScale;
        }

        cam.orthographicSize = newOrtho;

        // 여백은 검게
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
    }

    public void StartBattlePhase()
    {
        readyPhaseWindow.SetActive(false);
        shopCanvas.SetActive(false);
        characterSelectUI.SetActive(false);
        stageManager.StageSetting();
    }

    public void BattlePhaseStart()
    {
        if (stageManager.beads.Count == 0)
        {
            Bead bead = PoolManager.Spawn(ResourceEnum.Prefab.NormalBead, stageManager.bar.transform.position + new Vector3(0, 0.51f)).GetComponent<Bead>();
            bead.Initialize(1, 20, 0, 0, new());
            bead.activated = false;
            stageManager.beads.Add(bead);
            stageManager.bar.grabbedBeads.Add(bead);
        }
        foreach(var enemy in stageManager.currentStageEnemies)
        {
            ObjectUpdate += enemy.MyUpdate;
            ObjectUpdate += enemy.MyUpdateOnlyCurrentStage;
        }
        foreach (var enemy in stageManager.nextStageEnemies)
        {
            ObjectUpdate += enemy.MyUpdate;
        }
        phase = Phase.BattlePhase;
        Time.timeScale = 1f;

    }


    public void ReadyPhase()
    {
        readyPhaseUI.SetReadyPhase();
        readyPhaseWindow.SetActive(true);
        ObjectUpdate = null;
        phase = Phase.ReadyPhase;
    }

    public void StageClear()
    {
        mainUI.SetActive(true);
        stageManager.Clear();
        phase = Phase.ReadyPhase;
    }

    public void OpenShop()
    {
        shopCanvas.SetActive(true);
        shop.SetShop();
    }

    public void RandomEvent()
    {
        int rand = UnityEngine.Random.Range(0, randomEvents.Length);
        for (int i = 0; i < randomEvents.Length; i++)
        {
            randomEvents[i].SetActive(i == rand);
        }
    }

    public static void ClaimLoadInfo(string info, int numerator = 0, int denominator = 1)
    {
        //if (instance && instance.loadingCanvas)
        //{
        //    instance.loadingCanvas.SetLoadInfo(info, numerator, denominator);
        //    instance.loadingCanvas.gameObject.SetActive(true);
        //}
        //else
        //{
        //    Debug.LogWarning("There is no GameManager or loadingCanvas");
        //}
    }

    public static void CloseLoadInfo()
    {
        //if (instance && instance.loadingCanvas)
        //{
        //    instance.loadingCanvas.CloseLoadInfo();
        //    instance.loadingCanvas.gameObject.SetActive(false);
        //}
        //else
        //{
        //    Debug.LogWarning("There is no GameManager or loadingCanvas");
        //}
    }

    public void FixLayout(RectTransform rect)
    {
        StartCoroutine(FixLayoutNextFrame(rect));
    }

    IEnumerator FixLayoutNextFrame(RectTransform rect)
    {
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }
}
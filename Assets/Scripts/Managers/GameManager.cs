using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void CustomStart();
public delegate void CustomUpdate();
public delegate void CustomDestroy();

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
    PoolManager poolManager;
    public PoolManager PoolManager => poolManager;
    StageManager stageManager;
    public StageManager StageManager => stageManager;
    //public LoadingCanvas loadingCanvas;

    public GameObject description;


    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        stageManager = GetComponent<StageManager>();
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

        gameReady = true;
        ManagerUpdate += BattlePhaseUpdate;
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

        ManagerUpdate?.Invoke();
        ObjectUpdate?.Invoke();

        ObjectDestroy?.Invoke();
        ObjectDestroy = null;
    }

    public enum Phase { None, BattlePhase, ReadyPhase };
    public Phase phase = Phase.None;
    [SerializeField] GameObject upgradeWindow;

    public void StartBattlePhase()
    {
        upgradeWindow.SetActive(false);
        //CurRoundTimeLeft = roundTime;
        phase = Phase.BattlePhase;
        stageManager.StageSetting();
    }

    void BattlePhaseUpdate()
    {
    }

    public void ReadyPhase()
    {
        upgradeWindow.SetActive(true);
        phase = Phase.ReadyPhase;
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
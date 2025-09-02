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

    //public LoadingCanvas loadingCanvas;

    public GameObject description;


    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
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
        if (!gameReady || phase != Phase.BattlePhase) return;
        //SteamAPI.RunCallbacks(); // 필수!

        ManagerStart?.Invoke();
        ManagerStart = null;
        ObjectStart?.Invoke();
        ObjectStart = null;

        ManagerUpdate?.Invoke();
        ObjectUpdate?.Invoke();

        ObjectDestroy?.Invoke();
        ObjectDestroy = null;
    }

    enum Phase { BattlePhase, UpgradePhase };
    Phase phase;
    [SerializeField] TextMeshProUGUI roundTimeLeftText;
    [SerializeField] float roundTime = 30f;
    [SerializeField] float curRoundTimeLeft = 30f;
    float CurRoundTimeLeft
    {
        get => curRoundTimeLeft;
        set
        {
            curRoundTimeLeft = value;
            if (roundTime > 60) roundTimeLeftText.text = $"{CurRoundTimeLeft / 60:0} : {CurRoundTimeLeft % 60:00}";
            else roundTimeLeftText.text = $"{CurRoundTimeLeft:00}";
        }
    }
    [SerializeField] GameObject upgradeWindow;

    void BattlePhaseUpdate()
    {
        if (phase != Phase.BattlePhase) return;
        CurRoundTimeLeft -= Time.deltaTime;
        if(CurRoundTimeLeft < 0)
        {
            upgradeWindow.SetActive(true);
            phase = Phase.UpgradePhase;
        }
    }

    public void StartBattlePhase()
    {
        upgradeWindow.SetActive(false);
        CurRoundTimeLeft = roundTime;
        phase = Phase.BattlePhase;
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
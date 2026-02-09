using UnityEngine;
using UnityEngine.Events;


public class Area : CustomObject
{
    public bool sticky;
    public float speedMagnification = 1f;
    AutoDespawn autoDespawn;
    Vector3 originalBodyScale;

    private void Awake()
    {
        autoDespawn = GetComponentInParent<AutoDespawn>();
        originalBodyScale = transform.localScale;
    }

    private void Start()
    {
        void despawnAction() { GameManager.Instance.StageManager.areas.Remove(gameObject); }
        autoDespawn.despawnAction -= despawnAction;
        autoDespawn.despawnAction += despawnAction;
    }

    public override void MyUpdate(float deltaTime)
    {
        if(autoDespawn != null)
        {
            transform.localScale = (1 - autoDespawn.CurDespawnTime / autoDespawn.DespawnTime) * originalBodyScale;
        }
    }
}

using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField] bool isUnscaled;
    [SerializeField] float despawnTime;
    [SerializeField] float curDespawnTime;

    private void OnEnable()
    {
        curDespawnTime = 0;
    }

    private void Update()
    {
        if (isUnscaled) curDespawnTime += Time.unscaledDeltaTime;
        else curDespawnTime += Time.deltaTime;
        if(curDespawnTime > despawnTime)
        {
            PoolManager.Despawn(gameObject);
        }
    }
}

using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField] float despawnTime;
    [SerializeField] float curDespawnTime;

    private void OnEnable()
    {
        curDespawnTime = 0;
    }

    private void Update()
    {
        curDespawnTime += Time.deltaTime;
        if(curDespawnTime > despawnTime)
        {
            PoolManager.Despawn(gameObject);
        }
    }
}

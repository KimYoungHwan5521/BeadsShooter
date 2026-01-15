using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FeverAction(int level);

public class FeverManager
{
    public enum FeverName { Laser, Fireball, Phychokinesis }
    Dictionary<FeverName, FeverAction> fevers;
    public Dictionary<FeverName, FeverAction> Fevers => fevers;

    public IEnumerator Initiate()
    {
        fevers = new();
        fevers.Add(FeverName.Laser, (int level) => 
        {
            for(int i=0; i<=level; i++)
            {
                bool actived = false;
                float feverDelay = 0.1f * i;
                float curFeverDelay = 0;
                GameManager.Instance.ObjectUpdate += (deltaTime) =>
                {
                    if (actived || GameManager.Instance.StageManager.currentStageEnemies.Count == 0) return;
                    curFeverDelay += deltaTime;
                    if (curFeverDelay > feverDelay)
                    {
                        GameManager.Instance.ObjectStart += () =>
                        {
                            int rand = Random.Range(0, GameManager.Instance.StageManager.currentStageEnemies.Count);
                            Enemy target = GameManager.Instance.StageManager.currentStageEnemies[rand];
                            LineRenderer line = PoolManager.Spawn(ResourceEnum.Prefab.FeverAttack).GetComponent<LineRenderer>();
                            line.SetPositions(new Vector3[] { GameManager.Instance.StageManager.bar.transform.position, target.transform.position });
                            target.TakeDamage(1);
                        };
                        actived = true;
                    }
                };
            }
        });
        yield return null;
    }
}

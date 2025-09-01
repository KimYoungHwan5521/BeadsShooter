using UnityEngine;

public class Spawner : CustomObject
{
    [SerializeField] float spawnCool = 1f;
    float curSpawnCool;

    float leftEdge;
    float rightEdge;
    float topEdge;

    protected override void MyStart()
    {
        // ȭ�� ���� �Ʒ� (0,0) �� ���� ��ǥ
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));

        // ȭ�� ������ �� (Screen.width, Screen.height) �� ���� ��ǥ
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        leftEdge = Mathf.Max(bottomLeft.x, -5);
        rightEdge = Mathf.Min(topRight.x, 5);
        topEdge = topRight.y;

        base.MyStart();
    }

    protected override void MyUpdate()
    {
        curSpawnCool += Time.deltaTime;
        if(curSpawnCool > spawnCool)
        {
            curSpawnCool = 0;
            float spawnPosX = Random.Range(leftEdge + 1, rightEdge - 1);
            float SpawnPosY = topEdge + 1;
            PoolManager.Spawn(ResourceEnum.Prefab.Slime, new Vector3(spawnPosX, SpawnPosY, 0));
        }
    }
}

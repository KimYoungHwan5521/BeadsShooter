using UnityEngine;

public class Orbit : CustomObject
{
    public Satellite[] satellites;   // 돌 오브젝트들
    int ActivedSatellites
    {
        get
        {
            int result = 0;
            foreach(var satellite in satellites) if(satellite.gameObject.activeSelf) result++;
            return result;
        }
    }
    public float radius = 4f;     // 중심에서 거리
    public float speed = 30f;     // 초당 회전 각도

    float currentAngle;

    public override void MyUpdate(float deltaTime)
    {
        if (ActivedSatellites == 0) return;

        // 반시계 방향 회전
        currentAngle += speed * Time.deltaTime;

        float step = 360f / ActivedSatellites;

        for (int i = 0; i < ActivedSatellites; i++)
        {
            float angle = currentAngle + step * i;

            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(
                Mathf.Cos(rad),
                Mathf.Sin(rad)
            ) * radius;

            satellites[i].transform.position = transform.position + (Vector3)pos;
        }
    }

    public void SetActiveSatellites(int count)
    {
        for(int i = 0; i < satellites.Length; i++)
        {
            satellites[i].gameObject.SetActive(i < count);
        }
    }
}

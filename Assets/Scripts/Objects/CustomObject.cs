using System;
using UnityEngine;

public class CustomObject : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        GameManager.Instance.ObjectStart += MyStart;
        GameManager.Instance.ObjectUpdate -= MyUpdate;
        GameManager.Instance.ObjectUpdate += MyUpdate;
    }

    protected virtual void MyStart() { }
    public virtual void MyUpdate(float deltaTime) { }
    public virtual void MyUpdateOnlyCurrentStage(float deltaTime) { }
    protected virtual void MyDestroy()
    {
        GameManager.Instance.ObjectUpdate -= MyUpdate;
        Destroy(gameObject);
    }
}
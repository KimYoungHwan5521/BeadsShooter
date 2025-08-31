using System;
using UnityEngine;

public class CustomObject : MonoBehaviour
{
    protected virtual void Start()
    {
        GameManager.Instance.ObjectStart += MyStart;
        GameManager.Instance.ObjectUpdate += MyUpdate;
    }

    protected virtual void MyStart() { }
    protected virtual void MyUpdate() { }
    protected virtual void MyDestroy()
    {
        GameManager.Instance.ObjectUpdate -= MyUpdate;
        Destroy(gameObject);
    }
}
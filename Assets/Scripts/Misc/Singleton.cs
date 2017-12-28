using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("A instance already exists");
            Destroy(this); //Or GameObject as appropriate
            return;
        }
        Instance = this as T;

        AfterAwake();
    }

    protected virtual void AfterAwake()
    { }
}
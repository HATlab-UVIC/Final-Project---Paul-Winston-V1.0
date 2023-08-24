using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject gameobject = new GameObject("Singleton Manager");
                    GameObject managers = GameObject.FindWithTag("Managers");
                    gameobject.transform.SetParent(managers.transform, false);
                    instance = gameobject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
                Debug.Log($"Singleton object already exist in the scene. Deleting {instance.ToString()}");
            }
        }
    }
}

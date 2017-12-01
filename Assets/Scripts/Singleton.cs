using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            lock (olock)
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = string.Format("singleton-{0}", typeof(T));
                        DontDestroyOnLoad(singleton);
                    }
                }
                return instance;
            }
        }
    }

    private static T instance;
    private static object olock = new object();
}
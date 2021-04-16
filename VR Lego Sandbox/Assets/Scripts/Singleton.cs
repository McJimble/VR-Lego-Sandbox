using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = (T)FindObjectOfType(typeof(T));
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                _instance = obj.AddComponent<T>();
                obj.name = typeof(T).ToString();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        //Prevent duplicates
        if (_instance != null)
        {
            Debug.Log("Destroying duplicate singleton instance");
            Destroy(this.gameObject);
        }
    }
}
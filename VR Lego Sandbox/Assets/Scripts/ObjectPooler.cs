using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    [SerializeField] private List<Pool> pools;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject store = new GameObject(pool.prefab.name + " Store");


            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, store.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.name, objectPool);
        }
    }

    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool with tag " + name + " doesn't exist.");
            return null;
        }

        GameObject objToSpawn;
        if(poolDictionary[name].Count > 1)
            objToSpawn = poolDictionary[name].Dequeue();
        else
        {
            objToSpawn = Instantiate(poolDictionary[name].Peek());
        }

        objToSpawn.SetActive(true);
        objToSpawn.transform.parent = null;
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        return objToSpawn;
    }
}

[System.Serializable]
public class Pool
{
    public string name;
    public GameObject prefab;
    public int size;

    public GameObject ObjStore { get; set; }

}
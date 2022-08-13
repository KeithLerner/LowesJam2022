using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawer : MonoBehaviour
{
    public static PrefabSpawer instance;

    [Tooltip("Ignores prefabs with the TimedDestroyer component")]
    public List<GameObject> managedPrefabs;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }
    }

    public GameObject SpawnPrefab(GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        TimedDestroyer to;
        if (!go.TryGetComponent<TimedDestroyer>(out to))
            managedPrefabs.Add(go);
        go.transform.parent = transform;
        return go;
    }

    public GameObject GetPrefabByName(string name)
    {
        GameObject go = gameObject;
        foreach (GameObject item in managedPrefabs)
        {
            if (item.name == name) go = item;
        }
        return go;
    }

    public void RemovePrefabByName(string name)
    {
        GameObject go = gameObject;
        foreach (GameObject item in managedPrefabs)
        {
            if (item.name == name) go = item;
        }
        managedPrefabs.Remove(go);
    }

    public GameObject PopPrefabByName(string name)
    {
        GameObject go = gameObject;
        foreach (GameObject item in managedPrefabs)
        {
            if (item.name == name) go = item;
        }
        managedPrefabs.Remove(go);
        return go;
    }

    public void ClearManagedPrefabs()
    {
        foreach (GameObject item in managedPrefabs)
        {
            if (item.name == name) Destroy(item);
        }
        managedPrefabs.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroyer : MonoBehaviour
{
    [SerializeField] private float time = 1f;
    private float spawnTime;
    private void Awake()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime + time) Destroy(this.gameObject);
    }
}

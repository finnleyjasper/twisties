using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class LeakSpawnControl : MonoBehaviour
{

    [SerializeField] Leak[] leakSpawns;
    [SerializeField] private float defaultSpawnInterval = 4; // seconds between instantiations. should decrease based on WaterControl.CurrentWaterLevel
    [SerializeField] private float leakSpawnInterval;
    private float lastSpawnTime = 2; // allow player 3 seconds to orient themself before spawns start


    void Awake()
    {
        leakSpawnInterval = defaultSpawnInterval;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad >= lastSpawnTime + leakSpawnInterval)
        {
            ChooseLeak();
        }

        UpdateSpawnInterval();
    }

    private void UpdateSpawnInterval()
    {
        leakSpawnInterval = defaultSpawnInterval - (Time.timeSinceLevelLoad / 20);
    }
    private void ChooseLeak()
    {

        Leak[] possible = leakSpawns.Where(l => !l.IsActive).ToArray();

        if (possible.Length > 0)
        {
            possible[Random.Range(0, possible.Length)].SpawnLeak();
            lastSpawnTime = Time.timeSinceLevelLoad;
        }
    }
    
}

using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class LeakSpawnControl : MonoBehaviour
{
    
    WaterControl waterControl;
    [SerializeField] Leak[] leakSpawns;
    [SerializeField] private float defaultSpawnInterval = 4; // seconds between instantiations. should decrease based on WaterControl.CurrentWaterLevel
    private float leakSpawnInterval;
    private float lastSpawnTime = 2; // allow player 3 seconds to orient themself before spawns start
    
    // transform? array? something? -- should know all the possible spots leaks can spawn

    void Awake()
    {
        leakSpawnInterval = defaultSpawnInterval;
        waterControl = gameObject.GetComponent<WaterControl>();
    }

    void Update()
    {
        if (Time.time >= lastSpawnTime + leakSpawnInterval)
        {
            ChooseLeak();
        }

        UpdateSpawnInterval();
    }

    private void UpdateSpawnInterval()
    {
        // eg. if water level is 75 (out of 100), leaks will spawn every 2.5 seconds
        // these are totally arbitrary numbers i pulled out of my ass so will need to test the difficulty
        leakSpawnInterval = defaultSpawnInterval - (waterControl.CurrentWaterLevel / 50);
    }

    private void ChooseLeak()
    {
        int randLeak = Random.Range(0, 8);
        CheckLeak(randLeak);
        leakSpawns[randLeak].SpawnLeak();
        lastSpawnTime = Time.time;
    }

    private void CheckLeak(int leak)
    {
        if (leakSpawns[leak].IsLeaking)
        {
            ChooseLeak();
        }
        else
        {
            return;
        }
    }
}

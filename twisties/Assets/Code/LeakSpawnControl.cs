using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class LeakSpawnControl : MonoBehaviour
{
    GameObject leakPrefab;
    WaterControl waterControl;

    [SerializeField] private float defaultSpawnInterval = 4; // seconds between instantiations. should decrease based on WaterControl.CurrentWaterLevel
    private float leakSpawnInterval;
    private float lastSpawnTime = 2; // allow player 3 seconds to orient themself before spawns start

    // transform? array? something? -- should know all the possible spots leaks can spawn

    void Awake()
    {
        leakSpawnInterval = defaultSpawnInterval;

        try
        {
            leakPrefab = Resources.Load<GameObject>("Leak");
        }
        catch
        {
            Debug.LogWarning("Prefab 'Leak' could not be found for " + gameObject.name);
        }

        waterControl = gameObject.GetComponent<WaterControl>();
    }

    void Update()
    {
        if (Time.time >= lastSpawnTime + leakSpawnInterval)
        {
            // Transform position = pick a random spot from the availible spawn locations?

            // TEMP TEMP TEMP SHIT
            // sub in vector 0,0,0 with randomised position
            // location should also link to the keybind for the leak - ie. location 1 binds to A, etc.
            GameObject newLeak = Instantiate(leakPrefab, new UnityEngine.Vector3(0,0,1), UnityEngine.Quaternion.identity, gameObject.transform);
            // TEMP HARDCODED KEYCODE
            newLeak.GetComponent<Leak>().keyBinding = KeyCode.A;

            lastSpawnTime = Time.time;
        }

        UpdateSpawnInterval();
    }

    private void UpdateSpawnInterval()
    {
        // eg. if water level is 75 (out of 100), leaks will spawn every 2.5 seconds
        // these are totally arbitrary numbers i pulled out of my ass so will need to test the difficulty
        leakSpawnInterval = defaultSpawnInterval - (waterControl.CurrentWaterLevel / 50);
    }
}

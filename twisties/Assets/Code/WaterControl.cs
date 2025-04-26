/*
*   !! ALL LEAKS SHOULD BE A CHILD OF THIS OBJ !!
*
*   Basically GameManager - This script will track the increase of water & game state
*
*   Should control the increase in water visual as well when availible
*
*   When "maxWater" is reached, game over!
*
*   Water increases X amount per second, calculated by the number of leaking pipes. If no pipes are leaking,
*   water will not increase. Amount added per leak can be changed in increasePerLeak
*
*   Score should be calculated based on time. Basic rounding is done for now
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class WaterControl : MonoBehaviour
{
    public int maxWater = 100;
    private int currentWater = 0;

    private float timeWaterLastAdded = 0;
    [SerializeField] private int increasePerLeak = 1; // how much to add to waterCount each second, per active leak

    public Leak[] leaks; // all leaks - both held and active

    public double finalScore = 0;

    // [SerializeField] private GameObject waterAnimation;

    void Update()
    {
        // if its been one second since the water was last added... add it
        if (Time.time >= timeWaterLastAdded + 1) // should this be delta time? does that matter here?
        {
            IncreaseWater();
        }

        if (currentWater >= maxWater)
        {
            finalScore = Math.Round(Time.time, MidpointRounding.ToEven);
            Debug.Log("Max water reached! Final score: " + finalScore);

            SceneManager.LoadScene("End");
        }
    }

    private void IncreaseWater()
    {
        // TO DO: move the transform of the animation up

        // find all the current leaks (both leaking and held)
        leaks = GetComponentsInChildren<Leak>();
        int waterToAdd = 0;

        foreach (Leak leak in leaks)
        {
            if (leak.IsLeaking)
            {
                waterToAdd += increasePerLeak;
            }
        }

        if ((currentWater + waterToAdd) <= 100)
        {
            currentWater += waterToAdd;
        }
        else
        {
            currentWater = 100;
        }

        Debug.Log("Current water level: " + currentWater);
        timeWaterLastAdded = Time.time;
    }

    public int CurrentWaterLevel
    {
        get { return currentWater; }
    }

    public int CountActiveLeaks()
    {
        int count = 0;
        foreach (Leak leak in leaks)
        {
            if (leak.IsLeaking) {count++;}
        }
        return count;
    }

    public bool AnyLeaksHeld()
    {
        foreach (Leak leak in leaks)
        {
            if (leak.IsHeld)
            {
                return true;
            }
        }

        return false;
    }
}

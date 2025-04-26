
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // muffle effect
    public AudioMixer audioMixer;
    public string lowPassLevel = "lowPass";
    public string wetLevel = "wet";

    private int waterLevel;

    public float normalCutoff = 5000f;
    public float normalWet = -80f;

    // almost certainly a better way to do this but... we're here now
    // running water sound
    public AudioSource leakingWaterAudioSource;
    // another for the holding sound
    public AudioSource holdingAudioSource;
    // aaannnnd another for the fixed sounds
    // this is here and not per-leak because the leak gets destroyed as the same plays
    public AudioSource fixedAudioSource;

    void Update()
    {
        MuffleSound();
        PlayLeakingWater();
        PlayHolding(); // did this here bc multiple tape sounds would probably suck
    }

    private void PlayLeakingWater()
    {
        if (leakingWaterAudioSource.mute) // if its not already playing
        {
            if (gameObject.GetComponent<WaterControl>().CountActiveLeaks() > 0) // and there is at least one leak
            {
                // play the sound
                leakingWaterAudioSource.mute = false;
            }
        }
        else // is playing
        {
            if (gameObject.GetComponent<WaterControl>().CountActiveLeaks() <= 0) // no leaks
            {
                // stop the sound
                leakingWaterAudioSource.mute = true;
            }
        }
    }

    private void PlayHolding()
    {
        if (gameObject.GetComponent<WaterControl>().AnyLeaksHeld())
        {
            holdingAudioSource.mute = false;
        }
        else
        {
            holdingAudioSource.mute = true;
        }

    }

    private void MuffleSound()
    {
        waterLevel = gameObject.GetComponent<WaterControl>().CurrentWaterLevel;

        if (normalCutoff  - (waterLevel * 60) - 10 > 0)
        {
            audioMixer.SetFloat("lowPass", normalCutoff - (waterLevel * 60) - 10);
        }
        else
        {
            audioMixer.SetFloat("lowPass", 0.1f);
        }

        if (normalWet + (waterLevel * 1f) + 5 < 0)
        {
            audioMixer.SetFloat("wet", normalWet + (waterLevel * 1f) + 5);
        }
        else
        {
            audioMixer.SetFloat("wet", -0.1f);
        }
    }

    // called by the leak, passing in the clip randomly generated within the leak
    // calls this method in soundmanager and not within leak as the leak is destroyed
    public void PlayFixedSound(AudioClip clip)
    {
        fixedAudioSource.clip = clip;
        fixedAudioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerManager : MonoBehaviour
{
    public AudioMixer mainMixer;
    public AudioMixerSnapshot[] mainSnap = new AudioMixerSnapshot[1] ;
    public AudioMixerSnapshot[] mutedSnap = new AudioMixerSnapshot[1] ;
    public AudioMixerSnapshot[] onlyFXSnap = new AudioMixerSnapshot[1] ;
    private float[] weights = new float[] {1.0f};

    private bool on_off = true;

    public void MuteMixer()
    {
        if (on_off)
        {
            mainMixer.TransitionToSnapshots(mutedSnap, weights, 0.0f);
            on_off = false;
        }
        else if(!on_off)
        {
            mainMixer.TransitionToSnapshots(mainSnap, weights, 0.0f);
            on_off = true;
        }
    }

    public void OnlyFX(float seconds)
    {
        StartCoroutine(TransitionToFX(seconds));
    }

    IEnumerator TransitionToFX(float seconds_)
    {
        mainMixer.TransitionToSnapshots(onlyFXSnap, weights, 0.5f);

        yield return new WaitForSeconds(seconds_);

        mainMixer.TransitionToSnapshots(mainSnap, weights, 0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource track;
    private AudioClip soundtrack;

    public void SwapTrack(AudioClip newClip)
    {
        soundtrack = newClip;
        StartCoroutine(Transition());
    }
    
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(2.0f);
        track.Stop();
        track.clip = soundtrack;
        track.Play();
    }
}

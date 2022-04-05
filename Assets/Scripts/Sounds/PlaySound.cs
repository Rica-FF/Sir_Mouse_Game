using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource track;

    public void PlayTrack()
    {
        track.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public bool UseColliderTrigger = false; 
    public AudioClip[] sounds = new AudioClip[0];

    public void PlayRandomSound()
    {
        GetComponent<AudioSource>().clip = sounds[Random.Range(0, sounds.Length)];
        GetComponent<AudioSource>().Play();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && UseColliderTrigger)
        {
            PlayRandomSound();
        }
    }
}

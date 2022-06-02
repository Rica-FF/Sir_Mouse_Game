using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsOfUI : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private List<AudioClip> _soundEffectsUI = new List<AudioClip>();

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void PlaySoundEffect(int soundIndex)
    {
        _audioSource.PlayOneShot(_soundEffectsUI[soundIndex]);
    }
}

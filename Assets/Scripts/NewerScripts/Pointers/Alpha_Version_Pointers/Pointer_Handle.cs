using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Handle : Pointer_Base
{
    [SerializeField]
    private Animator _handleAnimator;

    private AudioSource _audioSource;

    private bool _activated;



    private void Start()
    {
        _audioSource = GetComponentInParent<AudioSource>();
    }



    public override void PlayEvent()
    {      
        base.PlayEvent();

        if (_activated == false)
        {
            _handleAnimator.Play("Down_New");
            _activated = true;

            StartCoroutine(DisableAnimator());

            // play sound 1
            _audioSource.PlayOneShot(SoundEffects[0]);
        }
    }


    private IEnumerator DisableAnimator()
    {
        yield return new WaitForSeconds(_handleAnimator.GetCurrentAnimatorStateInfo(0).length);

        _handleAnimator.enabled = false;

        // play sound 2
        _audioSource.PlayOneShot(SoundEffects[1]);
    }

}

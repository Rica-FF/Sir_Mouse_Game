using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_TomatoPlant : Pointer_Base
{
    // has a cooldown before it can be used again


    private Animator[] _tomatoAnimators;


    private void Start()
    {
        _tomatoAnimators = Interactible.transform.GetChild(0).GetComponentsInChildren<Animator>();  // tomato animators
    }

    public override void PlayEvent()
    {
        base.PlayEvent();

        StartCoroutine(DestroyTomato());
    }

    private IEnumerator DestroyTomato()
    {
        yield return new WaitForSeconds(0.25f);

        // animation activation
        foreach (Animator animator in _tomatoAnimators)
        {
            animator.SetTrigger("Hit");
        }
        // play sound
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[7]);

        // activate a cooldown
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Pumpkin : Pointer_Base
{
    private Animator _pumpkinAnimator;


    private void Start()
    {
        _pumpkinAnimator = Interactible.transform.GetChild(0).GetComponent<Animator>();  // pumpkin animator 
    }

    public override void PlayEvent()
    {
        base.PlayEvent();

        StartCoroutine(DestroyPumpkin());
    }

    private IEnumerator DestroyPumpkin()
    {
        yield return new WaitForSeconds(0.25f);

        // play sound
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[7]);
        // animation activation
        _pumpkinAnimator.enabled = true;

        InteractibleScript.UsedSuccesfully = true;
    }


}

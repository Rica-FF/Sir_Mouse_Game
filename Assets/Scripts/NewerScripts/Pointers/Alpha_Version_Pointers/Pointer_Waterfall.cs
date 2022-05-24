using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Waterfall : Pointer_Base
{
    [SerializeField]
    private Animator _waterfallAnimator;
    [SerializeField]
    private Animator _handleAnimator;

    private AudioSource _sourceHandle, _sourceWater;

    private void Start()
    {
        _sourceHandle = _handleAnimator.GetComponent<AudioSource>();
        _sourceWater = _waterfallAnimator.GetComponent<AudioSource>();
    }


    public override void PlayEvent()
    {
        base.PlayEvent();

        _handleAnimator.Play("Down_NewNoCord");
        _sourceHandle.Play();

        // move the character into position first perhaps...

        _waterfallAnimator.SetTrigger("Water");
        _sourceWater.Play();

        // if there's a bucket, give it water
        if (PlayerRefs.PickedUpObject == PickupType.Bucket)
        {
            StartCoroutine(PlayerRefs.attachedObject.GetComponentInChildren<Interactible_Bucket>().GetWater());
        }      
    }
}

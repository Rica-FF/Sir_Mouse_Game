using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Handle : Pointer_Base
{
    [SerializeField]
    private Animator _handleAnimator;

    private bool _activated;



    public override void PlayEvent()
    {      
        base.PlayEvent();

        if (_activated == false)
        {
            _handleAnimator.Play("Down_New");
            _activated = true;
        }
    }

}

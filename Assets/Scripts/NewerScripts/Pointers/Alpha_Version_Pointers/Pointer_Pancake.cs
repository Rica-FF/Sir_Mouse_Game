using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Pancake : Pointer_Base
{
    /*
     * sir mouse walks towards destination
     * 
     * animate the pan going to sir mouse hand.
     * parent pan to hand
     * 
     * -- get first pancake ready (animate milk being poured into pan)
     * -- swipe up to flip the pancake, pancake dissappears top of screen
     * -- pancakes start falling down 3 columns at intervals
     * -- swipe left/right to catch pancakes
     * -- when pancake is caught, play animation which swipes up the pancake of screen again, towards the table
     * 
     * 
     * -- when event is done, show animation of plate and pancakes landing on table (size correlates to succes of minigame)
     * 
     * -- sir mouse can now move again
     * 
     * 
     * 
     */

    private void GetPancaking()
    {
        // access the animator that moves pan sprite, as well as the jug, play its animation
        // parent the pan to sir mouse hand (animation event)
        // once the animation is completely done...

        // show upwarda arrow
    }



    private void Start()
    {
        // disable this script, so that update() does not run
        this.enabled = false;
    }


    public override void PlayEvent()
    {
        base.PlayEvent();


    }



    private void Update()
    {
        //if (_walking)
        //{
        //    if (PlayerControls.curSpeed < 0.01f)
        //    {
        //        StartCoroutine(GrabSword());
        //        _walking = false;
        //    }
        //}

        //if (_pullingSword)
        //{
        //    if (PlayerRefs.mouseControls)
        //    {
        //        if (Input.GetMouseButton(0))
        //        {
        //            if (_start)
        //            {
        //                _beginValue = Input.mousePosition.y;
        //                _start = false;
        //            }
        //        }
        //        else if (Input.GetMouseButtonUp(0))
        //        {
        //            if (Input.mousePosition.y > _beginValue + 100)
        //            {
        //                Pull();
        //                _beginValue = 0;
        //            }
        //            _start = true;
        //        }
        //    }
        //    else
        //    {
        //        if (Input.touchCount == 1)
        //        {
        //            if (_start)
        //            {
        //                _beginValue = Input.GetTouch(0).position.y;
        //                _start = false;
        //            }
        //            if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //            {
        //                if (Input.GetTouch(0).position.y > _beginValue + 100)
        //                {
        //                    Pull();
        //                    _beginValue = 0;
        //                }
        //            }
        //        }
        //        else if (Input.touchCount == 0)
        //        {
        //            _start = true;
        //        }
        //    }
        //}
    }





}

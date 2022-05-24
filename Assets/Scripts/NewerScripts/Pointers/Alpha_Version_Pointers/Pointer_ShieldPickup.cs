using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_ShieldPickup : Pointer_Base
{
    [SerializeField]
    private GameObject _closetObject;

    [HideInInspector]
    public Pointer_Closet ClosetPointerScript;



    private void Start()
    {
        ClosetPointerScript = _closetObject.GetComponentInChildren<Pointer_Closet>();
    }


    public override void PlayEvent()
    {
        base.PlayEvent();

        // set shield to closet pointer index 
        if (ClosetPointerScript.ShieldIndex == 0)
        {
            PlayerRefs.shieldGeo.SetActive(true); // enables shield geometry
            PlayerRefs.ShieldSprite.GetComponent<SpriteRenderer>().sprite = null; // set sprite to none
        }
        else
        {
            PlayerRefs.shieldGeo.SetActive(false);
            PlayerRefs.ShieldSprite.GetComponent<SpriteRenderer>().sprite = ClosetPointerScript.ShieldSprites[ClosetPointerScript.ShieldIndex - 1];
        }

        // update player shield index
        PlayerRefs.shieldIndex = ClosetPointerScript.ShieldIndex;

        // set sprite to false
        SpriteObject.SetActive(false);
        // set trigger to false
        Interactible.GetComponent<Collider>().enabled = false;

        // play sound
    }

}

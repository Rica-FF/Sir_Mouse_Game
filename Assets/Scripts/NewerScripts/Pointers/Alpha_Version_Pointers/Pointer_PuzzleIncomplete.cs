using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_PuzzleIncomplete : Pointer_Base
{
    [SerializeField]
    private Animator _princeAnimator;

    [SerializeField]
    private GameObject _incompletePuzzle, _completePuzzle;


    public override void PlayEvent()
    {
        base.PlayEvent();

        // get the piece pointer lord -> to get the sparkle objects -> disable all sparkle objects
        foreach (var sparkle in PlayerRefs.attachedObject.GetComponentInChildren<Pointer_Lord>().SparkleObjectsAll)
        {
            sparkle.SetActive(false);
        }

        // remove item
        Destroy(PlayerRefs.attachedObject);
        //PlayerRefs.attachedObject = null;
        PlayerRefs.PickedUpObject = PickupType.None;
        // re-equip gear
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);
        PlayerRefs.GetComponent<Animator>().Play("Unequipe_0");

        // sound
        InteractibleScript.AudioSource.PlayOneShot(SoundEffects[0]);

        // active full puzzle
        _completePuzzle.SetActive(true);
        _incompletePuzzle.SetActive(false);

        // prince dance activate
        _princeAnimator.SetTrigger("Dance");

        // used succesfully = true
        InteractibleScript.UsedSuccesfully = true;
    }
}

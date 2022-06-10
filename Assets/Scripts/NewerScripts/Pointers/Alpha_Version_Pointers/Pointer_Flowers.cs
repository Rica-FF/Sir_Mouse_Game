using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Flowers : Pointer_Base
{
    private Animator _flowerAnimator;

    [SerializeField]
    private Animator _princeAnimator;


    private void Start()
    {
        _flowerAnimator = Interactible.transform.GetChild(0).GetComponentInChildren<Animator>();
    }

    public override void PlayEvent()
    {
        base.PlayEvent();

        // call method pour on inter_bucket
        PlayerRefs.attachedObject.GetComponentInChildren<Interactible_Bucket>().PourWater();
        // grow flower and dance prince
        StartCoroutine(GrowFlowers());

        // update GameStateData
        GameStateData.OpenWorldBools[0] = true;
    }


    private IEnumerator GrowFlowers()
    {
        yield return new WaitForSeconds(1f);

        _flowerAnimator.Play("Bloom");

        yield return new WaitForSeconds(1f);

        _princeAnimator.SetTrigger("Dance");
    }
}

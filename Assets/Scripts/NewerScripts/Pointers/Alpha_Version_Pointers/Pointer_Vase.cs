using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Vase : Pointer_Base
{
    [SerializeField]
    private GameObject _spriteObject;

    private GameObject _particleDestruction;


    private void Start()
    {
        _particleDestruction = Interactible.GetComponentInChildren<ParticleSystem>(true).gameObject;  // vase destruction particle system
    }




    public override void PlayEvent()
    {
        base.PlayEvent();

        StartCoroutine(BreakVase());
    }


    IEnumerator BreakVase()
    {
        yield return new WaitForSeconds(0.25f);

        // play sound
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[3]);
        // particles
        _particleDestruction.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        // destroys... vase sprite - popuppointer - trigger
        Destroy(_spriteObject); 

        //yield return new WaitForSeconds(0.25f);

        //// destroys... object wrap
        //Destroy(destructable);

        // only 1 use
        InteractibleScript.UsedSuccesfully = true;
    }
}

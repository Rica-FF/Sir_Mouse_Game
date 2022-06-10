using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Candles : Pointer_Base
{
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameObject _setActiveCollection;
    [SerializeField]
    private GameObject _setFalseCollection;
    [SerializeField]
    private Sprite _darkBackground;


    public override void PlayEvent()
    {
        base.PlayEvent();

        // set specific object to active/in-active
        _setActiveCollection.SetActive(true);
        _setFalseCollection.SetActive(false);

        // background change
        _background.GetComponent<SpriteRenderer>().sprite = _darkBackground;

        // play sound
        InteractibleScript.AudioSource.PlayOneShot(SoundEffects[0]);

        // one use
        InteractibleScript.UsedSuccesfully = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Pancake : MonoBehaviour
{
    private GameObject _hand;

    [SerializeField]
    private GameObject _pan, _pancakeSprite;



    private void ParentPanToMouse()
    {
        _hand = transform.parent.GetComponentInChildren<Interactible_Base>().PlayerRefs.playerHand;
        _pan.transform.SetParent(_hand.transform);
    }

    private void PancakeSpriteActive()
    {
        _pancakeSprite.SetActive(true);
    }


    // do this before animation starts, else we get errors
    //private void UnParentPan()
    //{
    //    Pan.transform.SetParent(null);
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Pancake : MonoBehaviour
{
    private GameObject _hand;

    [SerializeField]
    private GameObject _pan, _pancakeSprite;



    public void ParentPanToHand()
    {
        _hand = transform.parent.GetComponentInChildren<Interactible_Base>().PlayerRefs.playerHand;
        _pan.transform.SetParent(null);
        _pan.transform.SetParent(_hand.transform);
    }



    // do this before animation starts, else we get errors
    //private void UnParentPan()
    //{
    //    Pan.transform.SetParent(null);
    //}

}

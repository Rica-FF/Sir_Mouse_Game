using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Pancake : MonoBehaviour
{
    private GameObject Hand;

    [SerializeField]
    private GameObject Pan;



    private void ParentPanToMouse()
    {
        Hand = transform.parent.GetComponentInChildren<Interactible_Base>().PlayerRefs.playerHand;
        Pan.transform.SetParent(Hand.transform);
    }

}

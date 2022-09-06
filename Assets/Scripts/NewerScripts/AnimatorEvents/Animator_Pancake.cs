using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Pancake : MonoBehaviour
{
    private GameObject Hand;

    [SerializeField]
    private GameObject Pan;

    private void Start()
    {
        Hand = GetComponentInParent<Interactible_Base>().PlayerRefs.playerHand;
    }



    private void ParentPanToMouse()
    {
        Pan.transform.SetParent(Hand.transform);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSequence : MonoBehaviour
{
    public void KillAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}

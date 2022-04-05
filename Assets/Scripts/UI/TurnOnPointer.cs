using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnPointer : MonoBehaviour
{
    public GameObject pointer;

    public void SetActive()
    {
        pointer.SetActive(true);
        pointer.GetComponent<BoxCollider>().enabled = false;
        pointer.GetComponent<Animator>().SetTrigger("NotActive");
    }

    public void ActivatePointer()
    {
        pointer.GetComponent<BoxCollider>().enabled = true;
        pointer.GetComponent<Animator>().SetTrigger("Active");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Milk")
        {
            collider.GetComponent<Animator>().SetBool("Pouring", true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Milk")
        {
            collider.GetComponent<Animator>().SetBool("Pouring", false);
        }
    }
}
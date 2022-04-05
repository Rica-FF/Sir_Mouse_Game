using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appletree_C : MonoBehaviour
{
    public Animator animator;
    public GameObject[] Apples = new GameObject[1];

    public void PlayAnimation()
    {
        animator.SetTrigger("Hit");
        StartCoroutine(SimulatePhysics());
    }

    IEnumerator SimulatePhysics()
    {
        foreach (GameObject objects in Apples)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.05f));
            objects.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}

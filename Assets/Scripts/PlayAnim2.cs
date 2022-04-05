using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim2 : MonoBehaviour
{
    public Animator animator;
    public string triggerName;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            animator.SetTrigger(triggerName);
        }
    }
}

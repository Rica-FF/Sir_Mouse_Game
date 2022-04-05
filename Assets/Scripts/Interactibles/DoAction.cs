using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class DoAction : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public bool specificDestination = false;
    [HideInInspector]
    public Vector3 destination;
    public Vector3 offset;

    public void DoTheAction()
    {
        if (specificDestination)
        {
            destination = transform.position + offset;
            onTriggerEnter.Invoke();
        }
        else
        {
            onTriggerEnter.Invoke();
        }
    }
}

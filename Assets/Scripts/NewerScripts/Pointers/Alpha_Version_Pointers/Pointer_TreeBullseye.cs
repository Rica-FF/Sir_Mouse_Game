using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_TreeBullseye : Pointer_Base
{
    private Animator _animator;
    private AudioSource _audioSource;

    private Rigidbody[] _appleRigids;

    private float _physicsDuration = 8;


    private void Start()
    {
        _animator = Interactible.transform.GetChild(0).GetComponent<Animator>();
        _appleRigids = Interactible.transform.GetChild(1).GetComponentsInChildren<Rigidbody>();

        _audioSource = Interactible.GetComponent<AudioSource>();
    }


    public override void PlayEvent()
    {
        base.PlayEvent();

        // play animation
        _animator.SetTrigger("Hit");
        // play sound effect
        _audioSource.PlayOneShot(SoundEffects[0]);

        // activates gravity on apples
        StartCoroutine(SimulatePhysics());
    }


    // kicks apples down
    IEnumerator SimulatePhysics()
    {
        Debug.Log(_appleRigids.Length + "length");

        foreach (Rigidbody rigid in _appleRigids)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.05f));

            rigid.useGravity = true;
            rigid.GetComponent<OrthoPhysics>().enabled = true;
        }

        StartCoroutine(StopPhysics());
    }


    // halts physics calculations (for performance)
    IEnumerator StopPhysics()
    {
        yield return new WaitForSeconds(_physicsDuration);

        foreach (Rigidbody rigid in _appleRigids)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
            rigid.GetComponent<OrthoPhysics>().enabled = false;
        }
    }



    //private void OnEnable()
    //{
    //    StartCoroutine(StopPhysics());
    //}
}

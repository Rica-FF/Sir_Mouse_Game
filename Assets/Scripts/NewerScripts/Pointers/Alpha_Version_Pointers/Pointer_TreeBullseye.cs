using System;
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
        float randomForce = 0;
        float randomTorque = 0;
        foreach (Rigidbody rigid in _appleRigids)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.05f));

            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.GetComponent<PhysicsCorrector_Spawnable>().enabled = true;

            randomForce = UnityEngine.Random.Range(-1f, 1f);
            randomTorque = UnityEngine.Random.Range(-20,20);
            rigid.AddForce(Camera.main.transform.right * randomForce, ForceMode.Impulse);
            rigid.AddTorque(new Vector3(randomTorque,0,0), ForceMode.Impulse);
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
            rigid.GetComponent<PhysicsCorrector_Spawnable>().enabled = false;
        }


        // empty the list
        Array.Clear(_appleRigids, 0, _appleRigids.Length);

        // re-grow some more apples


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script which is used to correct the position & rotation of interactibles that are moved around
// IF the objects RIGIDBODY get added force, UN-PARENT THE RIGIDBODY to make this logic work
// --- ex logic below ---
//   _rigidInteractible.isKinematic = false;
//   _rigidInteractible.useGravity = true;
//   _rigidInteractible.GetComponent<PhysicsCorrector>().enabled = true;
//   StartCoroutine(_physicsScript.StopPhysicsUpdate(_physicsDuration));
//   _rigidInteractible.transform.SetParent(null);


// examples :
// pointer that kicks an interactible (probably wont see use)
// spawned objects with physics, created from Tap-able interactables


public class PhysicsCorrector : MonoBehaviour
{
    private Rigidbody _rigid;
    private GameObject _interactible;

    private GameObject _interactibleParent; // parent object of which the transform.position will be updated
    private GameObject _sprite;             // sprite object of which the transform.rotation will be updated

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _interactibleParent = transform.parent.gameObject; // null
        _interactible = _interactibleParent.GetComponentInChildren<Interactible_Base>().gameObject;
        _sprite = _interactible.transform.GetChild(0).gameObject;

        this.enabled = false;
    }

    void FixedUpdate()
    {
        _interactibleParent.transform.position = transform.position; // null
        _sprite.transform.localEulerAngles = new Vector3(30, 0, transform.localEulerAngles.z);

        Debug.Log(" PHYSICS UPDATE CURRENTLY GOING ON " + _interactibleParent.name);
    }




    public IEnumerator StartANDStopPhysicsLogic(float timeActive)
    {
        _rigid = GetComponent<Rigidbody>();

        _rigid.isKinematic = false;
        _rigid.useGravity = true;
        _rigid.transform.SetParent(null);

        this.enabled = true;

        yield return new WaitForSeconds(timeActive);

        _rigid.isKinematic = true;
        _rigid.useGravity = false;
        _rigid.transform.SetParent(_interactibleParent.transform);

        this.enabled = false;
    }

    public IEnumerator StopPhysicsLogic()
    {
        _rigid = GetComponent<Rigidbody>();

        _rigid.isKinematic = true;
        _rigid.useGravity = false;
        _rigid.transform.SetParent(_interactibleParent.transform);

        this.enabled = false;

        yield return null;
    }
}

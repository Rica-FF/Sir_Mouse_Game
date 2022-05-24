using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script which is used to correct the position & rotation of interactibles that are moved around

public class PhysicsCorrector : MonoBehaviour
{
    private Rigidbody _rigid;
    private GameObject _interactible;

    private GameObject _interactibleParent; // parent object of which the transform.position will be updated
    private GameObject _sprite;             // sprite object of which the transform.rotation will be updated

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _interactibleParent = transform.parent.gameObject;
        _interactible = _interactibleParent.GetComponentInChildren<Interactible_Base>().gameObject;
        _sprite = _interactible.transform.GetChild(0).gameObject;

        this.enabled = false;
    }

    void FixedUpdate()
    {
        _interactibleParent.transform.position = transform.position;
        _sprite.transform.localEulerAngles = new Vector3(30, 0, transform.localEulerAngles.z);
    }




    // called from the override event on pointer_x
    public IEnumerator StopPhysicsUpdate(float timeActive)
    {
        yield return new WaitForSeconds(timeActive);

        _rigid.isKinematic = true;
        _rigid.useGravity = false;
        _rigid.transform.SetParent(_interactibleParent.transform);

        this.enabled = false;
    }
}

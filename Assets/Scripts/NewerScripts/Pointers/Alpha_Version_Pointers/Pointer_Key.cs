using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Key : Pointer_Base
{
    private Rigidbody _rigidInteractible;
    private PhysicsCorrector _physicsScript;

    [SerializeField]
    private float _physicsDuration;

    



    private void Start()
    {
        _rigidInteractible = InteractibleParent.GetComponentInChildren<Rigidbody>();

        if (_rigidInteractible.TryGetComponent(out PhysicsCorrector physics))
        {
            _physicsScript = physics;
        }
    }


    public override void PickupItemWrap(PickupType pickup)
    {
        base.PickupItemWrap(pickup);

        // set sprite to true
        Interactible.transform.GetChild(0).gameObject.SetActive(true);
    }


    public override void PlayEvent()
    {
        base.PlayEvent();

        StartCoroutine(_physicsScript.StartANDStopPhysicsLogic(_physicsDuration));

        float sidewaysForce = 0;
        if (PlayerRefs.transform.localScale.x > 0)
        {
            sidewaysForce = -3;
        }
        else
        {
            sidewaysForce = 3;
        }

        _rigidInteractible.AddForce(new Vector3(sidewaysForce, 8, 0), ForceMode.Impulse);
        _rigidInteractible.AddTorque(new Vector3(0, 0, 20));
    }
}

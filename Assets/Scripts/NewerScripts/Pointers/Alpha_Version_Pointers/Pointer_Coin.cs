using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Coin : Pointer_Base
{
    public int AssignedValue;

    public Interactible_Well WellFunctionality;

    private int _randomValue;

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

        if (WellFunctionality == null && TypeOfPointer == PointerType.Pickup)
        {
            WellFunctionality = FindObjectOfType<Interactible_Well>();
        }
    }



    public override void PickupItemWrap(PickupType pickup)
    {
        base.PickupItemWrap(pickup);

        // if picked up, and the slot is alrdy taken, randomize the value of this one
        if (WellFunctionality.TakenSpawns[AssignedValue] != null)
        {
            _randomValue = UnityEngine.Random.Range(0, WellFunctionality.CoinSpawns.Length);

            if (WellFunctionality.TakenSpawns[_randomValue] != null)
            {
                GenerateRandomValue();
            }
        }
        else
        {
            WellFunctionality.TakenSpawns[AssignedValue] = null;
        }

        if (WellFunctionality != null)
        {
            WellFunctionality.TakenSpawns[AssignedValue] = null;
        }
       
        // disable kinematic
        _rigidInteractible.isKinematic = true;
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


    private void GenerateRandomValue()
    {
        _randomValue = UnityEngine.Random.Range(0, WellFunctionality.CoinSpawns.Length); // stackoverflow

        if (WellFunctionality.TakenSpawns[_randomValue] != null)
        {
            GenerateRandomValue();
        }
        else
        {
            WellFunctionality.TakenSpawns[_randomValue] = null;
        }
    }
}

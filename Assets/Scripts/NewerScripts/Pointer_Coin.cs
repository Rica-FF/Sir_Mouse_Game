using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Coin : Pointer_Base
{
    public int AssignedValue;

    public Interactible_Well WellFunctionality;

    private int _randomValue;

    private Rigidbody _rigidInteractible;


    private void Start()
    {
        _rigidInteractible = GetComponentInParent<Rigidbody>();
    }



    public override void PickupItemWrap(PickupType pickup)
    {
        base.PickupItemWrap(pickup);

        // if picked up, and the slot is alrdy taken, randomize the value of this one
        //if (WellFunctionality.TakenSpawns[AssignedValue] != null)
        //{
        //    _randomValue = UnityEngine.Random.Range(0, WellFunctionality.CoinSpawns.Length);

        //    if (WellFunctionality.TakenSpawns[_randomValue] != null)
        //    {
        //        GenerateRandomValue();
        //    }
        //}
        //else
        //{
        //    WellFunctionality.TakenSpawns[AssignedValue] = null;
        //}

        _rigidInteractible.isKinematic = true;
        
    }


    public override void PlayEvent()
    {
        base.PlayEvent();

        _rigidInteractible.isKinematic = false;
        _rigidInteractible.AddForce(new Vector3(5,0,10),ForceMode.Impulse);
        _rigidInteractible.AddTorque(new Vector3(0,0,2));
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

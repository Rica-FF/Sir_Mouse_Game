using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Well : Interactible_Base
{
    // put this on the well_interactible , coin pointers can refer to this list for when they're picked up
    public GameObject[] CoinSpawns;

    public GameObject[] TakenSpawns;

    private void Start()
    {
        TakenSpawns = new GameObject[CoinSpawns.Length];
    }
}

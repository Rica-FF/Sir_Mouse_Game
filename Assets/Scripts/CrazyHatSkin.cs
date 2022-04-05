using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyHatSkin : MonoBehaviour
{
    public GameObject[] NewGeo = new GameObject[0];

    private bool active = false;

    public void SetCrazyHat()
    {
        if (active)
        {
            for (int i = 0; i < NewGeo.Length; i++)
            {
                NewGeo[i].SetActive(false);
            }
            active = false;
        }
        else
        {
            for (int i = 0; i < NewGeo.Length; i++)
            {
                NewGeo[i].SetActive(true);
            }
            active = true;
        }
    }
}

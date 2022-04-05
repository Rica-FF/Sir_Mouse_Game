using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSkin : MonoBehaviour
{
    public bool newGeo = false;
    public bool HideOriginal = false;
    public bool JustHide = false;
    public GameObject geometry;
    public GameObject[] moreOriginals = new GameObject[0];
    public GameObject[] moreNewGeo = new GameObject[0];

    public void GetChickenSkin()
    {           
        if(JustHide)
        {
            gameObject.SetActive(false);

            for (int i = 0; i < moreNewGeo.Length; i++)
            {
                moreNewGeo[i].SetActive(false);
            }
        }
        else
        {
            if (newGeo)
            {
                if (HideOriginal)
                {
                    for (int i = 0; i < moreOriginals.Length; i++)
                    {
                        moreOriginals[i].SetActive(false);
                    }
                    gameObject.SetActive(false);
                }
                for (int i = 0; i < moreNewGeo.Length; i++)
                {
                    moreNewGeo[i].SetActive(true);
                }
                geometry.SetActive(true);
            }
        }
    }
}

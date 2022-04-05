using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnGold : MonoBehaviour
{
    public Material[] materials = new Material[1];
    public bool newGeo = false;
    public GameObject[] moreNewGeo = new GameObject[0];
    public bool HideOriginal = false;
    public bool JustHide = false;
    public GameObject geometry;

    public void TurnMaterialGold()
    {
        if(JustHide)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if(newGeo)
            {
                for (int i = 0; i < moreNewGeo.Length; i++)
                {
                    moreNewGeo[i].SetActive(true);
                }
                geometry.SetActive(true);
                if(HideOriginal)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.GetComponent<SkinnedMeshRenderer>().materials = materials;
            }
        }

    }
}

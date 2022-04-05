using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnNormal : MonoBehaviour
{
    public Material[] materials = new Material[1];
    public bool newGeo = false;
    public bool HideOriginal = false;
    public GameObject geometry;

    public bool JustHide = false;

    public void TurnMaterialNormal()
    {
        if (JustHide)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (newGeo)
            {
                geometry.SetActive(true);
                if (HideOriginal)
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

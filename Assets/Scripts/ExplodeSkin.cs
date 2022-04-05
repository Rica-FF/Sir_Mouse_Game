using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeSkin : MonoBehaviour
{
    public Material[] materials = new Material[1];
    private Material[] OriginalMaterials = new Material[1];
    private bool dirtyFace = false;

    public void TurnExploded()
    {
        if (dirtyFace)
        {
            StartCoroutine(clean());
        }
        else
        {
            OriginalMaterials = gameObject.GetComponent<SkinnedMeshRenderer>().materials;
            gameObject.GetComponent<SkinnedMeshRenderer>().materials = materials;
            dirtyFace = true;
        }
    }

    IEnumerator clean()
    {
        float seconds = 2f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        gameObject.GetComponent<SkinnedMeshRenderer>().materials = OriginalMaterials;
    }
}

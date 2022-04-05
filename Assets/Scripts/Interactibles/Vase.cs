using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public GameObject player;

    private GameObject destructable;
    private GameObject vase;
    private GameObject particles;

    private void Start()
    {
        destructable = transform.parent.gameObject;
        vase = transform.parent.gameObject.transform.GetChild(0).gameObject;
        particles = transform.parent.gameObject.transform.GetChild(1).gameObject;
    }

    public void Pressed()
    {
        StartCoroutine(BrakeVase());
    }

    IEnumerator BrakeVase()
    {
        player.GetComponentInChildren<PlayerTouchControls>().Slash(3);

        float seconds = 0.25f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / 1f;
            yield return null;
        }

        particles.SetActive(true);

        seconds = 0.25f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / 1f;
            yield return null;
        }
        
        Destroy(vase);

        
        seconds = 0.25f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / 1f;
            yield return null;
        }
        Destroy(destructable);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRoses : MonoBehaviour
{
    public GameObject player;
    public GameObject roses;

    void Start()
    {
        StartCoroutine(CheckReference());
    }

    IEnumerator CheckReference()
    {
        float seconds = 0.5f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        if (player.GetComponent<PlayerTouchControls>().player.GetComponent<PlayerReferences>().rosesHaveWater)
        {
            if (roses.name == "RosesTower")
            {
                roses.SetActive(true);
            }
            else
            {
                roses.GetComponent<Animator>().SetTrigger("Bloom");
            }
        }

    }
}

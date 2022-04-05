using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public List<GameObject> plantedCorn = new List<GameObject>();
    public GameObject player;

    private GameObject playerRig;

    private bool addCorn = true;

    public bool playerInArea = false;

    private void Start()
    {
        playerRig = player.GetComponent<PlayerTouchControls>().player;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            playerInArea = false;
        }
    }

    public void UseSoil()
    {
        if(addCorn)
        {
            addCorn = false;
            if (playerRig.GetComponent<PlayerReferences>().attachedObject)
            {
                if (playerRig.GetComponent<PlayerReferences>().attachedObject.name == "Corn")
                {
                    StartCoroutine(PlantingSound());
                    plantedCorn.Add(playerRig.GetComponent<PlayerReferences>().attachedObject);
                }
                else if (playerRig.GetComponent<PlayerReferences>().attachedObject.name == "Bucket")
                {
                    for(int i = 0; i < plantedCorn.Count; i++)
                    {
                        plantedCorn[i].GetComponent<Corn>().GrowCorn();
                    }
                }
            }
            StartCoroutine(SetAddCornBool());
        }
    }

    IEnumerator PlantingSound()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().Play();
    }

    IEnumerator SetAddCornBool()
    {
        float seconds = 3f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        addCorn = true;
    }
}

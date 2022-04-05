using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornMountain : MonoBehaviour
{
    public GameObject player;
    public GameObject corn;
    private GameObject corn_Clone;

    public void SpawnCornAndPickUp()
    {
        corn_Clone = Instantiate(corn, new Vector3(-1000, -1000, -1000), Quaternion.identity);
        corn_Clone.GetComponent<Corn>().player = player;
        corn_Clone.name = "Corn";
        corn_Clone.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(PickUpCorn());
    }

    IEnumerator PickUpCorn()
    {
        float seconds = 0.2f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        corn_Clone.GetComponent<Corn>().PressedPickUp();

        seconds = 5f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
        
    }

    public void CornInBucket()
    {
        corn_Clone = Instantiate(corn, new Vector3(-25.5f, -1.15f, -9.65f), Quaternion.identity);
        corn_Clone.GetComponent<Corn>().player = player;
        corn_Clone.name = "Corn";
        corn_Clone.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(FlyingCorn());
    }

    IEnumerator FlyingCorn()
    {
        float seconds = 1f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        corn_Clone.GetComponent<Animator>().enabled = true;
        
        seconds = 4f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        corn_Clone.GetComponent<Corn>().CornInBucket();
    }
}

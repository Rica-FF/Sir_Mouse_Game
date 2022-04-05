using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsObject : MonoBehaviour
{
    public GameObject pointer;
    public string objectName;

    private bool objectInCol = false;
    private GameObject player;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !player)
        {
            player = collider.gameObject.transform.Find("RM_Player").gameObject;
        }

        if (collider.tag == "Player" && player.GetComponent<PlayerReferences>().attachedObject && player)
        {
            if (player.GetComponent<PlayerReferences>().attachedObject.name == objectName)
            {
                pointer.SetActive(true);
            }
        }

        if (collider.tag == objectName)
        {
            objectInCol = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            pointer.SetActive(false);
        }

        if (collider.tag == objectName)
        {
            objectInCol = false;
        }
    }

    private void Update()
    {
        if (pointer.activeSelf && objectInCol && !player.GetComponent<PlayerReferences>().attachedObject)
        {
            pointer.SetActive(false);
        }

        if (player)
        {
            if (player.GetComponent<PlayerReferences>().attachedObject)
            {
                if (!pointer.activeSelf && objectInCol && player.GetComponent<PlayerReferences>().attachedObject.name == objectName)
                {
                    pointer.SetActive(true);
                }
            }
        }
    }
}

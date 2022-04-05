using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpot : MonoBehaviour
{
    public GameObject sparkle;

    private GameObject player;
    private GameObject bucket;
    private bool onSpot = false;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (!player)
            {
                player = collider.gameObject.GetComponent<PlayerTouchControls>().player;
            }
            
            if(player)
            {
                player.GetComponent<PlayerReferences>().onWaterSpot = true;
            }
        }

        if (collider.tag == "Bucket")
        {
            bucket = collider.gameObject;
            onSpot = true;
            collider.gameObject.GetComponent<Bucket>().onSpot = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Bucket")
        {
            onSpot = false;
            collider.gameObject.GetComponent<Bucket>().onSpot = false;
        }

        if (collider.tag == "Player")
        {
            if (player)
            {
                player.GetComponent<PlayerReferences>().onWaterSpot = false;
            }
        }
    }

    private void Update()
    {
        if (onSpot)
        {
            if (player && !player.GetComponent<PlayerReferences>().attachedObject && !bucket.GetComponent<Bucket>().water.activeSelf)
            {
                sparkle.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                sparkle.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
    }
}

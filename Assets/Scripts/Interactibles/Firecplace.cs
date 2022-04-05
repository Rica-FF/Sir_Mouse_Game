using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firecplace : MonoBehaviour
{
    public GameObject popcornFX;
    public GameObject player;
    public GameObject fire;
    public GameObject smoke;

    public bool turnRight = false;
    public bool NewCorn = false;

    private GameObject playerRig;

    private void Start()
    {
        playerRig = player.GetComponent<PlayerTouchControls>().player;
    }

    public void ThrowInFire()
    {
        
        //playerRig.GetComponent<PlayerReferences>().ClearList();
        if (playerRig.GetComponent<PlayerReferences>().attachedObject)
        {
            StartCoroutine(DelayAction());
        }
    }

    IEnumerator DelayAction()
    {
        float seconds = 0.5f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        if(turnRight)
        {
            playerRig.transform.localScale = new Vector3(-6, 6, 6);
        }
        else
        {
            playerRig.transform.localScale = new Vector3(6, 6, 6);
        }

        if (playerRig.GetComponent<PlayerReferences>().attachedObject.name == "Corn")
        {
            popcornFX.GetComponent<ParticleSystem>().Play();
            if(NewCorn)
            {
                playerRig.GetComponent<PlayerReferences>().attachedObject.GetComponent<Corn>().ThrowOnStove();
                playerRig.GetComponent<PlayerReferences>().attachedObject.transform.parent = null;
                playerRig.GetComponent<PlayerReferences>().attachedObject.GetComponent<Animator>().enabled = true;
            }
            else
            {
                playerRig.GetComponent<PlayerReferences>().attachedObject.GetComponent<Corn>().ThrowCorn(-26.75f);
            }

            yield return new WaitForSeconds(1f);
            GetComponent<AudioSource>().Play();
        }
        else if (playerRig.GetComponent<PlayerReferences>().attachedObject.name == "Bucket")
        {
            player.GetComponent<PlayerTouchControls>().DoAction("");
            gameObject.GetComponent<BoxCollider>().enabled = false;
            fire.SetActive(false);
            smoke.SetActive(true);
        }
        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        
        player.transform.GetChild(1).gameObject.GetComponent<PlayerReferences>().madePopcorn = true;

        yield return new WaitForSeconds(2f);
        GetComponent<BoxCollider>().enabled = true;
    }
}

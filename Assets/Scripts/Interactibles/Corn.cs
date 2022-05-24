using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : MonoBehaviour
{
    public GameObject player;
    public GameObject smallCorn;
    public Sprite plantedCorn;

    private GameObject playerRig;
    private bool pressedToPickUp = false;
    private bool pressedToPlant = false;

    [HideInInspector]
    public int growPhase = 0;

    [HideInInspector]
    static public float offset = 0;

    private void Start()
    {
        StartCoroutine(SetPlayerRig());
    }

    IEnumerator SetPlayerRig()
    {
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        playerRig = player.transform.Find("RM_Player").gameObject;

    }

    private void Update()
    {
        if (pressedToPickUp)
        {
            if (player.GetComponent<PlayerTouchControls>().curSpeed < 0.01)
            {
                StartCoroutine(PickUpCorn());
                pressedToPickUp = false;
            }
        }

        if (pressedToPlant)
        {
            if (player.GetComponent<PlayerTouchControls>().curSpeed < 0.01)
            {
                StartCoroutine(PlantInSoil());
                pressedToPlant = false;
            }
        }
    }

    public void PressedPickUp()
    {
        PopUpPointer.disableIrrelevantPointers = true;
        StartCoroutine(DelayPressedBool_PickUp());
    }

    IEnumerator DelayPressedBool_PickUp()
    {
        // Short delay
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        pressedToPickUp = true;
    }

    IEnumerator PickUpCorn()
    {
        playerRig.GetComponent<PlayerReferences>().SetSound(1);
        playerRig.GetComponent<PlayerReferences>().attachedObject = gameObject;
        playerRig.transform.localScale = new Vector3(6, 6, 6);
        playerRig.GetComponent<Animator>().SetTrigger("PickUpCoin");
        playerRig.GetComponent<PlayerReferences>().ClearList();

        float seconds = 0.8f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        transform.parent = playerRig.GetComponent<PlayerReferences>().playerHand.transform;
        transform.localPosition = new Vector3(-0.031f, 0.005f, 0.01f);
        transform.localRotation = Quaternion.Euler(89, 310, -34.5f);

        seconds = 0.2f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        playerRig.GetComponent<Animator>().SetLayerWeight(1, 1f);

        //playerRig.transform.Find("Pointer").transform.GetChild(0).gameObject.SetActive(true);
    }

    public void CornInBucket()
    {
        GetComponent<Animator>().enabled = false;
        transform.parent = playerRig.GetComponent<PlayerReferences>().attachedObject.transform;
        transform.localPosition = new Vector3(-0.031f, 0.005f, 0.01f + offset);
        offset += 0.3f;
    }

    public void Drop()
    {
        PopUpPointer.disableIrrelevantPointers = false;
        playerRig.GetComponent<Animator>().SetTrigger("DropCoin");
        StartCoroutine(DetachCorn());
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "";
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
    }

    IEnumerator DetachCorn()
    {
        float seconds = 0.4f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        transform.parent = null;
        transform.localRotation = Quaternion.Euler(30, 0, 0);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.localScale = new Vector3(0.434895f, 0.434895f, 0.434895f);
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);
        transform.GetComponent<SphereCollider>().enabled = true;
    }

    public void PlantCorn()
    {
        //player.GetComponent<PlayerTouchControls>().TurnOffDropPointer();        
        StartCoroutine(DelayPressedBool_PlantCorn());
    }

    IEnumerator DelayPressedBool_PlantCorn()
    {
        // Short delay
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        growPhase = 1;
        pressedToPlant = true;
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "";
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
    }

    IEnumerator PlantInSoil()
    {
        PopUpPointer.disableIrrelevantPointers = false;
        playerRig.GetComponent<Animator>().SetTrigger("DropCoin");
        float seconds = 0.4f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        transform.parent = null;
        transform.localRotation = Quaternion.Euler(30, 0, -90);
        transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
        playerRig.GetComponent<PlayerReferences>().cornPositions.Add(new Vector3(transform.position.x, -0.5f, transform.position.z));
        playerRig.GetComponent<PlayerReferences>().cornGrowPhases.Add(1);
        transform.localScale = new Vector3(0.434895f, 0.434895f, 0.434895f);
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);

        GetComponent<SpriteRenderer>().sprite = plantedCorn;
        smallCorn.SetActive(true);
    }

    public void CornAlreadyPlanted()
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = plantedCorn;
        smallCorn.SetActive(true);
        if (growPhase > 1)
        {
            smallCorn.GetComponentInChildren<Animator>().SetTrigger("GrowBig");
        }
    }

    public void GrowCorn()
    {
        if(growPhase < 2)
        {
            smallCorn.GetComponentInChildren<Animator>().SetTrigger("GrowBig");
            for(int i = 0; i < playerRig.GetComponent<PlayerReferences>().cornGrowPhases.Count; i++)
            {
                playerRig.GetComponent<PlayerReferences>().cornGrowPhases[i] = 2;
            }
            growPhase = 2;
        }
    }

    public void ThrowCorn(float fireplacePos)
    {
        /*
        if (fireplacePos > player.transform.position.x)
        {
            playerRig.transform.localScale = new Vector3(-6, 6, 6);
            print("Left");
        }
        else
        {
            print("Right");
            playerRig.transform.localScale = new Vector3(6, 6, 6);
        }
        */
        PopUpPointer.disableIrrelevantPointers = false;
        playerRig.GetComponent<Animator>().SetTrigger("DropCoin");
        //playerRig.GetComponent<PlayerReferences>().attachedObject = null;
        StartCoroutine(DestroyCorn(0.4f));
    }

    public void ThrowOnStove()
    {
        PopUpPointer.disableIrrelevantPointers = false;
        StartCoroutine(DestroyCorn(2f));
    }

    IEnumerator DestroyCorn(float _seconds)
    {
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);
        float seconds = _seconds;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
        Destroy(gameObject);
    }
}

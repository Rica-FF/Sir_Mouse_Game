using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    public GameObject player;

    private GameObject playerRig;
    private bool pressedToPickUp = false;

    public GameObject candles;

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
                StartCoroutine(PickUpPuzzle());
                pressedToPickUp = false;
            }
        }
    }

    public void PressedPickUp()
    {
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

    IEnumerator PickUpPuzzle()
    {
        PopUpPointer.disableIrrelevantPointers = true;
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "PuzzlePiece";
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

    public void Drop()
    {
        player = playerRig.transform.parent.gameObject;
        PopUpPointer.disableIrrelevantPointers = false;
        playerRig.GetComponent<Animator>().SetTrigger("DropCoin");
        StartCoroutine(DetachPuzzlePiece());
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "";
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
    }

    IEnumerator DetachPuzzlePiece()
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
        transform.localScale = new Vector3(0.7822092f, 0.7822092f, 0.7822092f);
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);
        transform.GetComponent<SphereCollider>().enabled = true;
    }

    public void CompletePuzzle()
    {
        PopUpPointer.disableIrrelevantPointers = false;
        player = playerRig.transform.parent.gameObject;
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "";
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
        StartCoroutine(DestroyPuzzle());
    }

    IEnumerator DestroyPuzzle()
    {
        float seconds = 0.1f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (candles)
        {
            candles.GetComponent<Candles>().keyDiscovered = true;
        }
    }
}

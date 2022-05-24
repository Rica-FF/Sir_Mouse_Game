using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject player;
    public GameObject WishingWell;
    public GameObject CloudFx;

    private GameObject playerRig;
    private bool pressedToPickUp = false;
    private bool pressedToThrow = false;


    //////////



    private void Start()
    {
        StartCoroutine(SetPlayerRig());
    }



    private void Update()
    {
        if (pressedToPickUp)
        {
            if (player.GetComponent<PlayerTouchControls>().curSpeed < 0.01)
            {
                StartCoroutine(PickUpCoin());
                pressedToPickUp = false;
            }
        }

        if (pressedToThrow)
        {
            if (player.GetComponent<PlayerTouchControls>().curSpeed < 0.01)
            {
                StartCoroutine(ThrowCoinInWell());
                pressedToThrow = false;
            }
        }
    }






    public void PressedPickUp()
    {
        StartCoroutine(DelayPressedBool_PickUp());
    }
    public void Drop()
    {
        PopUpPointer.disableIrrelevantPointers = false;
        playerRig.GetComponent<Animator>().SetTrigger("DropCoin");
        StartCoroutine(DetachCoin());
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "";
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
    }



    public void PressedThrow()
    {
        //player.GetComponent<PlayerTouchControls>().TurnOffDropPointer();
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "";
        StartCoroutine(DelayPressedBool_Throw());
    }




    IEnumerator SetPlayerRig() // assign player object
    {
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        playerRig = player.transform.Find("RM_Player").gameObject;

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
    IEnumerator PickUpCoin()
    {
        playerRig.GetComponent<PlayerReferences>().SetSound(0);
        PopUpPointer.disableIrrelevantPointers = true;
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "Coin";
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
    IEnumerator DetachCoin()
    {
        float seconds = 0.4f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        // coin transform
        transform.parent = null;
        transform.localRotation = Quaternion.Euler(30, 0, 90);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);       
        transform.GetComponent<SphereCollider>().enabled = true;
    }
    IEnumerator DelayPressedBool_Throw()
    {
        // Short delay
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        pressedToThrow = true;
    }
    IEnumerator ThrowCoinInWell()
    {
        playerRig.transform.localScale = new Vector3(6, 6, 6); 
        playerRig.GetComponent<Animator>().SetTrigger("ThrowCoin");

        float seconds = 0.3f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        transform.parent = null;
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().SetTrigger("Flip");
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);

        seconds = 1f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        GetComponent<Animator>().SetTrigger("Idle");

        seconds = 0.1f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        GetComponent<Animator>().enabled = false;
        GetComponent<SphereCollider>().enabled = true;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        GetComponent<RandomSpawn>().SpawnRandomSpot();
        PopUpPointer.disableIrrelevantPointers = false;

        WishingWell.GetComponent<PopUpPointer>().EnablePointer();

        Instantiate(CloudFx, player.transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);
        Instantiate(CloudFx, GetComponent<Collider>().transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);

        SetNewCostume();
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
    }




    private void SetNewCostume()
    {
        WishingWell.GetComponent<AudioSource>().Play();
        // Set new costume
        if (playerRig.GetComponent<PlayerReferences>().costumeIndex == 0) // Crazy Hat
        {
            foreach (CrazyHatSkin component in player.GetComponentsInChildren<CrazyHatSkin>())
            {
                component.SetCrazyHat();
            }
            playerRig.GetComponent<PlayerReferences>().costumeIndex = 1;
        }
        else if (playerRig.GetComponent<PlayerReferences>().costumeIndex == 1) // Chicken Skin
        {
            foreach (CrazyHatSkin component in player.GetComponentsInChildren<CrazyHatSkin>())
            {
                component.SetCrazyHat();
            }
            foreach (ChickenSkin component in player.GetComponentsInChildren<ChickenSkin>())
            {
                component.GetChickenSkin();
            }
            playerRig.GetComponent<PlayerReferences>().costumeIndex = 2;
        }
        else if (playerRig.GetComponent<PlayerReferences>().costumeIndex == 2) // Ostrich Skin
        {
            foreach (ChickenSkin component in player.GetComponentsInChildren<ChickenSkin>())
            {
                component.GetChickenSkin();
            }
            foreach (OstrichSkin component in player.GetComponentsInChildren<OstrichSkin>())
            {
                component.GetOstrichSkin();
            }
            playerRig.GetComponent<PlayerReferences>().costumeIndex = 3;
        }
        else if (playerRig.GetComponent<PlayerReferences>().costumeIndex == 3) // PJ Skin
        {
            foreach (PJSkin component in player.GetComponentsInChildren<PJSkin>())
            {
                component.GetPJSkin();
            }
            playerRig.GetComponent<PlayerReferences>().costumeIndex = 4;
        }
        else if (playerRig.GetComponent<PlayerReferences>().costumeIndex == 4) // Gold Skin
        {
            foreach (TurnGold component in player.GetComponentsInChildren<TurnGold>())
            {
                component.TurnMaterialGold();
            }
            foreach (TurnGold component in player.GetComponentsInChildren<TurnGold>())
            {
                component.TurnMaterialGold();
            }
            playerRig.GetComponent<PlayerReferences>().costumeIndex = 5;
        }
        else if (playerRig.GetComponent<PlayerReferences>().costumeIndex == 5) // Normal Skin
        {
            foreach (TurnNormal component in player.GetComponentsInChildren<TurnNormal>())
            {
                component.TurnMaterialNormal();
            }
            playerRig.GetComponent<PlayerReferences>().costumeIndex = 0;
        }
    }
}

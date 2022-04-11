using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_New : Interactible_Base
{
    private GameObject player;
    public GameObject WishingWell;
    public GameObject CloudFx;

    private GameObject playerRig;
    private bool pressedToPickUp = false;
    private bool pressedToThrow = false;


    //////////

    private void Update()
    {
        if (pressedToThrow)
        {
            if (player.GetComponent<PlayerTouchControls>().curSpeed < 0.01)
            {
                StartCoroutine(ThrowCoinInWell());
                pressedToThrow = false;
            }
        }
    }








    public void PressedThrow()
    {
        //player.GetComponent<PlayerTouchControls>().TurnOffDropPointer();
        player.GetComponent<PlayerTouchControls>().attachedObject = "";
        StartCoroutine(DelayPressedBool_Throw());
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

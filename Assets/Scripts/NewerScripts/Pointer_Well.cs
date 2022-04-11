using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Well : Pointer_Base
{
    public GameObject WishingWell;
    public GameObject CloudFx;



    public override void PlayEvent()
    {
        base.PlayEvent();

        if (TypeOfPickup == PickupType.Coin)
        {
            ThrowCoinInWellWrap();
        }
    }



    private void ThrowCoinInWellWrap()
    {
        // Throwing the coin in the well
        StartCoroutine(ThrowCoinInWell());
    }



    IEnumerator ThrowCoinInWell()
    {
        //playerRig.transform.localScale = new Vector3(6, 6, 6);
        PlayerRefs.GetComponent<Animator>().SetTrigger("ThrowCoin");

        float seconds = 0.3f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        //transform.parent = null;
        Interactible.transform.parent = null;
        Interactible.GetComponent<Animator>().enabled = true;
        Interactible.GetComponent<Animator>().SetTrigger("Flip");
        //playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);

        seconds = 1f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        Interactible.GetComponent<Animator>().SetTrigger("Idle");

        seconds = 0.1f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        Interactible.GetComponent<Animator>().enabled = false;
        Interactible.GetComponent<SphereCollider>().enabled = true;
        Interactible.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        // respawn a coin
        //Interactible.GetComponent<RandomSpawn>().SpawnRandomSpot();

        // re-enable pointer
        //WishingWell.GetComponent<PopUpPointer>().EnablePointer();

        // particle effects
        Instantiate(CloudFx, PlayerControls.transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);
        Instantiate(CloudFx, Interactible.GetComponent<Collider>().transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);

        // give a new costume
        SetNewCostume();

        // set attached object to null
        PlayerRefs.attachedObject = null;
    }

    private void SetNewCostume()
    {
        // play sound effect
        WishingWell.GetComponent<AudioSource>().Play();


        // Set new costume
        if (PlayerRefs.costumeIndex == 0) // Crazy Hat
        {
            foreach (CrazyHatSkin component in PlayerControls.GetComponentsInChildren<CrazyHatSkin>())
            {
                component.SetCrazyHat();
            }
            PlayerRefs.costumeIndex = 1;
        }
        else if (PlayerRefs.costumeIndex == 1) // Chicken Skin
        {
            foreach (CrazyHatSkin component in PlayerControls.GetComponentsInChildren<CrazyHatSkin>())
            {
                component.SetCrazyHat();
            }
            foreach (ChickenSkin component in PlayerControls.GetComponentsInChildren<ChickenSkin>())
            {
                component.GetChickenSkin();
            }
            PlayerRefs.costumeIndex = 2;
        }
        else if (PlayerRefs.costumeIndex == 2) // Ostrich Skin
        {
            foreach (ChickenSkin component in PlayerControls.GetComponentsInChildren<ChickenSkin>())
            {
                component.GetChickenSkin();
            }
            foreach (OstrichSkin component in PlayerControls.GetComponentsInChildren<OstrichSkin>())
            {
                component.GetOstrichSkin();
            }
            PlayerRefs.costumeIndex = 3;
        }
        else if (PlayerRefs.costumeIndex == 3) // PJ Skin
        {
            foreach (PJSkin component in PlayerControls.GetComponentsInChildren<PJSkin>())
            {
                component.GetPJSkin();
            }
            PlayerRefs.costumeIndex = 4;
        }
        else if (PlayerRefs.costumeIndex == 4) // Gold Skin
        {
            foreach (TurnGold component in PlayerControls.GetComponentsInChildren<TurnGold>())
            {
                component.TurnMaterialGold();
            }
            foreach (TurnGold component in PlayerControls.GetComponentsInChildren<TurnGold>())
            {
                component.TurnMaterialGold();
            }
            PlayerRefs.costumeIndex = 5;
        }
        else if (PlayerRefs.costumeIndex == 5) // Normal Skin
        {
            foreach (TurnNormal component in PlayerControls.GetComponentsInChildren<TurnNormal>())
            {
                component.TurnMaterialNormal();
            }
            PlayerRefs.costumeIndex = 0;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Well : Pointer_Base
{
    //public GameObject WishingWell;
    public GameObject CloudFx;

    private Vector3 _flickPosition;


    public override void PlayEvent()
    {
        base.PlayEvent();

        ThrowCoinInWellWrap();
    }





    private void ThrowCoinInWellWrap()
    {
        StartCoroutine(ThrowCoinInWell());
    }



    IEnumerator ThrowCoinInWell()
    {
        _flickPosition = PlayerRefs.attachedObject.transform.position;

        PlayerRefs.PickedUpObject = PickupType.None;
        PlayerRefs.attachedObject.transform.parent = null;
        PlayerRefs.attachedObject.transform.rotation = Quaternion.Euler(30,0,90);
        PlayerRefs.attachedObject.transform.position = _flickPosition;
        
        PlayerRefs.attachedObject.GetComponent<Animator>().enabled = true;
        PlayerRefs.attachedObject.GetComponent<Animator>().Play("FlipCoin_New");

        yield return new WaitForSeconds(1f);

        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);

        // respawn a coin     
        RespawnCoin();

        // particle effects
        Instantiate(CloudFx, PlayerControls.transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);
        Instantiate(CloudFx, Interactible.GetComponent<Collider>().transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);

        // give a new costume
        SetNewCostume();

        PlayerRefs.attachedObject.GetComponent<Animator>().enabled = false;

        // set attached object to null
        PlayerRefs.attachedObject = null;
    }



    private void RespawnCoin()
    {
        if (Interactible.TryGetComponent(out Interactible_Well wellScript))
        {
            GenerateRandomSpawn(wellScript);        
        }

        PlayerRefs.attachedObject.GetComponent<SphereCollider>().enabled = true;
        PlayerRefs.attachedObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
    private void GenerateRandomSpawn(Interactible_Well wellScript)
    {
        var randomSpot = UnityEngine.Random.Range(0, wellScript.CoinSpawns.Length);
        PlayerRefs.attachedObject.GetComponentInChildren<Pointer_Coin>(true).AssignedValue = randomSpot; // out of bounds ?

        if (wellScript.TakenSpawns[randomSpot] == null)
        {
            Debug.Log(randomSpot + " assigned spot");
            PlayerRefs.attachedObject.transform.position = wellScript.CoinSpawns[randomSpot].transform.position; // put on position
            wellScript.TakenSpawns[randomSpot] = PlayerRefs.attachedObject; // put it in taken array
        }
        else
        {
            GenerateRandomSpawn(wellScript);
        }
    }




    private void SetNewCostume()
    {
        // play sound effect
        //WishingWell.GetComponent<AudioSource>().Play();


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

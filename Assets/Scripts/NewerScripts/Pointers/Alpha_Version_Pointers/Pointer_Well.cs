using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Well : Pointer_Base
{
    //public GameObject WishingWell;
    public GameObject CloudFx;

    private Vector3 _flickPosition;

    private GameObject _coinParent;



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
        // assign value 
        _coinParent = PlayerRefs.attachedObject;
        // get flick position
        _flickPosition = PlayerRefs.attachedObject.transform.position;
        _coinParent.transform.localScale = new Vector3(Mathf.Abs(_coinParent.transform.localScale.x), _coinParent.transform.localScale.y, _coinParent.transform.localScale.z); // setting direction correctly
        // make attached object null
        PlayerRefs.attachedObject = null;
        PlayerRefs.PickedUpObject = PickupType.None;
        // set transform of coin
        _coinParent.transform.SetParent(null);
        _coinParent.transform.position = _flickPosition;

        _coinParent.GetComponentInChildren<Rigidbody>().isKinematic = true;
        _coinParent.GetComponentInChildren<Rigidbody>().useGravity = false;

        // state not found
        // have new animation be possible !!!
        _coinParent.GetComponentInChildren<Animator>().enabled = true;
        _coinParent.GetComponentInChildren<Animator>().Play("FlipCoin_Newest");

        yield return new WaitForSeconds(1f);

        Interactible.GetComponent<SphereCollider>().enabled = true;

        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);

        // respawn a coin     
        RespawnCoin();

        // disable all sparkle objects
        foreach (var sparkle in _coinParent.GetComponentInChildren<Pointer_Lord>().SparkleObjectsAll)
        {
            sparkle.SetActive(false);
        }

        // particle effects
        Instantiate(CloudFx, PlayerControls.transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);
        Instantiate(CloudFx, Interactible.GetComponent<Collider>().transform.position + new Vector3(0f, 2f, -2f), Quaternion.identity);

        // give a new costume
        SetNewCostume();
    }



    private void RespawnCoin()
    {
        if (Interactible.TryGetComponent(out Interactible_Well wellScript))
        {
            GenerateRandomSpawn(wellScript);        
        }

        _coinParent.GetComponentInChildren<SphereCollider>().enabled = true;

        _coinParent.transform.localScale = new Vector3(1, 1, 1);
        _coinParent.transform.rotation = Quaternion.Euler(Vector3.zero);

        _coinParent.GetComponentInChildren<SpriteRenderer>(true).gameObject.SetActive(true);


    }
    private void GenerateRandomSpawn(Interactible_Well wellScript)
    {
        var randomSpot = UnityEngine.Random.Range(0, wellScript.CoinSpawns.Length);
        Debug.Log(randomSpot + " CALC");

        _coinParent.GetComponentInChildren<Pointer_Coin>(true).AssignedValue = randomSpot; // out of bounds ?

        if (wellScript.TakenSpawns[randomSpot] == null)
        {
            Debug.Log(randomSpot + " assigned spot");
            _coinParent.transform.position = wellScript.CoinSpawns[randomSpot].transform.position; // put on position
            wellScript.TakenSpawns[randomSpot] = _coinParent; // put it in taken array
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

        // 



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

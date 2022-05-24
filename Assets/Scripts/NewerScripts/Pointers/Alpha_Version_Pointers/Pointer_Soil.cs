using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Soil : Pointer_Base
{
    private Interactible_Soil _interactibleSoilScript;

    [SerializeField]
    private GameObject _plantedCornSpriteAnimator;

    private GameObject _pickedUpPickup;


    private void Start()
    {
        _interactibleSoilScript = GetComponentInParent<Interactible_Soil>();
    }


    public override void PlayEvent()
    {
        base.PlayEvent();

        // if something has been planted...
        if (_interactibleSoilScript.PlantedPickup != PickupType.None) 
        {
            //  if i have a bucket...
            if (PlayerRefs.PickedUpObject == PickupType.BucketWater)
            {
                // give water with bucket
                PlayerRefs.attachedObject.GetComponentInChildren<Interactible_Bucket>().PourWater();
                // check what pickup the water is being used on
                switch (_interactibleSoilScript.PlantedPickup) 
                {
                    case PickupType.Corn:
                        _plantedCornSpriteAnimator.GetComponent<Animator>().Play("BigGrow_New");
                        break;
                    case PickupType.Coin:
                        break;
                    case PickupType.Puzzle:
                        break;
                    case PickupType.Key:
                        break;
                }
            } // if no bucket...
            else
            {
                // uproot the planted pickup

                // reset the bool 
                InteractibleScript.InteractibleHasBeenUsedToActivateEventNone = false;

                // set false sprite
                switch (_interactibleSoilScript.PlantedPickup)
                {
                    case PickupType.Corn:
                        _plantedCornSpriteAnimator.SetActive(false);
                        _interactibleSoilScript.PlantedPickup = PickupType.None;
                        break;
                    case PickupType.Coin:
                        break;
                    case PickupType.Puzzle:
                        break;
                    case PickupType.Key:
                        break;
                }
            }
        }
        else // else if nothing is planted...
        {
            StartCoroutine(PlantSomething());
        }
    }


    private IEnumerator PlantSomething()
    {
        _pickedUpPickup = PlayerRefs.attachedObject;

        // 1) play reverse pickup animation
        PlayerRefs.GetComponent<Animator>().SetTrigger("DropFake");

        yield return new WaitForSeconds(0.2f);

        // 2) set active correct sprite in hierarchy according to what pickup the player has
        switch (PlayerRefs.PickedUpObject)
        {
            case PickupType.Corn:
                _plantedCornSpriteAnimator.SetActive(true);
                _plantedCornSpriteAnimator.GetComponent<Animator>().Play("SmallGrow_New");
                _interactibleSoilScript.PlantedPickup = PickupType.Corn;
                break;
            case PickupType.Coin:
                break;
            case PickupType.Puzzle:
                break;
            case PickupType.Key:
                break;
        }

        // disable any sparkles that the pickup had activated
        foreach (var sparkle in PlayerRefs.attachedObject.GetComponentInChildren<Pointer_Lord>().SparkleObjectsAll)
        {
            sparkle.SetActive(false);
        }
        // reset values
        PlayerRefs.attachedObject = null;
        PlayerRefs.PickedUpObject = PickupType.None;
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);
        // destroy the object
        Destroy(_pickedUpPickup.gameObject);

        InteractibleScript.InteractibleHasBeenUsedToActivateEventNone = true;
    }

}

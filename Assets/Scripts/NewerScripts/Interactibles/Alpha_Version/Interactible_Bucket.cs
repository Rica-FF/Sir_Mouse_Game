using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Bucket : Interactible_Base
{
    public List<PickupType> HeldItems = new List<PickupType>();
    public bool IsHoldingWater;

    // add extra prefabs here if they need to be spawned too from the bucket
    public GameObject CornPrefab, PuzzlePrefab, KeyPrefab, CoinPrefab;

    [SerializeField]
    private Animator _bucketAnimator, _waterAnimator;

    public GameObject SpriteWater;

    private Vector3 _originalScale;
    private Transform _handTransform;

    [SerializeField]
    private GameObject _pickupHasWaterSprite, _inBagHasWaterSprite;


    // call from : Fireplace/Soil/Flowers
    public void PourWater()
    {
        _originalScale = transform.parent.localScale;
        _handTransform = transform.parent.parent;
        StartCoroutine(PouringWater());

        // change the pointer type of pointer pickup bucket --- get pointerlord -- foreach base in bases --- if base.pickuptype == bucketWater --- pickuptype = bucket
        UpdatePickupType(false);
    }


    private IEnumerator PouringWater()
    {
        // detach the bucket from the player + reset transform
        transform.parent.SetParent(null);
        if (PlayerRefs.transform.localScale.x > 1) //left
        {
            transform.parent.localScale = new Vector3(1, 1, 1); // change this orientation depending on the orientation of the fireplace
        }
        else
        {
            transform.parent.localScale = new Vector3(-1, 1, 1); 
        }
        
        // play bucket animation
        _bucketAnimator.Play("Pour_New");

        yield return new WaitForSeconds(0.1f);

        // play water animation
        _waterAnimator.SetTrigger("Water");

        yield return new WaitForSeconds(0.3f);

        // disable sprite water in bucket after anim
        SpriteWater.SetActive(false);
        // set hasWater to false
        IsHoldingWater = false;

        yield return new WaitForSeconds(0.5f);

        // re-attach the bucket to the player + original transform
        transform.parent.SetParent(_handTransform);
        transform.parent.localScale = _originalScale;
    }


    public IEnumerator GetWater()
    {
        yield return new WaitForSeconds(1.5f);

        // set sprite water in bucket to active 
        SpriteWater.SetActive(true);
        // change the pointer type of pointer pickup bucket --- get pointerlord -- foreach base in bases --- if base.pickuptype == bucket --- pickuptype = bucketWater
        UpdatePickupType(true);
    }



    private void UpdatePickupType(bool currentlyHasWater)
    {
        Pointer_Base bucketPointer = null;      

        // figure out if i have water or not
        if (currentlyHasWater == true)
        {
            // set bool
            IsHoldingWater = true;
            // assign proper sprites in bubble popup (to-do)
            _pickupHasWaterSprite.SetActive(true);
            _inBagHasWaterSprite.SetActive(true);
            // set type
            foreach (var pointerBase in PointerLord.PointerBases)
            {
                if (pointerBase.TypeOfPickup == PickupType.Bucket && pointerBase.TypeOfPointer == PointerType.Pickup)
                {
                    pointerBase.TypeOfPickup = PickupType.BucketWater;
                    PlayerRefs.PickedUpObject = PickupType.BucketWater; // nullref possible here ? HAPPENS WHEN I USE THE BUKCET TO GET WATER, after the bucket was spawned in from the inventory
                    bucketPointer = pointerBase;
                }

                if (pointerBase.TypeOfPointer == PointerType.PutInBackpack)
                {
                    pointerBase.TypeOfPickup = PickupType.BucketWater;
                }
            }
        }
        else
        {
            // set bool
            IsHoldingWater = false;
            // assign proper sprites in bubble popup (to-do)
            _pickupHasWaterSprite.SetActive(false);
            _inBagHasWaterSprite.SetActive(false);
            // set type
            foreach (var pointerBase in PointerLord.PointerBases)
            {
                if (pointerBase.TypeOfPickup == PickupType.BucketWater && pointerBase.TypeOfPointer == PointerType.Pickup)
                {
                    pointerBase.TypeOfPickup = PickupType.Bucket;
                    PlayerRefs.PickedUpObject = PickupType.Bucket;
                    bucketPointer = pointerBase;
                }

                if (pointerBase.TypeOfPointer == PointerType.PutInBackpack)
                {
                    pointerBase.TypeOfPickup = PickupType.BucketWater;
                }
            }       
        }

        // update available pointer
        PointerLord.UpdateAvailablePointers();
        // update sparkles
        StartCoroutine(bucketPointer.GetSparkleRefs(false));  
    }
}

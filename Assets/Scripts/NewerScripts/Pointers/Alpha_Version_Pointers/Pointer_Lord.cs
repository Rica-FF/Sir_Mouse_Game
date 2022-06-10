using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Lord : MonoBehaviour
{
    // <summary>
    //            !!!!   MAKE SURE to have the extra behaviour pointers be BELOW THE FREE behaviours in the hierarchy   !!!!
    // </summary> 

    public List<GameObject> PointerTriggerObjects = new List<GameObject>();
    [HideInInspector]
    public Pointer_Base [] PointerBases;

    private GameObject _currentActivePointerTrigger;

    public int CurrentPointerIndex;
    private int _pointerIndexLimit;


    public List<GameObject> AvailableTriggerObjects = new List<GameObject>();
    private int _availablePointerIndexLimit;

    private Interactible_Base _interactibleScriptOnThisParent;

    // extra list of sparkles to more easily access them from the PickupDrop method in playerTouchControls
    public List<GameObject> SparkleObjectsAll = new List<GameObject>();


    void Start()
    {    
        _interactibleScriptOnThisParent = GetComponentInParent<Interactible_Base>();

        _pointerIndexLimit = 1;
        _availablePointerIndexLimit = 1;

        CurrentPointerIndex = 0;

        // steps 1 & 2
        UpdatePointerBases(); 

        // 3) if there are extra pickup behaviours, remove them at start
        foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupInteractions)
        {
            if (pickupType != PickupType.None) // i think this is needed
            {
                _availablePointerIndexLimit -= 1;
                AvailableTriggerObjects.RemoveAt(AvailableTriggerObjects.Count - 1);
            }
        }
    }

    // step 1 & 2 (check above)
    public void UpdatePointerBases()
    {
        PointerBases = GetComponentsInChildren<Pointer_Base>();

        // 1) get every pointer that has an extra pickup interaction      
        foreach (var pointer in PointerBases)
        {
            // add it to the list if it's != none 
            if (pointer.RequiredItemExtraInteraction != PickupType.None)
            {
                _interactibleScriptOnThisParent.ExtraPickupInteractions.Add(pointer.RequiredItemExtraInteraction);
            }
        }

        // 2) add all possible triggers to a list
        foreach (var pointer in PointerBases)
        {
            var pointerTrigger = pointer.transform.GetChild(0).gameObject;
            PointerTriggerObjects.Add(pointerTrigger);
            _pointerIndexLimit += 1;

            // separate list for when the arrow should be shown
            AvailableTriggerObjects.Add(pointerTrigger);
            _availablePointerIndexLimit += 1;
        }
    }




    public void SwapActivePointer()
    {
        if (_currentActivePointerTrigger != null)
        {
            _currentActivePointerTrigger.SetActive(false);
        }

        CurrentPointerIndex += 1;

        if (CurrentPointerIndex >= _availablePointerIndexLimit)
        {
            CurrentPointerIndex = 0;
        }

        if (CurrentPointerIndex != 0)
        {
            AvailableTriggerObjects[CurrentPointerIndex - 1].SetActive(false);
        }
        
        AvailableTriggerObjects[CurrentPointerIndex].SetActive(true);            
        _currentActivePointerTrigger = AvailableTriggerObjects[CurrentPointerIndex];
    }





    public void UpdateAvailablePointers()
    {
        // 1) reset to the original available triggers calculated in the start
        AvailableTriggerObjects.Clear();
        _availablePointerIndexLimit = 0;


        // 2) add every pointer initially
        foreach (var pointer in PointerBases)
        {
            var pointerTrigger = pointer.transform.GetChild(0).gameObject;

            AvailableTriggerObjects.Add(pointerTrigger);
            _availablePointerIndexLimit += 1;
        }


        // 3) remove pointers that require a pickup
        foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupInteractions)  
        {
            if (pickupType != PickupType.None)  // i think this is needed
            {
                _availablePointerIndexLimit -= 1;
                AvailableTriggerObjects.RemoveAt(AvailableTriggerObjects.Count - 1);  // these pointers are positioned at lowers levels in hierarchy
            }       
        }

        // 3.5) extra requirement here needed to check for
        if (_interactibleScriptOnThisParent.InteractibleHasBeenUsedToActivateEventNone == false)
        {
            foreach (var pointerBase in PointerBases)
            {
                if (pointerBase.RequiresUseBeforeEventNone == true)
                {
                    var pointerTriggerOfPickup = pointerBase.transform.GetChild(0).gameObject;
                    AvailableTriggerObjects.Remove(pointerTriggerOfPickup);
                }
            }
        }


        // 4) if the player has a pickup in their hands, dont show specific pointers
        if (_interactibleScriptOnThisParent.PlayerRefs.PickedUpObject != PickupType.None) 
        {           
            foreach (var pointer in PointerBases)
            {
                // remove said pointer trigger from available triggers
                // -- if i am a pickup pointer --OR-- pickupInfinite --OR-- event which is not part of requiredItem behaviour (! but allow item type none !) --OR-- a hit pointer --OR-- type putinbag
                if (pointer.TypeOfPointer == PointerType.Pickup 
                    || pointer.TypeOfPointer == PointerType.PickupInfinite
                    || _interactibleScriptOnThisParent.InteractibleType != InteractibleType.RequiresItem && pointer.TypeOfPointer == PointerType.Event 
                    || pointer.TypeOfPointer == PointerType.Hit
                    || pointer.TypeOfPointer == PointerType.PutInBackpack)
                {
                    var pointerTriggerOfPickup = pointer.transform.GetChild(0).gameObject;

                    _availablePointerIndexLimit -= 1;
                    AvailableTriggerObjects.Remove(pointerTriggerOfPickup);
                }
            }
        }


        // 5) check if the players picked up item == one of the extra behaviour items
        foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupInteractions)  
        {
            // if true...
            if (pickupType == _interactibleScriptOnThisParent.PlayerRefs.PickedUpObject) // update picked up object
            {
                // can have multiple extra behaviours with same requirement...
                foreach (var pointer in PointerBases)
                {
                    if (pointer.RequiredItemExtraInteraction == pickupType) // wrong requireditemextrainteraction
                    {
                        // add the specific trigger, of which the parent PointerBase, has said extra required item
                        _availablePointerIndexLimit += 1;

                        var pointerTrigger = pointer.transform.GetChild(0).gameObject;
                        AvailableTriggerObjects.Add(pointerTrigger);
                    }
                }
            }
        }
        
        // 6) checking for soil plantations
        if (_interactibleScriptOnThisParent.TryGetComponent(out Interactible_Soil soilScript))
        {
            if (soilScript.PlantedPickup != PickupType.None) // if somethings been planted -> remove all event pointers that plant pickups  ---- WRONG
            {
                foreach (var pointer in PointerBases)
                {
                    // checks for all the pickups that cant be planted (coversly i can check for ones that can oly be planted)
                    if (pointer.RequiredItemExtraInteraction != PickupType.None && pointer.RequiredItemExtraInteraction != PickupType.BucketWater)
                    {
                        // remove the trigger
                        _availablePointerIndexLimit -= 1;

                        var pointerTrigger = pointer.transform.GetChild(0).gameObject;
                        AvailableTriggerObjects.Remove(pointerTrigger);
                    }
                }
            }
            else if (soilScript.PlantedPickup == PickupType.None && _interactibleScriptOnThisParent.PlayerRefs.PickedUpObject == PickupType.BucketWater) // is nothings been planted, and i have a bucket, remove every trigger
            {
                foreach (var pointer in PointerBases)
                {
                    // remove the trigger
                    _availablePointerIndexLimit -= 1;
                    
                    var pointerTrigger = pointer.transform.GetChild(0).gameObject;
                    AvailableTriggerObjects.Remove(pointerTrigger);

                }
            }
        }

        // 7) fixing the emptying bucket pointer
        if (_interactibleScriptOnThisParent.TryGetComponent(out Interactible_Bucket bucketScript))
        {
            if (bucketScript.HeldItems.Count == 0)
            {
                foreach (var pointer in PointerBases)
                {
                    if (pointer.TypeOfPointer == PointerType.Event && pointer.RequiredItemExtraInteraction == PickupType.None) // specifics for the 1 pointer that empties a bucket
                    {
                        _availablePointerIndexLimit -= 1;

                        var pointerTrigger = pointer.transform.GetChild(0).gameObject;
                        AvailableTriggerObjects.Remove(pointerTrigger);
                    }
                }
            }
        }


        //foreach (var obje in AvailableTriggerObjects)
        //{
        //    Debug.Log(" eey " + obje.transform.parent + " POINTERS AFTER");
        //}

    }

}

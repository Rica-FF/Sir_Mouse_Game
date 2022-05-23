using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Lord : MonoBehaviour
{
    /// <summary>
    ///            !!!!   MAKE SURE to have the extra behaviour pointers be BELOW THE FREE behaviours in the hierarchy   !!!!
    /// </summary> 

    public List<GameObject> PointerTriggerObjects = new List<GameObject>();
    private Pointer_Base [] _pointerBases;

    private GameObject _currentActivePointerTrigger;

    public int CurrentPointerIndex;
    private int _pointerIndexLimit;


    public List<GameObject> AvailableTriggerObjects = new List<GameObject>();
    private int _availablePointerIndexLimit;

    private Interactible_Base _interactibleScriptOnThisParent;


    void Start()
    {
        _pointerBases = GetComponentsInChildren<Pointer_Base>();
        _interactibleScriptOnThisParent = GetComponentInParent<Interactible_Base>();

        _pointerIndexLimit = 1;
        _availablePointerIndexLimit = 1;

        CurrentPointerIndex = 0;

        // get every pointer that has an extra pickup interaction      
        foreach (var pointer in _pointerBases)
        {
            // add it to the list if it's != none , or if this type has alrdy been added
            if (pointer.RequiredItemExtraInteraction != PickupType.None && _interactibleScriptOnThisParent.ExtraPickupActions.Contains(pointer.RequiredItemExtraInteraction) == false)
            {
                _interactibleScriptOnThisParent.ExtraPickupActions.Add(pointer.RequiredItemExtraInteraction);
            }
        }

        // add all possible triggers to a list
        foreach (var pointer in _pointerBases)
        {
            var pointerTrigger = pointer.transform.GetChild(0).gameObject;
            PointerTriggerObjects.Add(pointerTrigger);
            _pointerIndexLimit += 1;


            // separate list for when the arrow should be shown
            AvailableTriggerObjects.Add(pointerTrigger);
            _availablePointerIndexLimit += 1;
        }


        //// if there are extra pickup behaviours...
        //foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupInteractions)
        //{
        //    // remove the TriggerObjects that need requireded items
        //    if (pickupType != PickupType.None)
        //    {
        //        _availablePointerIndexLimit -= 1;
        //        AvailableTriggerObjects.RemoveAt(AvailableTriggerObjects.Count - 1);           
        //    }            
        //}
        // if there are extra pickup behaviours...
        foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupActions)
        {

           _availablePointerIndexLimit -= 1;
           AvailableTriggerObjects.RemoveAt(AvailableTriggerObjects.Count - 1);

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
            //PointerTriggerObjects[CurrentPointerIndex - 1].SetActive(false);
            AvailableTriggerObjects[CurrentPointerIndex - 1].SetActive(false);
        }
        
        //PointerTriggerObjects[CurrentPointerIndex].SetActive(true);
        AvailableTriggerObjects[CurrentPointerIndex].SetActive(true);            
        //_currentActivePointerTrigger = PointerTriggerObjects[CurrentPointerIndex];
        _currentActivePointerTrigger = AvailableTriggerObjects[CurrentPointerIndex];
    }





    public void UpdateAvailablePointers()
    {
        // reset to the original available triggers calculated in the start
        AvailableTriggerObjects.Clear();
        _availablePointerIndexLimit = 0;


        foreach (var pointer in _pointerBases)
        {
            var pointerTrigger = pointer.transform.GetChild(0).gameObject;

            AvailableTriggerObjects.Add(pointerTrigger);
            _availablePointerIndexLimit += 1;
        }
        //foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupInteractions)
        //{
        //    if (pickupType != PickupType.None)
        //    {
        //        _availablePointerIndexLimit -= 1;
        //        AvailableTriggerObjects.RemoveAt(AvailableTriggerObjects.Count - 1);
        //    }
        //}
        foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupActions)
        {
           _availablePointerIndexLimit -= 1;
           AvailableTriggerObjects.RemoveAt(AvailableTriggerObjects.Count - 1);
        }


        // check if the players picked up item == one of the extra behaviour items
        foreach (var pickupType in _interactibleScriptOnThisParent.ExtraPickupActions)
        {
            // if true...
            if (pickupType == _interactibleScriptOnThisParent.PlayerRefs.PickedUpObject)
            {
                // can have multiple extra behaviours with same requirement...
                foreach (var pointer in _pointerBases)
                {
                    if (pointer.RequiredItemExtraInteraction == pickupType)
                    {
                        // add the specific trigger, of which the parent PointerBase, has said extra required item
                        _availablePointerIndexLimit += 1;

                        var pointerTrigger = pointer.transform.GetChild(0).gameObject;
                        AvailableTriggerObjects.Add(pointerTrigger);
                    }
                }
            }
        }
    }

}

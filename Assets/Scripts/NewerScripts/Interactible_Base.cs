using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Base : MonoBehaviour
{
    [SerializeField]
    private InteractibleType _interactibleType;
    [SerializeField]
    private PickupType _requiredItemType;
    

    public PickupType[] ExtraPickupInteractions;
    public List<PickupType> ExtraPickupActions = new List<PickupType>();


    public bool OneTimeUse;
    [HideInInspector]
    public bool UsedSuccesfully;

    [SerializeField]
    private AudioClip[] _audioClips;

    private Animator _animator;
    private AudioSource _audioSource;

    [HideInInspector]
    public PlayerReferences PlayerRefs;

    private int _pointerIndex;
    private int _pointersInListCountOnContact;
    private int _pointerLordIndex;
    private int _pointerLordsInListCountOnContact;

    [SerializeField]
    private Pointer_Lord _pointerLord;

    public GameObject Pointer;
    [SerializeField]
    private GameObject _pointerTriggerObject;

    public GameObject PointerNextTriggerObject;


    private void Awake()
    {
        if (TryGetComponent(out Animator animator))
        {
            _animator = animator;
        }

        if (TryGetComponent(out AudioSource audioSource))
        {
            _audioSource = audioSource;
        }
    }






    private void OnTriggerEnter(Collider collider)
    {
        // if player && object is not picked up
        //if (collider.gameObject.layer == 8 && this.transform.parent == null)

        if (collider.gameObject.layer == 8)
        {
            if (OneTimeUse == false || OneTimeUse == true && UsedSuccesfully == false)
            {
                // get player refs
                PlayerRefs = collider.GetComponentInChildren<PlayerReferences>();


                // check what type behaviour
                if (_interactibleType == InteractibleType.Generic)
                {
                    GenericBehaviour();
                }
                else if (_interactibleType == InteractibleType.ShowsPointer)
                {
                    ShowPointerBehaviour();
                    //Debug.Log(" ENTERING ");
                }
                else if (_interactibleType == InteractibleType.RequiresItem)
                {
                    RequiresItemBehaviour();
                }
            }

        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == 8)
        {
            if (_interactibleType == InteractibleType.ShowsPointer)
            {
                HidePointerBehaviour();
                //Debug.Log(" Exiting ");
            }
        }

    }






    // generic interactible. Just something that moves/plays a sound when you pass it as the player
    private void GenericBehaviour()
    {
        if (_audioSource != null)
        {
            var randomSound = UnityEngine.Random.Range(0, _audioClips.Length -1);
            _audioSource.PlayOneShot(_audioClips[randomSound]);
        }
        if (_animator != null)
        {
            //_animator.SetTrigger("Activate");  // create this trigger
            _animator.Play("BushWiggle");
        }

        ExtraBehaviour();
    }
    // method to be overwritten for more special functionality
    public virtual void ExtraBehaviour()
    {

    }




    // interactible that will show a pointer when approached
    public void ShowPointerBehaviour()
    {
        // method that will check what pointers are all viable actions the player can do
        _pointerLord.UpdateAvailablePointers();


        // if pointer_lord count is >= 2...
        if (_pointerLord.AvailableTriggerObjects.Count >= 2)
        {
            // show the arrow trigger
            PointerNextTriggerObject.SetActive(true);
        }




        // show the indexed trigger of the pointer lord
        //_pointerLord.PointerTriggerObjects[_pointerLord.CurrentPointerIndex].SetActive(true);
        _pointerLord.AvailableTriggerObjects[_pointerLord.CurrentPointerIndex].SetActive(true);

        // add pointerLord to list
        PlayerRefs.PointerLords.Add(_pointerLord.gameObject);
        // assign the index of the pointerLord
        _pointerLordIndex = PlayerRefs.PointerLords.Count - 1;
        // update current Pointer on player 
        //PlayerRefs._currentActivePointer = _pointerLord.PointerTriggerObjects[_pointerLord.CurrentPointerIndex].GetComponentInParent<Pointer_Base>();
        PlayerRefs._currentActivePointer = _pointerLord.AvailableTriggerObjects[_pointerLord.CurrentPointerIndex].GetComponentInParent<Pointer_Base>();


        // register the amount of things in the list !!!
        _pointerLordsInListCountOnContact = 0;       // UPDATE THIS, POINTER LORD LIST
        _pointerLordsInListCountOnContact = PlayerRefs.PointerLords.Count;


        /////////

        //// show child of pointer_parent instead
        //_pointerTriggerObject.SetActive(true); // UPDATE THIS

        //// add pointer to list
        //_playerRefs.pointers.Add(Pointer); // UPDATE THIS, POINTER LORD LIST
        //// assign the index of the pointer
        //_pointerIndex = _playerRefs.pointers.Count - 1; // UPDATE THIS, POINTER LORD LIST
        //// update current Pointer on player 
        //_playerRefs._currentActivePointer = Pointer.GetComponent<Pointer_Base>(); // UPDATE THIS, POINTER LORD LIST


        //// register the amount of things in the list !!!
        //_pointersInListCountOnContact = 0;       // UPDATE THIS, POINTER LORD LIST
        //_pointersInListCountOnContact = _playerRefs.pointers.Count; // UPDATE THIS, POINTER LORD LIST

    }
    public void HidePointerBehaviour()
    {
        // hide the indexed pointers' child
        //_pointerLord.PointerTriggerObjects[_pointerLord.CurrentPointerIndex].SetActive(false);
        _pointerLord.AvailableTriggerObjects[_pointerLord.CurrentPointerIndex].SetActive(false);

        // update current Pointer on player 
        //PlayerRefs._currentActivePointer = _pointerLord.PointerTriggerObjects[_pointerLord.CurrentPointerIndex].GetComponentInParent<Pointer_Base>();
        PlayerRefs._currentActivePointer = _pointerLord.AvailableTriggerObjects[_pointerLord.CurrentPointerIndex].GetComponentInParent<Pointer_Base>();




        // check the amount of things in the list register !!!
        // if another object has been removed from the list before this one....
        int pointersDifOnExit = 0;
        if (_pointerLordsInListCountOnContact > PlayerRefs.PointerLords.Count)
        {
            pointersDifOnExit = _pointerLordsInListCountOnContact - PlayerRefs.PointerLords.Count;
            _pointerLordIndex -= pointersDifOnExit;
        }

        //index needs to be updated when a second object is added, and then the first one removed before this second one
        //for (int i = 0; i < _playerRefs.PointerLords.Count; i++)
        //{
        //    if (_playerRefs.PointerLords[i].gameObject != null)
        //    {
        //Debug.Log(" ------- REMOVED -- " + _pointerLord.gameObject + " -- AT INDEX -- "  + _pointerLordIndex);
        //_playerRefs.PointerLords.RemoveAt(_pointerLordIndex);
        //    }
        //}

        // newer line of code for removing objects
        PlayerRefs.PointerLords.RemoveAll(t => t == _pointerLord.gameObject);


        // disable the arrow popup
        PointerNextTriggerObject.SetActive(false);


        ///////////

        //// show child of pointer_parent instead
        //_pointerTriggerObject.SetActive(false);

        //// update current Pointer on player 
        //_playerRefs._currentActivePointer = Pointer.GetComponent<Pointer_Base>();
        ////_playerRefs._currentActivePointer = null;

        //// check the amount of things in the list register !!!
        //int pointersDifOnExit = 0;      
        //if (_pointersInListCountOnContact > _playerRefs.pointers.Count)
        //{
        //    pointersDifOnExit = _pointersInListCountOnContact - _playerRefs.pointers.Count;
        //    _pointerIndex -= pointersDifOnExit;
        //}

        //// index needs to be updated when a second object is added, and then the first one removed before this second one
        //for (int i = 0; i < _playerRefs.pointers.Count; i++)
        //{
        //    if (_playerRefs.pointers[i].gameObject != null)
        //    {
        //        _playerRefs.pointers.RemoveAt(_pointerIndex);
        //    }
        //}

        //_pointerNextTriggerObject.SetActive(false);
    }





    // interactible that requires an item to be interacted with
    private void RequiresItemBehaviour()
    {
        // if player has required item
        if (PlayerRefs.PickedUpObject == _requiredItemType)
        {
            ShowPointerBehaviour();
        }     
    }

}

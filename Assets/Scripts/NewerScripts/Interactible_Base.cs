using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Base : MonoBehaviour
{
    [SerializeField]
    private InteractibleType _interactibleType;
    [SerializeField]
    private AudioClip[] _audioClips;

    private Animator _animator;
    private AudioSource _audioSource;
    private PlayerReferences _playerRefs;

    private int _pointerIndex;
    private int _pointersInListCountOnContact;

    public GameObject Pointer;


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
    //private void Start()
    //{
    //    Pointer = GetComponentInChildren<Pointer_Base>().gameObject;
    //}





    private void OnTriggerEnter(Collider collider)
    {
        // if player && object is not picked up
        if (collider.gameObject.layer == 8 && this.transform.parent == null)
        {
            // get player refs
            _playerRefs = collider.GetComponentInChildren<PlayerReferences>();
           

            // check what type behaviour
            if (_interactibleType == InteractibleType.Generic)
            {
                GenericBehaviour();
            }
            else if (_interactibleType == InteractibleType.ShowsPointer)
            {
                ShowPointerBehaviour();
            }
            else if (_interactibleType == InteractibleType.RequiresItem)
            {
                RequiresItemBehaviour();
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
    private void ShowPointerBehaviour()
    {
        Pointer.SetActive(true);

        // add pointer to list
        _playerRefs.pointers.Add(Pointer);
        // assign the index of the pointer
        _pointerIndex = _playerRefs.pointers.Count - 1;
        // update current Pointer on player 
        _playerRefs._currentActivePointer = Pointer.GetComponent<Pointer_Base>(); 

        // register the amount of things in the list !!!
        _pointersInListCountOnContact = 0;
        _pointersInListCountOnContact = _playerRefs.pointers.Count;

    }
    public void HidePointerBehaviour()
    {
        Pointer.SetActive(false);
        // update current Pointer on player 
        _playerRefs._currentActivePointer = Pointer.GetComponent<Pointer_Base>(); 

        // check the amount of things in the list register !!!
        int pointersDifOnExit = 0;      
        if (_pointersInListCountOnContact > _playerRefs.pointers.Count)
        {
            pointersDifOnExit = _pointersInListCountOnContact - _playerRefs.pointers.Count;
            _pointerIndex -= pointersDifOnExit;
        }

        // index needs to be updated when a second object is added, and then the first one removed before this second one
        for (int i = 0; i < _playerRefs.pointers.Count; i++)
        {
            if (_playerRefs.pointers[i].gameObject != null)
            {
                _playerRefs.pointers.RemoveAt(_pointerIndex);
            }
        }            
    }





    // interactible that requires an item to be interacted with
    private void RequiresItemBehaviour()
    {

    }

}

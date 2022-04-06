using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Base : MonoBehaviour
{
    //[HideInInspector]
    //static public bool DisableIrrelevantPointers = false;

    [SerializeField]
    private InteractibleType _interactibleType;
    [SerializeField]
    private AudioClip[] _audioClips;

    private Animator _animator;
    private AudioSource _audioSource;
    private PlayerReferences _playerRefs;

    private int _pointerIndex;

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
    //    // check for the pointerScript
    //    Pointer = GetComponentInChildren<DoAction>().gameObject;       
    //}




    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 8)
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
            //_animator.Play("BushWiggle");
        }
    }

    // interactible that will show a pointer when approached
    private void ShowPointerBehaviour()
    {
        Pointer.SetActive(true);

        // add pointer to list
        _playerRefs.pointers.Add(gameObject);
        // assign the index of the pointer
        _pointerIndex = _playerRefs.pointers.Count - 1;
    }
    private void HidePointerBehaviour()
    {
        _playerRefs.pointers[_pointerIndex] = null;
        _playerRefs.pointers.RemoveAt(_pointerIndex);

        Pointer.SetActive(false);
    }

    // interactible that requires an item to be interacted with
    private void RequiresItemBehaviour()
    {

    }

}

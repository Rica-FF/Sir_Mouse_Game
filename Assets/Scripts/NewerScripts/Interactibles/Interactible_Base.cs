using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Base : MonoBehaviour
{
    public InteractibleType InteractibleType;   
    public PickupType _requiredItemType;

    [HideInInspector]
    public List<PickupType> ExtraPickupInteractions = new List<PickupType>();
    [HideInInspector]
    public bool InteractibleHasBeenUsedToActivateEventNone;

    // one time use AND cooldown options (mostly for generic or event interactions) //
    public bool OneTimeUse;
    public bool HasACooldown;
    public bool PointerStaysActiveAfterUse;

    [SerializeField]
    private float _cooldownLength;
    [HideInInspector]
    public bool OnCooldown;

    [HideInInspector]
    public bool UsedSuccesfully;

    [SerializeField]
    private AudioClip[] _audioClipsGeneric;

    private Animator _animator;
    [SerializeField]
    private string _animationName;

    public AudioSource AudioSource;

    [HideInInspector]
    public PlayerReferences PlayerRefs;

    public Pointer_Lord PointerLord;

    public GameObject PointerNextTriggerObject;
    public GameObject _infinitelyRespawnablePickupPrefab;


    private void Awake()
    {
        if (TryGetComponent(out Animator animator))
        {
            _animator = animator;
        }

        if (TryGetComponent(out AudioSource audioSource))
        {
            AudioSource = audioSource;
        }

        if (GetComponentInChildren<Pointer_Lord>() != null)
        {
           PointerLord = GetComponentInChildren<Pointer_Lord>();
        }      
    }






    private void OnTriggerEnter(Collider collider)
    {
        // checks player layer
        if (collider.gameObject.layer == 8)
        {
            // check one time use count
            if (OneTimeUse == false || OneTimeUse == true && UsedSuccesfully == false) 
            {
                // get player refs
                PlayerRefs = collider.GetComponentInChildren<PlayerReferences>();

                // check for cooldown
                if (OnCooldown == false)
                {
                    // check what type behaviour
                    switch (InteractibleType)
                    {
                        case InteractibleType.Generic:
                            GenericBehaviour();
                            break;
                        case InteractibleType.ShowsPointer:
                            ShowPointerBehaviour();
                            break;
                        case InteractibleType.RequiresItem:
                            RequiresItemBehaviour();
                            break;
                        case InteractibleType.InfinitePickups:
                            ShowPointerBehaviourInfinitePickup();
                            break;
                    }

                    // if cooldown is present
                    if (HasACooldown == true)
                    {
                        StartCoroutine(ActivateCooldown());
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == 8)
        {
            switch (InteractibleType)
            {
                case InteractibleType.ShowsPointer:
                    HidePointerBehaviour();
                    break;
                case InteractibleType.RequiresItem:
                    HidePointerBehaviour();
                    break;
                case InteractibleType.InfinitePickups:
                    HidePointerBehaviour();
                    break;
            }
        }
    }






    // generic interactible. Just something that moves/plays a sound when you pass it as the player
    private void GenericBehaviour()
    {
        if (AudioSource != null)
        {
            var randomSound = UnityEngine.Random.Range(0, _audioClipsGeneric.Length -1);
            AudioSource.PlayOneShot(_audioClipsGeneric[randomSound]);
        }
        if (_animator != null)
        {
            //_animator.SetTrigger("Activate");  // !!! create this trigger !!!
            //_animator.Play("BushWiggle");      // outdated

            _animator.enabled = true;
            _animator.Play(_animationName);

            StartCoroutine(DeactivateAnimator());
        }

        // override-able method
        ExtraBehaviour();
    }




    // interactible that will show a pointer when approached
    public void ShowPointerBehaviour()
    {
        PointerLord.GetComponentInChildren<Animator>().Play("Pointer_Idle2");

        // method that will check what pointers are all viable actions the player can do
        PointerLord.UpdateAvailablePointers();
        // if pointer_lord count is >= 2...
        if (PointerLord.AvailableTriggerObjects.Count >= 2)
        {
            // show the arrow 
            PointerNextTriggerObject.SetActive(true);
        }

        // reset index to 0 on enter
        PointerLord.CurrentPointerIndex = 0;

        if (PointerLord.AvailableTriggerObjects.Count > 0)
        {
            // show the indexed trigger of the pointer lord
            PointerLord.AvailableTriggerObjects[PointerLord.CurrentPointerIndex].SetActive(true);
            // add pointerLord to list
            PlayerRefs.PointerLords.Add(PointerLord.gameObject);
            // update current Pointer on player 
            PlayerRefs._currentActivePointer = PointerLord.AvailableTriggerObjects[PointerLord.CurrentPointerIndex].GetComponentInParent<Pointer_Base>();
        }
    }
    public void ShowPointerBehaviourInfinitePickup()
    {
        PointerLord.GetComponentInChildren<Animator>().Play("Pointer_Idle2");

        // method that will check what pointers are all viable actions the player can do
        PointerLord.UpdateAvailablePointers();
        // if pointer_lord count is >= 2...
        if (PointerLord.AvailableTriggerObjects.Count >= 2)
        {
            // show the arrow 
            PointerNextTriggerObject.SetActive(true);
        }

        // reset index to 0 on enter
        PointerLord.CurrentPointerIndex = 0;

        if (PointerLord.AvailableTriggerObjects.Count > 0)
        {
            // show the indexed trigger of the pointer lord
            PointerLord.AvailableTriggerObjects[PointerLord.CurrentPointerIndex].SetActive(true);
            // add pointerLord to list
            PlayerRefs.PointerLords.Add(PointerLord.gameObject);
            // update current Pointer on player 
            PlayerRefs._currentActivePointer = PointerLord.AvailableTriggerObjects[PointerLord.CurrentPointerIndex].GetComponentInParent<Pointer_Base>();
        }
    }



    public void HidePointerBehaviour()
    {
        if (PointerLord.AvailableTriggerObjects.Count > 0)
        {
            // hide the index'd TriggerObject
            PointerLord.AvailableTriggerObjects[PointerLord.CurrentPointerIndex].SetActive(false);
            // update current Pointer on player 
            PlayerRefs._currentActivePointer = PointerLord.AvailableTriggerObjects[PointerLord.CurrentPointerIndex].GetComponentInParent<Pointer_Base>();
        }

        // remove the pointer lord from the list
        PlayerRefs.PointerLords.RemoveAll(t => t == PointerLord.gameObject);

        // disable the arrow popup
        PointerNextTriggerObject.SetActive(false);
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

    // method to be overwritten for more special functionality
    public virtual void ExtraBehaviour()
    {

    }



    public IEnumerator ActivateCooldown()
    {
        OnCooldown = true;

        yield return new WaitForSeconds(_cooldownLength);

        OnCooldown = false;
    }
    public IEnumerator DeactivateAnimator()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length + 0.2f);

        _animator.enabled = false;
    }


}

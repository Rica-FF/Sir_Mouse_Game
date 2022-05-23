using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Base : MonoBehaviour
{
    //public bool HasMultiplePointers;

    public PointerType TypeOfPointer;
    public PickupType TypeOfPickup;

    public PickupType RequiredItemExtraInteraction;

    public bool RequiresDestination;
    public Transform DestinationSpot;
    public bool OrientationLookingRight;

    [SerializeField]
    private AudioClip[] _soundEffects;

    [HideInInspector]
    public PlayerTouchControls PlayerControls;
    [HideInInspector]
    public PlayerReferences PlayerRefs;
    private AudioSource _playerAudioSource;

    [HideInInspector]
    public GameObject Interactible;
    private GameObject _interactibleParent;
    [HideInInspector]
    public Interactible_Base InteractibleScript;


    private void Awake()
    {
        PlayerControls = FindObjectOfType<PlayerTouchControls>();
        StartCoroutine(GetPlayerRefs());

        Interactible = GetComponentInParent<Interactible_Base>().gameObject;
        InteractibleScript = Interactible.GetComponent<Interactible_Base>();

        _interactibleParent = Interactible.GetComponentInParent<Rigidbody>().gameObject;
    }


    public void ActivateInteractibleAction()
    {
        if (TypeOfPointer == PointerType.Hit) // hit sword
        {
            HitSword();
        }
        else if (TypeOfPointer == PointerType.Pickup) // Pickup item
        {
            PickupItemWrap(TypeOfPickup);
        }
        else // play longer event
        {
            PlayEvent();
        }
    }
    public void SwapPointerAction()
    {

    }


    private void HitSword()
    {
        
    }
    public virtual void PlayEvent()
    {
        
    }
    public virtual void PickupItemWrap(PickupType pickup)
    {
        // generic pickup animation
        PlayerRefs.GetComponent<Animator>().SetTrigger("PickUpCoin");
        // lock the arm
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 1f);

        //PlayerControls.attachedObject = "Coin";
        //PlayerRefs.attachedObject = Interactible;
        PlayerRefs.attachedObject = _interactibleParent; 

        PlayerRefs.PickedUpObject = pickup;

        //_playerAudioSource.PlayOneShot(_soundEffects[randomsoundSetOnThisObject]);
        //_playerReferences.transform.localScale = new Vector3(6, 6, 6);            // why this ?

        //PlayerRefs.attachedObject.GetComponent<SphereCollider>().enabled = false;
        PlayerRefs.attachedObject.GetComponentInChildren<SphereCollider>().enabled = false; //


    }


    IEnumerator GetPlayerRefs()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerRefs = PlayerControls.GetComponentInChildren<PlayerReferences>();
        _playerAudioSource = PlayerRefs.GetComponent<AudioSource>();
    }





  









    public void PickupParentingLogic()
    {
        _interactibleParent.transform.SetParent(PlayerRefs.playerHand.transform);
        _interactibleParent.transform.localPosition = new Vector3(-0.031f, 0.005f, 0.01f);
        _interactibleParent.transform.localRotation = Quaternion.Euler(89, 310, -34.5f);

        Interactible.GetComponent<SphereCollider>().enabled = false;

        //////////
        
        //Interactible.transform.SetParent(PlayerRefs.playerHand.transform);
        //Interactible.transform.localPosition = new Vector3(-0.031f, 0.005f, 0.01f);
        //Interactible.transform.localRotation = Quaternion.Euler(89, 310, -34.5f);

        //Interactible.GetComponent<SphereCollider>().enabled = false;
    }
    public void DropPickupParentingLogic()
    {
        _interactibleParent.transform.SetParent(null);

        _interactibleParent.transform.position = new Vector3(PlayerControls.transform.position.x, 0, PlayerControls.transform.position.z);
        _interactibleParent.transform.localRotation = Quaternion.Euler(0, 0, 90);
        _interactibleParent.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Interactible.GetComponent<SphereCollider>().enabled = true;

        ///////////

        //Interactible.transform.SetParent(null);

        //Interactible.transform.position = new Vector3(PlayerControls.transform.position.x, 0, PlayerControls.transform.position.z);
        //Interactible.transform.localRotation = Quaternion.Euler(30, 0, 90);
        //Interactible.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        //Interactible.GetComponent<SphereCollider>().enabled = true;
    }
}

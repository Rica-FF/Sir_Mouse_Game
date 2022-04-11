using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Base : MonoBehaviour
{
    public PointerType TypeOfPointer;
    public PickupType TypeOfPickup;

    public bool RequiresDestination;
    public Transform DestinationSpot;

    [SerializeField]
    private AudioClip[] _soundEffects;

    [HideInInspector]
    public PlayerTouchControls PlayerControls;
    [HideInInspector]
    public PlayerReferences PlayerRefs;
    private AudioSource _playerAudioSource;

    [HideInInspector]
    public GameObject Interactible;
    private Interactible_Base _interactibleScript;


    private void Awake()
    {
        PlayerControls = FindObjectOfType<PlayerTouchControls>();
        PlayerRefs = PlayerControls.GetComponentInChildren<PlayerReferences>();
        _playerAudioSource = PlayerRefs.GetComponent<AudioSource>();

        Interactible = GetComponentInParent<Interactible_Base>().gameObject;
        _interactibleScript = Interactible.GetComponent<Interactible_Base>();
    }


    public void ActivateInteractibleAction()
    {
        if (TypeOfPointer == PointerType.Hit) // hit sword
        {
            HitSword();
        }
        else if (TypeOfPointer == PointerType.Pickup) // Pickup item
        {
            PickupItemWrap();
        }
        else // play longer event
        {
            PlayEvent();
        }
    }


    private void HitSword()
    {
        throw new NotImplementedException();
    }
    public virtual void PlayEvent()
    {
        throw new NotImplementedException();
    }
    private void PickupItemWrap()
    {
        PlayerControls.attachedObject = "Coin";
        PlayerRefs.attachedObject = Interactible;
        //_playerAudioSource.PlayOneShot(_soundEffects[randomsoundSetOnThisObject]);
        //_playerReferences.transform.localScale = new Vector3(6, 6, 6);            // why this ?

        PlayerRefs.GetComponent<Animator>().SetTrigger("PickUpCoin");
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 1f);
    }





  









    public void PickupParentingLogic()
    {
        Interactible.transform.SetParent(PlayerRefs.playerHand.transform);
        Interactible.transform.localPosition = new Vector3(-0.031f, 0.005f, 0.01f);
        Interactible.transform.localRotation = Quaternion.Euler(89, 310, -34.5f);

        Interactible.GetComponent<SphereCollider>().enabled = false;
    }
    public void DropPickupParentingLogic()
    {
        Interactible.transform.SetParent(null);

        Interactible.transform.position = new Vector3(PlayerControls.transform.position.x, 0, PlayerControls.transform.position.z);
        Interactible.transform.localRotation = Quaternion.Euler(30, 0, 90);
        Interactible.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Interactible.GetComponent<SphereCollider>().enabled = true;
    }
}

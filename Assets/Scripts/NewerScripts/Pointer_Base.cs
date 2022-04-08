using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Base : MonoBehaviour
{
    public PointerType PointerType;
    public PickupType PickupType;

    public bool RequiresDestination;
    public Transform DestinationSpot;

    [SerializeField]
    private AudioClip[] _soundEffects;


    private PlayerTouchControls _playerControls;
    private PlayerReferences _playerReferences;
    private AudioSource _playerAudioSource;

    private GameObject _interactible;


    private void Awake()
    {
        _playerControls = FindObjectOfType<PlayerTouchControls>();
        _playerReferences = _playerControls.GetComponentInChildren<PlayerReferences>();
        _playerAudioSource = _playerReferences.GetComponent<AudioSource>();

        _interactible = GetComponentInParent<Interactible_Base>().gameObject;
    }


    public void ActivateInteractibleAction()
    {
        if (PointerType == PointerType.Hit) // hit sword
        {
            HitSword();
        }
        else if (PointerType == PointerType.Pickup) // Pickup item
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
    private void PlayEvent()
    {
        throw new NotImplementedException();
    }
    private void PickupItemWrap()
    {
        _playerControls.attachedObject = "Coin";
        _playerReferences.attachedObject = _interactible;
        //_playerAudioSource.PlayOneShot(_soundEffects[randomsoundSetOnThisObject]);
        //_playerReferences.transform.localScale = new Vector3(6, 6, 6);            // why this ?

        _playerReferences.GetComponent<Animator>().SetTrigger("PickUpCoin");
        _playerReferences.GetComponent<Animator>().SetLayerWeight(1, 1f);
    }





    public void PickupParentingLogic()
    {
        _interactible.transform.SetParent(_playerReferences.playerHand.transform);
        _interactible.transform.localPosition = new Vector3(-0.031f, 0.005f, 0.01f);
        _interactible.transform.localRotation = Quaternion.Euler(89, 310, -34.5f);

        _interactible.GetComponent<SphereCollider>().enabled = false;
    }
    public void DropPickupParentingLogic()
    {
        _interactible.transform.SetParent(null);

        _interactible.transform.position = new Vector3(_playerControls.transform.position.x, 0, _playerControls.transform.position.z);
        _interactible.transform.localRotation = Quaternion.Euler(30, 0, 90);
        _interactible.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _interactible.GetComponent<SphereCollider>().enabled = true;
    }
}

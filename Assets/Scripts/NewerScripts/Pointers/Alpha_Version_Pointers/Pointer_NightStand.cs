using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_NightStand : Pointer_Base
{
    private Animator _nightStandAnimator;
    private Animator _playerAnimator;
    private GameObject _keyPickup;

    [SerializeField]
    private GameObject _confettiObject, _lockClosed, _lockOpen;


    private void Start()
    {
        _nightStandAnimator = Interactible.transform.GetChild(0).GetComponent<Animator>();
    }

    public override void PlayEvent()
    {
        base.PlayEvent();

        _playerAnimator = PlayerRefs.GetComponent<Animator>();

        // set lock_closed false
        _lockClosed.SetActive(false);
        // set lock_open true
        _lockOpen.SetActive(true);

        // play animation
        _nightStandAnimator.Play("Drawer_Open");

        // set confetti active
        _confettiObject.SetActive(true);

        // play sound
        InteractibleScript.AudioSource.PlayOneShot(SoundEffects[0]);

        _keyPickup = PlayerRefs.attachedObject.GetComponentInChildren<Interactible_Base>().transform.parent.gameObject;
        // disable all sparkle objects
        foreach (var sparkle in _keyPickup.GetComponentInChildren<Pointer_Lord>().SparkleObjectsAll)
        {
            sparkle.SetActive(false);
        }
        // lose the key object
        PlayerRefs.attachedObject = null;
        PlayerRefs.PickedUpObject = PickupType.None;
        Destroy(_keyPickup);

        // re-equip gear
        StartCoroutine(ReEquipGear());

    }


       
    private IEnumerator ReEquipGear()
    {
        yield return new WaitForSeconds(0.2f);

        _playerAnimator.Play("Unequipe_0");
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);
    }
}

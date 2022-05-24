using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Fireplace : Pointer_Base
{
    private Interactible_Fireplace _interactibleFireScript;

    private Animator _pickupAnimator, _playerAnimator;


    private void Start()
    {
        _interactibleFireScript = InteractibleParent.GetComponentInChildren<Interactible_Fireplace>();
    }


    public override void PlayEvent()
    {
        base.PlayEvent();


        if (RequiredItemExtraInteraction == PickupType.BucketWater)
        {
            // call method pour on inter_bucket
            PlayerRefs.attachedObject.GetComponentInChildren<Interactible_Bucket>().PourWater();
            _interactibleFireScript.GetDoused();
        }
        else if (RequiredItemExtraInteraction == PickupType.Corn)
        {
            _playerAnimator = PlayerRefs.GetComponent<Animator>();         
            _pickupAnimator = PlayerRefs.attachedObject.GetComponentInChildren<Animator>();
            _pickupAnimator.enabled = true;

            // detach thhe pickup from the player
            _pickupAnimator.transform.parent.SetParent(null);
            // fix orientation object
            if (OrientationPlayer == OrientationType.Left)
            {
                _pickupAnimator.transform.parent.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                _pickupAnimator.transform.parent.localScale = new Vector3(-1, 1, 1);
            }
            

            // disable any sparkles that the pickup had activated
            foreach (var sparkle in _pickupAnimator.GetComponentInChildren<Pointer_Lord>().SparkleObjectsAll)
            {
                sparkle.SetActive(false);
            }

            StartCoroutine(PlayAnimation());
            StartCoroutine(ReEquipGear());
        }

    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.2f);

        _pickupAnimator.Play("PickupThrowingInBucket");

        // play the sound effect thats attached on the player // get audiosource - source.play(sounds[0])
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[2]);

        // play particle
        _interactibleFireScript.MakePopcorn();

        yield return new WaitForSeconds(0.7f);

        // destroy the coin
        Destroy(_pickupAnimator.transform.parent.gameObject);
    }

    private IEnumerator ReEquipGear()
    {
        yield return new WaitForSeconds(0.2f);

        _playerAnimator.Play("Unequipe_0");
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);

        PlayerRefs.attachedObject = null;
        PlayerRefs.PickedUpObject = PickupType.None;
    }
}

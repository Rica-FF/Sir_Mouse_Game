using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Bucket : Pointer_Base
{
    // pointer events for kicking and putting in items

    private Animator _bucketAnimator;
    private Animator _pickupAnimator;
    private Animator _playerAnimator;

    private Interactible_Bucket _interactibleBucket;

    private float _sidewaysForce;


    private void Start()
    {
        _interactibleBucket = GetComponentInParent<Interactible_Bucket>();

        // might become wrong animator(waterfall)
        _bucketAnimator = _interactibleBucket.GetComponentInChildren<Animator>();
        Debug.Log(_bucketAnimator.name + " got animator bucket");

        _sidewaysForce = 150;
    }

    public override void PlayEvent()
    {
        base.PlayEvent();

        // THROW IN ITEM //

        _playerAnimator = PlayerRefs.GetComponent<Animator>();
        // if i am a pointer that plays an event, that has an extra item required
        if (RequiredItemExtraInteraction != PickupType.None)
        {
            // -> plays animation of putting the pickup in the bucket
            _pickupAnimator = PlayerRefs.attachedObject.GetComponentInChildren<Animator>();
            _pickupAnimator.enabled = true;

            // detach thhe pickup from the player
            _pickupAnimator.transform.parent.SetParent(null);
            _pickupAnimator.transform.parent.localScale = new Vector3(1, 1, 1);

            // disable any sparkles that the pickup had activated
            foreach (var sparkle in _pickupAnimator.GetComponentInChildren<Pointer_Lord>().SparkleObjectsAll)
            {
                sparkle.SetActive(false);
            }

            // play anim
            StartCoroutine(PlayAnimation());
            
            StartCoroutine(ReEquipGear());

            // once that animation is done, play the bucket shake animation (use of enumerator yield return ?)       
            StartCoroutine(StartBucketShake());
        }




        // KICK BUCKET //

        // if i am a pointer that does not have extra items required
        if (RequiredItemExtraInteraction == PickupType.None)
        {
            // -> kick the bucket

            // reset side force
            _sidewaysForce = 150;

            // instantiate every item
            foreach (var pickupType in _interactibleBucket.HeldItems)
            {
                switch (pickupType)
                {
                    case PickupType.Corn:
                        var objCorn = Instantiate(_interactibleBucket.CornPrefab, transform.position, Quaternion.identity);
                        AddUpwardsForce(objCorn);
                        break;
                    case PickupType.Coin:
                        var objCoin = Instantiate(_interactibleBucket.CoinPrefab, transform.position, Quaternion.identity);
                        AddUpwardsForce(objCoin);
                        break;
                    case PickupType.Puzzle:
                        var objPuzzle = Instantiate(_interactibleBucket.PuzzlePrefab, transform.position, Quaternion.identity);
                        AddUpwardsForce(objPuzzle);
                        break;
                    case PickupType.Key:
                        var objKey = Instantiate(_interactibleBucket.KeyPrefab, transform.position, Quaternion.identity);
                        AddUpwardsForce(objKey);
                        break;
                }
            }

            // remove the pickups from the bucket
            _interactibleBucket.HeldItems.Clear();
        }
    }


    private void AddUpwardsForce(GameObject objToAddForceTo)
    {
        //objToAddForceTo.GetComponent<Rigidbody>().isKinematic = false;
        //objToAddForceTo.GetComponent<Rigidbody>().AddForce(new Vector3(_sidewaysForce, 200, 0));

        objToAddForceTo.GetComponentInChildren<Rigidbody>().isKinematic = false;
        objToAddForceTo.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(_sidewaysForce, 200, 0));
        // ADD SCRIPT WITH PHYSICS UPDATE !!!

        // sideways forcee is to expel object at different angles.
        _sidewaysForce -= 50;
        if (_sidewaysForce <= -151)
        {
            _sidewaysForce = 150;
        }
    }



    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(0.2f);

        _pickupAnimator.Play("PickupThrowingInBucket");

        // play the sound effect thats attached on the player // get audiosource - source.play(sounds[0])
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[2]);

        // adds the type of pickup to the bucket list
        _interactibleBucket.HeldItems.Add(RequiredItemExtraInteraction);
    }

    private IEnumerator ReEquipGear()
    {
        yield return new WaitForSeconds(0.2f);


        _playerAnimator.Play("Unequipe_0");
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);

        PlayerRefs.attachedObject = null;
        PlayerRefs.PickedUpObject = PickupType.None;
    }

    private IEnumerator StartBucketShake()
    {
        yield return new WaitForSeconds(1f);

        // destroy the coin
        Destroy(_pickupAnimator.transform.parent.gameObject);


        // play the sound effect thats attached on the player // get audiosource - source.play(sounds[...])
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[8]);

        _bucketAnimator.enabled = true;
        _bucketAnimator.Play("Bucket_Shake");
    }
}
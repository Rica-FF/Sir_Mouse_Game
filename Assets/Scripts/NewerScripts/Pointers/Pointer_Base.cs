using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pointer_Base : MonoBehaviour
{
    public PointerType TypeOfPointer;

    public PickupType TypeOfPickup;

    public PickupType RequiredItemExtraInteraction;
    public bool RequiresUseBeforeEventNone;

    public bool RequiresDestination;
    public Transform DestinationSpot;

    public OrientationType OrientationPlayer;

    public AudioClip[] SoundEffects;

    [HideInInspector]
    public PlayerTouchControls PlayerControls;
    [HideInInspector]
    public PlayerReferences PlayerRefs;
    private AudioSource _playerAudioSource;

    [HideInInspector]
    public GameObject Interactible;
    [HideInInspector]
    public GameObject InteractibleParent;
    [HideInInspector]
    public Interactible_Base InteractibleScript;
    [HideInInspector]
    public Rigidbody InteractibleRigid;

    [HideInInspector]
    public GameObject SpriteObject;
    private Quaternion _spriteOriginalRotation;

    private List<Pointer_Base> _pointerBasesInScene = new List<Pointer_Base>();
    public List<GameObject> SparkleObjects = new List<GameObject>();

    [HideInInspector]
    public Pointer_Lord PointerLord;

    private Pointer_Lord[] _bothPointerLords;
    private Interactible_Base _pickupFromInfiniteSupply;
    private GameObject _pickupFromInfiniteSupplyObject;

    //public GameObject ImageExample;
    private GameObject _uiImageForBag;
    private Transform _panelUiIcons;

    private void Awake()
    {
        PlayerControls = FindObjectOfType<PlayerTouchControls>();
        StartCoroutine(GetPlayerRefs());

        InteractibleScript = GetComponentInParent<Interactible_Base>();
        Interactible = InteractibleScript.gameObject;
        InteractibleParent = Interactible.transform.parent.gameObject;

        SpriteObject = Interactible.transform.GetChild(0).gameObject;
        _spriteOriginalRotation = SpriteObject.transform.rotation;

        PointerLord = GetComponentInParent<Pointer_Lord>();

        if (InteractibleParent.GetComponentInChildren<Rigidbody>() != null)
        {
            InteractibleRigid = InteractibleParent.GetComponentInChildren<Rigidbody>();
        }
        
        //StartCoroutine(GetSparkleRefs());
    }


    public void ActivateInteractibleAction()
    {
        switch (TypeOfPointer)
        {
            case PointerType.Hit:
                HitSword();     // hit sword (also uses PlayEvent())
                break;
            case PointerType.Pickup:
                PickupItemWrap(TypeOfPickup);
                break;
            case PointerType.PickupInfinite:
                PickupItemInfiniteWrap(TypeOfPickup);
                break;
            case PointerType.Event:  // plays specific event
                PlayEvent();
                break;
            case PointerType.ChangeLevel:
                ChangeLevel();
                break;
            case PointerType.PutInBackpack:
                PutItemInBackpack();
                break;
        }
    }



    private void HitSword()
    {
        // play sword animation  // stop movement
        PlayerRefs.GetComponent<Animator>().Play("Slash");

        // play sound effect slash

        // activate event
        PlayEvent();
    }

    public virtual void PlayEvent()
    {
        // when sound effects are needed
        // -> use Pointer_"ObjectName" override PlayEvent() to call them 
        // - see Pointer_Bucket for reference
    }

    public virtual void PickupItemWrap(PickupType pickup)
    {
        // generic pickup animation
        PlayerRefs.GetComponent<Animator>().SetTrigger("PickUpCoin");
        // lock the arm after x seconds
        StartCoroutine(SetRigWeights());

        PlayerRefs.attachedObject = InteractibleParent;

        // disable the physics
        if (InteractibleRigid.TryGetComponent(out PhysicsCorrector pxCorrector))
        {
            StartCoroutine(pxCorrector.StopPhysicsLogic());
        }

        PlayerRefs.PickedUpObject = pickup;

        StartCoroutine(GetSparkleRefs(false));
    }
    private IEnumerator SetRigWeights()
    {
        yield return new WaitForSeconds(0.9f);
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 1f);
    }

    public virtual void PickupItemInfiniteWrap(PickupType pickup)
    {
        // generic pickup animation
        PlayerRefs.GetComponent<Animator>().SetTrigger("PickUpCoin");
        // lock the arm
        PlayerRefs.GetComponent<Animator>().SetLayerWeight(1, 1f);

        // get the 2 pointer lords (because infinite spawning objects will only have 2)
        _bothPointerLords = InteractibleParent.GetComponentsInChildren<Pointer_Lord>();

        // if there's only 1 lord...
        if (_bothPointerLords.Length < 2)
        {
            // re-instantiate the corn prefab in corn mountain interactible
            Instantiate(InteractibleScript._infinitelyRespawnablePickupPrefab, Interactible.transform);
            // refill array
            _bothPointerLords = InteractibleParent.GetComponentsInChildren<Pointer_Lord>();
        }
        // logic to properly get the interactible object again
        foreach (var lord in _bothPointerLords)
        {
            lord.UpdatePointerBases();
            foreach (var pointerbase in lord.PointerBases)
            {
                if (pointerbase.TypeOfPointer == PointerType.Pickup && pointerbase.InteractibleScript.InteractibleType == InteractibleType.ShowsPointer)
                {
                    _pickupFromInfiniteSupply = pointerbase.GetComponentInParent<Interactible_Base>();
                    _pickupFromInfiniteSupplyObject = _pickupFromInfiniteSupply.transform.parent.gameObject;  
                }
            }            
        }
        _pickupFromInfiniteSupplyObject.GetComponentInChildren<Rigidbody>().isKinematic = true;

        PlayerRefs.attachedObject = _pickupFromInfiniteSupplyObject;       
        PlayerRefs.PickedUpObject = pickup;

        StartCoroutine(GetSparkleRefsInfiniteSpawner(_pickupFromInfiniteSupplyObject));
    }

    public void ChangeLevel()
    {
        // get the additional script on this objects parent (Interactible_Level_Changer) and read what index it is to then apply the needed logic,
        Interactible_LevelChanger interactibleScript = null;
        interactibleScript = GetComponentInParent<Interactible_LevelChanger>();

        interactibleScript.LoadLevelSlow();
    }

    private void PutItemInBackpack()
    {
        // (play animation on the interactible that flings it into the backpack)
        StartCoroutine(ForceObjectInBag());
        // disable the interactible sprite
        Interactible.transform.GetChild(0).gameObject.SetActive(false);
    }
    IEnumerator ForceObjectInBag() 
    {
        // get the world to screen pos of the interactible
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(InteractibleParent.transform.position); 

        // instantiate a copy image on an overlay canvas    
        _panelUiIcons = FindObjectOfType<BackPack_Minimap_Manager>().transform;
        var canvasToUse = _panelUiIcons.transform.parent;
        Panel_Pickups_Chugger panelPickups = canvasToUse.GetComponentInChildren<Panel_Pickups_Chugger>();

        GameObject uiCopy = Instantiate(panelPickups.PickupImagePrefabs[((int)TypeOfPickup) - 1], panelPickups.transform);
        uiCopy.transform.position = screenPosition;
        _uiImageForBag = uiCopy;

        // the position of the bag
        var targetPosition = canvasToUse.transform.GetChild(0).transform.GetChild(1).transform.position;

        // Calculate distance to target
        float target_Distance = Vector2.Distance(targetPosition, screenPosition);
        float speed = 400f;
        float arcHeight = 0.5f;
        float _stepScale = 0f;
        float _progress = 0f;
        _stepScale = speed / target_Distance;
        arcHeight = arcHeight * target_Distance;

        Interactible_Chugger chuggScript = GetComponent<Interactible_Chugger>();
        chuggScript.AllocateValues(speed, arcHeight, _stepScale, _progress, screenPosition, targetPosition, uiCopy);
        chuggScript.enabled = true;

        yield return null;
    }
    public void ImageArrivedInBag()
    {
        GetComponent<Interactible_Chugger>().enabled = false;

        // sets bool true
        Backpack_Inventory.ItemsInBackpack[((int)TypeOfPickup - 1)] = true;
        // activates animation bag
        _panelUiIcons.GetComponent<Animator>().Play("Popout_Backpack");
        // destroy the UI image
        Destroy(_uiImageForBag);
        // destroys the interactible
        Destroy(InteractibleParent);
    }



    IEnumerator GetPlayerRefs()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerRefs = PlayerControls.GetComponentInChildren<PlayerReferences>();
        _playerAudioSource = PlayerRefs.GetComponent<AudioSource>();
    }

    public IEnumerator GetSparkleRefs(bool isChangingLevel)
    {
        DeActivateSparkles(isChangingLevel);

        yield return new WaitForSeconds(1f);

        // get all the sparkle objects that matter for this pickup
        _pointerBasesInScene = FindObjectsOfType<Pointer_Base>().ToList();
        foreach (var pointerBase in _pointerBasesInScene)
        {
            // get interactible script -> get parent transform -> getcomponentInChildren(true) of particlesystem  --- POSITION SPARKLES ABOVE THE INTERACTIBLE (to prevent wrong particles from being used)
            GameObject sparklesObject = pointerBase.GetComponentInParent<Interactible_Base>().transform.parent.GetComponentInChildren<ParticleSystem>(true).gameObject; 

            if (pointerBase.RequiredItemExtraInteraction == TypeOfPickup && SparkleObjects.Contains(sparklesObject) == false) // extra if in case the object was alrdy added
            {
                // add extra logic here in case other bools need to be checked for (like water in a bucket)(change bucket to different type of pickup)

                if (pointerBase.InteractibleScript.TryGetComponent(out Interactible_Soil soilScript)) // checking soil
                {
                    if (soilScript.PlantedPickup == PickupType.None && TypeOfPickup != PickupType.BucketWater)
                    {
                        SparkleObjects.Add(sparklesObject);
                        PointerLord.SparkleObjectsAll.Add(sparklesObject);
                    }
                    else if (soilScript.PlantedPickup != PickupType.None && TypeOfPickup == PickupType.BucketWater)
                    {
                        SparkleObjects.Add(sparklesObject);
                        PointerLord.SparkleObjectsAll.Add(sparklesObject);
                    }
                }
                else // checking everything else
                {
                    SparkleObjects.Add(sparklesObject);
                    PointerLord.SparkleObjectsAll.Add(sparklesObject);
                }    
            }

            // extra if for required item interactions
            if (pointerBase.GetComponentInParent<Interactible_Base>()._requiredItemType == TypeOfPickup && SparkleObjects.Contains(sparklesObject) == false)
            {
                SparkleObjects.Add(sparklesObject);
                PointerLord.SparkleObjectsAll.Add(sparklesObject);
            }



            // logic from lord cuzz pointer works fine
            // 6) checking for soil plantations
            //if (_interactibleScriptOnThisParent.TryGetComponent(out Interactible_Soil soilScript))
            //{
            //    if (soilScript.PlantedPickup != PickupType.None) // if somethings been planted -> remove all event pointers that plant pickups  ---- WRONG
            //    {
            //        foreach (var pointer in PointerBases)
            //        {
            //            // checks for all the pickups that cant be planted (coversly i can check for ones that can oly be planted)
            //            if (pointer.RequiredItemExtraInteraction != PickupType.None && pointer.RequiredItemExtraInteraction != PickupType.BucketWater)
            //            {
            //                // remove the trigger
            //                _availablePointerIndexLimit -= 1;

            //                var pointerTrigger = pointer.transform.GetChild(0).gameObject;
            //                AvailableTriggerObjects.Remove(pointerTrigger);
            //            }
            //        }
            //    }
            //    else if (soilScript.PlantedPickup == PickupType.None && _interactibleScriptOnThisParent.PlayerRefs.PickedUpObject == PickupType.BucketWater) // is nothings been planted, and i have a bucket, remove every trigger
            //    {
            //        foreach (var pointer in PointerBases)
            //        {
            //            // remove the trigger
            //            _availablePointerIndexLimit -= 1;

            //            var pointerTrigger = pointer.transform.GetChild(0).gameObject;
            //            AvailableTriggerObjects.Remove(pointerTrigger);

            //        }
            //    }
            //}



        }

        // activate the found sparkle objects
        ActivateSparkles();
    }

    public IEnumerator GetSparkleRefsInfiniteSpawner(GameObject pickupInMountain)
    {
        DeActivateSparkles(false);

        // getting the pointer_base which is of type pickup, in the infinite spawner
        var bases = pickupInMountain.GetComponentsInChildren<Pointer_Base>();
        Pointer_Base pointerOfInterest = null;
        foreach (var pointBase in bases)
        {
            if (pointBase.TypeOfPointer == PointerType.Pickup)
            {
                pointerOfInterest = pointBase;
            }
        }

        yield return new WaitForSeconds(1f);

        // get all the sparkle objects that matter for the instantiated pickup
        _pointerBasesInScene = FindObjectsOfType<Pointer_Base>().ToList();
        foreach (var pointerBase in _pointerBasesInScene)
        {
            // get interactible script -> get parent transform -> getcomponentInChildren(true) of particlesystem  --- POSITION SPARKLES ABOVE THE INTERACTIBLE (to prevent wrong particles from being used)
            GameObject sparklesObject = pointerBase.GetComponentInParent<Interactible_Base>().transform.parent.GetComponentInChildren<ParticleSystem>(true).gameObject;

            if (pointerBase.RequiredItemExtraInteraction == TypeOfPickup && pointerOfInterest.SparkleObjects.Contains(sparklesObject) == false) // if extra == this type && if in case the object was alrdy added
            {
                // add extra logic here in case other bools need to be checked for (like water in a bucket)

                if (pointerBase.InteractibleScript.TryGetComponent(out Interactible_Soil soilScript)) // checking soil
                {
                    if (soilScript.PlantedPickup == PickupType.None)
                    {
                        pointerOfInterest.SparkleObjects.Add(sparklesObject);
                        pointerOfInterest.PointerLord.SparkleObjectsAll.Add(sparklesObject);
                    }
                    else if (soilScript.PlantedPickup != PickupType.None && TypeOfPickup == PickupType.Bucket)
                    {
                        pointerOfInterest.SparkleObjects.Add(sparklesObject);
                        pointerOfInterest.PointerLord.SparkleObjectsAll.Add(sparklesObject);
                    }
                }
                else // checking everything else
                {
                    pointerOfInterest.SparkleObjects.Add(sparklesObject);
                    pointerOfInterest.PointerLord.SparkleObjectsAll.Add(sparklesObject);
                }
            }

            // extra if for required item interactions
            if (pointerBase.GetComponentInParent<Interactible_Base>()._requiredItemType == TypeOfPickup && pointerOfInterest.SparkleObjects.Contains(sparklesObject) == false)
            {
                pointerOfInterest.SparkleObjects.Add(sparklesObject);
                pointerOfInterest.PointerLord.SparkleObjectsAll.Add(sparklesObject);
            }
        }

        // activate the found sparkle objects
        ActivateSparklesInfinite(pointerOfInterest);
    }


    public void ActivateSparkles()
    {
        foreach (var sparkle in SparkleObjects)
        {
            sparkle.SetActive(true);
        }
    }
    public void DeActivateSparkles(bool isChangingLevel)
    {
        if (isChangingLevel == true)
        {
            SparkleObjects.Clear();
            PointerLord.SparkleObjectsAll.Clear();
        }
        else
        {
            // also call this when a pickup is thrown 
            foreach (var sparkle in SparkleObjects)
            {
                sparkle.SetActive(false);
            }

            SparkleObjects.Clear();
            PointerLord.SparkleObjectsAll.Clear();
        }
    }
    public void ActivateSparklesInfinite(Pointer_Base pickupPointerInMountain)
    {
        foreach (var sparkle in pickupPointerInMountain.SparkleObjects)
        {
            sparkle.SetActive(true);
        }
    }


    public void PickupParentingLogic()  
    {
        InteractibleParent.transform.SetParent(PlayerRefs.playerHand.transform);
        InteractibleParent.transform.localPosition = new Vector3(-0.03f, 0.02f, 0.02f);
        InteractibleRigid.isKinematic = true;

        // set sprite transform to 0,0,0
        SpriteObject.transform.rotation = _spriteOriginalRotation;

        // play the sound effect thats attached on the player // get audiosource - source.play(sounds[0])
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[0]);

        // rotation can require specifics, depending on how the pickup sprites are organized
        if (PlayerRefs.PickedUpObject == PickupType.Bucket)
        {
            InteractibleParent.transform.localRotation = Quaternion.Euler(64, 39, 5);
        }
        else
        {
            InteractibleParent.transform.localRotation = Quaternion.Euler(60, 0, -30);
        }

        Interactible.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Interactible.transform.localPosition = new Vector3(0, 0, 0);

        Interactible.GetComponent<SphereCollider>().enabled = false;
    }
    public void PickupInfiniteParentingLogic()  
    {
        // if this pointer is type infinite -> Interactibleparent = _pickupFromInfiniteSupplyObject
        _pickupFromInfiniteSupplyObject.transform.SetParent(PlayerRefs.playerHand.transform);
        _pickupFromInfiniteSupplyObject.transform.localPosition = new Vector3(-0.03f, 0.02f, 0.02f);
        _pickupFromInfiniteSupplyObject.GetComponentInChildren<Rigidbody>().isKinematic = true;

        // play the sound effect thats attached on the player // get audiosource - source.play(sounds[0])
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[0]);

        // rotation can require specifics, depending on how the pickup sprites are organized
        if (PlayerRefs.PickedUpObject == PickupType.Bucket)
        {
            _pickupFromInfiniteSupplyObject.transform.localRotation = Quaternion.Euler(64, 39, 5);
        }
        else
        {
            _pickupFromInfiniteSupplyObject.transform.localRotation = Quaternion.Euler(60, 0, -30);
        }

        _pickupFromInfiniteSupply.transform.localRotation = Quaternion.Euler(0, 0, 0);
        _pickupFromInfiniteSupply.transform.localPosition = new Vector3(0, 0, 0);

        _pickupFromInfiniteSupply.GetComponent<SphereCollider>().enabled = false;
    }
    public void DropPickupParentingLogic()
    {
        InteractibleParent.transform.SetParent(null);

        // play the sound effect thats attached on the player // get audiosource - source.play(sounds[1])
        PlayerRefs.GetComponent<AudioSource>().PlayOneShot(PlayerRefs.playerSounds[1]);

        // re-search the PlayerControls when entering a new scene !!!!!
        if (PlayerControls == null)
        {
            PlayerControls = PlayerRefs.GetComponentInParent<PlayerTouchControls>();
        }

        InteractibleParent.transform.position = new Vector3(PlayerControls.transform.position.x, 0, PlayerControls.transform.position.z);
        InteractibleParent.transform.localRotation = Quaternion.Euler(0, 0, 0);
        InteractibleParent.transform.localScale = new Vector3(1f, 1f, 1f);

        Interactible.GetComponent<SphereCollider>().enabled = true;
    }
}

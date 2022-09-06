using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    public GameObject playerHand;
    public GameObject playerHead;
    public GameObject swordJoint;
    public GameObject swordGeo;
    public GameObject shieldJoint;
    public GameObject shieldGeo;
    public GameObject attachedObject;
    public GameObject dropPointer;


    [HideInInspector]
    public List<Vector3> cornPositions = new List<Vector3>();
    [HideInInspector]
    public List<int> cornGrowPhases = new List<int>();
    [HideInInspector]
    public int costumeIndex = 0;
    [HideInInspector]
    public bool rosesHaveWater = false;

    public List<GameObject> pointers = new List<GameObject>();
    public List<GameObject> PointerLords = new List<GameObject>();
    
    public int shieldIndex = 0;
    public GameObject newShield;
    public GameObject ShieldSprite;

    public bool hasGoldenSword = false;
    //[HideInInspector]
    public bool madePopcorn = false;
    public bool exploded = false;
    public bool onWaterSpot = false;

    public AudioClip[] playerSounds = new AudioClip[0];

    public bool mouseControls = false;


    ////////////

    public PickupType PickedUpObject;
    [HideInInspector]
    public Animator PlayerAnimator;
    public Pointer_Base _currentActivePointer;
    [HideInInspector]
    public Pointer_Base PointerOfPickUpInHands;



    private void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
    }



    private void Update()
    {
        //only check specific logic if there's more than 2 pointers ...
        if (PointerLords.Count >= 2)
        {
            GameObject closestPointerLord = null;
            Pointer_Lord closestPointerLordScript = null;
            GameObject closestTrigger = null;

            // first of, give closestPointer a value
            if (closestPointerLord == null)
            {
                for (int i = 0; i < PointerLords.Count; i++)
                {
                    if (PointerLords[i] != null)
                    {
                        closestPointerLord = PointerLords[i];
                        closestPointerLordScript = closestPointerLord.GetComponent<Pointer_Lord>();
                        closestTrigger = closestPointerLordScript.AvailableTriggerObjects[closestPointerLordScript.CurrentPointerIndex];
                        break;
                    }
                }
            }
            // calculate the distance between the interactible objects to decide nearest pointer
            if (closestPointerLord != null)
            {             
                for (int i = 0; i < PointerLords.Count; i++)
                {
                    if (PointerLords[i] != null)
                    {
                        var distance1 = Vector3.Distance(transform.position, closestPointerLord.GetComponentInParent<Interactible_Base>().transform.position);
                        var distance2 = Vector3.Distance(transform.position, PointerLords[i].GetComponentInParent<Interactible_Base>().transform.position);

                        if (distance1 > distance2)
                        {
                            // update closest pointer
                            closestPointerLord = PointerLords[i];
                            closestPointerLordScript = closestPointerLord.GetComponent<Pointer_Lord>();                           
                            closestTrigger = closestPointerLordScript.AvailableTriggerObjects[closestPointerLordScript.CurrentPointerIndex].gameObject;
                        }
                        else
                        {
                            PointerLords[i].GetComponent<Pointer_Lord>().AvailableTriggerObjects[PointerLords[i].GetComponent<Pointer_Lord>().CurrentPointerIndex].SetActive(false);
                            PointerLords[i].GetComponentInParent<Interactible_Base>().PointerNextTriggerObject.SetActive(false);
                        }
                    }
                }
                closestTrigger.SetActive(true);


                // only set Pointer_arrow to true if it has more than 1 available pointer
                if (closestPointerLordScript.AvailableTriggerObjects.Count > 1)
                {
                    closestTrigger.GetComponentInParent<Interactible_Base>().PointerNextTriggerObject.SetActive(true);
                }
                
                _currentActivePointer = closestTrigger.GetComponentInParent<Pointer_Base>();

            }
        }
    }


    // start this method on start of touchcontrols
    public void SearchReferencesForObjectInHand()
    {
        if (attachedObject != null)
        {
            // update available pointer
            attachedObject.GetComponentInChildren<Pointer_Lord>().UpdateAvailablePointers();
            // update sparkles
            StartCoroutine(attachedObject.GetComponentInChildren<Pointer_Lord>().GetComponentInChildren<Pointer_Base>().GetSparkleRefs(true));
        }
    }



    public void DropObject()
    {
        if(attachedObject)
        {
            SetSound(1);

            if(attachedObject.name == "Coin_C")
            {
                attachedObject.GetComponent<Coin>().Drop();
            }
            else if (attachedObject.name == "Corn")
            {
                attachedObject.GetComponent<Corn>().Drop();
            }
            else if (attachedObject.name == "Bucket")
            {
                attachedObject.GetComponent<Bucket>().Drop();
            }
            else if (attachedObject.name == "Puzzle_piece")
            {
                attachedObject.GetComponent<PuzzlePiece>().Drop();
            }
            else if (attachedObject.name == "Key")
            {
                attachedObject.GetComponent<Key>().Drop();
            }
        }
    }
    public void DropObjectNew()
    {
        PlayerAnimator.SetTrigger("DropCoin");
        StartCoroutine(DetachObject());
    }
    IEnumerator DetachObject()
    {
        yield return new WaitForSeconds(0.4f);

        // cointransform
        transform.parent = null; 
        transform.localRotation = Quaternion.Euler(30, 0, 90);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        transform.GetComponent<SphereCollider>().enabled = true;

        GetComponent<Animator>().SetLayerWeight(1, 0f);      
    }



    // EVENTS called on ANIMATIONS on the player
    private void AttachObjectAnimationEvent()
    {
        // if this pointer is type infinite -> Interactibleparent = _pickupFromInfiniteSupplyObject
        // SpriteObject. rotation (hardcode / ignore)
        if (_currentActivePointer.TypeOfPointer == PointerType.PickupInfinite)
        {
            _currentActivePointer.PickupInfiniteParentingLogic();
            PointerOfPickUpInHands = attachedObject.GetComponentInChildren<Pointer_Base>();   
        }
        else
        {
            _currentActivePointer.PickupParentingLogic();
            PointerOfPickUpInHands = _currentActivePointer;
        }        

        // re-establish what possible pointerlords are in the vicinity and which one should be active
        // -- pointer lord is still in list, might have to do with requirement
        // -- pointer lord well not being removed properly ?
    }
    private void DetachObjectAnimationEvent()
    {
        Debug.Log(PointerOfPickUpInHands + " is the pointer");
        PointerOfPickUpInHands.DropPickupParentingLogic();
        PointerOfPickUpInHands = null;
    }
    /////////////////////////




    public void ClearList()
    {
        for (int i = 0; i < pointers.Count; i++)
        {
            if (pointers[i])
            {
                pointers[i].GetComponent<PopUpPointer>().pointer.SetActive(false);
            }
        }
        pointers.Clear();
    }





    public void SetSound(int index)
    {
        GetComponent<AudioSource>().clip = playerSounds[index];
    }
}

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
    
    public int shieldIndex = 0;
    public GameObject newShield;

    public bool hasGoldenSword = false;
    //[HideInInspector]
    public bool madePopcorn = false;
    public bool exploded = false;
    public bool onWaterSpot = false;

    public AudioClip[] playerSounds = new AudioClip[0];

    public bool mouseControls = false;


    ////////////

    public PickupType _pickedUpObject;

    private Animator _playerAnimator;

    public Pointer_Base _currentActivePointer;
    private Pointer_Base _pointerOfPickUpInHands;


    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
    }



    private void Update()
    {
        //only check specific logic if there's more than 2 pointers ...
        if (pointers.Count >= 2)
        {
            GameObject closestPointer = null;

            // first of, give closestPointer a value
            if (closestPointer == null)
            {
                for (int i = 0; i < pointers.Count; i++)
                {
                    if (pointers[i] != null)
                    {
                        closestPointer = pointers[i];
                        break;
                    }
                }
            }
            // calculate the distance between objects to decide nearest pointer
            if (closestPointer != null)
            {
                for (int i = 0; i < pointers.Count; i++)
                {
                    if (pointers[i] != null)
                    {
                        // update closest pointer
                        if (Vector3.Distance(gameObject.transform.position, closestPointer.transform.position) > Vector3.Distance(gameObject.transform.position, pointers[i].transform.position))
                        {
                            closestPointer = pointers[i];
                        }
                        else
                        {
                            pointers[i].SetActive(false);
                        }
                    }
                }
                closestPointer.SetActive(true);
                _currentActivePointer = closestPointer.GetComponent<Pointer_Base>();
            }
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
        _playerAnimator.SetTrigger("DropCoin");
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



    // events called on animations on the player
    private void AttachObjectAnimationEvent()
    {
        _currentActivePointer.PickupParentingLogic();
        _pointerOfPickUpInHands = _currentActivePointer;
    }
    private void DetachObjectAnimationEvent()
    {
        _pointerOfPickUpInHands.DropPickupParentingLogic();
        _pointerOfPickUpInHands = null;
    }





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

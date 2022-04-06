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




    private void Update()
    {
        if (pointers.Count > 0)
        {   
            // bool reset
            bool isEmpty = true;

            for (int i = 0; i < pointers.Count; i++)
            {
                if (pointers[i] != null)
                {
                    isEmpty = false;
                }
            }
            // bool check
            if(isEmpty)
            {
                pointers.Clear();
            }
            else // else if there's atleast 1 pointer in the list
            {
                GameObject closestPointer = null;

                // if there's only 1 pointer available...
                if (pointers.Count < 2)
                {
                    if (pointers[0] != null)
                    {
                        closestPointer = pointers[0];
                        pointers[0].GetComponent<PopUpPointer>().pointer.SetActive(true);
                    }
                }
                else // else if more than 1 pointer
                {                    
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
                    else
                    {
                        for (int i = 0; i < pointers.Count; i++)
                        {
                            if(pointers[i] != null)
                            {
                                if (Vector3.Distance(gameObject.transform.position, closestPointer.transform.position) > Vector3.Distance(gameObject.transform.position, pointers[i].transform.position))
                                {
                                    closestPointer = pointers[i];
                                }
                            }
                        }
                    }
                }
                closestPointer.GetComponent<PopUpPointer>().pointer.SetActive(true);
            }
        }



        // fresher logic below //

        //only check specific logic if there's more than 2 pointers available //
        //if (pointers.Count >= 2)
        //{
        //    GameObject closestPointer = null;

        //    // first of, give closestPointer a value
        //    if (closestPointer == null)
        //    {
        //        for (int i = 0; i < pointers.Count; i++)
        //        {
        //            if (pointers[i] != null)
        //            {
        //                closestPointer = pointers[i];
        //                break;
        //            }
        //        }
        //    }

        //    // calculate the distance between objects to decide closes pointer
        //    if (closestPointer != null)
        //    {
        //        for (int i = 0; i < pointers.Count; i++)
        //        {
        //            if (pointers[i] != null)
        //            {
        //                if (Vector3.Distance(gameObject.transform.position, closestPointer.transform.position) > Vector3.Distance(gameObject.transform.position, pointers[i].transform.position))
        //                {
        //                    closestPointer = pointers[i];
        //                }
        //            }
        //        }
        //    }

        //    if (closestPointer.activeSelf == false)
        //    {
        //        closestPointer.GetComponent<Interactible_Base>().Pointer.SetActive(true);
        //    }
        //}
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

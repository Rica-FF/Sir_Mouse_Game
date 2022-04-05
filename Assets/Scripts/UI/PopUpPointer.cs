using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpPointer : MonoBehaviour
{
    public GameObject pointer;
    public bool needsObject = false;
    public bool alwaysActive = false;
    public string objectName;
    public string objectName2;

    public GameObject cornImage;
    public GameObject waterImage;

    [HideInInspector]
    static public bool disableIrrelevantPointers = false;

    public int pointerIndex;
    public GameObject player;

    public bool DontDisablePointer = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !player)
        {
            player = collider.gameObject.transform.Find("RM_Player").gameObject;
        }

         if (needsObject)
         {
             if (collider.tag == "Player" && collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject)
             {
                 if(collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject.name == objectName)
                 {
                     pointer.SetActive(true);
                     if(cornImage)
                     {
                         cornImage.SetActive(true);
                         waterImage.SetActive(false);
                     }
                 }

                 if(collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject.name == objectName2 &&
                     collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject.GetComponent<Bucket>().isFilled)
                 {
                     pointer.SetActive(true);
                     cornImage.SetActive(false);
                     waterImage.SetActive(true);
                 }
             }
         }
         else if(collider.tag == "Player" && !disableIrrelevantPointers)
         {
            player.GetComponent<PlayerReferences>().pointers.Add(gameObject);
             pointerIndex = player.GetComponent<PlayerReferences>().pointers.Count - 1;
         }
         else if(alwaysActive && collider.tag == "Player")
         {
             pointer.SetActive(true);           
         }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (needsObject)
        {
            if (collider.tag == "Player" && collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject)
            {
                if (collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject.name == objectName ||
                    collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject.name == objectName2 &&
                    collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject.GetComponent<Bucket>().isFilled)
                {
                    pointer.SetActive(false);
                    //collider.gameObject.GetComponent<PlayerTouchControls>().TurnOnDropPointer();
                }
            }
        }
        else if (collider.tag == "Player" && !disableIrrelevantPointers)
        {
            if (player.GetComponent<PlayerReferences>().pointers.Count > 0)
            {
                player.GetComponent<PlayerReferences>().pointers[pointerIndex] = null;
            }
            pointer.SetActive(false);
        }
        else if (alwaysActive && collider.tag == "Player")
        {
            pointer.SetActive(false);
            if(collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().attachedObject)
            {
                //collider.gameObject.transform.Find("RM_Player").GetComponent<PlayerReferences>().dropPointer.SetActive(true);
            }
        }
        
    }

    public void DisablePointer()
    {
        if (!DontDisablePointer)
        {
            if(gameObject.GetComponent<SphereCollider>())
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
            }
            else if(gameObject.GetComponent<BoxCollider>())
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            pointer.SetActive(false);

            if (player)
            {
                if (player.GetComponent<PlayerReferences>().pointers.Count > 0)
                {
                    player.GetComponent<PlayerReferences>().pointers[pointerIndex] = null;
                }
            }
        }
    }

    public void DisablePointer2()
    {
        pointer.SetActive(false);

        if (gameObject.GetComponent<SphereCollider>())
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
        }
        else if (gameObject.GetComponent<BoxCollider>())
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (player)
        {
            if (player.GetComponent<PlayerReferences>().pointers.Count > 0)
            {
                player.GetComponent<PlayerReferences>().pointers[pointerIndex] = null;
            }
        }
    }

    public void EnablePointer()
    {
        if (gameObject.GetComponent<BoxCollider>())
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
    }

    public void DelayEnablePointer(float seconds)
    {
        if (gameObject.tag == "Tomatenplant")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(_DelayEnablePointer2(seconds));
        }    
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            StartCoroutine(_DelayEnablePointer(seconds));
        }
    }

    IEnumerator _DelayEnablePointer(float seconds)
    {
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        gameObject.GetComponent<SphereCollider>().enabled = true;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator _DelayEnablePointer2(float seconds)
    {
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void DelayEnablePointer5(float seconds)
    {
        StartCoroutine(DelayEnablePointer5_(seconds));
    }

    IEnumerator DelayEnablePointer5_(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        gameObject.GetComponent<BoxCollider>().enabled = true;
        pointer.SetActive(true);
    }

    public void PrintPointerIndex()
    {
        print(gameObject.name + " " + pointerIndex);
    }
}

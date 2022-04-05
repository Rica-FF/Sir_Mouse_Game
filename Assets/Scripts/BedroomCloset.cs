using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomCloset : MonoBehaviour
{
    public GameObject closed;
    public GameObject open;
    public GameObject pointer;
    public GameObject shield;
    public GameObject shieldGeo;
    public GameObject pointerIcon;
    public GameObject playerRigid;

    public Sprite[] shields = new Sprite[4];

    private bool playerInLocation = false;
    public int spriteIndex = 0;

    public AudioClip OpenDoor;
    public AudioClip CloseDoor;

    private bool OpenOrClosed = false;

    void SetUp()
    {
        spriteIndex = playerRigid.GetComponent<PlayerTouchControls>().player.GetComponent<PlayerReferences>().shieldIndex;
    }

    public void OpenOrClose()
    {
        if (OpenOrClosed) // if Open
        {
            print("Closed");
            /*
            GetComponent<AudioSource>().clip = OpenDoor
            shield.SetActive(false);
            closed.SetActive(true);
            open.SetActive(false);*/

            GetComponent<AudioSource>().clip = OpenDoor;
            shield.SetActive(false);
            closed.SetActive(true);
            open.SetActive(false);
            StartCoroutine(OpenOrCloseCloset());

        }
        else // if closed
        {
            print("Opened");

            /*
            GetComponent<AudioSource>().clip = CloseDoor;
            SetShieldSprite();
            shield.SetActive(true);
            shield.GetComponent<BoxCollider>().enabled = true;
            transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<PopUpPointer>().DisablePointer2();
            closed.SetActive(false);
            open.SetActive(true);*/
            
            GetComponent<AudioSource>().clip = CloseDoor;
            SetShieldSprite();
            GetComponent<PopUpPointer>().DisablePointer2();
            closed.SetActive(false);
            open.SetActive(true);
            shield.SetActive(true);
            StartCoroutine(ShowShieldCollider());
            StartCoroutine(OpenOrCloseCloset());
        }
    }

    IEnumerator ShowShieldCollider()
    {
        yield return new WaitForSeconds(0.5f);
        shield.GetComponent<BoxCollider>().enabled = true;
    }

    public void ShowClosetCollider()
    {
        StartCoroutine(ShowClosetCollider_());
    }

    IEnumerator ShowClosetCollider_()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator OpenOrCloseCloset()
    {
        yield return new WaitForSeconds(0.5f);

        if (OpenOrClosed)
        {
            OpenOrClosed = false;
        }
        else
        {
            OpenOrClosed = true;
        }
    }







    IEnumerator TurnOnPointer()
    {
        float seconds = 1.5f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            playerInLocation = true;
        }        
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            playerInLocation = false;
        }
    }

    private void SetShieldSprite()
    {
        spriteIndex = playerRigid.GetComponent<PlayerTouchControls>().player.GetComponent<PlayerReferences>().shieldIndex;

        shieldGeo.GetComponent<SpriteRenderer>().sprite = shields[spriteIndex];
        pointerIcon.GetComponent<SpriteRenderer>().sprite = shields[spriteIndex];       
    }
}

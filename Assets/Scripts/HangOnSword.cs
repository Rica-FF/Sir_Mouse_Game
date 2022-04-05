using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangOnSword : MonoBehaviour
{
    public GameObject playerRigid;
    private GameObject playerRig;

    public GameObject goldSword;
    [HideInInspector]
    public GameObject goldSwordClone;
    public GameObject redHead;
    [HideInInspector]
    private GameObject redHeadClone;
    public GameObject angryMeter;
    
    public GameObject halfSword;
    public GameObject popUpPointer;
    public GameObject pointer;

    public float counter = 3f;
    public float previousCounter = 3f;
    public bool pullingSword = false;

    public bool walking = false;

    //
    private float beginValue = 0;

    private bool start = true;

    private void Start()
    {
        StartCoroutine(SetPlayerRig());
    }

    private void Update()
    {
        if (walking)
        {
            if(playerRigid.GetComponent<PlayerTouchControls>().curSpeed < 0.01f)
            {
                StartCoroutine(GrabSword());
                walking = false;
            }
        }

        if (pullingSword)
        {
            if (playerRig.GetComponent<PlayerReferences>().mouseControls)
            {
                if (Input.GetMouseButton(0))
                {
                    if (start)
                    {
                        beginValue = Input.mousePosition.y;
                        start = false;
                    }                    
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (Input.mousePosition.y > beginValue + 100)
                    {
                        Pull();
                        beginValue = 0;
                    }
                    start = true;
                }
            }
            else
            {
                if (Input.touchCount == 1)
                {
                    if (start)
                    {
                        beginValue = Input.GetTouch(0).position.y;
                        start = false;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        if (Input.GetTouch(0).position.y > beginValue + 100)
                        {
                            Pull();
                            beginValue = 0;
                        }
                    }
                }
                else if (Input.touchCount == 0)
                {
                    start = true;
                }
            }


            if (counter > 10)
            {
                StartCoroutine(BlowUp());
            }
            if (counter < 0.1f)
            {
                transform.parent.GetComponent<PopUpPointer>().EnablePointer();
                pointer.SetActive(true);
                ReleaseSword();
            }
            TurnHeadRed();
            if (angryMeter)
            {
                angryMeter.transform.GetChild(1).transform.localScale = new Vector3(1, 1, counter / 10);
                angryMeter.transform.GetChild(1).transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.Lerp(new Color(1f, 0f, 0f, 1f), new Color(0.3f, 1f, 0f, 1f), counter / 10);
            }
        }
    }

    IEnumerator SetPlayerRig()
    {
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        playerRig = playerRigid.GetComponent<PlayerTouchControls>().player;

    }

    public void PullSword()
    {
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = false;
        StartCoroutine(DelayBool());
    }

    IEnumerator DelayBool()
    {
        float seconds = 0.1f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        walking = true;
    }

    IEnumerator GrabSword()
    {
        playerRig.transform.localScale = new Vector3(-6, 6, 6);
        playerRig.GetComponent<Animator>().SetTrigger("HangOn");

        yield return new WaitForSeconds(0.3f);

        playerRig.GetComponent<Animator>().SetTrigger("Grab");

        // Red Head
        redHeadClone = Instantiate(redHead, playerRig.GetComponent<PlayerReferences>().playerHead.transform.position, Quaternion.identity);
        redHeadClone.transform.parent = playerRig.GetComponent<PlayerReferences>().playerHead.transform;
        redHeadClone.transform.localPosition = new Vector3(0.0186f, 0.0f, -0.0082f);
        redHeadClone.transform.localRotation = Quaternion.Euler(-90, 180, -105f);

        angryMeter.SetActive(true);
        pullingSword = true;
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (counter > 0.1f)
        {
            yield return new WaitForSeconds(0.2f);
            counter -= 0.1f;

            previousCounter = counter;
        }
    }

    private void ReleaseSword()
    {
        pullingSword = false;
        angryMeter.SetActive(false);
        playerRig.GetComponent<Animator>().SetTrigger("ToIdle");
        counter = 3f;
        previousCounter = 3;
        StartCoroutine(walkingEnabled());
        Destroy(redHeadClone);
    }

    IEnumerator walkingEnabled()
    {
        yield return new WaitForSeconds(1.0f);
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = true;
    }

    private void TurnHeadRed()
    {
        if (counter > 3)
        {
            redHeadClone.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0, 1, (counter / 7) / 2));
        }
        else
        {
            redHeadClone.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    IEnumerator BlowUp()
    {
        ReleaseSword();
        TurnExploded();
        changeSword();

        GetComponent<AudioSource>().Stop();
        playerRig.GetComponent<MixerManager>().OnlyFX(3.3f);
        playerRig.GetComponent<PlayerReferences>().SetSound(4);
        playerRig.GetComponent<AudioSource>().Play();

        transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        Destroy(transform.GetChild(0).gameObject);

        playerRig.GetComponent<PlayerReferences>().hasGoldenSword = true;
    }

    private void TurnExploded()
    {
        foreach (ExplodeSkin component in playerRig.GetComponentsInChildren<ExplodeSkin>())
        {
            component.TurnExploded();
        }
        playerRig.GetComponent<PlayerReferences>().exploded = true;
    }

    private void changeSword()
    {
        playerRig.GetComponent<PlayerReferences>().swordGeo.SetActive(false);

        goldSwordClone = Instantiate(goldSword, playerRig.GetComponent<PlayerReferences>().swordJoint.transform.position, Quaternion.identity);
        goldSwordClone.transform.parent = playerRig.GetComponent<PlayerReferences>().swordJoint.transform;
        goldSwordClone.transform.localScale = new Vector3(1, 1, 1);
        goldSwordClone.transform.localRotation = Quaternion.Euler(0, 0, 0);

        Destroy(halfSword);
        popUpPointer.GetComponent<BoxCollider>().enabled = false;
    }

    private void Pull()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<RandomSound>().PlayRandomSound();
        }
        counter += 1.5f;
        playerRig.GetComponent<Animator>().SetBool("Pulling", true);
    }
}
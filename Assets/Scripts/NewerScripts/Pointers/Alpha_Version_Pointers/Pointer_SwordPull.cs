using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_SwordPull : Pointer_Base
{
    public GameObject goldSword;
    [HideInInspector]
    public GameObject goldSwordClone;
    public GameObject redHead;
    [HideInInspector]
    private GameObject redHeadClone;
    public GameObject angryMeter;

    public GameObject halfSword;

    [SerializeField]
    private GameObject _explosionObject;

    private float _beginValue = 0;
    private float _pullCounter = 3f;
    private float _previousCounter = 3f;

    private bool _pullingSword = false;
    private bool _walking = false;
    private bool _start = true;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _sounds = new AudioClip[0];


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }



    private void Update()
    {
        if (_walking)
        {
            if (PlayerControls.curSpeed < 0.01f)
            {
                StartCoroutine(GrabSword());
                _walking = false;
            }
        }

        if (_pullingSword)
        {
            if (PlayerRefs.mouseControls)
            {
                if (Input.GetMouseButton(0))
                {
                    if (_start)
                    {
                        _beginValue = Input.mousePosition.y;
                        _start = false;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (Input.mousePosition.y > _beginValue + 100)
                    {
                        Pull();
                        _beginValue = 0;
                    }
                    _start = true;
                }
            }
            else
            {
                if (Input.touchCount == 1)
                {
                    if (_start)
                    {
                        _beginValue = Input.GetTouch(0).position.y;
                        _start = false;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        if (Input.GetTouch(0).position.y > _beginValue + 100)
                        {
                            Pull();
                            _beginValue = 0;
                        }
                    }
                }
                else if (Input.touchCount == 0)
                {
                    _start = true;
                }
            }


            if (_pullCounter > 10)
            {
                StartCoroutine(BlowUp());
            }
            if (_pullCounter < 0.1f) // failure of pickin it up
            {
                ReleaseSword();

                // re-enable the pointer
                InteractibleScript.ShowPointerBehaviour();
            }

            TurnHeadRed();

            if (angryMeter)
            {
                angryMeter.transform.GetChild(1).transform.localScale = new Vector3(1, 1, _pullCounter / 10);
                angryMeter.transform.GetChild(1).transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.Lerp(new Color(1f, 0f, 0f, 1f), new Color(0.3f, 1f, 0f, 1f), _pullCounter / 10);
            }
        }
    }

    public override void PlayEvent()
    {
        base.PlayEvent();

        PullSword();
    }


    public void PullSword()
    {
        PlayerControls.walkingEnabled = false;

        StartCoroutine(GrabSword());
    }



    IEnumerator GrabSword()
    {
        PlayerRefs.transform.localScale = new Vector3(-6, 6, 6);
        PlayerRefs.GetComponent<Animator>().SetTrigger("HangOn");

        yield return new WaitForSeconds(0.3f);

        PlayerRefs.GetComponent<Animator>().SetTrigger("Grab");

        // Red Head
        redHeadClone = Instantiate(redHead, PlayerRefs.playerHead.transform.position, Quaternion.identity);
        redHeadClone.transform.parent = PlayerRefs.playerHead.transform;
        redHeadClone.transform.localPosition = new Vector3(0.0186f, 0.0f, -0.0082f);
        redHeadClone.transform.localRotation = Quaternion.Euler(-90, 180, -105f);

        angryMeter.SetActive(true);
        _pullingSword = true;
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        while (_pullCounter > 0.1f)
        {
            yield return new WaitForSeconds(0.2f);
            _pullCounter -= 0.1f;

            _previousCounter = _pullCounter;
        }
    }

    private void ReleaseSword()
    {
        _pullingSword = false;
        angryMeter.SetActive(false);

        PlayerRefs.GetComponent<Animator>().SetTrigger("ToIdle");

        _pullCounter = 3f;
        _previousCounter = 3;

        StartCoroutine(walkingEnabled());
        Destroy(redHeadClone);
    }

    IEnumerator walkingEnabled()
    {
        yield return new WaitForSeconds(1.0f);
        PlayerControls.walkingEnabled = true;
    }

    private void TurnHeadRed()
    {
        if (_pullCounter > 3)
        {
            redHeadClone.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0, 1, (_pullCounter / 7) / 2));
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

        // stop playing the grunting sounds
        _audioSource.Stop();

        PlayerRefs.GetComponent<MixerManager>().OnlyFX(3.3f);
        PlayerRefs.SetSound(4);
        PlayerRefs.GetComponent<AudioSource>().Play();

        _explosionObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        InteractibleScript.UsedSuccesfully = true;

        PlayerRefs.hasGoldenSword = true;
    }

    private void TurnExploded()
    {
        foreach (ExplodeSkin component in PlayerRefs.GetComponentsInChildren<ExplodeSkin>())
        {
            component.TurnExploded();
        }
        PlayerRefs.exploded = true;
    }

    private void changeSword()
    {
        PlayerRefs.swordGeo.SetActive(false);

        goldSwordClone = Instantiate(goldSword, PlayerRefs.swordJoint.transform.position, Quaternion.identity);
        goldSwordClone.transform.parent = PlayerRefs.swordJoint.transform;
        goldSwordClone.transform.localScale = new Vector3(1, 1, 1);
        goldSwordClone.transform.localRotation = Quaternion.Euler(0, 0, 0);

        Destroy(halfSword);
    }

    private void Pull()
    {
        if (_audioSource.isPlaying == false)
        {
            _audioSource.clip = _sounds[Random.Range(0, _sounds.Length)];
            _audioSource.Play();
        }

        _pullCounter += 1.5f;
        PlayerRefs.GetComponent<Animator>().SetBool("Pulling", true);
    }
}

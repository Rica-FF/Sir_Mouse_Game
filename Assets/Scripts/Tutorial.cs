using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private LayerMask IgnoreMe;
    public GameObject playerRig;
    public GameObject tutorHand;
    public GameObject canvas;
    private GameObject camPivot;

    private int state = 0;
    private GameObject playerRigid;
    private Animator tutorialAnims;
    private bool accurateTouch = false;
    private Vector3 touchPosition;

    private bool targetFollowHand = false;
    private bool checkIfCornEquipped = false;
    private bool dropTheCorn = false;
    private bool pickUpAgain = false;
    private bool checkIfPopcornMade = false;

    void Start()
    {
        IgnoreMe = LayerMask.GetMask("UI");
        camPivot = Camera.main.gameObject.transform.parent.gameObject;
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        PopUpPointer.disableIrrelevantPointers = true;
        playerRigid = playerRig.transform.parent.gameObject;
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = false;
        tutorialAnims = GetComponent<Animator>();
        StartCoroutine(StartTutorial());
    }

    private void CheckPosition()
    {
        if (playerRig.GetComponent<PlayerReferences>().mouseControls)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray that represents finger press
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // Ray that represents finger press
        }

        RaycastHit hit; // Object hit by ray
    }

    void Update()
    {
        if (targetFollowHand)
        {
            playerRigid.GetComponent<PlayerTouchControls>().target.transform.position = tutorHand.transform.position;
        }

        if ( (Input.touchCount == 1 || Input.GetMouseButton(0)) && accurateTouch)
        {
            AccurateTouch(touchPosition);
        }

        // Pick up tutorial
        if (checkIfCornEquipped && playerRig.GetComponent<PlayerReferences>().attachedObject)
        {
            playerRigid.GetComponent<NavMeshAgent>().speed = 7;
            if (playerRig.GetComponent<PlayerReferences>().attachedObject.name == "Corn")
            {
                StartCoroutine(DropTheCorn());
                checkIfCornEquipped = false;
            }
        }
        else if (dropTheCorn && !playerRig.GetComponent<PlayerReferences>().attachedObject)
        {
            playerRigid.GetComponent<NavMeshAgent>().isStopped = true;
            tutorialAnims.SetTrigger("PickUpAgain");
            StartCoroutine(DelaySpeed());
            pickUpAgain = true;
            dropTheCorn = false;
        }
        else if (pickUpAgain && playerRig.GetComponent<PlayerReferences>().attachedObject)
        {
            playerRigid.GetComponent<NavMeshAgent>().isStopped = false;
            playerRigid.GetComponent<NavMeshAgent>().speed = 7;
            tutorialAnims.SetTrigger("PutInFire");
            checkIfPopcornMade = true;
            pickUpAgain = false;
        }
        else if (checkIfPopcornMade)
        {
            if (playerRig.GetComponent<PlayerReferences>().madePopcorn)
            {
                playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = false;
                StartCoroutine(GoToExit());
                checkIfPopcornMade = false;
            }
        }
    }

    IEnumerator GoToExit()
    {
        yield return new WaitForSeconds(2f);
        camPivot.GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(4.5f);
        tutorialAnims.SetTrigger("GoToExit");
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = true;
    }


    IEnumerator DelaySpeed()
    {
        float seconds = 2f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        playerRigid.GetComponent<NavMeshAgent>().speed = 7;
        playerRigid.GetComponent<PlayerTouchControls>().MoveTo(playerRigid.transform.position);
        playerRigid.GetComponent<NavMeshAgent>().isStopped = false;
    }


    IEnumerator StartTutorial()
    {
        yield return new WaitForSeconds(3.0f);

        transform.parent = null;
        transform.position = new Vector3(-30.33f, -1.83f, -7.46f);
        GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(1.45f);

        playerRigid.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(-25.09f, 0.02f, -14.56f));

        StartCoroutine(Target(new Vector3(-25.09f, 0.02f, -14.56f)));

        yield return new WaitForSeconds(3.0f);

        playerRigid.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(-30.34f, 0.27f, -10.31f));

        StartCoroutine(Target(new Vector3(-30.34f, 0.27f, -10.31f)));

        yield return new WaitForSeconds(0.2f);

        accurateTouch = true;
        touchPosition = new Vector3(-25.7f, 0.0f, -1.6f);
        
    }

    IEnumerator Target(Vector3 position)
    {
        yield return new WaitForSeconds(0.1f);

        playerRigid.GetComponent<PlayerTouchControls>().target.transform.position = position;
        playerRigid.GetComponent<PlayerTouchControls>().target.SetActive(true);
    }

    private void AccurateTouch(Vector3 position)
    {
        Ray ray;
        if (playerRig.GetComponent<PlayerReferences>().mouseControls)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray that represents finger press
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // Ray that represents finger press
        }

        RaycastHit hit; // Object hit by ray

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe))
        {
            if (hit.point.x > touchPosition.x - 2 && hit.point.x < touchPosition.z + 2 && hit.point.z > touchPosition.z - 2 && hit.point.z < touchPosition.z + 2)
            {
                tutorialAnims.SetTrigger("Off");
                playerRigid.GetComponent<PlayerTouchControls>().MoveTo(position);
                StartCoroutine(Target(position));
                if (state == 0)
                {
                    StartCoroutine(Walk_02());
                    touchPosition = new Vector3(-22.1f, 0.0f, -6.3f);
                    state = 1;
                    accurateTouch = false;
                }
                else if (state == 1)
                {
                    StartCoroutine(Walk_03());
                    state = 2;
                    accurateTouch = false;
                }
            }
        }
    }

    IEnumerator Walk_02()
    {
        yield return new WaitForSeconds(2.0f);

        tutorHand.SetActive(true);
        tutorialAnims.SetTrigger("Walk_02");


        yield return new WaitForSeconds(2.0f);

        accurateTouch = true;
    }

    IEnumerator Walk_03()
    {
        tutorialAnims.SetTrigger("Off");

        yield return new WaitForSeconds(2.0f);

        playerRigid.GetComponent<PlayerTouchControls>().Jump();

        yield return new WaitForSeconds(2.0f);

        tutorialAnims.SetTrigger("Walk_03");
        targetFollowHand = true;

        yield return new WaitForSeconds(1.0f);

        playerRigid.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(-25.6f, 0, -14));

        yield return new WaitForSeconds(0.1f);

        playerRigid.GetComponent<PlayerTouchControls>().target.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        playerRigid.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(-29.5f, 0, -14));

        yield return new WaitForSeconds(1.0f);

        targetFollowHand = false;
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = true;

        yield return new WaitForSeconds(0.5f);

        playerRig.transform.localScale = new Vector3(-6, 6, 6);

    }

    public void Walk_04()
    {
        StartCoroutine(moveTo());
    }

    IEnumerator moveTo()
    {               
        tutorialAnims.SetTrigger("Off");
        yield return new WaitForSeconds(0.5f);

        playerRigid.GetComponent<PlayerTouchControls>().Jump();
        
        yield return new WaitForSeconds(2f);
        tutorialAnims.SetTrigger("PointCorn");
        playerRigid.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(-23f, 0.02f, -9.67f));
        checkIfCornEquipped = true;

        yield return new WaitForSeconds(2f);
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = true;
        playerRigid.GetComponent<NavMeshAgent>().speed = 0;
    }

    IEnumerator DropTheCorn()
    {
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = false;

        float seconds = 2f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        playerRigid.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(-21f, 0.02f, -9f));


        seconds = 2f;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        tutorialAnims.SetTrigger("DropCorn");
        playerRigid.GetComponent<NavMeshAgent>().speed = 0;
        playerRigid.GetComponent<PlayerTouchControls>().walkingEnabled = true;
        dropTheCorn = true;
    }

    public void ActivateExitPointer()
    {
        camPivot.GetComponent<TurnOnPointer>().ActivatePointer();
    }
}

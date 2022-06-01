using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
//using UnityEngine.Touch;

public class PlayerTouchControls : MonoBehaviour
{
    private LayerMask IgnoreMe;
    public GameObject player;
    public GameObject dropPointer;
    NavMeshAgent agent;
    private Animator animator;

    private Rigidbody rigidbody;

    public GameObject target;

    private Vector3 previousPosition;
    [HideInInspector]
    public float curSpeed = 0;

    [SerializeField]
    private float cameraSpeed = 4f;
    public bool readyToWalk = true;

    [HideInInspector]
    public string AttachedObjectString;


    private TouchPhase touchPhase = TouchPhase.Ended;
    private bool doAction = true;

    [HideInInspector]
    public bool walkingEnabled = false;

    public bool shortTouch = false;

    public bool clickedOnPointer = false;

    public int clicked = 0;

    private float beginValue = 0;
    private bool start = true;
    private UnityEngine.Touch touch;
    private CharacterController controller;

    public bool zoom = false;


    ///////////////

    private PlayerReferences _playerRefs;
    public bool GettingToDestination;

    private AudioSource _audioSource;




    void Start()
    {
        IgnoreMe = LayerMask.GetMask("UI");
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        animator = player.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(GetPlayerReferences());
    }

    void Update()
    {
        // touchCount 0 -> 0 fingers  // touchCount 1 -> 1 finger  // touchCount 2 -> 2 fingers

        // Controls //
        if(Input.touchCount == 0 && !clickedOnPointer) 
        {
            StartCoroutine(SetReadyToWalk());
            start = true;
        }
        else if (Input.touchCount == 1 && readyToWalk && walkingEnabled) 
        {
            touch = Input.GetTouch(0);
            StartCoroutine(SingleFinger(Input.GetTouch(0).position));
        }
        else if (Input.touchCount == 2 && zoom)
        {
            StartCoroutine(ZoomDetection());
        }

        MovementLogic();
        ZoomLogic();

        SetRunAimation();
    }

    private void ZoomLogic()
    {
        //if (Camera.main.orthographicSize > 5 && Camera.main.orthographicSize < 12)
        //{
        //    Camera.main.orthographicSize += Input.mouseScrollDelta.y / 10;
        //}
        //else if (Camera.main.orthographicSize < 5)
        //{
        //    Camera.main.orthographicSize = 5f;
        //}
        //else if (Camera.main.orthographicSize > 12)
        //{
        //    Camera.main.orthographicSize = 11.5f;
        //}
    }
    private void MovementLogic()
    {
        if (!Input.GetMouseButton(0) && !clickedOnPointer) // if not left clicked && not pointerClicked
        {
            StartCoroutine(SetReadyToWalk());
            start = true;
        }
        else if (Input.GetMouseButton(0) && readyToWalk && walkingEnabled) // if clicked && rdy to walk && walking enabled
        {
            StartCoroutine(SingleMouseClick(Input.mousePosition));
        }
    }


    //IEnumerator SingleMouseClick(Vector2 position)
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(position); // Ray that represents finger press

    //    yield return new WaitForSeconds(0.1f);


    //    RaycastHit hit; // Object hit by ray

    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        if (hit.collider.tag == "Player" && player.GetComponent<PlayerReferences>().attachedObject)
    //        {
    //            player.GetComponent<PlayerReferences>().DropObject();
    //        }

    //        if (hit.collider.tag == "Pointer" && !clickedOnPointer) // If pressed on a pointer-object, do action
    //        {
    //            clickedOnPointer = true;
    //            shortTouch = false;
    //            StartCoroutine(SetClickedOnPointerBool());

    //            yield return new WaitForSeconds(0.1f);

    //            hit.collider.gameObject.GetComponent<DoAction>().DoTheAction();

    //            if (hit.collider.gameObject.GetComponent<DoAction>().specificDestination)
    //            {
    //                agent.SetDestination(hit.collider.gameObject.GetComponent<DoAction>().destination);
    //            }

    //            yield return new WaitForSeconds(0.5f);

    //            if (hit.collider.gameObject.transform.parent.GetComponent<PopUpPointer>())
    //            {
    //                hit.collider.gameObject.transform.parent.GetComponent<PopUpPointer>().DisablePointer();
    //            }
    //            else if (hit.collider.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>())
    //            {
    //                hit.collider.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>().DisablePointer();
    //            }
    //            else if (hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>())
    //            {
    //                hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>().DisablePointer();
    //            }

    //        }
    //        else if (hit.collider.tag == "Soil" && player.GetComponent<PlayerReferences>().attachedObject)
    //        {
    //            if (hit.collider.GetComponent<Soil>().playerInArea && (player.GetComponent<PlayerReferences>().attachedObject.name == "Corn" || player.GetComponent<PlayerReferences>().attachedObject.name == "Bucket" && player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Bucket>().isFilled))
    //            {
    //                hit.collider.GetComponent<Soil>().UseSoil();
    //                DoAction("");
    //            }
    //            else
    //            {
    //                target.SetActive(true);
    //                agent.SetDestination(hit.point);
    //                target.transform.position = hit.point;
    //            }
    //        }
    //        else if (!clickedOnPointer && hit.collider.tag != "Player") // If not pressed on a pointer, move to pointed location
    //        {
    //            target.SetActive(true);
    //            agent.SetDestination(hit.point);
    //            target.transform.position = hit.point;
    //        }
    //    }

    //}

    IEnumerator SingleMouseClick(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position); // Ray that represents finger press

        yield return new WaitForSeconds(0.1f);

        RaycastHit hit; // Object hit by ray

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (hit.collider.tag == "Player" && player.GetComponent<PlayerReferences>().attachedObject) // PLAYER CLICK
            {                        
                DropPickUpWrap();
            }

            if (hit.collider.gameObject.CompareTag("Pointer") && !clickedOnPointer) // POINTER ACTIVATE
            {
                var pointer = hit.collider.GetComponentInParent<Pointer_Base>();
                var InteractibleScript = pointer.GetComponentInParent<Interactible_Base>();
                float distance = 0f;
                float travelTime = 0f;

                // boolean setters
                clickedOnPointer = true;
                shortTouch = false;
                StartCoroutine(SetClickedOnPointerBool());

                // set destination if it needs one
                if (pointer.RequiresDestination == true)
                {
                    GettingToDestination = true;
                    agent.SetDestination(pointer.DestinationSpot.position);
                    distance = Vector3.Distance(transform.position, pointer.DestinationSpot.position);
                }

                travelTime = distance;

                // calculate wait for seconds on the distance needed to walk
                yield return new WaitForSeconds(travelTime / 4f);

                // this is getting overwritten by movement (semi fixed)
                switch (pointer.OrientationPlayer)
                {
                    case OrientationType.None:
                        break;
                    case OrientationType.Right:
                        _playerRefs.gameObject.transform.localScale = new Vector3(-6, _playerRefs.gameObject.transform.localScale.y, _playerRefs.gameObject.transform.localScale.z);
                        break;
                    case OrientationType.Left:
                        _playerRefs.gameObject.transform.localScale = new Vector3(6, _playerRefs.gameObject.transform.localScale.y, _playerRefs.gameObject.transform.localScale.z);
                        break;
                }


                yield return new WaitForSeconds(0.2f);

                GettingToDestination = false;
                pointer.ActivateInteractibleAction();

                // turns the pointer off
                yield return new WaitForSeconds(0.5f);
            
                if (InteractibleScript.PointerStaysActiveAfterUse == false)
                {
                    pointer.GetComponentInParent<Interactible_Base>().HidePointerBehaviour();
                }    
                
            }
            else if (hit.collider.gameObject.CompareTag("PointerNext") && !clickedOnPointer)  // SWAP POINTER
            {
                var pointerLord = hit.collider.GetComponentInParent<Pointer_Lord>();

                // boolean setters
                clickedOnPointer = true;
                shortTouch = false;
                StartCoroutine(SetClickedOnPointerBool());

                // swap the pointer
                pointerLord.SwapActivePointer();

                // turns the pointer off (maybe remove it from the list too)      
                //pointer.GetComponentInParent<Interactible_Base>().HidePointerBehaviour();               

            }
            // If not pressed on a pointer, move to pointed location
            else if (!clickedOnPointer && hit.collider.tag != "Player")
            {
                target.SetActive(true);
                agent.SetDestination(hit.point);
                target.transform.position = hit.point;
            }
        }

    }



    IEnumerator SingleFinger(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position); // Ray that represents finger press

        if (Input.GetTouch(0).phase == touchPhase)
        {
            shortTouch = true;
        }
        else
        {
            shortTouch = false;
        }
         
        
        // Short delay to avoid walking while zooming
        yield return new WaitForSeconds(0.1f);


        // Second check to avoid walking while zooming
        if (Input.touchCount != 2) 
        {
            RaycastHit hit; // Object hit by ray

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreMe) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider.tag == "Player" && player.GetComponent<PlayerReferences>().attachedObject)
                {
                    if (start)
                    {
                        beginValue = touch.position.y;
                        start = false;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (touch.position.y > beginValue - 100)
                        {
                            player.GetComponent<PlayerReferences>().DropObject();
                            beginValue = 0;
                        }
                    }
                }

                if (hit.collider.tag == "Pointer" && shortTouch) // If pressed on a pointer-object, do action
                {
                    clickedOnPointer = true;
                    shortTouch = false;
                    StartCoroutine(SetClickedOnPointerBool());
    
                    yield return new WaitForSeconds(0.1f);


                    hit.collider.gameObject.GetComponent<DoAction>().DoTheAction();

                    if(hit.collider.gameObject.GetComponent<DoAction>().specificDestination)
                    {
                        agent.SetDestination(hit.collider.gameObject.GetComponent<DoAction>().destination);
                    }
                    
                    yield return new WaitForSeconds(0.5f);

                    if (hit.collider.gameObject.transform.parent.GetComponent<PopUpPointer>())
                    {
                        hit.collider.gameObject.transform.parent.GetComponent<PopUpPointer>().DisablePointer();
                    }
                    else if(hit.collider.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>())
                    {
                        hit.collider.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>().DisablePointer();
                    }
                    else if(hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>())
                    {
                        hit.collider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.GetComponent<PopUpPointer>().DisablePointer();
                    }
                    
                }
                else if(hit.collider.tag == "Soil" && player.GetComponent<PlayerReferences>().attachedObject)
                {
                    if (hit.collider.GetComponent<Soil>().playerInArea && (player.GetComponent<PlayerReferences>().attachedObject.name == "Corn" || player.GetComponent<PlayerReferences>().attachedObject.name == "Bucket" && player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Bucket>().isFilled))
                    {
                        hit.collider.GetComponent<Soil>().UseSoil();
                        DoAction("");
                    }
                    else
                    {
                        target.SetActive(true);
                        agent.SetDestination(hit.point);
                        target.transform.position = hit.point;
                    }
                }
                else if(!clickedOnPointer && hit.collider.tag != "Player") // If not pressed on a pointer, move to pointed location
                {
                    target.SetActive(true);
                    agent.SetDestination(hit.point);
                    target.transform.position = hit.point;

                    Debug.Log("moving shmoving");
                }
            }
        }
    }
    IEnumerator ZoomDetection()
    {
        readyToWalk = false;
        float previousDistance = 0f, distance = 0f;
        while (true)
        {
            if (Input.touchCount == 2)
            {
                distance = Vector2.Distance(Input.GetTouch(0).position,
                        Input.GetTouch(1).position);
            }
            // Detection
            // Zoom out
            if (distance > previousDistance && Camera.main.orthographicSize > 5)
            {
                float targetSize = Camera.main.orthographicSize;
                targetSize--;
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,
                                                        targetSize,
                                                        Time.deltaTime * cameraSpeed);
            }
            // Zoom in
            else if (distance < previousDistance && Camera.main.orthographicSize < 12)
            {
                float targetSize = Camera.main.orthographicSize;
                targetSize++;
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,
                                                        targetSize,
                                                        Time.deltaTime * cameraSpeed);
            }
            // Keep track of previous distance for next loop
            previousDistance = distance;
            yield return null;
        }
    }



    IEnumerator SetReadyToWalk()
    {
        yield return new WaitForSeconds(0.1f);
        readyToWalk = true;
    }
    IEnumerator SetClickedOnPointerBool()
    {
        yield return new WaitForSeconds(1.0f);

        clickedOnPointer = false;
    }
    IEnumerator JumpChar()
    {
        yield return new WaitForSeconds(1.1f);
        agent.enabled = true;
        controller.enabled = true;
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = true;
        player.GetComponent<Animator>().SetBool("InAir", false);
    }
    IEnumerator GetPlayerReferences()
    {
        yield return new WaitForSeconds(1f);

        _playerRefs = GetComponentInChildren<PlayerReferences>();
    }
    IEnumerator SetActionBool()
    {
        yield return new WaitForSeconds(3f);

        doAction = true;
    }



    public void EndTutorial()
    {
        GetComponent<NavMeshAgent>().speed = 7;
    }

    // only used in tutorial and bucket
    public void MoveTo(Vector3 position) 
    {
        agent.SetDestination(position);
    }


    public void Jump()
    {
        agent.enabled = false;
        controller.enabled = false;
        rigidbody.isKinematic = false;
        rigidbody.detectCollisions = false;
        rigidbody.AddRelativeForce(new Vector3(0, 20, 0), ForceMode.Impulse);
        player.GetComponent<Animator>().SetBool("InAir", true);

        StartCoroutine(JumpChar());
    }
    private void SetRunAimation()
    {
        // Calculate moving speed
        if (previousPosition != Vector3.zero)
        {
            Vector3 curMove = transform.position - previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;
        }

        // Trigger run animation based on speed
        if (curSpeed > 0)
        {
            animator.SetFloat("Speed", 1);

            if (GettingToDestination == false)
            {
                if (previousPosition.x < transform.position.x)
                {
                    player.GetComponent<Transform>().localScale = new Vector3(-6, 6, 6);
                }
                if (previousPosition.x > transform.position.x)
                {
                    player.GetComponent<Transform>().localScale = new Vector3(6, 6, 6);
                }
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
            target.SetActive(false);
        }

        previousPosition = transform.position;
    }
    public void Slash(int soundIndex)
    {
        _playerRefs.SetSound(soundIndex);
        // player.GetComponent<PlayerReferences>().SetSound(soundIndex);
        player.GetComponent<Animator>().SetTrigger("Slash");
    }
    public void DoAction(string setBool)
    {
        if (doAction)
        {
            doAction = false;
            if (player.GetComponent<PlayerReferences>().attachedObject)
            {
                GameObject attachedObject = player.GetComponent<PlayerReferences>().attachedObject;

                if (attachedObject.name == "Coin_C")
                {
                    attachedObject.GetComponent<Coin>().PressedThrow();
                }
                else if (attachedObject.name == "Corn")
                {
                    attachedObject.GetComponent<Corn>().PlantCorn();
                }
                else if (attachedObject.name == "Bucket")
                {
                    if (attachedObject.GetComponent<Bucket>().water.active)
                    {
                        player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Bucket>().PourWater();
                        if (setBool == "roses")
                        {
                            player.GetComponent<PlayerReferences>().rosesHaveWater = true;
                        }
                    }
                }
                else if (attachedObject.name == "Puzzle_piece")
                {
                    attachedObject.GetComponent<PuzzlePiece>().CompletePuzzle();
                }
            }

            StartCoroutine(SetActionBool());
        }
    }

    private void DropPickUpWrap()
    {
        // sparkles need to be re-invigorated when entering new area !!!!

        // get the pointer lord -> to get the sparkle objects -> disable all sparkle objects
        //foreach (var sparkle in _playerRefs.attachedObject.GetComponentInChildren<Pointer_Lord>().SparkleObjectsAll)
        //{
        //    sparkle.SetActive(false);
        //}

        // plays the item drop animation
        _playerRefs.GetComponent<Animator>().SetTrigger("DropCoin");
        _playerRefs.GetComponent<Animator>().SetLayerWeight(1, 0f);

        AttachedObjectString = "";
        _playerRefs.attachedObject = null;
        _playerRefs.PickedUpObject = PickupType.None;     
        

    }
}

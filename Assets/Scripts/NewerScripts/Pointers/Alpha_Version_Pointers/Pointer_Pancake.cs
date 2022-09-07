using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pointer_Pancake : Pointer_Base
{
    /*
     * sir mouse walks towards destination
     * 
     * walk to destination
     * unequip sword animation
     * animate the pan going to sir mouse hand.
     * parent pan to hand
     * 
     * -- get first pancake ready (animate milk being poured into pan)
     * -- swipe up to flip the pancake, pancake dissappears top of screen
     * -- pancakes start falling down 3 columns at intervals
     * -- swipe left/right to catch pancakes
     * -- when pancake is caught, play animation which swipes up the pancake of screen again, towards the table
     * 
     * 
     * -- when event is done, show animation of plate and pancakes landing on table (size correlates to succes of minigame)
     * 
     * -- sir mouse can now move again
     * 
     * 
     * 
     */

    [SerializeField]
    private Animator _pancakeAnimator;

    [SerializeField]
    private GameObject _initialPancakeSprite;

    public float ThresholdYDistanceCatchable, ThresholdYDistanceFailed;

    [SerializeField]
    private Transform _lane0, _lane1, _lane2;
    private Transform _lane0Player, _lane1Player, _lane2Player;
    public int CurrentActiveLane;

    private float _beginValue = 0;

    private bool _hasTouched;
    
    private bool _finishedStep1, _finishedStep2, _finishedStep3;

    private bool _movingLeft, _movingRight;

    private float _moveSpeed, _moveSpeedReducer;
    private bool _isSlowingDown;
    private float _timer;

    [SerializeField]
    private List<GameObject> _inactivePancakeObjects = new List<GameObject>();

    private List<Transform> _laneTransforms = new List<Transform>();

    private bool _initializedPancakes, _coroutineStarted;

    private bool _minigameFinished;



    private void Start()
    {
        _lane0Player = _lane0.GetChild(0);
        _lane1Player = _lane1.GetChild(0);
        _lane2Player = _lane2.GetChild(0);

        CurrentActiveLane = 1;

        _hasTouched = false;

        _moveSpeedReducer = 50f;
        _moveSpeed = 10f;

        _laneTransforms.Add(_lane0);
        _laneTransforms.Add(_lane1);
        _laneTransforms.Add(_lane2);

        // disable this script, so Update() does not run
        this.enabled = false;      
    }


    public override void PlayEvent()
    {
        base.PlayEvent();

        StartCoroutine(GetPancaking());
    }

    private IEnumerator GetPancaking()
    {
        // disable walking
        PlayerControls.walkingEnabled = false;
        PlayerControls.enabled = false;
        PlayerControls.GetComponent<NavMeshAgent>().enabled = false;
        PlayerControls.GetComponent<CharacterController>().enabled = false;

        // play sir mouse unequip animation (A.play(Unequip_Backpack))
        PlayerRefs.PlayerAnimator.Play("Unequip_Backpack");

        yield return new WaitForSeconds(0.5f);

        // access the animator that moves pan sprite, as well as the jug, play its animation
        _pancakeAnimator.Play("PourMilkInPan");

        // parent the pan to sir mouse hand (animation event)

        yield return new WaitForSeconds(1f);

        // once the animation is completely done...
        // show upwards arrow (starts minigame)
        StartMinigame();
    }



    private void StartMinigame()
    {
        this.enabled = true;     
    }
    private void EndMinigame()
    {
        this.enabled = false;
        PlayerControls.walkingEnabled = true;
    }



    private void Update()
    {
        // 1) swipe upwards requirement
        if (PlayerRefs.mouseControls)
        {
            // 2) translate pancake going up of screen
            // 2.5) perhaps distance camera a bit (is to see if current view is fine or not)
            if (_finishedStep1 == false)
            {
                PancakeFlipInput();
            }
            else if (_finishedStep2 == false)
            {
                PancakeToSpace();
            }
            else if (_finishedStep3 == false)
            {
                // 3) be able to swipe left or right to move sir mouse
                // 4) you cannot move out of bounds (only 3 lanes: Left = 0, Middle = 1, Right = 2)
                MovementInput();
                MovementLogic();

                // 5) spawn pancakes at intervals at the top. they have lane integer assigned to them
                if (_initializedPancakes == false)
                {
                    PancakeInitializing();
                }
                if (_coroutineStarted == false)
                {
                    StartCoroutine(PancakeSpawning());
                }

            }
            else
            {
                // 10) once all pancake are finished (bool), EndMinigame()

            }
        }



        // on PancakeMinigame script //

        // 6) said pancakes fall down (translate)
        // 7) when they fall down, if they're below a certain height, they can get caught by sir mouse (bool value on pancake)
        // 8) if mouse and pancake are in the same lane, and the pancake can be caught ----> CatchPancake()
        // 8.5)  - play pancake flip animation, pan animation, mouse animation
        //       - pancake flies up again (coded translate)
        // 9) if pancakes fall below another certain height ----> KillPancake()
        // 9.5) Pancakes have a bool IsFinished, once they die or get caught this bool is set to true      
    }





    private void PancakeFlipInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (_hasTouched == false)
            {
                _beginValue = Input.mousePosition.y;
                _hasTouched = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.y > _beginValue + 50)
            {
                _finishedStep1 = true;
                Debug.Log("Up swipe");
                _beginValue = 0;
            }
            _hasTouched = false;
        }
    }
    private void PancakeToSpace()
    {
        _initialPancakeSprite.transform.SetParent(null);
        _initialPancakeSprite.transform.Translate(Vector3.right * Time.deltaTime * 4, Space.Self);

        Debug.Log("pancake to spaaaaace");

        if (_initialPancakeSprite.transform.position.y >= 5)
        {
            _initialPancakeSprite.SetActive(false);
            _finishedStep2 = true;
        }
    }

    private void MovementInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (_hasTouched == false)
            {
                _beginValue = Input.mousePosition.x;
                _hasTouched = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.x > _beginValue + 100 && CurrentActiveLane <= 1 && _movingRight == false && _movingLeft == false)
            {
                //SirMouseRight();
                _moveSpeed = 10;
                _movingRight = true;

                Debug.Log("Right swipe");
                CurrentActiveLane += 1;
                _beginValue = 0;
            }
            else if (Input.mousePosition.x < _beginValue - 100 && CurrentActiveLane >= 1 && _movingRight == false && _movingLeft == false)
            {
                //SirMouseLeft();
                _moveSpeed = 10;
                _movingLeft = true;
                

                Debug.Log("Left swipe");
                CurrentActiveLane -= 1;
                _beginValue = 0;
            }
            _hasTouched = false;
        }
    }
    private void MovementLogic()
    {
        if (_movingLeft == true)
        {
            PlayerControls.transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime, Space.Self);
        }
        else if (_movingRight == true)
        {
            PlayerControls.transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime, Space.Self);
        }


        if (_movingLeft == true || _movingRight == true)
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.25f)
            {
                //_timer = 0;
                _isSlowingDown = true;
            }
        }

        if (_isSlowingDown == true)
        {
            _moveSpeed -= +_moveSpeedReducer * Time.deltaTime;
            if (_moveSpeed <= 0)
            {
                _moveSpeed = 0;
                _timer = 0;

                _movingLeft = false;
                _movingRight = false;
                _isSlowingDown = false;

                Debug.Log("DONEEE");
            }
        }
    }




    private IEnumerator PancakeSpawning()
    {
        _coroutineStarted = true;

        foreach (var pancake in _inactivePancakeObjects)
        {
            pancake.SetActive(true);

            yield return new WaitForSeconds(2);
        }


        yield return new WaitForSeconds(5);
        _finishedStep3 = true;
    }

    private void PancakeInitializing()
    {
        foreach (var pancake in _inactivePancakeObjects)
        {
            // assign random lane
            int randomLane = Random.Range(0, 3);
            var pancakeScript = pancake.GetComponent<PancakeMinigame>();
            pancakeScript.MyLaneValue = randomLane;

            // move it to correct spawn
            for (int i = 0; i < 3; i++)
            {
                if (pancakeScript.MyLaneValue == i)
                {
                    pancake.transform.position = _laneTransforms[i].position;
                }           
            }
        }

        _initializedPancakes = true;
    }
}

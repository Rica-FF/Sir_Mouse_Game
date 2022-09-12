using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pointer_Pancake : Pointer_Base
{
    /*
     * sir mouse walks towards destination
     * -- get first pancake ready (animate milk being poured into pan)
     * -- swipe up to flip the pancake, pancake dissappears top of screen
     * -- pancakes start falling down 3 columns at intervals
     * -- swipe left/right to catch pancakes
     * -- when pancake is caught, play animation which swipes up the pancake of screen again, towards the table
     * 
     * 
     * -- when event is done, show animation of plate and pancakes landing on table (size correlates to succes of minigame)
     */

    [SerializeField]
    private Animator _pancakeAnimator;

    [SerializeField]
    private GameObject _initialPancakeSprite;

    public float ThresholdYDistanceCatchable, ThresholdYDistanceFailed;

    private GameObject _playerVisuals;

    [SerializeField]
    private Transform _lane0, _lane1, _lane2;
    public int CurrentActiveLane;

    private float _beginValue = 0;

    private bool _hasTouched;
    
    private bool _finishedStep1, _finishedStep2, _finishedStep3, _finishedStep4;

    private bool _movingLeft, _movingRight;

    private float _moveSpeed, _moveSpeedReducer;
    private bool _isSlowingDown;
    private float _timer;

    [SerializeField]
    private List<GameObject> _inactivePancakeObjects = new List<GameObject>();

    private List<Transform> _laneTransforms = new List<Transform>();

    private bool _initializedPancakes, _coroutineStarted;

    private bool _foundEndLane;
    private bool _minigameFinished;
    private bool _startedEndCoroutine;
    private bool _playingFlipAnim;

    [SerializeField]
    private GameObject _panSpritesWrap, _pan;

    private GameObject _hand;



    private void Start()
    {
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
        PlayerControls.walkingEnabled = false;
        _playerVisuals = PlayerControls.GetComponentInChildren<Animator>().gameObject;

        // play sir mouse unequip animation (A.play(Unequip_Backpack))
        PlayerRefs.PlayerAnimator.Play("Unequip_Backpack");

        yield return new WaitForSeconds(0.5f);

        // access the animator that moves pan sprite, as well as the jug, play its animation
        _pancakeAnimator.enabled = true;
        _pancakeAnimator.Play("PourMilkInPan");

        // parent the pan to sir mouse hand (animation event)

        yield return new WaitForSeconds(_pancakeAnimator.GetCurrentAnimatorStateInfo(0).length + 0.2f);
        // once the animation is completely done...

        // parent the pan
        _pancakeAnimator.enabled = false;
        Debug.Log("disabled pancakes");
        ParentPanToHand();

        // show upwards arrow (starts minigame)
        StartMinigame();
    }
    private void ParentPanToHand()
    {
        _hand = PlayerRefs.playerHand;

        //_pan.transform.SetParent(null);
        _pan.transform.SetParent(_hand.transform);
        _pan.transform.localPosition = new Vector3(-0.04f, -0.022f, 0.008f);
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
            else if (_finishedStep4 == false)
            {
                //-- move back to center
                MoveToCenter();
            }
            else
            {
                // 10) once all pancakes are finished (bool), EndMinigame()
                if (_startedEndCoroutine == false)
                {
                    StartCoroutine(EndMinigame());
                    _startedEndCoroutine = true;
                }               
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






    private void StartMinigame()
    {
        this.enabled = true;
    }
    private IEnumerator EndMinigame()
    {
        yield return new WaitForSeconds(0);

        Debug.Log("ending");

        _pan.transform.SetParent(_panSpritesWrap.transform);
        _initialPancakeSprite.transform.SetParent(_pan.transform);


        _initialPancakeSprite.GetComponent<Animation>().enabled = false;
        _playingFlipAnim = false;
        _initialPancakeSprite.transform.position = Vector3.zero;

        Debug.Log(_initialPancakeSprite.transform.position);

        _initialPancakeSprite.SetActive(true);

        _pancakeAnimator.enabled = true;
        _pancakeAnimator.Play("ResetPan");

        // initial pancake sprite goes fucky here !!!

        PlayerControls.walkingEnabled = enabled;


        // reset bools
        _finishedStep1 = false;
        _finishedStep2 = false;
        _finishedStep3 = false;
        _finishedStep4 = false;
        _coroutineStarted = false;
        _startedEndCoroutine = false;
        _initializedPancakes = false;
        _foundEndLane = false;
        CurrentActiveLane = 1;
        //

        this.enabled = false;
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
                _beginValue = 0;
            }
            _hasTouched = false;
        }
    }
    private void PancakeToSpace()
    {
        _initialPancakeSprite.transform.SetParent(null);
        _initialPancakeSprite.transform.Translate(Vector3.right * Time.deltaTime * 4, Space.Self);

        if (_playingFlipAnim == false)
        {
            _initialPancakeSprite.GetComponent<Animation>().enabled = true;
            _initialPancakeSprite.GetComponent<Animation>().Play();
            _playingFlipAnim = true;
        }


        if (_initialPancakeSprite.transform.position.y >= 7)
        {
            _initialPancakeSprite.SetActive(false);
            _initialPancakeSprite.transform.SetParent(_pan.transform);
            _initialPancakeSprite.transform.localPosition = new Vector3(0.001f, 0, -0.038f);

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
                _moveSpeed = 10;
                _movingRight = true;

                CurrentActiveLane += 1;
                _beginValue = 0;
            }
            else if (Input.mousePosition.x < _beginValue - 100 && CurrentActiveLane >= 1 && _movingRight == false && _movingLeft == false)
            {
                _moveSpeed = 10;
                _movingLeft = true;
                
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
            //PlayerControls.transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime, Space.Self);
            _playerVisuals.transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime, Space.World);
        }
        else if (_movingRight == true)
        {
            //PlayerControls.transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime, Space.Self);
            _playerVisuals.transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime, Space.World);
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
            }
        }
    }
    private void MoveToCenter()
    {
        // if im left,  ------ move right
        // else if im right,-- move left
        if (_foundEndLane == false)
        {
            if (CurrentActiveLane == 0)
            {
                _movingRight = true;
            }
            else if (CurrentActiveLane == 2)
            {
                _movingLeft = true;
            }

            _foundEndLane = true;
            _moveSpeed = 10;
        }



        if (_movingLeft == true)
        {
            _playerVisuals.transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime, Space.World);
        }
        else if (_movingRight == true)
        {
            _playerVisuals.transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            _finishedStep4 = true;
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

                _finishedStep4 = true;
            }
        }
    }






    private void PancakeInitializing()
    {
        int lastLaneValue = 2;

        foreach (var pancake in _inactivePancakeObjects)
        {
            //// assign random lane
            //int randomLane = Random.Range(0, 3);
            //var pancakeScript = pancake.GetComponent<PancakeMinigame>();
            //pancakeScript.MyLaneValue = randomLane;

            // this block of code makes it so the pancake always spawns on lane next to the previous one (don't work oof)
            int CalculatedLaneValue = Random.Range(0, 3);
            while (CalculatedLaneValue == lastLaneValue)
            {
                if (lastLaneValue == 0)
                {
                    CalculatedLaneValue = 1;
                }
                else if (lastLaneValue == 2)
                {
                    CalculatedLaneValue = 1;
                }
                else
                {
                    CalculatedLaneValue = Random.Range(0, 3);
                }
            }
            var pancakeScript = pancake.GetComponent<PancakeMinigame>();
            pancakeScript.MyLaneValue = CalculatedLaneValue;

            pancakeScript.enabled = true;
            pancakeScript.Catchable = false;
            pancakeScript.IsFinished = false;
            pancakeScript.Success = false;

            lastLaneValue = CalculatedLaneValue;
            // -- end block -- //


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
    private IEnumerator PancakeSpawning()
    {
        _coroutineStarted = true;

        foreach (var pancake in _inactivePancakeObjects)
        {
            pancake.SetActive(true);

            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(5);

        _finishedStep3 = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float _currentActiveLane;

    private float _beginValue = 0;

    private bool _hasTouched;
    
    private bool _finishedStep1, _finishedStep2, _finishedStep3;




    private void Start()
    {
        _lane0Player = _lane0.GetChild(0);
        _lane1Player = _lane1.GetChild(0);
        _lane2Player = _lane2.GetChild(0);

        _currentActiveLane = 1;

        _hasTouched = false;

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
                MovementInput();
            }
        }
        // 2) translate pancake going up of screen
        // 2.5) perhaps distance camera a bit (is to see if current view is fine or not)

        // 3) be able to swipe left or right to move sir mouse
        // 4) you cannot move out of bounds (only 3 lanes: 0 = left, 1 = middle, 2 = right)

        // 5) spawn pancakes at intervals at the top. they have lane integer assigned to them

        // 6) said pancakes fall down (translate)

        // 7) when they fall down, if they're below a certain height, they can get caught by sir mouse (bool value on pancake)
        // 8) if mouse and pancake are in the same lane, and the pancake can be caught ----> CatchPancake()
        // 8.5)  - play pancake flip animation, pan animation, mouse animation
        //       - pancake flies up again (coded translate)

        // 9) if pancakes fall below another certain height ----> KillPancake()
        // 9.5) Pancakes have a bool IsFinished, once they die or get caught this bool is set to true

        // 10) once all pancake are finished (bool), EndMinigame()
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
        _initialPancakeSprite.transform.Translate(Vector3.up, Space.Self);

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
            if (Input.mousePosition.x > _beginValue + 100 && _currentActiveLane <= 1)
            {
                SirMouseRight();

                Debug.Log("Right swipe");
                _currentActiveLane += 1;
                _beginValue = 0;
            }
            else if (Input.mousePosition.x < _beginValue - 100 && _currentActiveLane >= 1)
            {
                SirMouseLeft();

                Debug.Log("Left swipe");
                _currentActiveLane -= 1;
                _beginValue = 0;
            }
            _hasTouched = false;
        }
    }




    private void SirMouseLeft()
    {
        // swiftly moves the player left
        PlayerControls.transform.Translate(Vector3.left * 50 * Time.deltaTime, Space.Self);
    }
    private void SirMouseRight()
    {
        // swiftly moves the player right
        PlayerControls.transform.Translate(Vector3.right * 50 * Time.deltaTime, Space.Self);
    }





}

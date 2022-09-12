using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeMinigame : MonoBehaviour
{
    [SerializeField]
    Pointer_Pancake _pointerPancake;

    public float MyFallingSpeed;

    public int MyLaneValue;
    public int PlayerLaneValue;

    public bool Catchable, Success, IsFinished;

    [SerializeField]
    private Animation _pancakeSpriteAnimation;

    void Update()
    {
        PancakeMovement();

        PancakeHeightChecks();
    }


    private void PancakeMovement()
    {
        if (Success == true)
        {
            // fly upwards
            transform.Translate(new Vector3(-0.2f,1,0) * 20 * Time.deltaTime, Space.Self);
        }
        else
        {
            // fall downwards
            transform.Translate(Vector3.down * MyFallingSpeed * Time.deltaTime, Space.Self);
        }
    }



    private void PancakeHeightChecks()
    {
        PlayerLaneValue = _pointerPancake.CurrentActiveLane;

        if (transform.position.y <= _pointerPancake.ThresholdYDistanceFailed) // if I hit the floor...
        {
            KillPancake();
        }
        else if (Catchable == true && MyLaneValue == PlayerLaneValue) // if i'm catch-able and the player is in my lane...
        {
            CaughtPancake();
        }
        else if (transform.position.y <= _pointerPancake.ThresholdYDistanceCatchable) // if I'm at the height where i can get caught...
        {
            Catchable = true;
        }

 
    }

    private void CaughtPancake()
    {
        Success = true;
        IsFinished = true;

        Debug.Log("GREAT SUCCESS");
        _pancakeSpriteAnimation.Play("PancakeSpin");

        transform.Translate(Vector3.up * 5 * Time.deltaTime, Space.Self);


        Invoke("DisablePancake", 2);
    }
    private void KillPancake()
    {
        IsFinished = true;
        Catchable = false;

        Debug.Log("whiffed");

        this.enabled = false;
    }

    private void DisablePancake()
    {
        this.enabled = false;

        this.gameObject.SetActive(false);
    }



}

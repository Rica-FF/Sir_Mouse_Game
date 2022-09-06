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

    void Update()
    {
        // fall downwards
        transform.Translate(Vector3.down * MyFallingSpeed * Time.deltaTime, Space.Self);


        if (transform.position.y <= _pointerPancake.ThresholdYDistanceFailed) // if I hit the floor...
        {
            IsFinished = true;
            Catchable = false;

            this.enabled = false;
        }
        else if (transform.position.y <= _pointerPancake.ThresholdYDistanceCatchable) // if I'm at the height where i can get caught...
        {
            Catchable = true;
        }


        
        if (Catchable == true && MyLaneValue == PlayerLaneValue) // if i'm catch-able and the player is in my lane...
        {
            Success = true;
            IsFinished = true;

            this.enabled = false;
        }
    }
}

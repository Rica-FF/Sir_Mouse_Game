using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinigameInputs 
{
    static Vector2 StartPosition;
    static Vector2 EndPosition;



    public static bool InputDetectionMouseUp()
    {
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    StartPosition = Input.GetTouch(0).position;
        //}
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //{
        //    EndPosition = Input.GetTouch(0).position;
        //}

        // mouse controls work only with bottom chunk

        if (Input.GetMouseButtonDown(0))
        {
            StartPosition = Input.mousePosition;

            return false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndPosition = Input.mousePosition;

            return true;
        }
        else
            return false;
    }

    public static bool InputDetectionMouseHold()
    {
        if (Input.GetMouseButton(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    /// ------------ Swipe methods --------
    
    public static bool CheckSwipeUp()
    {
        if (EndPosition.y > StartPosition.y)
        {
            StartPosition = Vector2.zero;
            EndPosition = Vector2.zero;

            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool CheckSwipeDown()
    {
        if (EndPosition.y < StartPosition.y)
        {
            StartPosition = Vector2.zero;
            EndPosition = Vector2.zero;

            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool CheckSwipeLeft()
    {
        if (EndPosition.x < StartPosition.x)
        {
            StartPosition = Vector2.zero;
            EndPosition = Vector2.zero;

            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool CheckSwipeRight()
    {
        if (EndPosition.x > StartPosition.x)
        {
            StartPosition = Vector2.zero;
            EndPosition = Vector2.zero;

            return true;
        }
        else
        {
            return false;
        }
    }






    public static bool SwipeHoldRight()
    {
        // check for input.buttonDown --> gives start position
        // check for input.button --> wait For x Distance to have been made between StartPosition and CurrentPosition...
        // once x distance has been made, check the direction of the vector(start,current) and classify it as left/right/up/down

        // if the chosen swipe direction is == required swipe we need...

        // wait for input.GetMousebuttonUp...
        // return true once it's up


        //if ()
        //{

        //}

        return true;
    }
}

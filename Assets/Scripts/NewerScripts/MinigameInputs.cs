using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinigameInputs 
{
    static Vector2 StartPosition;
    static Vector2 EndPosition;

    static Vector2 CurrentPosition;
    static Vector2 TempEndPosition;

    static float DistanceMoved;

    public static float SwipeAngle;
    static float SwipeDistanceRequired = 75;

    public static bool Swiped;
    public static bool SwipedLeft, SwipedRight, SwipedUp, SwipedDown;



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



    /// ------------ Swipe methods -------- DATED
    
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




    /////  RE-VAMPED SWIPING METHODS  /////



    // check for this boolean function if you're looking to swipe
    public static bool SwipeAlternate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPosition = Input.mousePosition;
            DistanceMoved = 0;

            return false;
        }
        if (Input.GetMouseButton(0) && Swiped == false)
        {
            CurrentPosition = Input.mousePosition;
            DistanceMoved = Vector2.Distance(CurrentPosition, StartPosition);

            // if distance moved is greater than x -> count as swipe
            if (DistanceMoved >= SwipeDistanceRequired)
            {
                var swipeDirectionNormal = (CurrentPosition - StartPosition).normalized;
                FindSwipeDirectionNormal(swipeDirectionNormal);

                Swiped = true;

                return true;
            }
            return false;
        }

        // if I let go -> re-enable swiping
        if (Input.GetMouseButtonUp(0))
        {
            Swiped = false;
        }

        return false;
    }




    static void FindSwipeDirectionNormal(Vector2 swipeDir)
    {
        ResetSwipeBools();

        // check horizontal VS vertical
        if (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y))
        {
            if (swipeDir.x > 0)
            {
                // right
                SwipedRight = true;
            }
            else
            {
                // left
                SwipedLeft = true;
            }
        }
        else
        {
            if (swipeDir.y > 0)
            {
                // up
                SwipedUp = true;
            }
            else
            {
                // down
                SwipedDown = true;
            }
        }
    }
    static void ResetSwipeBools()
    {
        SwipedUp = false;
        SwipedDown = false;
        SwipedLeft = false;
        SwipedRight = false;
    }


    public static void ShowRequiredSwipeDirection(List<GameObject> listOfSwipeVisuals, int listIndex)
    {
        listOfSwipeVisuals[listIndex].SetActive(true);

    }
    public static void HideSwipeDirections(List<GameObject> listOfSwipeVisuals)
    {
        for (int i = 0; i < listOfSwipeVisuals.Count; i++)
        {
            listOfSwipeVisuals[i].SetActive(false);
        }     
    }



    //static void FindSwipeDirection(float angle)
    //{
    //    Debug.Log("angle is " + SwipeAngle);


    //    if (angle < -157.5 || angle > 157.5)
    //    {
    //        // Left
    //        Debug.Log("Left");
    //    }
    //    else if (angle < -112.5)
    //    {
    //        // Down Left
    //    }
    //    else if (angle < -67.5)
    //    {
    //        // Down
    //        Debug.Log("Down");
    //    }
    //    else if (angle < -22.5)
    //    {
    //        // Down Right
    //    }
    //    else if (angle < 22.5)
    //    {
    //        // Right
    //        Debug.Log("Right");
    //    }
    //    else if (angle < 67.5)
    //    {
    //        // Up Right
    //    }
    //    else if (angle < 112.5)
    //    {
    //        // Up
    //        Debug.Log("Up");
    //    }
    //    else
    //    {
    //        // Up Left;
    //    }
    //}
}

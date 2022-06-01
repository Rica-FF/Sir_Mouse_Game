using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Chugger : MonoBehaviour
{
    float _speed;
    float _arcHeight;
    float _stepScale;
    float _progress;

    GameObject _objectToMove;

    Vector2 _startPos, _endPos;

    void Update()
    {
        // Increment our progress from 0 at the start, to 1 when we arrive.
        _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);

        // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
        float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);

        // Travel in a straight line from our start position to the target.        
        Vector3 nextPos = Vector3.Lerp(_startPos, _endPos, _progress);

        // Then add a vertical arc in excess of this.
        nextPos.y += parabola * _arcHeight;

        // Continue as before.
        _objectToMove.transform.position = nextPos;

        // I presume you disable/destroy the arrow in Arrived so it doesn't keep arriving.
        if (_progress == 1.0f)
        {
            // activate a method on pointer base that --- sets bool true, activates animation bag, destroy image object
            GetComponent<Pointer_Base>().ImageArrivedInBag();
        }
    }


    public void AllocateValues(float speed, float arcHeight, float stepScale, float progress, Vector2 startPos, Vector2 endPos, GameObject uICopy)
    {
        _speed = speed;
        _progress = progress;
        _stepScale = stepScale;
        _arcHeight = arcHeight;

        _startPos = startPos;
        _endPos = endPos;

        _objectToMove = uICopy;
    }
}

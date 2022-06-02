using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourMilk : MonoBehaviour
{
    public Pointer_Cooking pointer_Cooking;

    private bool pickedUp = false;
    private Ray _ray;
    private float _offset;
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            CalculateRay(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(0))
        {
            SingleFinger(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            pickedUp = false;
        }

        if (pickedUp)
        {
            _offset = Mathf.Clamp((_ray.origin.y - 7) / 2, -1, 2.7f);
            transform.position = _ray.origin + transform.forward * (12f + _offset);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _initialPosition, Time.deltaTime * 10);
        }
    }

    private void CalculateRay(Vector2 position)
    {
        _ray = Camera.main.ScreenPointToRay(position);
    }

    private void SingleFinger(Vector2 position)
    {
        RaycastHit hit; // Object hit by ray

        if (Physics.Raycast(_ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Milk")
            {
                pickedUp = true;
            }
        }
    }

    private void PouringMilk()
    {
        if (pointer_Cooking.dishState == 4)
        {
            if (pointer_Cooking.milkVolume < 1)
            {
                pointer_Cooking.milkVolume += 0.1f;
            }
            else
            {
                pointer_Cooking.ShowInstruction();
                pointer_Cooking.milkVolume = 0;
            }
        }
    }
}

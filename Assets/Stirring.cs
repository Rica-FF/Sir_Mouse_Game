using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stirring : MonoBehaviour
{
    // PUBLIC
    public Pointer_Cooking pointer_Cooking;

    // PRIVATE
    public float _stirringValue;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SingleFinger(Input.mousePosition);
        }
    }

    private void SingleFinger(Vector2 position)
    {
        RaycastHit hit; // Object hit by ray
        Ray _ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(_ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "StirringArea" && pointer_Cooking.dishState == 8)
            {
                if (_stirringValue < 1)
                {
                    _stirringValue += 0.005f;
                }
                else
                {
                    pointer_Cooking.ShowInstruction();
                    _stirringValue = 0;
                }
            }
        }
    }
}
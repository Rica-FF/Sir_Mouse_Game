using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpawnVegtables : MonoBehaviour
{
    public GameObject _tomatoeKitchen;

    private GameObject _tomatoeClone;
    private bool pickedUp = false;
    private Ray _ray;

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
            _tomatoeClone.GetComponent<Rigidbody>().useGravity = true;
            _tomatoeClone = null;
        }

        if (pickedUp)
        {
            _tomatoeClone.transform.position = _ray.origin;
        }
    }

    private void CalculateRay(Vector2 position)
    {
        _ray = Camera.main.ScreenPointToRay(position);
    }

    private void SingleFinger(Vector2 position)
    {
        if (_tomatoeClone)
        {
            _tomatoeClone.transform.position = _ray.origin;
        }

        RaycastHit hit; // Object hit by ray

        if (Physics.Raycast(_ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Tomatoes")
            {
                _tomatoeClone = Instantiate(_tomatoeKitchen, hit.collider.transform.GetChild(0).transform.position, Quaternion.Euler(30, 0, 0));
                pickedUp = true;
            }
        }
    }
}
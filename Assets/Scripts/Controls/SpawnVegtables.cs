using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpawnVegtables : MonoBehaviour
{
    public GameObject _tomatoeKitchen;
    public GameObject _appleKitchen;
    public GameObject _onionKitchen;

    private GameObject _tomatoeClone;
    private bool pickedUp = false;
    private Ray _ray;
    private float offset;

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

            if(_tomatoeClone)
            {
                _tomatoeClone.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                _tomatoeClone.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
            }
            _tomatoeClone = null;
        }

        if (pickedUp)
        {
            offset = Mathf.Clamp((_ray.origin.y - 7) / 2, -1, 2.7f);
            print(offset);
            _tomatoeClone.transform.position = _ray.origin + _tomatoeClone.transform.GetChild(0).forward * (12f + offset);
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
                _tomatoeClone = Instantiate(_tomatoeKitchen, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                pickedUp = true;
            }
            else if (hit.collider.tag == "Onions")
            {
                _tomatoeClone = Instantiate(_onionKitchen, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                pickedUp = true;
            }
            else if (hit.collider.tag == "Apples")
            {
                _tomatoeClone = Instantiate(_appleKitchen, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                pickedUp = true;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpawnVegetables : MonoBehaviour
{
    public GameObject tomatoeKitchen;
    public GameObject appleKitchen;
    public GameObject onionKitchen;
    public PlayerTouchControls playerControls;
    [SerializeField]
    public bool cookingGameActive = false;

    private GameObject _tomatoeClone;
    private bool _pickedUp = false;
    private Ray _ray;
    private float _offset;

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
            if (_pickedUp && !cookingGameActive)
            {
                playerControls.walkingEnabled = true;
            }

            _pickedUp = false;

            if(_tomatoeClone)
            {
                _tomatoeClone.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                _tomatoeClone.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
            }
            _tomatoeClone = null;
        }

        if (_pickedUp)
        {
            _offset = Mathf.Clamp((_ray.origin.y - 7) / 2, -1, 2.7f);
            _tomatoeClone.transform.position = _ray.origin + _tomatoeClone.transform.GetChild(0).forward * (12f + _offset);
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
                if (!cookingGameActive)
                {
                    playerControls.walkingEnabled = false;
                }
                _tomatoeClone = Instantiate(tomatoeKitchen, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                _pickedUp = true;
            }
            else if (hit.collider.tag == "Onions")
            {
                if (!cookingGameActive)
                {
                    playerControls.walkingEnabled = false;
                }
                _tomatoeClone = Instantiate(onionKitchen, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                _pickedUp = true;
            }
            else if (hit.collider.tag == "Apples")
            {
                if (!cookingGameActive)
                {
                    playerControls.walkingEnabled = false;
                }
                _tomatoeClone = Instantiate(appleKitchen, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                _pickedUp = true;
            }
        }
    }
}
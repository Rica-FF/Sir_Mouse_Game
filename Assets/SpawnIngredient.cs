using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIngredient : MonoBehaviour
{
    // PUBLIC
    [HideInInspector]
    public bool cookingGameActive = false;

    // PRIVATE
    [SerializeField]
    private PlayerTouchControls playerControls;
    private GameObject _IngredientClone;
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

            if (_IngredientClone)
            {
                _IngredientClone.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                _IngredientClone.transform.GetChild(0).GetComponent<SphereCollider>().enabled = true;
            }
            _IngredientClone = null;
        }

        if (_pickedUp)
        {
            _offset = Mathf.Clamp((_ray.origin.y - 7) / 2, -1, 2.7f);
            _IngredientClone.transform.position = _ray.origin + _IngredientClone.transform.GetChild(0).forward * (12f + _offset);
        }
    }

    private void CalculateRay(Vector2 position)
    {
        _ray = Camera.main.ScreenPointToRay(position);
    }

    private void SingleFinger(Vector2 position)
    {
        if (_IngredientClone)
        {
            _IngredientClone.transform.position = _ray.origin;
        }

        RaycastHit hit; // Object hit by ray

        if (Physics.Raycast(_ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.GetComponent<Ingredient>())
            {
                if (!cookingGameActive)
                {
                    playerControls.walkingEnabled = false;
                }
                _IngredientClone = Instantiate(hit.collider.GetComponent<Ingredient>().ingredientPrefab, hit.collider.transform.position, Quaternion.Euler(0, 0, 0));
                _pickedUp = true;
            }
        }
    }
}
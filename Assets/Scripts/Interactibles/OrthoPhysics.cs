using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoPhysics : MonoBehaviour
{
    [SerializeField]
    private GameObject _sprite; // sprite object to update

    void FixedUpdate()
    {
        //Debug.Log("111");

        // updates the sprite transform  with this rigidbody one
        _sprite.transform.position = transform.position;
        _sprite.transform.localEulerAngles = new Vector3(30, 0, transform.localEulerAngles.z);
    }
}

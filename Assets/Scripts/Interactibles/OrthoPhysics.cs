using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoPhysics : MonoBehaviour
{
    [SerializeField]
    private GameObject _sprite; // sprite object to update

    private void Start()
    {
        _sprite.transform.localEulerAngles = new Vector3(30, 0, 0);
    }

    void FixedUpdate()
    {
        //Debug.Log("111");

        // updates the sprite transform  with this rigidbody one
        _sprite.transform.position = transform.position;
        //_sprite.transform.localEulerAngles = new Vector3(30, 0, transform.localEulerAngles.z);

        _sprite.transform.localEulerAngles += new Vector3(0, 0, GetComponent<Rigidbody>().velocity.z*4);
        _sprite.transform.localEulerAngles += new Vector3(0, 0, GetComponent<Rigidbody>().velocity.x*4);
    }
}

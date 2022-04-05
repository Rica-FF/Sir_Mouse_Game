using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoPhysics : MonoBehaviour
{
    public GameObject geometry;
    void Update()
    {
        geometry.transform.position = transform.position;
        geometry.transform.localEulerAngles = new Vector3(30, 0, transform.localEulerAngles.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithCam : MonoBehaviour
{
    public Camera currentCam;
    private float scale;

    void Start()
    {
        currentCam = Camera.main;
    }

    void Update()
    {
        if(currentCam)
        {
            scale = currentCam.orthographicSize / 4.9f;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchOrMouse : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Player.GetComponent<PlayerReferences>().mouseControls = true;
        }
    }
}

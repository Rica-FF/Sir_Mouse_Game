using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapButton : MonoBehaviour
{
    public Sprite button1;
    public Sprite button2;

    private bool swap = true;

    public void SwappingButton()
    {
        if (swap)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = button2;
            swap = false;
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().sprite = button1;
            swap = true;
        }
    }
}

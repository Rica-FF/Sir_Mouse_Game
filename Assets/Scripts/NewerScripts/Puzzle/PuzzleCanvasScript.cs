using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCanvasScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _puzzleReference;
    


    public void ShowPuzzleReference()
    {
        if (_puzzleReference.activeSelf == true)
        {
            _puzzleReference.SetActive(false);
        }
        else
        {
            _puzzleReference.SetActive(true);
        }
    }
}

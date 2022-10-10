using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_PuzzleGame : Pointer_Base
{
    public GameObject PuzzleGameWrap;



    public override void PlayEvent()
    {
        base.PlayEvent();

        PlayerControls.walkingEnabled = false;
        PuzzleGameWrap.SetActive(true);
    }




    public void DisablePuzzleMinigame()
    {
        PuzzleGameWrap.SetActive(false);
        PlayerControls.walkingEnabled = true;
    }
}

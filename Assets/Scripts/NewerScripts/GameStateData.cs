using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateData 
{
    // set up arrays that memorize a number of things in a level
    // give the object that need to be memorized a script that checks in Start() for boolean values here

    public static bool[] OpenWorldBools = new bool[1];


    public static bool[] FindCorrectArray(string sceneName)
    {
        switch (sceneName)
        {
            case "OpenWorld_C_TEST":
                return OpenWorldBools;
                break;
            default:
                return null;
                break;
        }
    }

}

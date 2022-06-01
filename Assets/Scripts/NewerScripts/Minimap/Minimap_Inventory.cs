using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Minimap_Inventory
{
    public static bool[] ItemsInMinimap = new bool[3];


    // call this when entering a new area (maybe call it on pointers next level)
    public static void UnlockArea(int index)
    {
        ItemsInMinimap[index] = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack_Minimap_Manager : MonoBehaviour
{
    public GameObject Panel_Minimap, Panel_Backpack;

    public Backpack_Manager BackpackManagerScript;


    public void MiniMapIconClick()  // MAP CLICK
    {
        if (Panel_Minimap.activeSelf == true)
        {
            CloseMap();
        }
        else if (Panel_Backpack.activeSelf == true)
        {
            CloseBackpack();
            OpenMap();
        }
        else
        {
            OpenMap();
        }
    }

    public void BackpackIconClick() // BACKPACK CLICK
    {
        if (Panel_Backpack.activeSelf == true)
        {
            CloseBackpack();
        }
        else if (Panel_Minimap.activeSelf == true)
        {
            CloseMap();
            OpenBackpack();
        }
        else
        {
            OpenBackpack();
        }
    }






    private void OpenMap()
    {
        Panel_Minimap.SetActive(true);
    }
    private void CloseMap()
    {
        Panel_Minimap.SetActive(false);
    }
    private void OpenBackpack()
    {
        Panel_Backpack.SetActive(true);
        BackpackManagerScript.UpdateInventoryButtons();
    }
    public void CloseBackpack()
    {
        Panel_Backpack.SetActive(false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack_Minimap_Manager : MonoBehaviour
{
    public GameObject Panel_Minimap, Panel_Backpack;

    public Backpack_Manager BackpackManagerScript;

    private Animator _animator;

    // bools to alternate between 2 animations that essentially do the same thing (because animator struggles with playing the same animation if it has yet to end)
    private bool _swapBagAnim, _swapMinimapAnim;

    private const string _bagAnimation0 = "Popout_Backpack";
    private const string _bagAnimation1 = "Popout_Backpack_1";

    private const string _minimapAnimation0 = "Popout_Minimap";
    private const string _minimapAnimation1 = "Popout_Minimap_1";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    public void MiniMapIconClick()  // MAP CLICK
    {
        if (Panel_Minimap.activeSelf == true)
        {
            CloseMap();
            PlayMinimapButtonAnimation();
        }
        else if (Panel_Backpack.activeSelf == true)
        {
            CloseBackpack();
            OpenMap();
            PlayMinimapButtonAnimation();
        }
        else
        {
            OpenMap();
            PlayMinimapButtonAnimation(); 
        }

        // disable movement for 0.2 seconds;
    }

    public void BackpackIconClick() // BACKPACK CLICK
    {
        if (Panel_Backpack.activeSelf == true)
        {
            CloseBackpack();
            PlayBagButtonAnimation();
        }
        else if (Panel_Minimap.activeSelf == true)
        {
            CloseMap();
            OpenBackpack();
            PlayBagButtonAnimation();
        }
        else
        {
            OpenBackpack();
            PlayBagButtonAnimation();
        }

        // disable movemetn for 0.2 seconds;
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


    private void PlayBagButtonAnimation()
    {
        if (_swapBagAnim == false)
        {
            _animator.Play(_bagAnimation0);          
        }
        else
        {
            _animator.Play(_bagAnimation1);
        }
        // swap bool
        _swapBagAnim = !_swapBagAnim;
    }
    private void PlayMinimapButtonAnimation()
    {
        if (_swapMinimapAnim == false)
        {
            _animator.Play(_minimapAnimation0);
        }
        else
        {
            _animator.Play(_minimapAnimation1);
        }
        // swap bool
        _swapMinimapAnim = !_swapMinimapAnim;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Cooking : Pointer_Base
{
    // PUBLIC
    public GameObject chefHat;
    public GameObject kettle;
    public GameObject camDestination;
    public GameObject mainCamera;
    public GameObject scroll;
    public List<Sprite> vegtables = new List<Sprite>();

    private Animator _scrollAnim;

    private void Start()
    {
        _scrollAnim = scroll.GetComponent<Animator>();
    }

    public override void PlayEvent()
    {
        base.PlayEvent();
        StartCookingSession();
    }

    private void StartCookingSession()
    {
        chefHat.GetComponent<Animator>().SetTrigger("OnHead");
        kettle.GetComponent<Animator>().SetTrigger("OnTable");
        _scrollAnim.SetTrigger("EnterScreen");
        PlayerControls.walkingEnabled = false;
        mainCamera.GetComponent<FollowCam>().target = camDestination.transform;
        mainCamera.GetComponent<FollowCam>().borders = false;
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");
        StartCoroutine(FirstRecipe());
    }

    IEnumerator FirstRecipe()
    {
        yield return new WaitForSeconds(1);
        ShowRecipe();
    }

    public void AttachChefHat()
    {
        chefHat.GetComponent<Animator>().enabled = false;
        StartCoroutine(DelayDettachment());
    }

    IEnumerator DelayDettachment()
    {
        yield return new WaitForSeconds(0.0f);
        chefHat.transform.SetParent(null);
        chefHat.transform.SetParent(PlayerRefs.playerHead.transform);
    }

    public void ShowRecipe()
    {
        Sprite temp = vegtables[Random.Range(0, vegtables.Count)];
        scroll.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = temp;
        scroll.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = temp;
        scroll.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = temp;

        int tempIndex = Random.Range(0, 3);

        switch(tempIndex)
        {
            case 0:
            _scrollAnim.SetTrigger("1-Item");
                break;
            case 1:
                _scrollAnim.SetTrigger("2-Items");
                break;
            case 2:
                _scrollAnim.SetTrigger("3-Items");
                break;
        }
    }
}
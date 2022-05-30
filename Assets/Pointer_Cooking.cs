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


    public override void PlayEvent()
    {
        base.PlayEvent();
        StartCookingSession();
    }

    private void StartCookingSession()
    {
        chefHat.GetComponent<Animator>().SetTrigger("OnHead");
        kettle.GetComponent<Animator>().SetTrigger("OnTable");
        scroll.GetComponent<Animator>().SetTrigger("EnterScreen");
        PlayerControls.walkingEnabled = false;
        mainCamera.GetComponent<FollowCam>().target = camDestination.transform;
        mainCamera.GetComponent<FollowCam>().borders = false;
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomIn");
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
}
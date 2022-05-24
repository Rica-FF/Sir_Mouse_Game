using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bucket : MonoBehaviour
{
    public GameObject player;
    public GameObject water;
    public GameObject bucketBottom;
    public GameObject WaterFall;

    private GameObject playerRig;
    private bool pressedToPickUp = false;

    //[HideInInspector]
    public bool onSpot = false;
    [HideInInspector]
    public bool isFilled = false;

    private void Start()
    {
        StartCoroutine(SetPlayerRig());
    }

    IEnumerator SetPlayerRig()
    {
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        playerRig = player.transform.Find("RM_Player").gameObject;

    }

    private void Update()
    {
        if (pressedToPickUp)
        {
            if (player.GetComponent<PlayerTouchControls>().curSpeed < 0.01)
            {
                StartCoroutine(PickUpBucket());
                pressedToPickUp = false;
            }
        }
    }

    public void PressedPickUp()
    {
        PopUpPointer.disableIrrelevantPointers = true;
        GetComponent<NavMeshObstacle>().enabled = false;
        StartCoroutine(DelayPressedBool_PickUp());
    }

    IEnumerator DelayPressedBool_PickUp()
    {
        // Short delay
        for (float t = 0f; t < 0.1f; t += Time.deltaTime)
        {
            float normalizedTime = t / 0.1f;
            yield return null;
        }
        pressedToPickUp = true;
    }

    IEnumerator PickUpBucket()
    {
        playerRig.GetComponent<PlayerReferences>().SetSound(1);
        //PopUpPointer.disableIrrelevantPointers = true;      
        playerRig.GetComponent<PlayerReferences>().attachedObject = gameObject;
        playerRig.transform.localScale = new Vector3(6, 6, 6);
        playerRig.GetComponent<Animator>().SetTrigger("PickUpCoin");

        float seconds = 0.8f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        transform.parent = playerRig.GetComponent<PlayerReferences>().playerHand.transform;
        transform.localPosition = new Vector3(-0.031f, 0.005f, 0.01f);
        transform.localRotation = Quaternion.Euler(62, 33, -1.25f);

        seconds = 0.2f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        playerRig.GetComponent<Animator>().SetLayerWeight(1, 1f);
        //player.GetComponent<Collider>().GetComponentInChildren<Animator>().SetLayerWeight(1, 1f);

        //playerRig.transform.Find("Pointer").transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Drop()
    {
        PopUpPointer.disableIrrelevantPointers = false;
        if (onSpot)
        {
            StartCoroutine(MoveToSpot());
        }
        else
        {
            StartCoroutine(DetachBucket());
        }
        
        player.GetComponent<PlayerTouchControls>().AttachedObjectString = "";
        playerRig.GetComponent<PlayerReferences>().attachedObject = null;
    }

    IEnumerator MoveToSpot()
    {
        player.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(13.6f, 0.9f, 0.65f));
        float seconds = 0.8f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        playerRig.transform.localScale = new Vector3(6, 6, 6);
        StartCoroutine(DetachBucket());
    }

    IEnumerator DetachBucket()
    {
        playerRig.GetComponent<Animator>().SetTrigger("DropCoin");
        
        float seconds = 0.2f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        transform.parent = null;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z+0.6f);
        transform.localScale = new Vector3(1.045236f, 1.045236f, 1.045236f);
        playerRig.GetComponent<Animator>().SetLayerWeight(1, 0f);
        transform.GetComponent<SphereCollider>().enabled = true;
    }

    public void FillBucket()
    {
        if(onSpot)
        {
            StartCoroutine(FillWater());
            isFilled = true;
        }
        else if(playerRig.GetComponent<PlayerReferences>().exploded && playerRig.GetComponent<PlayerReferences>().onWaterSpot)
        {
            StartCoroutine(CleanSM());
        }
    }

    IEnumerator CleanSM()
    {
        yield return new WaitForSeconds(0);

        foreach (ExplodeSkin component in playerRig.GetComponentsInChildren<ExplodeSkin>())
        {
            component.TurnExploded();
        }
    }

    IEnumerator FillWater()
    {
        player.GetComponent<PlayerTouchControls>().MoveTo(new Vector3(11.88192f, 0.05001211f, -0.2772646f));

        float seconds = 1f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        playerRig.transform.localScale = new Vector3(-6, 6, 6);

        seconds = 1.5f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        water.SetActive(true);
    }

    public void PourWater()
    {
        GetComponent<Animator>().SetTrigger("Pour");
        water.SetActive(false);
        isFilled = false;
        WaterFall.GetComponent<Animator>().SetTrigger("Water");
        StartCoroutine(ResetBucket());
    }

    IEnumerator ResetBucket()
    {
        float seconds = 1f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        bucketBottom.transform.localRotation = Quaternion.Euler(30, 0, 0);
        //playerRig.transform.Find("Pointer").transform.GetChild(0).gameObject.SetActive(true);
    }
}

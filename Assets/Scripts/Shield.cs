using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject shieldGeo;
    public GameObject closet;

    public GameObject playerRigid;
    private GameObject playerRig;

    private GameObject shieldClone;

    public void SetShield()
    {
        playerRig = playerRigid.GetComponent<PlayerTouchControls>().player;

        if (closet.GetComponent<BedroomCloset>().spriteIndex == 0)
        {
            playerRig.GetComponent<PlayerReferences>().shieldGeo.SetActive(false);
            shieldClone = Instantiate(shieldGeo);
            playerRig.GetComponent<PlayerReferences>().newShield = shieldClone;
            shieldClone.transform.parent = playerRig.GetComponent<PlayerReferences>().shieldJoint.transform;
            shieldClone.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (closet.GetComponent<BedroomCloset>().spriteIndex == 3)
        {
            Destroy(playerRig.GetComponent<PlayerReferences>().newShield);
            playerRig.GetComponent<PlayerReferences>().shieldGeo.SetActive(true);
        }
        else
        {
            playerRig.GetComponent<PlayerReferences>().newShield.GetComponent<SpriteRenderer>().sprite = closet.GetComponent<BedroomCloset>().shields[closet.GetComponent<BedroomCloset>().spriteIndex];
        }

        if (playerRig.GetComponent<PlayerReferences>().shieldIndex < 3)
        {
            playerRig.GetComponent<PlayerReferences>().shieldIndex++;
        }
        else
        {
            playerRig.GetComponent<PlayerReferences>().shieldIndex = 0;
        }
    }
}

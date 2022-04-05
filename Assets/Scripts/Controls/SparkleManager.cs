using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleManager : MonoBehaviour
{
    private GameObject player;

    public string objectName;
    public GameObject[] sparkles = new GameObject[0];

    public string objectName2;
    public GameObject[] sparkles2 = new GameObject[0];

    public string objectName3;
    public GameObject[] sparkles3 = new GameObject[0];

    private void Start()
    {
        player = GetComponent<SetUpLevel>().player;
    }

    private void Update()
    {
        if (player.GetComponent<PlayerReferences>().attachedObject)
        {
            for (int i = 0; i < sparkles.Length; i++)
            {
                if (sparkles[i].GetComponent<Sparkle>().objectName == player.GetComponent<PlayerReferences>().attachedObject.name)
                {
                    if (player.GetComponent<PlayerReferences>().attachedObject.name == "Bucket")
                    {
                        if (sparkles[i].GetComponent<Sparkle>().waterIncluded == player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Bucket>().water.activeSelf)
                        {
                            sparkles[i].GetComponent<Sparkle>().sparkle.SetActive(true);
                        }
                        else
                        {
                            sparkles[i].GetComponent<Sparkle>().sparkle.SetActive(false);
                        }
                    }
                    else
                    {
                        sparkles[i].GetComponent<Sparkle>().sparkle.SetActive(true);
                    }
                }
            }

            for (int i = 0; i < sparkles2.Length; i++)
            {
                if (sparkles2[i].GetComponent<Sparkle>().objectName == player.GetComponent<PlayerReferences>().attachedObject.name)
                {
                    sparkles2[i].GetComponent<Sparkle>().sparkle.SetActive(true);
                }
            }

            for (int i = 0; i < sparkles3.Length; i++)
            {
                if (sparkles3[i].GetComponent<Sparkle>().objectName == player.GetComponent<PlayerReferences>().attachedObject.name)
                {
                    sparkles3[i].GetComponent<Sparkle>().sparkle.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < sparkles.Length; i++)
            {
                sparkles[i].GetComponent<Sparkle>().sparkle.SetActive(false);
            }

            for (int i = 0; i < sparkles2.Length; i++)
            {
                sparkles2[i].GetComponent<Sparkle>().sparkle.SetActive(false);
            }

            for (int i = 0; i < sparkles3.Length; i++)
            {
                sparkles3[i].GetComponent<Sparkle>().sparkle.SetActive(false);
            }
        }
    }

}

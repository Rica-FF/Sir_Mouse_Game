using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candles : MonoBehaviour
{
    public GameObject candles;
    public GameObject darkness;
    public Sprite darkBG;
    public GameObject background;
    public GameObject glowingBear;
    public GameObject key;
    public bool keyDiscovered = false;
    private bool suspicious = true;

    private Sprite BG;

    private void Start()
    {
        BG = background.GetComponent<SpriteRenderer>().sprite;
    }

    public void TurnOn_Off()
    {
        if (candles.activeSelf)
        {
            if (suspicious)
            {
                GetComponent<AudioSource>().Play();
                suspicious = false;
            }
            darkness.SetActive(true);
            candles.SetActive(false);
            background.GetComponent<SpriteRenderer>().sprite = darkBG;
            glowingBear.SetActive(true);
            key.SetActive(true);
        }
        else if (!candles.activeSelf)
        {
            darkness.SetActive(false);
            candles.SetActive(true);
            background.GetComponent<SpriteRenderer>().sprite = BG;
            glowingBear.SetActive(false);
            if (!keyDiscovered)
            {
                key.SetActive(false);
            }
        }
    }
}

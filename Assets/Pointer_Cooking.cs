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
    public List<Sprite> ingredients = new List<Sprite>();
    public Sprite milkVase;

    // PRIVATE
    private int _dishState = 0;
    private Animator _scrollAnim;
    private int _ingredientIndex;
    private int _amount;

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
        ShowInstruction();
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
    IEnumerator ResetScroll()
    {
        _scrollAnim.SetTrigger("Close");
        yield return new WaitForSeconds(1);
        ShowInstruction();
    }

    public void ShowInstruction()
    {
        if (_dishState < 3 || _dishState > 3 && _dishState < 6)
        {
            ShowIngredient();
        }
        else if (_dishState == 3)
        {
            scroll.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = milkVase;
            _scrollAnim.SetTrigger("1-Item");
        }

        _dishState++;
    }

    private void ShowIngredient()
    {
        _ingredientIndex = Random.Range(0, ingredients.Count);
        Sprite tempSprite = ingredients[_ingredientIndex];
        scroll.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = tempSprite;
        scroll.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = tempSprite;
        scroll.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = tempSprite;

        _amount = Random.Range(1, 4);

        switch(_amount)
        {
            case 1:
            _scrollAnim.SetTrigger("1-Item");
                break;
            case 2:
                _scrollAnim.SetTrigger("2-Items");
                break;
            case 3:
                _scrollAnim.SetTrigger("3-Items");
                break;
        }

    }

    public void CheckIngredients(KettleContainer kettleContainer)
    {

        int amountFound = 0;
        for (int i = 0; i < kettleContainer.contaminants.Count; i++)
        {
            switch (_ingredientIndex)
            {
                case 0:
                    if (kettleContainer.contaminants[i] == "Tom")
                    {
                        amountFound++;
                    }
                    break;
                case 1:
                    if (kettleContainer.contaminants[i] == "App")
                    {
                        amountFound++;
                    }
                    break;
                case 2:
                    if (kettleContainer.contaminants[i] == "Oni")
                    {
                        amountFound++;
                    }
                    break;
            }
            if (amountFound == _amount)
            {
                kettleContainer.contaminants.Clear();
                StartCoroutine(ResetScroll());
            }
        }
    }

}
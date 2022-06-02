using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Cooking : Pointer_Base
{
    // PUBLIC
    public int dishState = 0;
    public GameObject chefHat;
    public GameObject kettle;
    public GameObject mainCamera;
    public GameObject scroll;
    public List<Sprite> ingredients = new List<Sprite>();
    public Sprite milkVase;
    public float milkVolume;
    public Sprite stirring;
    public GameObject fireplace;
    public Transform destination2;
    public Sprite goldenChefHat;
    public SpawnIngredient vegetableSpawner;
    public GameObject camDestination;

    // PRIVATE
    private Animator _scrollAnim;
    private Animator _kettleAnim;
    private int _ingredientIndex;
    private int _amount;
    private Transform _playerTransform;
    private GameObject _cloneChefHat;
    private Vector3 _initialCamDestination;
    private int _previousIngredientIndex = -1;
    private int _amountWrong;

    private void Start()
    {
        _scrollAnim = scroll.GetComponent<Animator>();
        _kettleAnim = kettle.GetComponent<Animator>();
        _playerTransform = mainCamera.GetComponent<FollowCam>().target;
        _initialCamDestination = camDestination.transform.position;
    }

    public override void PlayEvent()
    {
        base.PlayEvent();
        StartCookingSession();
    }

    private void StartCookingSession()
    {
        vegetableSpawner.cookingGameActive = true;
        chefHat.GetComponent<Animator>().SetTrigger("OnHead");
        _kettleAnim.SetTrigger("OnTable");
        _scrollAnim.SetTrigger("EnterScreen");
        PlayerControls.walkingEnabled = false;
        camDestination.transform.position = _initialCamDestination;
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
        _cloneChefHat = Instantiate(chefHat, chefHat.transform.position, chefHat.transform.rotation);
        _cloneChefHat.GetComponent<Animator>().enabled = false;
        chefHat.SetActive(false);
        StartCoroutine(DelayDettachment());
    }

    IEnumerator DelayDettachment()
    {
        yield return new WaitForSeconds(0.0f);
        _cloneChefHat.transform.SetParent(null);
        _cloneChefHat.transform.SetParent(PlayerRefs.playerHead.transform);
    }
    IEnumerator ResetScroll()
    {
        _scrollAnim.SetTrigger("Close");
        yield return new WaitForSeconds(1);
        ShowInstruction();
    }

    public void ShowInstruction()
    {
        if (dishState < 3 || dishState > 3 && dishState < 7)
        {
            StartCoroutine(ShowIngredient());
        }
        else if (dishState == 3)
        {
            scroll.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = milkVase;
            _scrollAnim.SetTrigger("1-Item");
        }
        else if (dishState == 7)
        {
            scroll.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = stirring;
            _scrollAnim.SetTrigger("1-Item");
        }
        else if(dishState > 7)
        {
            _scrollAnim.SetTrigger("Hide");
            _kettleAnim.SetTrigger("OnStove");
            camDestination.transform.position = new Vector3(6f, _initialCamDestination.y, _initialCamDestination.z);
            fireplace.SetActive(true);
            PlayerControls.MoveTo(destination2.position);
            StartCoroutine(Result());
        }
        dishState++;
    }

    IEnumerator ShowIngredient()
    {
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

        yield return new WaitForSeconds(2);

        while (_ingredientIndex == _previousIngredientIndex)
        {
            _ingredientIndex = Random.Range(0, ingredients.Count);
        }
        _previousIngredientIndex = _ingredientIndex;
        Sprite tempSprite = ingredients[_ingredientIndex];
        scroll.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = tempSprite;
        scroll.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = tempSprite;
        scroll.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = tempSprite;
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
                    else
                    {
                        _amountWrong++;
                    }
                    break;
                case 1:
                    if (kettleContainer.contaminants[i] == "App")
                    {
                        amountFound++;
                    }
                    else
                    {
                        _amountWrong++;
                    }
                    break;
                case 2:
                    if (kettleContainer.contaminants[i] == "Oni")
                    {
                        amountFound++;
                    }
                    else
                    {
                        _amountWrong++;
                    }
                    break;
                case 3:
                    if (kettleContainer.contaminants[i] == "Pot")
                    {
                        amountFound++;
                    }
                    else
                    {
                        _amountWrong++;
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

    IEnumerator Result()
    {
        yield return new WaitForSeconds(2);

        if (_amountWrong > 4)
        {
            Destroy(_cloneChefHat);
        }
        else
        {
            _cloneChefHat.GetComponent<SpriteRenderer>().sprite = goldenChefHat;
        }
        mainCamera.GetComponent<FollowCam>().target = _playerTransform;
        mainCamera.GetComponent<FollowCam>().borders = true;
        mainCamera.GetComponent<Animator>().SetTrigger("ZoomOut");
        PlayerControls.walkingEnabled = true;
        vegetableSpawner.cookingGameActive = false;
        chefHat.SetActive(true);
        chefHat.GetComponent<Animator>().SetTrigger("Reset");
        dishState = 0;
        camDestination.transform.position = _initialCamDestination;
    }
}
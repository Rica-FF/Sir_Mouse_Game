using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpForest : MonoBehaviour
{
    public GameObject corn;
    public GameObject soil;
    private GameObject[] _cornClones = new GameObject[10];
    public List<GameObject> cornClones = new List<GameObject>();
    public GameObject goldenSword;
    public GameObject swordBoxCollider;

    void Start()
    {
        GameObject player = GetComponent<SetUpLevel>().player;

        
        for(int i = 0; i < player.GetComponent<PlayerReferences>().cornPositions.Count; i++)
        {
            _cornClones[i] = Instantiate(corn, player.GetComponent<PlayerReferences>().cornPositions[i], Quaternion.Euler(30, 0, -90));
            soil.GetComponent<Soil>().plantedCorn.Add(_cornClones[i]);
            soil.GetComponent<Soil>().plantedCorn[i].GetComponent<Corn>().growPhase = player.GetComponent<PlayerReferences>().cornGrowPhases[i];
            _cornClones[i].GetComponent<Corn>().player = GetComponent<SetUpLevel>().playerRigid;
            _cornClones[i].GetComponent<Corn>().CornAlreadyPlanted();
        }

        if (player.GetComponent<PlayerReferences>().hasGoldenSword)
        {
            goldenSword.SetActive(false);
            swordBoxCollider.GetComponent<BoxCollider>().enabled = false;
        }
    }
}

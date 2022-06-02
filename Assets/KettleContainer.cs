using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleContainer : MonoBehaviour
{
    public List<string> contaminants = new List<string>();
    public Pointer_Cooking pointer_Cooking;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Vegetable")
        {
            contaminants.Add(collider.transform.parent.name.Remove(3));
            pointer_Cooking.CheckIngredients(GetComponent<KettleContainer>());
            Destroy(collider.transform.parent.gameObject, 1);
        }
    }
}

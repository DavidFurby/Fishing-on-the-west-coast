using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] public Item item;

    protected void DisplayModel(GameObject modelPrefab)
    {
        Destroy(gameObject);

        // Instantiate the new model
        GameObject model = Instantiate(modelPrefab);
        // Set the position and rotation of the new model
        model.transform.position = transform.position;
        model.transform.rotation = transform.rotation;
        // Set the parent of the new model to be this game object
        model.transform.SetParent(transform);
    }
}
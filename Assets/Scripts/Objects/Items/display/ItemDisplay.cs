using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] public Item item;

protected void DisplayModel(GameObject modelPrefab)
{
    // Store the scale of the previous instance
    Vector3 previousScale = transform.localScale;

    // Instantiate the new model
    GameObject model = Instantiate(modelPrefab);
    // Set the position, rotation, and scale of the new model
    model.transform.position = transform.position;
    model.transform.rotation = transform.rotation;
    model.transform.localScale = previousScale;
    // Set the parent of the new model to be this game object
    model.transform.SetParent(transform);
}

}
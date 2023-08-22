using UnityEngine;
using UnityEngine.ProBuilder;

public class ItemDisplay : MonoBehaviour
{
    public Item item;
    

    protected void SetNewItemModel(Item newItem)
    {
        item = newItem;

        // Destroy the old model if it exists
        if (transform.childCount > 0)
        {
            Transform oldModelTransform = transform.GetChild(0);
            Vector3 oldModelLocalPosition = oldModelTransform.localPosition;
            Quaternion oldModelLocalRotation = oldModelTransform.localRotation;
            Vector3 oldModelLocalScale = oldModelTransform.localScale;

            Destroy(oldModelTransform.gameObject);

            // Instantiate the new model and set its local position, rotation, and scale to be equal to the old model's local position, rotation, and scale
            GameObject newModel = Instantiate(item.model);
            newModel.transform.SetParent(transform, false);
            newModel.transform.SetLocalPositionAndRotation(oldModelLocalPosition, oldModelLocalRotation);
            newModel.transform.localScale = oldModelLocalScale;
        }
        else
        {
            // Instantiate the new model and set it as a child of this gameObject
            GameObject newModel = Instantiate(item.model);
            newModel.transform.SetParent(transform, false);
        }
    }
}

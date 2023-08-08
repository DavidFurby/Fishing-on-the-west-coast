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
            Destroy(oldModelTransform.gameObject);

            // Instantiate the new model and set its local position to be equal to the old model's local position
            GameObject newModel = Instantiate(item.model);
            newModel.transform.SetParent(transform, false);
            newModel.transform.SetLocalPositionAndRotation(oldModelLocalPosition, oldModelTransform.localRotation);
            newModel.transform.localScale = oldModelTransform.localScale;
        }
        else
        {
            // Instantiate the new model and set it as a child of this gameObject
            GameObject newModel = Instantiate(item.model);
            newModel.transform.SetParent(transform, false);
        }
    }
}

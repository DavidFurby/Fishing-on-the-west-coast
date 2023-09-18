using UnityEngine;
using UnityEngine.ProBuilder;

public class ItemDisplay : MonoBehaviour
{
    public Item item;


    protected void SetNewItemModel(Item newItem)
    {
        item = newItem;
        Transform oldModel = null;
        for(int i = 0; i < transform.childCount; i++) {
            if(transform.GetChild(i).GetComponent<MeshRenderer>()) {
                oldModel = transform.GetChild(i);
            }
        }
        if (transform.childCount > 0)
        {
            Vector3 oldModelLocalPosition = oldModel.localPosition;
            Quaternion oldModelLocalRotation = oldModel.localRotation;
            Vector3 oldModelLocalScale = oldModel.localScale;

            Destroy(oldModel.gameObject);

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

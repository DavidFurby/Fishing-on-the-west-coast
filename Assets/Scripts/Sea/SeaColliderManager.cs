using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class SeaColliderController : MonoBehaviour
{
    private BoxCollider SeaGroupCollider { get; set; }
    private BoxCollider SeaFloorGroupCollider { get; set; }



    public void SetBoxCollider(List<GameObject> seaTiles, Vector3 seaSize)
    {
        if (seaTiles.Count == 0)
        {
            return;
        }

        var seaBounds = CalculateBounds(seaTiles, "SeaFloor");
        var (xMin, xMax, zMin, zMax, seaFloorYPosition) = CalculateBounds(seaTiles, "SeaFloor");

        SeaGroupCollider = CreateOrUpdateCollider(SeaGroupCollider, (seaBounds.xMin, seaBounds.xMax, seaBounds.zMin, seaBounds.zMax), "SeaGroupCollider", typeof(WaterCollision), seaFloorYPosition / 2, seaSize.y);
        SeaFloorGroupCollider = CreateOrUpdateCollider(SeaFloorGroupCollider, (xMin, xMax, zMin, zMax), "SeaFloorGroupCollider", typeof(SeaFloorCollision), seaFloorYPosition);
    }

    private (float xMin, float xMax, float zMin, float zMax, float seaFloorYPosition) CalculateBounds(List<GameObject> seaTiles, string seaObjectType)
    {
        float xMin = Mathf.Infinity;
        float xMax = -Mathf.Infinity;
        float zMin = Mathf.Infinity;
        float zMax = -Mathf.Infinity;
        float seaFloorYPosition = 0;

        foreach (GameObject seaTile in seaTiles)
        {
            Transform seaTileObject = seaTile.GetComponentsInChildren<Transform>().First(childTransform => childTransform.CompareTag(seaObjectType));
            Bounds bounds = seaTileObject.GetComponent<Renderer>().bounds;
            xMin = Mathf.Min(xMin, bounds.min.x);
            xMax = Mathf.Max(xMax, bounds.max.x);
            zMin = Mathf.Min(zMin, bounds.min.z);
            zMax = Mathf.Max(zMax, bounds.max.z);
            seaFloorYPosition = bounds.center.y;
        }

        return (xMin, xMax, zMin, zMax, seaFloorYPosition);
    }

    private BoxCollider CreateOrUpdateCollider(BoxCollider boxCollider, (float xMin, float xMax, float zMin, float zMax) colliderBounds, string colliderName, Type collisionComponentType, float yPosition = 0, float ySize = 0)
    {
        Vector3 colliderCenter = new((colliderBounds.xMin + colliderBounds.xMax) / 2, yPosition == 0 ? transform.position.y : yPosition, (colliderBounds.zMin + colliderBounds.zMax) / 2);
        Vector3 colliderSize = new(colliderBounds.xMax - colliderBounds.xMin, ySize, colliderBounds.zMax - colliderBounds.zMin);

        if (boxCollider == null)
        {
            GameObject colliderObject = new(colliderName);
            colliderObject.transform.SetParent(transform);
            boxCollider = colliderObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            colliderObject.AddComponent(collisionComponentType);
        }

        boxCollider.center = colliderCenter;
        boxCollider.size = colliderSize;

        return boxCollider;
    }
}

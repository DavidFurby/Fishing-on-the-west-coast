using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
public class SeaColliderController : MonoBehaviour
{
    BoxCollider seaGroupCollider;
    BoxCollider seaFloorGroupCollider;

    public void UpdateSeaColliders(List<GameObject> seaTileList)
    {
        SetBoxCollider(seaTileList);
    }
    private void SetBoxCollider(List<GameObject> seaTileList)
    {
        var seaBounds = CalculateSeaFloorBounds(seaTileList, "Sea");
        var (xMin, xMax, zMin, zMax, seaFloorYPosition) = CalculateSeaFloorBounds(seaTileList, "SeaFloor");

        seaGroupCollider = CreateOrUpdateCollider(seaGroupCollider, (seaBounds.xMin, seaBounds.xMax, seaBounds.zMin, seaBounds.zMax), "SeaGroupCollider", typeof(WaterCollision));
        seaFloorGroupCollider = CreateOrUpdateCollider(seaFloorGroupCollider, (xMin, xMax, zMin, zMax), "SeaFloorGroupCollider", typeof(SeaFloorCollision), seaFloorYPosition);
    }

    private (float xMin, float xMax, float zMin, float zMax, float seaFloorYPosition) CalculateSeaFloorBounds(List<GameObject> seaTileList, string tag)
    {
        float xMin = Mathf.Infinity;
        float xMax = -Mathf.Infinity;
        float zMin = Mathf.Infinity;
        float zMax = -Mathf.Infinity;
        float yPosition = 0;

        foreach (GameObject seaTile in seaTileList)
        {
            Transform seaObject = seaTile.GetComponentsInChildren<Transform>().First(r => r.CompareTag(tag));
            Bounds bounds = tag == "Sea" ? seaObject.GetComponentInChildren<Renderer>().bounds : seaObject.GetComponent<Renderer>().bounds;
            xMin = Mathf.Min(xMin, bounds.min.x);
            xMax = Mathf.Max(xMax, bounds.max.x);
            zMin = Mathf.Min(zMin, bounds.min.z);
            zMax = Mathf.Max(zMax, bounds.max.z);
            yPosition = bounds.center.y;
        }

        return (xMin, xMax, zMin, zMax, yPosition);
    }
    private BoxCollider CreateOrUpdateCollider(BoxCollider collider, (float xMin, float xMax, float zMin, float zMax) bounds, string colliderName, Type collisionComponentType, float yPosition = 0)
    {
        Vector3 colliderCenter = new((bounds.xMin + bounds.xMax) / 2, yPosition == 0 ? transform.position.y : yPosition, (bounds.zMin + bounds.zMax) / 2);
        Vector3 colliderSize = new(bounds.xMax - bounds.xMin, transform.position.y, bounds.zMax - bounds.zMin);

        if (collider == null)
        {
            GameObject colliderObject = new(colliderName);
            colliderObject.transform.SetParent(transform);
            collider = colliderObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            colliderObject.AddComponent(collisionComponentType);
        }

        collider.center = colliderCenter;
        collider.size = colliderSize;

        return collider;
    }
}

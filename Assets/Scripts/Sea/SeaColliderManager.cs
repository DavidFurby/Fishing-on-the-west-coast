using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class SeaColliderController : MonoBehaviour
{
    private BoxCollider SeaGroupCollider;
    private BoxCollider SeaFloorGroupCollider;

    public struct BoundsData
    {
        public float xMin;
        public float xMax;
        public float zMin;
        public float zMax;
        public float seaFloorYPosition;
    }

    public void SetBoxCollider(List<GameObject> seaTiles, Vector3 seaSize)
    {
        if (seaTiles.Count == 0) return;

        BoundsData seaBounds = CalculateBounds(seaTiles, "SeaFloor");
        SeaGroupCollider = CreateOrUpdateCollider(SeaGroupCollider, seaBounds, "SeaGroupCollider", typeof(WaterCollision), seaBounds.seaFloorYPosition / 2, seaSize.y);
        SeaFloorGroupCollider = CreateOrUpdateCollider(SeaFloorGroupCollider, seaBounds, "SeaFloorGroupCollider", typeof(SeaFloorCollision), seaBounds.seaFloorYPosition - 2, 6);
        if (SeaGroupCollider.GetComponent<FishSpawner>() == null)
        {
            SeaGroupCollider.AddComponent<FishSpawner>();

        }
    }

    private BoundsData CalculateBounds(List<GameObject> seaTiles, string seaObjectType)
    {
        float xMin = Mathf.Infinity;
        float xMax = -Mathf.Infinity;
        float zMin = Mathf.Infinity;
        float zMax = -Mathf.Infinity;
        float seaFloorYPosition = 0;

        foreach (GameObject seaTile in seaTiles)
        {
            Transform seaTileObject = FindChildTransformWithTag(seaTile, seaObjectType);
            Bounds bounds = GetRendererBounds(seaTileObject);
            UpdateMinMaxValues(ref xMin, ref xMax, ref zMin, ref zMax, bounds);
            seaFloorYPosition = bounds.center.y;
        }

        return new BoundsData { xMin = xMin, xMax = xMax, zMin = zMin, zMax = zMax, seaFloorYPosition = seaFloorYPosition };
    }

    private Transform FindChildTransformWithTag(GameObject parent, string tag)
    {
        return parent.GetComponentsInChildren<Transform>().First(childTransform => childTransform.CompareTag(tag));
    }

    private Bounds GetRendererBounds(Transform transform)
    {
        return transform.GetComponent<Renderer>().bounds;
    }

    private void UpdateMinMaxValues(ref float xMin, ref float xMax, ref float zMin, ref float zMax, Bounds bounds)
    {
        xMin = Mathf.Min(xMin, bounds.min.x);
        xMax = Mathf.Max(xMax, bounds.max.x);
        zMin = Mathf.Min(zMin, bounds.min.z);
        zMax = Mathf.Max(zMax, bounds.max.z);
    }

    private BoxCollider CreateOrUpdateCollider(BoxCollider boxCollider, BoundsData colliderBounds, string colliderName, Type collisionComponentType, float yPosition = 0, float ySize = 0)
    {
        Vector3 colliderCenter = CalculateColliderCenter(colliderBounds.xMin, colliderBounds.xMax, yPosition, colliderBounds.zMin, colliderBounds.zMax);
        Vector3 colliderSize = CalculateColliderSize(colliderBounds.xMin, colliderBounds.xMax, ySize, colliderBounds.zMin, colliderBounds.zMax);

        if (boxCollider == null)
        {
            boxCollider = CreateNewCollider(colliderName, collisionComponentType);
        }

        boxCollider.center = colliderCenter;
        boxCollider.size = colliderSize;

        return boxCollider;
    }

    private Vector3 CalculateColliderCenter(float xMin, float xMax, float yPosition, float zMin, float zMax)
    {
        return new Vector3((xMin + xMax) / 2, yPosition == 0 ? transform.position.y : yPosition, (zMin + zMax) / 2);
    }

    private Vector3 CalculateColliderSize(float xMin, float xMax, float ySize, float zMin, float zMax)
    {
        return new Vector3(xMax - xMin, ySize, zMax - zMin);
    }

    private BoxCollider CreateNewCollider(string colliderName, Type collisionComponentType)
    {
        GameObject colliderObject = new(colliderName);
        colliderObject.transform.SetParent(transform);
        BoxCollider newBoxCollider = colliderObject.AddComponent<BoxCollider>();
        newBoxCollider.isTrigger = true;
        colliderObject.AddComponent(collisionComponentType);
        return newBoxCollider;
    }
}

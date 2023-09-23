using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SeaColliderManager : MonoBehaviour
{
    internal BoxCollider seaGroupCollider;
    internal BoxCollider seaFloorGroupCollider;

    internal void UpdateColliders(List<GameObject> seaTileList)
    {
        SetBoxCollider(seaTileList);
    }
    private void SetBoxCollider(List<GameObject> seaTileList)
    {
        float xMin = Mathf.Infinity;
        float xMax = -Mathf.Infinity;
        float zMin = Mathf.Infinity;
        float zMax = -Mathf.Infinity;
        foreach (GameObject seaTile in seaTileList)
        {
            Transform seaObject = seaTile.GetComponentsInChildren<Transform>().First(r => r.CompareTag("Sea"));
            Bounds seaPlaneBounds = seaObject.GetComponentInChildren<Renderer>().bounds;
            xMin = Mathf.Min(xMin, seaPlaneBounds.min.x);
            xMax = Mathf.Max(xMax, seaPlaneBounds.max.x);
            zMin = Mathf.Min(zMin, seaPlaneBounds.min.z);
            zMax = Mathf.Max(zMax, seaPlaneBounds.max.z);
        }
        Vector3 colliderCenter = new((xMin + xMax) / 2, transform.position.y, (zMin + zMax) / 2);
        Vector3 colliderSize = new(xMax - xMin, transform.position.y, zMax - zMin);
        if (seaGroupCollider == null)
        {
            GameObject collider = new("SeaGroupCollider");
            collider.transform.SetParent(transform);
            seaGroupCollider = collider.AddComponent<BoxCollider>();
            seaGroupCollider.isTrigger = true;
            seaGroupCollider.AddComponent<WaterCollision>();
        }
        seaGroupCollider.center = colliderCenter;
        seaGroupCollider.size = colliderSize;
        xMin = Mathf.Infinity;
        xMax = -Mathf.Infinity;
        zMin = Mathf.Infinity;
        zMax = -Mathf.Infinity;
        float seaFloorYPosition = 0;

        foreach (GameObject seaTile in seaTileList)
        {
            Bounds floorBounds = seaTile.GetComponentsInChildren<Renderer>().First(r => r.CompareTag("SeaFloor")).bounds;
            xMin = Mathf.Min(xMin, floorBounds.min.x);
            xMax = Mathf.Max(xMax, floorBounds.max.x);
            zMin = Mathf.Min(zMin, floorBounds.min.z);
            zMax = Mathf.Max(zMax, floorBounds.max.z);
            seaFloorYPosition = floorBounds.center.y;
        }
        if (seaFloorGroupCollider == null)
        {
            GameObject collider = new("SeaFloorGroupCollider");
            collider.transform.SetParent(transform);
            seaFloorGroupCollider = collider.AddComponent<BoxCollider>();
            seaFloorGroupCollider.isTrigger = true;
            seaFloorGroupCollider.AddComponent<SeaFloorCollision>();
            seaFloorGroupCollider = collider.AddComponent<BoxCollider>();
        }
        Vector3 seaFloorColliderCenter = new((xMin + xMax) / 2, seaFloorYPosition, (zMin + zMax) / 2);
        Vector3 seaFloorColliderSize = new(xMax - xMin, transform.position.y, zMax - zMin);
        seaFloorGroupCollider.center = seaFloorColliderCenter;
        seaFloorGroupCollider.size = seaFloorColliderSize;
    }
}

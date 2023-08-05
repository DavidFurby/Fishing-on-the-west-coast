using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] public Item item;
    [SerializeField] private ProBuilderMesh proBuilderMesh;

    protected void DisplayModel(GameObject modelPrefab)
    {
        // Change the mesh of the existing game object
        Mesh newMesh = modelPrefab.GetComponent<MeshFilter>().sharedMesh;
        proBuilderMesh.ToMesh(MeshTopology.Triangles);
        proBuilderMesh.positions = newMesh.vertices;
        proBuilderMesh.Refresh();
    }
}
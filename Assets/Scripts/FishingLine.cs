using System.Collections.Generic;
using UnityEngine;

public class FishingLine : MonoBehaviour
{

    [SerializeField] GameObject boneTemplate;
    [SerializeField] GameObject firstBone;
    private GameObject newBone;

    void Start()
    {
        newBone = Instantiate(boneTemplate);
    }

    void Update()
    {
        firstBone.transform.parent = newBone.transform;
    }


}

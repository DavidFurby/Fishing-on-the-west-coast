using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitCamera : MonoBehaviour
{
    [SerializeField] FishingControlls fishingControlls;
    [SerializeField] GameObject bait;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (fishingControlls.fishingStatus != FishingControlls.GetFishingStatus.StandBy)
        {
            transform.position = new Vector3(bait.transform.position.x, 3, -6);

        }
    }
}

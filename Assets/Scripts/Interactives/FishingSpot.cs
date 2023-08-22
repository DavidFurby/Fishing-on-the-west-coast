using System;
using UnityEngine;
using Yarn.Unity;

public class FishingSpot : MonoBehaviour, IInteractive
{
    public static event Action StartFishing;

    public void Interact()
    {
        Debug.Log("Start Fishing");
        StartFishing.Invoke();

    }

}

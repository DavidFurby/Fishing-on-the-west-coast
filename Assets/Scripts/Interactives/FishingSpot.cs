using System;
using UnityEngine;
using Yarn.Unity;
[RequireComponent(typeof(Interactive))]
public class FishingSpot : MonoBehaviour, IInteractive
{
    public static event Action StartFishing;

    public void Interact()
    {
        StartFishing.Invoke();

    }

}

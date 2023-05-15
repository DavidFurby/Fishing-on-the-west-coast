using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour, IInteractible
{
    public void Interact()
    {
        GoToSleep();
    }
    public void GoToSleep()
    {
        Debug.Log("Sleep");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EveningTimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TransitArea transitArea;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndDay()
    {
        Debug.Log("End day");
        transitArea.Transition();
    }
}

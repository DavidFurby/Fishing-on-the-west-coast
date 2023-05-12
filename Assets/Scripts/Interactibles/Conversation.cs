using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] PlayerController playerController;
    [SerializeField] DialogController dialogController;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialog()
    {
        string[] dialogArray = { "Jararararar" };
        dialogController.initDialog("Harry", dialogArray);
    }
}

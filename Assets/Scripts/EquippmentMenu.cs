using UnityEngine;

public class EquippmentMenu : MonoBehaviour
{
    [SerializeField] private GameObject equipmentWheel;
    [SerializeField] private GameObject[] listOfWheels;
    private GameObject focusedWheel;
    private int focusedWheelIndex;

    // Start is called before the first frame update
    void Start()
    {
        equipmentWheel.SetActive(false);
        focusedWheel = listOfWheels[0];
        focusedWheelIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            equipmentWheel.SetActive(!equipmentWheel.activeSelf);
        }
        if (equipmentWheel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                focusedWheelIndex = (focusedWheelIndex - 1 + listOfWheels.Length) % listOfWheels.Length;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                focusedWheelIndex = (focusedWheelIndex + 1 + listOfWheels.Length) % listOfWheels.Length;
            }
            focusedWheel = listOfWheels[focusedWheelIndex];
            Debug.Log(focusedWheel);
        }
    }
}
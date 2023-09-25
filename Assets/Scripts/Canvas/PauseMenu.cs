using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        menuContainer.SetActive(false);

        // Set up navigation between buttons
        SetNavigation(continueButton, settingsButton, null);
        SetNavigation(settingsButton, exitButton, continueButton);
        SetNavigation(exitButton, null, settingsButton);

        continueButton.onClick.AddListener(OnContinueClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    private void Update()
    {
        SetPauseMenu();
        // Change the color of the currently focused button
        var selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject == null) return;

        var selectedButton = selectedObject.GetComponent<Button>();
        if (selectedButton == null) return;

        // Reset the color of all buttons
        ResetButtonColors();

        // Change the color of the selected button
        selectedButton.GetComponent<Image>().color = Color.red;
    }

    private void SetPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            menuContainer.SetActive(!menuContainer.activeSelf);
        }

    }

    private void OnContinueClick() => SetPauseMenu();

    private void OnExitClick() => Application.Quit();

    private static void SetNavigation(Selectable current, Selectable down, Selectable up)
    {
        var navigation = current.navigation;
        navigation.selectOnDown = down;
        navigation.selectOnUp = up;
        current.navigation = navigation;
    }

    private void ResetButtonColors()
    {
        continueButton.GetComponent<Image>().color = Color.white;
        settingsButton.GetComponent<Image>().color = Color.white;
        exitButton.GetComponent<Image>().color = Color.white;
    }
}


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject menuContainer;
    [SerializeField] Button continueButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;

    void Start()
    {
        menuContainer.SetActive(false);

        // Set up navigation between buttons
        Navigation continueButtonNav = continueButton.navigation;
        continueButtonNav.selectOnDown = settingsButton;
        continueButton.navigation = continueButtonNav;

        Navigation settingsButtonNav = settingsButton.navigation;
        settingsButtonNav.selectOnUp = continueButton;
        settingsButtonNav.selectOnDown = exitButton;
        settingsButton.navigation = settingsButtonNav;

        Navigation exitButtonNav = exitButton.navigation;
        exitButtonNav.selectOnUp = settingsButton;
        exitButton.navigation = exitButtonNav;
        continueButton.onClick.AddListener(OnContinueClick);
        exitButton.onClick.AddListener(OnExitClick);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseMenu();
        }

        // Change the color of the currently focused button
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null)
        {
            Button selectedButton = selectedObject.GetComponent<Button>();
            if (selectedButton != null)
            {
                // Reset the color of all buttons
                continueButton.GetComponent<Image>().color = Color.white;
                settingsButton.GetComponent<Image>().color = Color.white;
                exitButton.GetComponent<Image>().color = Color.white;

                // Change the color of the selected button
                selectedButton.GetComponent<Image>().color = Color.red;
            }
        }
    }

    private void SetPauseMenu()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        menuContainer.SetActive(!menuContainer.activeSelf);
    }

    public void OnContinueClick()
    {
        SetPauseMenu();
    }
    public void OnExitClick()
    {
        Application.Quit();
    }

}

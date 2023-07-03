using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject continueButton;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            mainMenu.SetActive(true);
        }
        if (MainManager.Instance.game.Scene == "")
        {
            continueButton.SetActive(false);
        }
    }
    public void NewGame()
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.game.NewGame();
        }
        mainMenu.SetActive(false);

        SceneManager.LoadScene("Boat");
    }

    public void Continue()
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.game.LoadGame();
            if (MainManager.Instance.game.Scene != null)
            {
                mainMenu.SetActive(false);
                SceneManager.LoadScene(MainManager.Instance.game.Scene);
            }
        }


    }
}

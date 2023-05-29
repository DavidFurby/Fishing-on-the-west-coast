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
        if (MainManager.Instance.game.scene == "")
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

        SceneManager.LoadScene("Home");
    }

    public void Continue()
    {
        Debug.Log("Continue");
        if (MainManager.Instance != null)
        {
            MainManager.Instance.game.LoadGame();
            if (MainManager.Instance.game.scene != null)
            {
                SceneManager.LoadScene(MainManager.Instance.game.scene);
            }
        }


    }
}

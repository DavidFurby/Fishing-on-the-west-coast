using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{

    public static MainManager Instance;
    public Game game;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (Instance != null && SceneManager.GetActiveScene().name != "Main Menu")
        {
            Instance.game.LoadGame();
        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnApplicationQuit()
    {
        if (Instance != null)
        {
            Instance.game.SaveGame();

        }
    }
}

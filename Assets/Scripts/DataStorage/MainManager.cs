using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : Game
{

    public static MainManager Instance;

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
            Instance.LoadGame();
        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnApplicationQuit()
    {
        if (Instance != null)
        {
            Instance.SaveGame();

        }
    }
}

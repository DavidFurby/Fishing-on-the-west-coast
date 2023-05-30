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
        if (Instance != null)
        {
            Instance.game.LoadGame();
        }
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance != null)
        {
            Instance.game.scene = scene.name;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnApplicationQuit()
    {
        if (Instance != null)
        {
            Instance.game.SaveGame();

        }
    }
}

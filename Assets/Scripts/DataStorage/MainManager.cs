using UnityEngine;

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
    private void OnApplicationQuit()
    {
        if (Instance != null)
        {
            Instance.game.SaveGame();

        }
    }
}

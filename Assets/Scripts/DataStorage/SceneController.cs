using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.Scene = scene.name;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

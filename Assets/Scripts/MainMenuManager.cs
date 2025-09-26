using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
        // Execute code in the next scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            GameManager.Instance.SpawnPlayer(new Vector3(0, 1, 0));
            // Unsubscribe from the event to avoid multiple calls
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

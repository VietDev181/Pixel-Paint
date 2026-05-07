using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService : MonoBehaviour, ISceneService
{
    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string levelSelectSceneName = "LevelScene";
    [SerializeField] private string gameSceneName = "GameScene";

    public void LoadStartScene() => Load(startSceneName);
    public void LoadLevelSelectScene() => Load(levelSelectSceneName);
    public void LoadGameScene() => Load(gameSceneName);

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void Load(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}

using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    private const string SelectedLevelKey = "SelectedLevel";

    [Header("Infrastructure")]
    [SerializeField] private AudioService audioService;
    [SerializeField] private SceneService sceneService;

    [Header("Scene Components")]
    [SerializeField] private GameController gameController;
    [SerializeField] private UIGame uiGame;
    [SerializeField] private UISetting uiSetting;

    private void Awake()
    {
        IAudioService audio = audioService;
        ISceneService scene = sceneService;
        ILevelProgressRepository progressRepo = new PlayerPrefsLevelProgress();

        var progressUseCase = new LevelProgressUseCase(progressRepo);
        var starPolicy = new StarRatingPolicy();
        var session = new GameSession(progressUseCase, starPolicy);

        uiGame.Initialize(audio, scene);
        uiSetting.Initialize(audio);
        gameController.Initialize(session, audio);

        uiGame.OnPauseRequested += () => Time.timeScale = 0f;
        uiGame.OnResumeRequested += () => Time.timeScale = 1f;
    }

    private void Start()
    {
        int levelIndex = PlayerPrefs.GetInt(SelectedLevelKey, 0);
        gameController.StartLevel(levelIndex);
    }
}

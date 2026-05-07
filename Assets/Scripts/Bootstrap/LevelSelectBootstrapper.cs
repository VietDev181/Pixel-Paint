using UnityEngine;

public class LevelSelectBootstrapper : MonoBehaviour
{
    [SerializeField] private AudioService audioService;
    [SerializeField] private SceneService sceneService;
    [SerializeField] private UILevel uiLevel;
    [SerializeField] private UISetting uiSetting;

    private void Awake()
    {
        IAudioService audio = audioService;
        ISceneService scene = sceneService;

        ILevelProgressRepository repo = new PlayerPrefsLevelProgress();
        var progress = new LevelProgressUseCase(repo);

        uiLevel.Initialize(audio, scene, progress);
        if (uiSetting != null) uiSetting.Initialize(audio);

        audio.PlayLevelBGM();
    }
}

using UnityEngine;

public class StartBootstrapper : MonoBehaviour
{
    [SerializeField] private AudioService audioService;
    [SerializeField] private SceneService sceneService;
    [SerializeField] private UIStart uiStart;

    private void Awake()
    {
        IAudioService audio = audioService;
        ISceneService scene = sceneService;

        audio.PlayMainMenuBGM();
        if (uiStart != null) uiStart.Initialize(audio, scene);
    }
}

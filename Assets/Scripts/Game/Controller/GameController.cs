using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private PaintInputController paintInput;
    [SerializeField] private ColorPaletteView palette;
    [SerializeField] private UIGame uiGame;

    private GameSession session;
    private IAudioService audioService;

    public void Initialize(GameSession session, IAudioService audio)
    {
        this.session = session;
        this.audioService = audio;

        paintInput.Configure(audio);

        palette.OnColorSelected += HandleColorSelected;
        levelLoader.OnLevelLoaded += HandleLevelLoaded;

        uiGame.OnSubmitRequested += HandleSubmit;
        uiGame.OnRetryRequested += HandleRetry;
        uiGame.OnNextLevelRequested += HandleNextLevel;

        session.OnLevelCompleted += HandleResult;
        session.OnLevelFailed += HandleResult;
    }

    public void StartLevel(int levelIndex)
    {
        if (!levelLoader.TryLoad(levelIndex)) return;
        session.StartLevel(levelIndex);
        uiGame.OnLevelStarted(levelIndex + 1);
        paintInput.SetEnabled(true);
        audioService.PlayGameBGM();
    }

    private void HandleLevelLoaded(int levelIndex, LevelData data, PaintingBoard board)
    {
        palette.ClearSelection();
        paintInput.SetColor(PaintColor.None);
    }

    private void HandleColorSelected(PaintColor color)
    {
        audioService.PlayClickSFX();
        session.SelectColor(color);
        paintInput.SetColor(color);
    }

    private void HandleSubmit()
    {
        var board = levelLoader.CurrentBoard;
        if (board == null) return;

        paintInput.SetEnabled(false);
        float accuracy = board.CalculateAccuracy();
        session.Submit(accuracy);
    }

    private void HandleRetry()
    {
        StartLevel(session.CurrentLevelIndex);
    }

    private void HandleNextLevel()
    {
        int next = session.CurrentLevelIndex + 1;
        if (next >= levelLoader.LevelCount) return;
        StartLevel(next);
    }

    private void HandleResult(LevelResult result)
    {
        uiGame.ShowResult(result);
    }

    private void OnDestroy()
    {
        if (palette != null) palette.OnColorSelected -= HandleColorSelected;
        if (levelLoader != null) levelLoader.OnLevelLoaded -= HandleLevelLoaded;
        if (uiGame != null)
        {
            uiGame.OnSubmitRequested -= HandleSubmit;
            uiGame.OnRetryRequested -= HandleRetry;
            uiGame.OnNextLevelRequested -= HandleNextLevel;
        }
        if (session != null)
        {
            session.OnLevelCompleted -= HandleResult;
            session.OnLevelFailed -= HandleResult;
        }
    }
}

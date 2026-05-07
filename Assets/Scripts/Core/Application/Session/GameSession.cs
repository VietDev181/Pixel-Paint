using System;

public class GameSession
{
    private readonly LevelProgressUseCase progress;
    private readonly StarRatingPolicy starPolicy;

    public int CurrentLevelIndex { get; private set; } = -1;
    public PaintColor SelectedColor { get; private set; } = PaintColor.None;
    public bool IsAcceptingInput { get; private set; }
    public LevelResult? LastResult { get; private set; }

    public event Action<int> OnLevelStarted;
    public event Action<PaintColor> OnColorSelected;
    public event Action<LevelResult> OnLevelCompleted;
    public event Action<LevelResult> OnLevelFailed;

    public GameSession(LevelProgressUseCase progress, StarRatingPolicy starPolicy)
    {
        this.progress = progress;
        this.starPolicy = starPolicy;
    }

    public void StartLevel(int levelIndex)
    {
        CurrentLevelIndex = levelIndex;
        SelectedColor = PaintColor.None;
        IsAcceptingInput = true;
        LastResult = null;
        OnLevelStarted?.Invoke(levelIndex);
    }

    public void SelectColor(PaintColor color)
    {
        if (!IsAcceptingInput) return;
        SelectedColor = color;
        OnColorSelected?.Invoke(color);
    }

    public void Submit(float accuracy)
    {
        if (!IsAcceptingInput) return;
        IsAcceptingInput = false;

        int stars = starPolicy.Rate(accuracy);
        var result = new LevelResult(CurrentLevelIndex, accuracy, stars);
        LastResult = result;

        if (result.IsCleared)
        {
            progress.RecordResult(result);
            OnLevelCompleted?.Invoke(result);
        }
        else
        {
            OnLevelFailed?.Invoke(result);
        }
    }
}

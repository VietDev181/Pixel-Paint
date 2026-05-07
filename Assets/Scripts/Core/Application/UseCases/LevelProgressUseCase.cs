public class LevelProgressUseCase
{
    private readonly ILevelProgressRepository repository;

    public LevelProgressUseCase(ILevelProgressRepository repository)
    {
        this.repository = repository;
    }

    public bool IsUnlocked(int levelIndex)
    {
        return levelIndex + 1 <= repository.HighestUnlockedLevelOneBased;
    }

    public int GetStars(int levelIndex) => repository.GetStars(levelIndex);

    public void RecordResult(LevelResult result)
    {
        if (!result.IsCleared) return;

        int best = repository.GetStars(result.LevelIndex);
        if (result.Stars > best)
            repository.SetStars(result.LevelIndex, result.Stars);

        int unlockedTo = result.LevelIndex + 2;
        if (unlockedTo > repository.HighestUnlockedLevelOneBased)
            repository.HighestUnlockedLevelOneBased = unlockedTo;
    }

    public void Reset() => repository.ClearAll();
}

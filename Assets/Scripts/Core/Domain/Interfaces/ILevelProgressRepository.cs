public interface ILevelProgressRepository
{
    int HighestUnlockedLevelOneBased { get; set; }
    int GetStars(int levelIndex);
    void SetStars(int levelIndex, int stars);
    void ClearAll();
}

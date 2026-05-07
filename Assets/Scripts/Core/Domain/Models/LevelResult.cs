public readonly struct LevelResult
{
    public int LevelIndex { get; }
    public float Accuracy { get; }
    public int Stars { get; }
    public bool IsCleared => Stars >= 1;

    public LevelResult(int levelIndex, float accuracy, int stars)
    {
        LevelIndex = levelIndex;
        Accuracy = accuracy;
        Stars = stars;
    }
}

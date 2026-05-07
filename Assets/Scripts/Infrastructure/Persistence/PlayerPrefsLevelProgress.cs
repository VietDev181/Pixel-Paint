using UnityEngine;

public class PlayerPrefsLevelProgress : ILevelProgressRepository
{
    private const string UnlockedKey = "UnlockedLevel";
    private const string StarKeyFormat = "Level_{0}_Stars";

    public int HighestUnlockedLevelOneBased
    {
        get => PlayerPrefs.GetInt(UnlockedKey, 1);
        set
        {
            PlayerPrefs.SetInt(UnlockedKey, value);
            PlayerPrefs.Save();
        }
    }

    public int GetStars(int levelIndex)
    {
        return PlayerPrefs.GetInt(StarKey(levelIndex), 0);
    }

    public void SetStars(int levelIndex, int stars)
    {
        PlayerPrefs.SetInt(StarKey(levelIndex), stars);
        PlayerPrefs.Save();
    }

    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    private static string StarKey(int levelIndex) => string.Format(StarKeyFormat, levelIndex);
}

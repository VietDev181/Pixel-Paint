using System;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Transform levelRoot;
    [SerializeField] private LevelData[] levels;

    private GameObject currentInstance;

    public LevelData[] Levels => levels;
    public int LevelCount => levels != null ? levels.Length : 0;
    public LevelData CurrentLevel { get; private set; }
    public PaintingBoard CurrentBoard { get; private set; }

    public event Action<int, LevelData, PaintingBoard> OnLevelLoaded;

    public bool TryLoad(int levelIndex)
    {
        if (levels == null || levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError($"LevelLoader: invalid level index {levelIndex}.");
            return false;
        }

        Clear();

        CurrentLevel = levels[levelIndex];
        if (CurrentLevel == null || CurrentLevel.mapPrefab == null)
        {
            Debug.LogError($"LevelLoader: level {levelIndex} has no mapPrefab.");
            return false;
        }

        Transform parent = levelRoot != null ? levelRoot : transform;
        currentInstance = Instantiate(CurrentLevel.mapPrefab, parent);

        CurrentBoard = currentInstance.GetComponent<PaintingBoard>();
        if (CurrentBoard == null)
        {
            Debug.LogError($"LevelLoader: prefab '{CurrentLevel.mapPrefab.name}' missing PaintingBoard.");
            return false;
        }

        CurrentBoard.Initialize();
        OnLevelLoaded?.Invoke(levelIndex, CurrentLevel, CurrentBoard);
        return true;
    }

    public void Clear()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }
        CurrentLevel = null;
        CurrentBoard = null;
    }
}

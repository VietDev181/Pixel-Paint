using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Pixel Paint/Level Data")]
public class LevelData : ScriptableObject
{
    public GameObject mapPrefab;

    [Header("Palette")]
    public List<PaintColor> availableColors = new();

    private void Reset()
    {
        FillWithAllColors();
    }

    [ContextMenu("Fill With All Colors")]
    private void FillWithAllColors()
    {
        availableColors.Clear();
        foreach (PaintColor color in Enum.GetValues(typeof(PaintColor)))
        {
            if (color == PaintColor.None) continue;
            availableColors.Add(color);
        }
    }
}

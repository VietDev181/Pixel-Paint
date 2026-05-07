using System.Collections.Generic;
using UnityEngine;

public class PaintingBoard : MonoBehaviour
{
    [SerializeField] private Transform templateRoot;
    [SerializeField] private Transform playerRoot;

    private Tile[] templateTiles;
    private Tile[] playerTiles;

    public int TileCount => templateTiles?.Length ?? 0;

    public void Initialize()
    {
        if (templateRoot == null) templateRoot = transform.Find("Template");
        if (playerRoot == null) playerRoot = transform.Find("Player");

        templateTiles = templateRoot != null ? templateRoot.GetComponentsInChildren<Tile>() : new Tile[0];
        playerTiles = playerRoot != null ? playerRoot.GetComponentsInChildren<Tile>() : new Tile[0];

        if (templateTiles.Length != playerTiles.Length)
            Debug.LogWarning($"PaintingBoard: tile count mismatch (template={templateTiles.Length}, player={playerTiles.Length}).");
    }

    public void ResetPlayerTiles()
    {
        if (playerTiles == null) return;
        foreach (var tile in playerTiles) tile.SetColor(PaintColor.None);
    }

    public float CalculateAccuracy()
    {
        if (templateTiles == null || playerTiles == null) return 0f;
        if (templateTiles.Length == 0 || playerTiles.Length == 0) return 0f;

        var templateMap = new Dictionary<Vector2Int, PaintColor>(templateTiles.Length);
        foreach (var tile in templateTiles)
        {
            if (tile == null) continue;
            templateMap[KeyOf(tile, templateRoot)] = tile.CurrentColor;
        }

        int total = templateMap.Count;
        if (total == 0) return 0f;

        int correct = 0;
        foreach (var tile in playerTiles)
        {
            if (tile == null) continue;
            if (templateMap.TryGetValue(KeyOf(tile, playerRoot), out var expected)
                && tile.CurrentColor == expected) correct++;
        }

        return (float)correct / total;
    }

    private static Vector2Int KeyOf(Tile tile, Transform root)
    {
        Vector3 local = root != null
            ? root.InverseTransformPoint(tile.transform.position)
            : tile.transform.localPosition;
        return new Vector2Int(Mathf.RoundToInt(local.x * 100f), Mathf.RoundToInt(local.y * 100f));
    }
}

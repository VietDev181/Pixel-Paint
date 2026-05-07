using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorPaletteView : MonoBehaviour
{
    [SerializeField] private List<ColorSwatchButton> swatches = new();

    public event Action<PaintColor> OnColorSelected;

    private void Awake()
    {
        foreach (var swatch in swatches)
        {
            if (swatch == null) continue;
            swatch.OnSelected += HandleSelected;
            swatch.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        foreach (var swatch in swatches)
            if (swatch != null) swatch.OnSelected -= HandleSelected;
    }

    public void ClearSelection()
    {
        foreach (var swatch in swatches)
            if (swatch != null) swatch.SetSelected(false);
    }

    private void HandleSelected(PaintColor color)
    {
        foreach (var swatch in swatches)
            if (swatch != null) swatch.SetSelected(swatch.Color == color);
        OnColorSelected?.Invoke(color);
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ColorSwatchButton : MonoBehaviour
{
    [SerializeField] private PaintColor color;
    [SerializeField] private Image colorTarget;
    [SerializeField] private GameObject selectedIndicator;

    private Button button;

    public PaintColor Color => color;
    public event Action<PaintColor> OnSelected;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnSelected?.Invoke(color));
        ApplyTint();
        SetSelected(false);
    }

    private void OnValidate()
    {
        ApplyTint();
    }

    public void SetSelected(bool selected)
    {
        if (selectedIndicator != null) selectedIndicator.SetActive(selected);
    }

    private void ApplyTint()
    {
        if (colorTarget != null)
            colorTarget.color = PaintColorPresets.GetColor(color);
    }
}

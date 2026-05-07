using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] private PaintColor currentColor = PaintColor.None;
    [SerializeField] private Sprite filledSprite;
    [SerializeField] private Sprite emptySprite;

    private SpriteRenderer spriteRenderer;

    public PaintColor CurrentColor => currentColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyColor();
    }

    public void SetColor(PaintColor color)
    {
        if (currentColor == color) return;
        currentColor = color;
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (spriteRenderer == null) return;

        if (currentColor == PaintColor.None)
        {
            spriteRenderer.sprite = emptySprite;
            spriteRenderer.color = Color.white;
            return;
        }

        spriteRenderer.sprite = filledSprite != null ? filledSprite : emptySprite;
        spriteRenderer.color = PaintColorPresets.GetColor(currentColor);
    }
}

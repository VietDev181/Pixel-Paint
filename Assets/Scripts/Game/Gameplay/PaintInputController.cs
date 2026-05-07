using UnityEngine;
using UnityEngine.EventSystems;

public class PaintInputController : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [SerializeField] private LayerMask paintableLayer = ~0;
    [SerializeField] private GameObject paintVfxPrefab;
    [SerializeField] private float vfxLifetime = 1.5f;

    private PaintColor currentColor = PaintColor.None;
    private bool inputEnabled;
    private IAudioService audioService;
    private Tile lastPaintedTile;

    public void Configure(IAudioService audio)
    {
        audioService = audio;
    }

    public void SetColor(PaintColor color)
    {
        currentColor = color;
    }

    public void SetEnabled(bool enabled)
    {
        inputEnabled = enabled;
        if (!enabled) lastPaintedTile = null;
    }

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;
    }

    private void Update()
    {
        if (!inputEnabled || currentColor == PaintColor.None) return;
        if (!Input.GetMouseButton(0))
        {
            lastPaintedTile = null;
            return;
        }
        if (IsPointerOverUI()) return;

        Vector2 worldPos = worldCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, paintableLayer);
        if (hit == null) return;

        if (!hit.TryGetComponent<Tile>(out var tile)) return;
        if (tile == lastPaintedTile) return;
        if (tile.CurrentColor == currentColor)
        {
            lastPaintedTile = tile;
            return;
        }

        tile.SetColor(currentColor);
        lastPaintedTile = tile;
        audioService?.PlayBrushSFX();

        if (paintVfxPrefab != null)
        {
            var vfx = Instantiate(paintVfxPrefab, tile.transform.position, Quaternion.identity);
            Destroy(vfx, vfxLifetime);
        }
    }

    private static bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return EventSystem.current.IsPointerOverGameObject();
    }
}

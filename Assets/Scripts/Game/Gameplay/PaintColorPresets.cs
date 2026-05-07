using UnityEngine;

public static class PaintColorPresets
{
    public static Color GetColor(PaintColor color)
    {
        return color switch
        {
            PaintColor.Blue       => Hex("#0000FF"),
            PaintColor.Green      => Hex("#008000"),
            PaintColor.Pink       => Hex("#FFC0CB"),
            PaintColor.Yellow     => Hex("#FFFF00"),
            PaintColor.Red        => Hex("#FF0000"),
            PaintColor.Orange     => Hex("#FFA500"),
            PaintColor.Purple     => Hex("#800080"),
            PaintColor.Black      => Hex("#000000"),
            PaintColor.White      => Hex("#FFFFFF"),
            PaintColor.Gray       => Hex("#808080"),
            PaintColor.Brown      => Hex("#A52A2A"),
            PaintColor.LightGreen => Hex("#90EE90"),
            PaintColor.Cream      => Hex("#FFFDD0"),
            PaintColor.DarkGreen  => Hex("#006400"),
            PaintColor.Cyan       => Hex("#00FFFF"),
            PaintColor.Magenta    => Hex("#FF00FF"),
            PaintColor.Lime       => Hex("#00FF00"),
            PaintColor.Maroon     => Hex("#800000"),
            PaintColor.Indigo     => Hex("#4B0082"),
            _ => Color.white
        };
    }

    private static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }
}

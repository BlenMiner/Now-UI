using UnityEngine;

public struct NowUIRectangle
{
    public Vector4 Rect;

    public Vector4 Radius;

    public Color Color;

    public Vector4 Padding;

    public float Blur;

    public float Outline;

    public Color OutlineColor;

    public NowUIRectangle(Vector4 rect)
    {
        Rect = rect;
        Radius = default;
        Padding = default;
        Blur = default;
        Outline = default;
        Color = new Color(1, 1, 1, 1);
        OutlineColor = default;
    }

    public NowUIRectangle SetBlur(float blur)
    {
        Blur = blur;
        return this;
    }

    public NowUIRectangle SetRadius(float allRadius)
    {
        Radius = new Vector4(allRadius, allRadius, allRadius, allRadius);
        return this;
    }
    
    public NowUIRectangle SetPadding(float all)
    {
        Padding = new Vector4(all, all, all, all);
        return this;
    }

    public NowUIRectangle SetOutline(float outline)
    {
        Outline = outline;
        return this;
    }

    public NowUIRectangle SetPosition(Vector4 rect)
    {
        Rect = rect;
        return this;
    }

    public NowUIRectangle SetColor(Color color)
    {
        Color = color;
        return this;
    }

    public NowUIRectangle SetOutlineColor(Color color)
    {
        OutlineColor = color;
        return this;
    }

    public NowUIRectangle Draw()
    {
        NowUI.DrawRect(this);
        return this;
    }
}

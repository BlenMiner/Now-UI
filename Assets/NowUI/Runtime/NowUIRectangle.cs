using UnityEngine;

public struct NowUIRectangle
{
    public Rect Rect {get; private set;}

    public Vector4 Radius {get; private set;}

    public Color Color {get; private set;}

    public Vector4 Padding {get; private set;}

    public float Blur {get; private set;}

    public float Outline {get; private set;}

    public Color OutlineColor {get; private set;}

    public NowUIRectangle(Rect rect)
    {
        Rect = rect;
        Radius = default;
        Padding = default;
        Blur = 0f;
        Outline = 0f;
        Color = new Color(1, 1, 1, 1);
        OutlineColor = new Color(0, 0, 0, 1);
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

    public NowUIRectangle SetPosition(Rect rect)
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

using UnityEngine;

[System.Serializable]
public struct NowFontAtlasInfo
{
    [System.Serializable]
    public struct Atlas
    {
        public string type;
        public int distanceRange;
        public int size;
        public int width;
        public int height;
        public string yOrigin;
    }

    [System.Serializable]
    public struct Metrics
    {
        public float emSize;
        public float lineHeight;
        public float ascender;
        public float descender;
        public float underlineY;
        public float underlineThickness;
    }

    [System.Serializable]
    public struct Bounds
    {
        public float left;
        public float bottom;
        public float right;
        public float top;
    }

    [System.Serializable]
    public struct Glyph
    {
        public int unicode;
        public float advance;
        public Bounds planeBounds;
        public Bounds atlasBounds;
    }

    public Atlas atlas;

    public Metrics metrics;

    public Glyph[] glyphs;
}

public class NowFont : ScriptableObject
{
    public Texture2D Atlas;

    public NowFontAtlasInfo AtlasInfo;

    public Material Material;
}

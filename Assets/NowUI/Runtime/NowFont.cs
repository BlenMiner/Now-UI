using System.Collections.Generic;
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

    private Dictionary<char, NowFontAtlasInfo.Glyph> m_glyphTable;

    public bool GetGlyph(char c, out NowFontAtlasInfo.Glyph glyph)
    {
        if (m_glyphTable == null)
        {
            m_glyphTable = new Dictionary<char, NowFontAtlasInfo.Glyph>();

            foreach(var g in AtlasInfo.glyphs)
            {
                var glyphValue = g;

                glyphValue.atlasBounds.left /= Atlas.width;
                glyphValue.atlasBounds.right /= Atlas.width;
                glyphValue.atlasBounds.top /= Atlas.height;
                glyphValue.atlasBounds.bottom /= Atlas.height;

                m_glyphTable.Add((char)g.unicode, glyphValue);
            }
        }

        return m_glyphTable.TryGetValue(c, out glyph);
    }
}

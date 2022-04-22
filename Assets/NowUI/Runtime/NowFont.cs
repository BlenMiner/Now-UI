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

    [System.NonSerialized]
    NowFontAtlasInfo.Glyph[] m_glyphTable;

    [System.NonSerialized]
    int m_glyphTableOffset;

    [System.NonSerialized]
    public int MaterialID;

    public bool GetGlyph(char c, out NowFontAtlasInfo.Glyph glyph)
    {
        if (m_glyphTable == null)
        {
            int first = AtlasInfo.glyphs[0].unicode;
            int last = AtlasInfo.glyphs[AtlasInfo.glyphs.Length - 1].unicode;

            m_glyphTableOffset = first;
            m_glyphTable = new NowFontAtlasInfo.Glyph[last - first + 1];

            foreach(var g in AtlasInfo.glyphs)
            {
                var glyphValue = g;

                glyphValue.atlasBounds.left /= Atlas.width;
                glyphValue.atlasBounds.right /= Atlas.width;
                glyphValue.atlasBounds.top /= Atlas.height;
                glyphValue.atlasBounds.bottom /= Atlas.height;

                m_glyphTable[g.unicode - m_glyphTableOffset] = glyphValue;
            }
        }

        int unicode = c;
        int idx = unicode - m_glyphTableOffset;

        if (idx < 0 || idx >= m_glyphTable.Length) 
        {
            glyph = default;
            return false;
        }

        glyph = m_glyphTable[idx];
        return glyph.unicode == unicode;
    }
}

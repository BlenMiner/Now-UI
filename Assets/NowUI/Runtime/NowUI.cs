using System;
using System.Collections.Generic;
using NowUIInternal;
using UnityEngine;

public static class NowUI
{
    static NowUIBootstrap m_bootstrap;

    static Material m_defaultMaterial;

    static Vector2 m_screen;

    static Dictionary<Material, NowMesh> m_meshes = 
       new Dictionary<Material, NowMesh>();

    static NowMesh GetMesh(Material mat)
    {
        if (m_meshes.TryGetValue(mat, out var res))
            return res;

        res = new NowMesh();
        m_meshes.Add(mat, res);

        return res;
    }

    private static void Initialize()
    {
        if (m_defaultMaterial == null)
            m_defaultMaterial = Resources.Load<Material>("NowUI/UIMaterial");
    }

    public static void BeingUI()
    {
        m_screen = new Vector2(Screen.width, Screen.height);
        Initialize();
    }

    public static void EndUI()
    {
        foreach(var mesh in m_meshes)
            mesh.Value.UploadMesh();

        GL.PushMatrix();
        GL.LoadIdentity();
        var proj = Matrix4x4.Ortho(0, m_screen.x, -m_screen.y, 0, -1, 100);
        GL.LoadProjectionMatrix(proj);

        foreach(var mesh in m_meshes)
        {
            mesh.Key.SetPass(0);
            Graphics.DrawMeshNow(mesh.Value.UnityMesh, Camera.current.transform.localToWorldMatrix);
        }

        GL.PopMatrix();

        foreach(var mesh in m_meshes)
            mesh.Value.ClearVerticies();
    }

    static readonly Vector4 defaultColor = new Vector4(1, 1, 1, 1);

    static readonly Vector4 defaultUV = new Vector4(0, 0, 1, 1);

    public static void DrawRect(Vector4 position)
    {
        NowUIRectangle rect = default;
        rect.Rect = position;
        rect.Color = defaultColor;
        DrawRect(rect);
    }

    static Vector4 rectPos, rectCol, rectOCol;

    public static void DrawRect(NowUIRectangle rectangle)
    {
        var position = rectangle.Rect;

        int rectHeight = (int)position.w;

        var pad = rectangle.Padding;

        var c = rectangle.Color;
        var oc = rectangle.OutlineColor;

        rectPos.x = (int)(position.x + pad.x);
        rectPos.y = -(int)(position.y + pad.y) - rectHeight;
        rectPos.z = (int)(position.z - pad.x - pad.z);
        rectPos.w = (int)(rectHeight - pad.y - pad.w);

        rectCol.x = c.r;
        rectCol.y = c.g;
        rectCol.z = c.b;
        rectCol.w = c.a;

        rectOCol.x = oc.r;
        rectOCol.y = oc.g;
        rectOCol.z = oc.b;
        rectOCol.w = oc.a;

        GetMesh(m_defaultMaterial).AddRect(rectPos, rectangle.Radius, rectCol, rectangle.Blur, rectangle.Outline, rectOCol, defaultUV);
    }

    public static void DrawString(Vector4 rect, string value, NowFont font, float fontSize)
    {
        const int tabSpaces = 4;
        Vector2 currentPos = new Vector2(rect.x, rect.y);

        for (int i = 0; i < value.Length; ++i)
        {
            if (value[i] == '\n')
            {
                currentPos.x = rect.x;
                currentPos.y += font.AtlasInfo.metrics.lineHeight * fontSize;
            }
            else if (value[i] == '\t')
            {
                if (font.GetGlyph(' ', out var space))
                    currentPos.x += space.advance * fontSize * tabSpaces;
            }
            else
            {
                if (font.GetGlyph(value[i], out var glyph) && glyph.atlasBounds.left != glyph.atlasBounds.right)
                    DrawCharacter(currentPos, glyph, font, fontSize);
            
                currentPos.x += glyph.advance * fontSize;
            }
        }
    }

    public static void DrawCharacter(Vector2 pos, NowFontAtlasInfo.Glyph glyph, NowFont font, float fontSize)
    {
        var planeBounds = glyph.planeBounds;

        float lineHeight = font.AtlasInfo.metrics.lineHeight * fontSize;

        planeBounds.left *= fontSize;
        planeBounds.right *= fontSize;
        planeBounds.bottom *= fontSize;
        planeBounds.top *= fontSize;

        Vector4 position = new Vector4(
            pos.x + planeBounds.left, pos.y - planeBounds.bottom,
            planeBounds.right - planeBounds.left,
            planeBounds.top - planeBounds.bottom
        );

        position.y += lineHeight - position.w;

        var atlasBounds = glyph.atlasBounds;
        int rectHeight = (int)position.w;

        rectPos.x = (int)position.x;
        rectPos.y = -(int)(position.y + rectHeight);
        rectPos.z = (int)position.z;
        rectPos.w = rectHeight;

        var uvwh = new Vector4(atlasBounds.left, atlasBounds.bottom,
            atlasBounds.right - atlasBounds.left,
            atlasBounds.top - atlasBounds.bottom
        );
        GetMesh(font.Material).AddRect(rectPos, default, defaultColor, 1f, fontSize, default, uvwh);
    }

    public static NowUIRectangle Rectangle(Vector4 position)
    {
        return new NowUIRectangle(position);
    }
}

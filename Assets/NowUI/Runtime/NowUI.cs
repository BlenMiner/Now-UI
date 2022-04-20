using System;
using NowUIInternal;
using UnityEngine;

public static class NowUI
{
    static NowUIBootstrap m_bootstrap;

    static Material m_defaultMaterial;

    static NowMesh m_mesh;

    static Vector2 m_screen;

    private static void Initialize()
    {
        if (m_defaultMaterial == null)
            m_defaultMaterial = Resources.Load<Material>("NowUI/UIMaterial");

        if (m_mesh == null)
            m_mesh = new NowMesh();
    }

    public static void BeingUI()
    {
        m_screen = new Vector2(Screen.width, Screen.height);
        Initialize();
    }

    public static void EndUI()
    {
        m_mesh.UploadMesh();

        GL.PushMatrix();
        GL.LoadIdentity();
        var proj = Matrix4x4.Ortho(0, m_screen.x, -m_screen.y, 0, -1, 100);
        GL.LoadProjectionMatrix(proj);

        m_defaultMaterial.SetPass(0);

        Graphics.DrawMeshNow(m_mesh.UnityMesh, Camera.current.transform.localToWorldMatrix);

        GL.PopMatrix();

        m_mesh.ClearVerticies();
    }

    static readonly Color defaultColor = Color.white;

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

        float rectHeight = position.w;

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

        m_mesh.AddRect(rectPos, 0f, rectangle.Radius, rectCol, rectangle.Blur, rectangle.Outline, rectOCol);
    }

    public static NowUIRectangle Rectangle(Vector4 position)
    {
        return new NowUIRectangle(position);
    }
}

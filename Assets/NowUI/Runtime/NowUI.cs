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

    public static void DrawRect(Rect position)
    {
        position.size = new Vector2(Mathf.RoundToInt(position.size.x), Mathf.RoundToInt(position.size.y));
        position.position = new Vector2(Mathf.RoundToInt(position.position.x), -Mathf.RoundToInt(position.position.y) - position.size.y);

        m_mesh.AddRect(position, 0f);
    }
}

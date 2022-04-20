using System.Collections.Generic;
using UnityEngine;

namespace NowUIInternal
{
    public class NowMesh
    {
        public Mesh UnityMesh {get; private set;}

        List<Vector4> m_color;
        List<Vector4> m_rect;

        List<Vector4> m_radius;

        List<Vector3> m_verts;

        List<Vector2> m_uvs;

        List<int> m_tris;

        public NowMesh()
        {
            m_radius = new List<Vector4>();
            m_rect = new List<Vector4>();
            m_verts = new List<Vector3>();
            m_uvs = new List<Vector2>();
            m_color = new List<Vector4>();
            m_tris = new List<int>();
        }

        readonly Vector4[] V4_BUFFER = new Vector4[4];

        readonly Vector3[] V3_BUFFER = new Vector3[4];

        readonly Vector2[] V2_BUFFER = new Vector2[4];

        readonly int[] I_BUFFER = new int[6];

        const int PRECISION = 4096;

        float Pack(Vector2 input, Vector2 bounds)
        {
            Vector2 output = Vector2.Scale(input, new Vector2(1 / bounds.x, 1 / bounds.y));
            output.x = Mathf.Floor(output.x * (PRECISION - 1));
            output.y = Mathf.Floor(output.y * (PRECISION - 1));

            return (output.x * PRECISION) + output.y;
        }

        Vector2 Pack(Vector4 input, Vector4 bounds)
        {
            return new Vector2(
                Pack(new Vector2(input.x, input.y), new Vector2(bounds.x, bounds.y)),
                Pack(new Vector2(input.z, input.w), new Vector2(bounds.z, bounds.w)));
        }

        Vector4 Pack(Vector4 a, Vector4 aBounds, Vector4 b, Vector4 bBounds)
        {
            var a2 = Pack(a, aBounds);
            var b2 = Pack(b, bBounds);

            return new Vector4(a2.x, a2.y, b2.x, b2.y);
        }

        public void AddRect(Rect position, float z)
        {
            AddRect(position, z, default, Color.white, default, 0f, Color.black);
        }

        public void AddRect(Rect position, float z, Vector4 radius, Color color, float blur, float outline, Color outlineColor)
        {
            int indexOffset = m_verts.Count;

            var rectV4 = new Vector4(position.position.x, position.position.y, position.size.x, position.size.y);
            var packedRectRad = Pack(rectV4, new Vector4(8192, 4096, 8192, 4096), radius, new Vector4(8192, 4096, 8192, 4096));

            V4_BUFFER[0] = packedRectRad;
            V4_BUFFER[1] = packedRectRad;
            V4_BUFFER[2] = packedRectRad;
            V4_BUFFER[3] = packedRectRad;

            m_rect.AddRange(V4_BUFFER);

            var packedColorPadding = Pack(
                color,                      new Vector4(1, 1, 1, 1),
                new Vector4(blur, outline, 0, 0), new Vector4(4096, 4096, 1, 1));

            V4_BUFFER[0] = packedColorPadding;
            V4_BUFFER[1] = packedColorPadding;
            V4_BUFFER[2] = packedColorPadding;
            V4_BUFFER[3] = packedColorPadding;

            m_radius.AddRange(V4_BUFFER);

            V4_BUFFER[0] = outlineColor;
            V4_BUFFER[1] = outlineColor;
            V4_BUFFER[2] = outlineColor;
            V4_BUFFER[3] = outlineColor;

            m_color.AddRange(V4_BUFFER);

            V3_BUFFER[0] = new Vector3(position.x,                  position.y,                   0);
            V3_BUFFER[1] = new Vector3(position.x,                  position.y + position.height, 0);
            V3_BUFFER[2] = new Vector3(position.x + position.width, position.y + position.height, 0);
            V3_BUFFER[3] = new Vector3(position.x + position.width, position.y                  , 0);

            m_verts.AddRange(V3_BUFFER);

            V2_BUFFER[0] = new Vector2(0, 0);
            V2_BUFFER[1] = new Vector2(0, 1);
            V2_BUFFER[2] = new Vector2(1, 1);
            V2_BUFFER[3] = new Vector2(1, 0);

            m_uvs.AddRange(V2_BUFFER);

            I_BUFFER[0] = indexOffset + 0;
            I_BUFFER[1] = indexOffset + 1;
            I_BUFFER[2] = indexOffset + 2;
            I_BUFFER[3] = indexOffset + 0;
            I_BUFFER[4] = indexOffset + 2;
            I_BUFFER[5] = indexOffset + 3;

            m_tris.AddRange(I_BUFFER);
        }

        public void ClearVerticies()
        {
            m_radius.Clear();
            m_rect.Clear();
            m_verts.Clear();
            m_uvs.Clear();
            m_tris.Clear();
            m_color.Clear();
        }

        public void UploadMesh()
        {
            if (UnityMesh == null) 
            {
                UnityMesh = new Mesh();
                UnityMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                UnityMesh.MarkDynamic();
            }
            
            UnityMesh.Clear(true);

            UnityMesh.SetVertices(m_verts);
            UnityMesh.SetUVs(0, m_uvs);
            UnityMesh.SetUVs(1, m_rect);
            UnityMesh.SetUVs(2, m_radius);
            UnityMesh.SetUVs(3, m_color);
            UnityMesh.SetTriangles(m_tris, 0, false);
        }
    }
}
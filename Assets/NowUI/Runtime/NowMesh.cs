using System.Collections.Generic;
using UnityEngine;

namespace NowUIInternal
{
    public struct StaticList<T> where T : unmanaged
    {
        public int Count;

        public T[] Array;

        public StaticList(int count)
        {
            Count = 0;
            Array = new T[count];
        }

        public void Push(T val)
        {
            Array[Count++] = val;
        }

        public void Clear()
        {
            Count = 0;
        }
    }

    public class NowMesh
    {
        public Mesh UnityMesh {get; private set;}

        StaticList<Vector4> m_extra;

        StaticList<Vector4> m_outlineColor;

        StaticList<Vector4> m_color;

        StaticList<Vector4> m_rect;

        StaticList<Vector4> m_radius;

        StaticList<Vector3> m_verts;

        StaticList<Vector2> m_uvs;

        List<int> m_tris;

        public NowMesh()
        {
            const int v = 1 << 18;

            m_radius = new StaticList<Vector4>(v);
            m_rect = new StaticList<Vector4>(v);
            m_verts = new StaticList<Vector3>(v);
            m_uvs = new StaticList<Vector2>(v);
            m_color = new StaticList<Vector4>(v);
            m_outlineColor = new StaticList<Vector4>(v);
            m_extra = new StaticList<Vector4>(v);
            m_tris = new List<int>(v);
        }

        readonly int[] I_BUFFER = new int[6];

        public void AddRect(Rect position, float z)
        {
            AddRect(position, z, default, Color.white, default, 0f, Color.black);
        }

        public void AddRect(Rect position, float z, Vector4 radius, Color color, float blur, float outline, Color outlineColor)
        {
            int indexOffset = m_verts.Count;

            var rectV4 = new Vector4(position.position.x, position.position.y, position.size.x, position.size.y);

            m_rect.Push(rectV4);
            m_rect.Push(rectV4);
            m_rect.Push(rectV4);
            m_rect.Push(rectV4);

            m_radius.Push(radius);
            m_radius.Push(radius);
            m_radius.Push(radius);
            m_radius.Push(radius);

            m_color.Push(color);
            m_color.Push(color);
            m_color.Push(color);
            m_color.Push(color);

            m_outlineColor.Push(outlineColor);
            m_outlineColor.Push(outlineColor);
            m_outlineColor.Push(outlineColor);
            m_outlineColor.Push(outlineColor);

            var extraData = new Vector4(blur, outline, 0, 0);

            m_extra.Push(extraData);
            m_extra.Push(extraData);
            m_extra.Push(extraData);
            m_extra.Push(extraData);

            m_verts.Push(new Vector3(position.x,                  position.y,                   0));
            m_verts.Push(new Vector3(position.x,                  position.y + position.height, 0));
            m_verts.Push(new Vector3(position.x + position.width, position.y + position.height, 0));
            m_verts.Push(new Vector3(position.x + position.width, position.y                  , 0));

            m_uvs.Push(new Vector2(0, 0));
            m_uvs.Push(new Vector2(0, 1));
            m_uvs.Push(new Vector2(1, 1));
            m_uvs.Push(new Vector2(1, 0));

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
            m_outlineColor.Clear();
            m_extra.Clear();
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

            UnityMesh.SetVertices(m_verts.Array, 0, m_verts.Count);
            UnityMesh.SetUVs(0, m_uvs.Array, 0, m_uvs.Count);
            UnityMesh.SetUVs(1, m_rect.Array, 0, m_rect.Count);
            UnityMesh.SetUVs(2, m_radius.Array, 0, m_radius.Count);
            UnityMesh.SetUVs(3, m_color.Array, 0, m_color.Count);
            UnityMesh.SetUVs(4, m_outlineColor.Array, 0, m_outlineColor.Count);
            UnityMesh.SetUVs(5, m_extra.Array, 0, m_extra.Count);
            UnityMesh.SetTriangles(m_tris, 0, false);
        }
    }
}
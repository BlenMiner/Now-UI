using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        StaticList<int> m_tris;

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
            m_tris = new StaticList<int>(v * 3);
        }

        Vector2[] uvConst = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        Vector3 a, b, c, d;

        Vector4 extra;

        public void AddRect(Vector4 position, Vector4 radius, Vector4 color, float blur, float outline, Vector4 outlineColor)
        {
            int indexOffset = m_verts.Count;

            extra.x = blur;
            extra.y = outline;

            var rarray = m_rect.Array;
            var rcount = m_rect.Count;

            rarray[rcount] = position;
            rarray[rcount + 1] = position;
            rarray[rcount + 2] = position;
            rarray[rcount + 3] = position;

            m_rect.Count += 4;

            var rrad= m_radius.Array;
            var rrcount = m_radius.Count;

            rrad[rrcount] = radius;
            rrad[rrcount + 1] = radius;
            rrad[rrcount + 2] = radius;
            rrad[rrcount + 3] = radius;

            m_radius.Count += 4;

            var rcol = m_color.Array;
            var ccount = m_color.Count;

            rcol[ccount] = color;
            rcol[ccount + 1] = color;
            rcol[ccount + 2] = color;
            rcol[ccount + 3] = color;

            m_color.Count += 4;

            var rout = m_outlineColor.Array;
            var ocount = m_outlineColor.Count;

            rout[ocount] = outlineColor;
            rout[ocount + 1] = outlineColor;
            rout[ocount + 2] = outlineColor;
            rout[ocount + 3] = outlineColor;

            m_outlineColor.Count += 4;

            var rextra = m_extra.Array;
            var extraC = m_extra.Count;

            rextra[extraC] = extra;
            rextra[extraC + 1] = extra;
            rextra[extraC + 2] = extra;
            rextra[extraC + 3] = extra;

            m_extra.Count += 4;

            a.x = position.x;
            a.y = position.y;
            a.z = 0;
            
            b.x = a.x;
            b.y = a.y + position.w;
            b.z = 0;

            c.x = a.x + position.z;
            c.y = b.y;
            c.z = 0;

            d.x = c.x;
            d.y = a.y;
            d.z = 0;

            var varr = m_verts.Array;
            var vcount = m_verts.Count;

            varr[vcount++] = a;
            varr[vcount++] = b;
            varr[vcount++] = c;
            varr[vcount++] = d;

            m_verts.Count = vcount;

            var ruvs = m_uvs.Array;
            var ruvsCount = m_uvs.Count;

            ruvs[ruvsCount] = uvConst[0];
            ruvs[ruvsCount + 1] = uvConst[1];
            ruvs[ruvsCount + 2] = uvConst[2];
            ruvs[ruvsCount + 3] = uvConst[3];

            m_uvs.Count += 4;

            int ucount = m_uvs.Count;
            var uarr = m_uvs.Array;

            uarr[ucount] = uvConst[0];
            uarr[ucount + 1] = uvConst[1];
            uarr[ucount + 2] = uvConst[2];
            uarr[ucount + 3] = uvConst[3];

            int triCount = m_tris.Count;
            var triArr = m_tris.Array;

            triArr[triCount + 0] = indexOffset + 0;
            triArr[triCount + 1] = indexOffset + 1;
            triArr[triCount + 2] = indexOffset + 2;
            triArr[triCount + 3] = indexOffset + 0;
            triArr[triCount + 4] = indexOffset + 2;
            triArr[triCount + 5] = indexOffset + 3;

            m_tris.Count += 6;
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
            UnityMesh.SetTriangles(m_tris.Array, 0, m_tris.Count, 0);
        }
    }
}
using UnityEngine;

namespace NowUIInternal
{
    public struct StaticList<T>
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

    public struct NowRectVertex
    {
        public Vector4 mask;

        public Vector4 position;

        public Vector4 radius;

        public Vector4 color;

        public Vector4 outlineColor;

        public Vector4 uvwh;

        public bool IsOutsideMask(Vector4 rect)
        {
            return rect.x + rect.z < mask.x ||
                rect.x >= mask.x + mask.z ||
                -rect.y < mask.y ||
                -rect.y - rect.w >= mask.y + mask.w;
        }
    }

    public class SubMesh
    {
        public Material Material;

        public StaticList<int> Tris;

        public SubMesh(Material mat)
        {
            const int v = 1 << 18;
            Material = mat;
            Tris = new StaticList<int>(v * 3);
        }
    }

    public class NowMesh
    {
        public Mesh UnityMesh {get; private set;}

        StaticList<Vector4> m_rawuv;

        StaticList<Vector4> m_mask;

        StaticList<Vector4> m_extra;

        StaticList<Vector4> m_outlineColor;

        StaticList<Vector4> m_color;

        StaticList<Vector4> m_rect;

        StaticList<Vector4> m_radius;

        StaticList<Vector3> m_verts;

        StaticList<Vector2> m_uvs;

        StaticList<int> m_tris;

        public Material Material;

        public NowMesh(Material mat)
        {
            Material = mat;
            const int v = 1 << 18;

            m_radius = new StaticList<Vector4>(v);
            m_rect = new StaticList<Vector4>(v);
            m_verts = new StaticList<Vector3>(v);
            m_uvs = new StaticList<Vector2>(v);
            m_color = new StaticList<Vector4>(v);
            m_outlineColor = new StaticList<Vector4>(v);
            m_extra = new StaticList<Vector4>(v);
            m_tris = new StaticList<int>(v * 3);
            m_mask = new StaticList<Vector4>(v);
            m_rawuv= new StaticList<Vector4>(v);
        }

        Vector2[] uvConst = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        Vector3 a, b, c, d;

        Vector4 extra;

        public void AddRect(NowRectVertex vertexData, float extraX, float extraY)
        {
            if (vertexData.IsOutsideMask(vertexData.position)) return;

            int indexOffset = m_verts.Count;

            extra.x = extraX;
            extra.y = extraY;

            var maskarr = m_mask.Array;
            var maskcount = m_mask.Count;

            var mask = vertexData.mask;

            maskarr[maskcount] = mask;
            maskarr[maskcount + 1] = mask;
            maskarr[maskcount + 2] = mask;
            maskarr[maskcount + 3] = mask;

            m_mask.Count += 4;

            var rarray = m_rect.Array;
            var rcount = m_rect.Count;

            var position = vertexData.position;

            rarray[rcount] = position;
            rarray[rcount + 1] = position;
            rarray[rcount + 2] = position;
            rarray[rcount + 3] = position;

            m_rect.Count += 4;

            var rrad= m_radius.Array;
            var rrcount = m_radius.Count;

            rrad[rrcount] = vertexData.radius;
            rrad[rrcount + 1] = vertexData.radius;
            rrad[rrcount + 2] = vertexData.radius;
            rrad[rrcount + 3] = vertexData.radius;

            m_radius.Count += 4;

            var rcol = m_color.Array;
            var ccount = m_color.Count;

            rcol[ccount] = vertexData.color;
            rcol[ccount + 1] = vertexData.color;
            rcol[ccount + 2] = vertexData.color;
            rcol[ccount + 3] = vertexData.color;

            m_color.Count += 4;

            var rout = m_outlineColor.Array;
            var ocount = m_outlineColor.Count;

            rout[ocount] = vertexData.outlineColor;
            rout[ocount + 1] = vertexData.outlineColor;
            rout[ocount + 2] = vertexData.outlineColor;
            rout[ocount + 3] = vertexData.outlineColor;

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

            var uv0 = uvConst[0];
            var uv1 = uvConst[1];
            var uv2 = uvConst[2];
            var uv3 = uvConst[3];

            var uvwh = vertexData.uvwh;

            uv0.x = uvwh.x + uv0.x * uvwh.z;
            uv0.y = uvwh.y + uv0.y * uvwh.w;

            uv1.x = uvwh.x + uv1.x * uvwh.z;
            uv1.y = uvwh.y + uv1.y * uvwh.w;

            uv2.x = uvwh.x + uv2.x * uvwh.z;
            uv2.y = uvwh.y + uv2.y * uvwh.w;

            uv3.x = uvwh.x + uv3.x * uvwh.z;
            uv3.y = uvwh.y + uv3.y * uvwh.w;

            ruvs[ruvsCount] = uv0;
            ruvs[ruvsCount + 1] = uv1;
            ruvs[ruvsCount + 2] = uv2;
            ruvs[ruvsCount + 3] = uv3;

            var rawuvs = m_rawuv.Array;
            var rawuvsCount = m_rawuv.Count;

            rawuvs[rawuvsCount] = uvConst[0];
            rawuvs[rawuvsCount + 1] = uvConst[1];
            rawuvs[rawuvsCount + 2] = uvConst[2];
            rawuvs[rawuvsCount + 3] = uvConst[3];

            m_rawuv.Count += 4;
            m_uvs.Count += 4;

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
            m_mask.Clear();
            m_rawuv.Clear();
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
            UnityMesh.SetUVs(6, m_mask.Array, 0, m_mask.Count);
            UnityMesh.SetUVs(7, m_rawuv.Array, 0, m_rawuv.Count);
            UnityMesh.SetTriangles(m_tris.Array, 0, m_tris.Count, 0);
        }
    }
}
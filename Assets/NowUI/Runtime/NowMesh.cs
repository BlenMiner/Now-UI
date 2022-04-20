using System.Collections.Generic;
using UnityEngine;

namespace NowUIInternal
{
    public struct NowVertex
    {
        public Vector3 position;

        public Vector3 normal;

        public Vector2 uv;

        public NowVertex(Vector3 pos, Vector2 uv)
        {
            position = pos;
            this.uv = uv;
            normal = new Vector3(0, 0, -1);
        }
    }

    public class NowMesh
    {
        public Mesh UnityMesh {get; private set;}

        List<Vector3> m_verts;

        List<Vector3> m_normals;

        List<Vector2> m_uvs;

        List<int> m_tris;

        public NowMesh()
        {
            UnityMesh = new Mesh();
            UnityMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            GameObject.DontDestroyOnLoad(UnityMesh);

            m_verts = new List<Vector3>();
            m_normals = new List<Vector3>();
            m_uvs = new List<Vector2>();
            m_tris = new List<int>();
        }

        readonly Vector3[] V3_BUFFER = new Vector3[4];

        readonly Vector2[] V2_BUFFER = new Vector2[4];

        readonly int[] I_BUFFER = new int[6];

        public void AddRect(Rect position, float z)
        {
            int indexOffset = m_verts.Count;

            V3_BUFFER[0] = new Vector3(position.x,                  position.y,                   0);
            V3_BUFFER[1] = new Vector3(position.x,                  position.y + position.height, 0);
            V3_BUFFER[2] = new Vector3(position.x + position.width, position.y + position.height, 0);
            V3_BUFFER[3] = new Vector3(position.x + position.width, position.y                  , 0);

            m_verts.AddRange(V3_BUFFER);

            V3_BUFFER[0] = new Vector3(0, 0, -1);
            V3_BUFFER[1] = new Vector3(0, 0, -1);
            V3_BUFFER[2] = new Vector3(0, 0, -1);
            V3_BUFFER[3] = new Vector3(0, 0, -1);

            m_normals.AddRange(V3_BUFFER);

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

        public void AddVertex(NowVertex vertex)
        {
            m_tris.Add(m_verts.Count);
            m_verts.Add(vertex.position);
            m_normals.Add(vertex.normal);
            m_uvs.Add(vertex.uv);
        }

        public void AddIndex(int index)
        {
            m_tris.Add(m_verts.Count + index);
        }

        public void ClearVerticies()
        {
            m_verts.Clear();
            m_normals.Clear();
            m_uvs.Clear();
            m_tris.Clear();
        }

        public void UploadMesh()
        {
            if (UnityMesh == null) 
            {
                UnityMesh = new Mesh();
                UnityMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }
            
            UnityMesh.Clear(true);

            UnityMesh.SetVertices(m_verts);
            UnityMesh.SetNormals(m_normals);
            UnityMesh.SetUVs(0, m_uvs);
            UnityMesh.SetTriangles(m_tris, 0);
        }
    }
}
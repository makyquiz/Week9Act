using UnityEngine;

public class GLRectangularColumn : MonoBehaviour
{
    public Material material;
    public float width = 1f;
    public float height = 1.5f;
    public float depth = 1f;
    public Vector2 pos;
    public float zPos;

    private void OnPostRender() => DrawShape(true);
    private void OnDrawGizmos() => DrawShape(false);

    private void DrawShape(bool isPlayMode)
    {
        if (material == null) return;

        if (isPlayMode)
        {
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            material.SetPass(0);
        }
        else
            Gizmos.color = Color.red;

        float hw = width / 2f;
        float hh = height / 2f;
        float hd = depth / 2f;

        Vector3[] verts =
        {
            new Vector3(pos.x - hw, pos.y - hh, zPos - hd), 
            new Vector3(pos.x + hw, pos.y - hh, zPos - hd), 
            new Vector3(pos.x + hw, pos.y + hh, zPos - hd), 
            new Vector3(pos.x - hw, pos.y + hh, zPos - hd), 
            new Vector3(pos.x - hw, pos.y - hh, zPos + hd), 
            new Vector3(pos.x + hw, pos.y - hh, zPos + hd), 
            new Vector3(pos.x + hw, pos.y + hh, zPos + hd), 
            new Vector3(pos.x - hw, pos.y + hh, zPos + hd) 
        };

        if (isPlayMode)
        {
            for (int i = 0; i < verts.Length; i++)
            {
                float p = PerspectiveCamera.Instance.GetPerspective(verts[i].z);
                verts[i] = new Vector3(verts[i].x * p, verts[i].y * p, 0);
            }
        }

        int[,] edges =
        {
            {0,1},{1,2},{2,3},{3,0},
            {4,5},{5,6},{6,7},{7,4},
            {0,4},{1,5},{2,6},{3,7}
        };

        for (int i = 0; i < edges.GetLength(0); i++)
            DrawLine(verts[edges[i,0]], verts[edges[i,1]], isPlayMode);

        if (isPlayMode)
        {
            GL.End();
            GL.PopMatrix();
        }
    }

    private void DrawLine(Vector3 a, Vector3 b, bool useGL)
    {
        if (useGL) { GL.Vertex(a); GL.Vertex(b); }
        else Gizmos.DrawLine(a, b);
    }
}

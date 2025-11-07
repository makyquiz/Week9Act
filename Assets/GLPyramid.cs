using UnityEngine;

public class GLPyramid : MonoBehaviour
{
    public Material material;
    public float baseSize = 1f;
    public float height = 1.5f;
    public Vector2 basePos;
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

        float half = baseSize * 0.5f;

        Vector3[] baseVerts =
        {
            new Vector3(basePos.x - half, basePos.y, zPos - half),
            new Vector3(basePos.x + half, basePos.y, zPos - half),
            new Vector3(basePos.x + half, basePos.y, zPos + half),
            new Vector3(basePos.x - half, basePos.y, zPos + half)
        };

        Vector3 apex = new Vector3(basePos.x, basePos.y + height, zPos);

        for (int i = 0; i < 4; i++)
        {
            float p = isPlayMode ? PerspectiveCamera.Instance.GetPerspective(baseVerts[i].z) : 1f;
            baseVerts[i] = new Vector3(baseVerts[i].x * p, baseVerts[i].y * p, 0);
        }

        float ap = isPlayMode ? PerspectiveCamera.Instance.GetPerspective(apex.z) : 1f;
        apex = new Vector3(apex.x * ap, apex.y * ap, 0);

        for (int i = 0; i < 4; i++)
            DrawLine(baseVerts[i], baseVerts[(i + 1) % 4], isPlayMode);

        for (int i = 0; i < 4; i++)
            DrawLine(baseVerts[i], apex, isPlayMode);

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

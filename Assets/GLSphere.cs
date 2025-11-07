using UnityEngine;

public class GLSphere : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    public int segments = 12;
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

        for (int i = 0; i <= segments; i++)
        {
            float lat = Mathf.PI * (-0.5f + (float)i / segments);
            float y = Mathf.Sin(lat) * radius;
            float r = Mathf.Cos(lat) * radius;

            for (int j = 0; j <= segments; j++)
            {
                float lon = 2f * Mathf.PI * j / segments;
                Vector3 p1 = new Vector3(pos.x + r * Mathf.Cos(lon), pos.y + y, zPos + r * Mathf.Sin(lon));

                float lon2 = 2f * Mathf.PI * (j + 1) / segments;
                Vector3 p2 = new Vector3(pos.x + r * Mathf.Cos(lon2), pos.y + y, zPos + r * Mathf.Sin(lon2));

                if (isPlayMode)
                {
                    float p1f = PerspectiveCamera.Instance.GetPerspective(p1.z);
                    float p2f = PerspectiveCamera.Instance.GetPerspective(p2.z);
                    p1 = new Vector3(p1.x * p1f, p1.y * p1f, 0);
                    p2 = new Vector3(p2.x * p2f, p2.y * p2f, 0);
                }

                DrawLine(p1, p2, isPlayMode);
            }
        }

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

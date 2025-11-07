using UnityEngine;

public class GLCylinder : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    public float height = 2f;
    public int segments = 20;
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

        float halfH = height / 2f;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * Mathf.PI * 2f / segments;
            float angle2 = (i + 1) * Mathf.PI * 2f / segments;

            Vector3 top1 = new Vector3(pos.x + Mathf.Cos(angle1) * radius, pos.y + halfH, zPos + Mathf.Sin(angle1) * radius);
            Vector3 top2 = new Vector3(pos.x + Mathf.Cos(angle2) * radius, pos.y + halfH, zPos + Mathf.Sin(angle2) * radius);
            Vector3 bot1 = new Vector3(pos.x + Mathf.Cos(angle1) * radius, pos.y - halfH, zPos + Mathf.Sin(angle1) * radius);
            Vector3 bot2 = new Vector3(pos.x + Mathf.Cos(angle2) * radius, pos.y - halfH, zPos + Mathf.Sin(angle2) * radius);

            if (isPlayMode)
            {
                top1 = ApplyPerspective(top1);
                top2 = ApplyPerspective(top2);
                bot1 = ApplyPerspective(bot1);
                bot2 = ApplyPerspective(bot2);
            }

            DrawLine(top1, top2, isPlayMode);
            DrawLine(bot1, bot2, isPlayMode);

            DrawLine(top1, bot1, isPlayMode);
        }

        if (isPlayMode)
        {
            GL.End();
            GL.PopMatrix();
        }
    }

    private Vector3 ApplyPerspective(Vector3 point)
    {
        float p = PerspectiveCamera.Instance.GetPerspective(point.z);
        return new Vector3(point.x * p, point.y * p, 0);
    }

    private void DrawLine(Vector3 a, Vector3 b, bool useGL)
    {
        if (useGL) { GL.Vertex(a); GL.Vertex(b); }
        else Gizmos.DrawLine(a, b);
    }
}

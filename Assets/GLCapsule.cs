using UnityEngine;

public class GLCapsule : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    public float height = 3f;
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
            Gizmos.color = Color.cyan;

        float halfH = height / 2f;
        float hemiSteps = segments / 2f;

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

        for (int i = 0; i < hemiSteps; i++)
        {
            float theta1 = i * Mathf.PI / 2f / hemiSteps;
            float theta2 = (i + 1) * Mathf.PI / 2f / hemiSteps;

            float r1 = Mathf.Cos(theta1) * radius;
            float r2 = Mathf.Cos(theta2) * radius;
            float y1 = Mathf.Sin(theta1) * radius;
            float y2 = Mathf.Sin(theta2) * radius;

            for (int j = 0; j < segments; j++)
            {
                float phi1 = j * Mathf.PI * 2f / segments;
                float phi2 = (j + 1) * Mathf.PI * 2f / segments;

                Vector3 topA = new Vector3(pos.x + Mathf.Cos(phi1) * r1, pos.y + halfH + y1, zPos + Mathf.Sin(phi1) * r1);
                Vector3 topB = new Vector3(pos.x + Mathf.Cos(phi2) * r1, pos.y + halfH + y1, zPos + Mathf.Sin(phi2) * r1);
                Vector3 topC = new Vector3(pos.x + Mathf.Cos(phi1) * r2, pos.y + halfH + y2, zPos + Mathf.Sin(phi1) * r2);

                Vector3 botA = new Vector3(pos.x + Mathf.Cos(phi1) * r1, pos.y - halfH - y1, zPos + Mathf.Sin(phi1) * r1);
                Vector3 botB = new Vector3(pos.x + Mathf.Cos(phi2) * r1, pos.y - halfH - y1, zPos + Mathf.Sin(phi2) * r1);
                Vector3 botC = new Vector3(pos.x + Mathf.Cos(phi1) * r2, pos.y - halfH - y2, zPos + Mathf.Sin(phi1) * r2);

                if (isPlayMode)
                {
                    topA = ApplyPerspective(topA);
                    topB = ApplyPerspective(topB);
                    topC = ApplyPerspective(topC);
                    botA = ApplyPerspective(botA);
                    botB = ApplyPerspective(botB);
                    botC = ApplyPerspective(botC);
                }

                DrawLine(topA, topB, isPlayMode);
                DrawLine(topA, topC, isPlayMode);

                DrawLine(botA, botB, isPlayMode);
                DrawLine(botA, botC, isPlayMode);
            }
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

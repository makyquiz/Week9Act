using UnityEngine;

public class GLShapes : MonoBehaviour
{
    public Material material;

    public Vector3 sphereRot;
    public Vector3 cylinderRot;
    public Vector3 capsuleRot;
    public Vector3 pyramidRot;
    public Vector3 columnRot;

    [Header("Sphere")]
    public float sphereRadius = 1f;
    public int sphereSegments = 12;
    public Vector2 spherePos;

    [Header("Cylinder")]
    public float cylinderRadius = 1f;
    public float cylinderHeight = 2f;
    public int cylinderSegments = 20;
    public Vector2 cylinderPos;
    public float cylinderZPos;

    [Header("Capsule")]
    public float capsuleRadius = 1f;
    public float capsuleHeight = 3f;
    public int capsuleSegments = 20;
    public Vector2 capsulePos;
    public float capsuleZPos;

    [Header("Pyramid")]
    public float pyramidBase = 1f;
    public float pyramidHeight = 1.5f;
    public Vector2 pyramidBasePos;
    public float pyramidZPos;

    [Header("Rectangular Column")]
    public float columnWidth = 1f;
    public float columnHeight = 1.5f;
    public float columnDepth = 1f;
    public Vector2 columnPos;
    public float columnZPos;

    private void OnPostRender() => DrawAllShapes(true);
    private void OnDrawGizmos() => DrawAllShapes(false);

    private void DrawAllShapes(bool isPlayMode)
    {
        if (material == null) return;

        if (isPlayMode)
        {
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            material.SetPass(0);
        }

        DrawSphere(spherePos, sphereRadius, sphereSegments, isPlayMode, sphereRot);
        DrawCylinder(cylinderPos, cylinderRadius, cylinderHeight, cylinderSegments, cylinderZPos, isPlayMode, cylinderRot);
        DrawCapsule(capsulePos, capsuleRadius, capsuleHeight, capsuleSegments, capsuleZPos, isPlayMode, capsuleRot);
        DrawPyramid(pyramidBasePos, pyramidBase, pyramidHeight, pyramidZPos, isPlayMode, pyramidRot);
        DrawRectangularColumn(columnPos, columnWidth, columnHeight, columnDepth, columnZPos, isPlayMode, columnRot);

        if (isPlayMode)
        {
            GL.End();
            GL.PopMatrix();
        }
    }

    private void DrawSphere(Vector2 pos, float radius, int segments, bool isPlayMode, Vector3 angle)
    {
        if (!isPlayMode) Gizmos.color = Color.red;
        Vector3 center = new Vector3(pos.x, pos.y, 0f);

        for (int i = 0; i <= segments; i++)
        {
            float lat = Mathf.PI * (-0.5f + (float)i / segments);
            float y = Mathf.Sin(lat) * radius;
            float r = Mathf.Cos(lat) * radius;

            for (int j = 0; j <= segments; j++)
            {
                float lon = 2f * Mathf.PI * j / segments;
                Vector3 p1 = new Vector3(pos.x + r * Mathf.Cos(lon), pos.y + y, r * Mathf.Sin(lon));
                float lon2 = 2f * Mathf.PI * (j + 1) / segments;
                Vector3 p2 = new Vector3(pos.x + r * Mathf.Cos(lon2), pos.y + y, r * Mathf.Sin(lon2));

                p1 = ApplyRotation(p1, center, angle);
                p2 = ApplyRotation(p2, center, angle);

                if (isPlayMode) { p1 = ApplyPerspective(p1, p1.z); p2 = ApplyPerspective(p2, p2.z); }

                DrawLine(p1, p2, isPlayMode);
            }
        }
    }

    private void DrawCylinder(Vector2 pos, float radius, float height, int segments, float zPos, bool isPlayMode, Vector3 angle)
    {
        if (!isPlayMode) Gizmos.color = Color.green;
        float halfH = height / 2f;
        Vector3 center = new Vector3(pos.x, pos.y, zPos);

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * Mathf.PI * 2f / segments;
            float angle2 = (i + 1) * Mathf.PI * 2f / segments;

            Vector3 top1 = new Vector3(pos.x + Mathf.Cos(angle1) * radius, pos.y + halfH, zPos + Mathf.Sin(angle1) * radius);
            Vector3 top2 = new Vector3(pos.x + Mathf.Cos(angle2) * radius, pos.y + halfH, zPos + Mathf.Sin(angle2) * radius);
            Vector3 bot1 = new Vector3(pos.x + Mathf.Cos(angle1) * radius, pos.y - halfH, zPos + Mathf.Sin(angle1) * radius);
            Vector3 bot2 = new Vector3(pos.x + Mathf.Cos(angle2) * radius, pos.y - halfH, zPos + Mathf.Sin(angle2) * radius);

            top1 = ApplyRotation(top1, center, angle);
            top2 = ApplyRotation(top2, center, angle);
            bot1 = ApplyRotation(bot1, center, angle);
            bot2 = ApplyRotation(bot2, center, angle);

            if (isPlayMode)
            {
                top1 = ApplyPerspective(top1, top1.z);
                top2 = ApplyPerspective(top2, top2.z);
                bot1 = ApplyPerspective(bot1, bot1.z);
                bot2 = ApplyPerspective(bot2, bot2.z);
            }

            DrawLine(top1, top2, isPlayMode);
            DrawLine(bot1, bot2, isPlayMode);
            DrawLine(top1, bot1, isPlayMode);
        }
    }

    private void DrawCapsule(Vector2 pos, float radius, float height, int segments, float zPos, bool isPlayMode, Vector3 angle)
    {
        if (!isPlayMode) Gizmos.color = Color.cyan;
        float halfH = height / 2f;
        int hemiSteps = segments / 2;
        Vector3 center = new Vector3(pos.x, pos.y, zPos);

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * Mathf.PI * 2f / segments;
            float angle2 = (i + 1) * Mathf.PI * 2f / segments;

            Vector3 top1 = new Vector3(pos.x + Mathf.Cos(angle1) * radius, pos.y + halfH, zPos + Mathf.Sin(angle1) * radius);
            Vector3 top2 = new Vector3(pos.x + Mathf.Cos(angle2) * radius, pos.y + halfH, zPos + Mathf.Sin(angle2) * radius);
            Vector3 bot1 = new Vector3(pos.x + Mathf.Cos(angle1) * radius, pos.y - halfH, zPos + Mathf.Sin(angle1) * radius);
            Vector3 bot2 = new Vector3(pos.x + Mathf.Cos(angle2) * radius, pos.y - halfH, zPos + Mathf.Sin(angle2) * radius);

            top1 = ApplyRotation(top1, center, angle);
            top2 = ApplyRotation(top2, center, angle);
            bot1 = ApplyRotation(bot1, center, angle);
            bot2 = ApplyRotation(bot2, center, angle);

            if (isPlayMode)
            {
                top1 = ApplyPerspective(top1, top1.z);
                top2 = ApplyPerspective(top2, top2.z);
                bot1 = ApplyPerspective(bot1, bot1.z);
                bot2 = ApplyPerspective(bot2, bot2.z);
            }

            DrawLine(top1, top2, isPlayMode);
            DrawLine(bot1, bot2, isPlayMode);
            DrawLine(top1, bot1, isPlayMode);
        }

        for (int i = 0; i <= hemiSteps; i++)
        {
            float theta1 = Mathf.PI / 2f * i / hemiSteps;
            float theta2 = Mathf.PI / 2f * (i + 1) / hemiSteps;
            float y1 = Mathf.Sin(theta1) * radius; float r1 = Mathf.Cos(theta1) * radius;
            float y2 = Mathf.Sin(theta2) * radius; float r2 = Mathf.Cos(theta2) * radius;

            for (int j = 0; j < segments; j++)
            {
                float phi1 = j * 2f * Mathf.PI / segments;
                float phi2 = (j + 1) * 2f * Mathf.PI / segments;

                Vector3 topA = new Vector3(pos.x + Mathf.Cos(phi1) * r1, pos.y + halfH + y1, zPos + Mathf.Sin(phi1) * r1);
                Vector3 topB = new Vector3(pos.x + Mathf.Cos(phi2) * r1, pos.y + halfH + y1, zPos + Mathf.Sin(phi2) * r1);
                Vector3 topC = new Vector3(pos.x + Mathf.Cos(phi1) * r2, pos.y + halfH + y2, zPos + Mathf.Sin(phi1) * r2);

                Vector3 botA = new Vector3(pos.x + Mathf.Cos(phi1) * r1, pos.y - halfH - y1, zPos + Mathf.Sin(phi1) * r1);
                Vector3 botB = new Vector3(pos.x + Mathf.Cos(phi2) * r1, pos.y - halfH - y1, zPos + Mathf.Sin(phi2) * r1);
                Vector3 botC = new Vector3(pos.x + Mathf.Cos(phi1) * r2, pos.y - halfH - y2, zPos + Mathf.Sin(phi1) * r2);

                topA = ApplyRotation(topA, center, angle);
                topB = ApplyRotation(topB, center, angle);
                topC = ApplyRotation(topC, center, angle);
                botA = ApplyRotation(botA, center, angle);
                botB = ApplyRotation(botB, center, angle);
                botC = ApplyRotation(botC, center, angle);

                if (isPlayMode)
                {
                    topA = ApplyPerspective(topA, topA.z);
                    topB = ApplyPerspective(topB, topB.z);
                    topC = ApplyPerspective(topC, topC.z);
                    botA = ApplyPerspective(botA, botA.z);
                    botB = ApplyPerspective(botB, botB.z);
                    botC = ApplyPerspective(botC, botC.z);
                }

                DrawLine(topA, topB, isPlayMode);
                DrawLine(topA, topC, isPlayMode);
                DrawLine(botA, botB, isPlayMode);
                DrawLine(botA, botC, isPlayMode);
            }
        }
    }

    private void DrawPyramid(Vector2 basePos, float baseSize, float height, float zPos, bool isPlayMode, Vector3 angle)
    {
        if (!isPlayMode) Gizmos.color = Color.yellow;
        float half = baseSize * 0.5f;
        Vector3 center = new Vector3(basePos.x, basePos.y, zPos);

        Vector3[] baseVerts = new Vector3[4]
        {
            new Vector3(-half, 0, -half),
            new Vector3(half, 0, -half),
            new Vector3(half, 0, half),
            new Vector3(-half, 0, half)
        };
        Vector3 apex = new Vector3(0, height, 0);

        for (int i = 0; i < baseVerts.Length; i++)
            baseVerts[i] = ApplyRotation(baseVerts[i] + center, center, angle);
        apex = ApplyRotation(apex + center, center, angle);

        if (isPlayMode)
        {
            for (int i = 0; i < baseVerts.Length; i++)
                baseVerts[i] = ApplyPerspective(baseVerts[i], baseVerts[i].z);
            apex = ApplyPerspective(apex, apex.z);
        }

        for (int i = 0; i < 4; i++)
            DrawLine(baseVerts[i], baseVerts[(i + 1) % 4], isPlayMode);
        for (int i = 0; i < 4; i++)
            DrawLine(baseVerts[i], apex, isPlayMode);
    }

    private void DrawRectangularColumn(Vector2 pos, float width, float height, float depth, float zPos, bool isPlayMode, Vector3 angle)
    {
        if (!isPlayMode) Gizmos.color = Color.magenta;
        float hw = width / 2f;
        float hh = height / 2f;
        float hd = depth / 2f;
        Vector3 center = new Vector3(pos.x, pos.y, zPos);

        Vector3[] verts = new Vector3[8]
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

        for (int i = 0; i < verts.Length; i++)
            verts[i] = ApplyRotation(verts[i], center, angle);
        if (isPlayMode) for (int i = 0; i < verts.Length; i++) verts[i] = ApplyPerspective(verts[i], verts[i].z);

        int[,] edges = {
            {0,1},{1,2},{2,3},{3,0},
            {4,5},{5,6},{6,7},{7,4},
            {0,4},{1,5},{2,6},{3,7}
        };

        for (int i = 0; i < edges.GetLength(0); i++)
            DrawLine(verts[edges[i, 0]], verts[edges[i, 1]], isPlayMode);
    }

    private Vector3 ApplyPerspective(Vector3 point, float z)
    {
        float p = PerspectiveCamera.Instance.GetPerspective(z);
        return new Vector3(point.x * p, point.y * p, 0);
    }

    private Vector3 ApplyRotation(Vector3 point, Vector3 center, Vector3 eulerAngles)
    {
        Vector3 p = point - center;
        Quaternion rot = Quaternion.Euler(eulerAngles);
        p = rot * p;
        return p + center;
    }

    private void DrawLine(Vector3 a, Vector3 b, bool useGL)
    {
        if (useGL) { GL.Vertex(a); GL.Vertex(b); }
        else Gizmos.DrawLine(a, b);
    }
}

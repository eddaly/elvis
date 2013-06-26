using System;
using UnityEngine;
using UnityEditor;

public class Drawing
{
    public static Texture2D aaLineTex = null;
    public static Texture2D lineTex = null;
    static Vector3 startVertex;
    static Vector3 endVertex;
    public static Material ms_materialForLine;


    /// <summary>
    /// TODO: This is *very* unoptimal and should be revisited if performance problems appear in the DataLevel view.
    /// Need to avoid push/popping view matrix, avoid dupliicating the points and batch items together
    /// </summary>
    /// <param name="containingViewRect"></param>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    /// <param name="color"></param>
    /// <param name="width"></param>
    /// <param name="antiAlias"></param>
    public static void DrawLine(Rect containingViewRect, Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
    {
        if (ms_materialForLine == null)
        {
            ms_materialForLine = new Material(Shader.Find("Diffuse"));
            if (ms_materialForLine == null)
            {
                Debug.LogError("Could not find the shader to draw a line with!");
            }
        }
        GL.PushMatrix();
        ms_materialForLine.SetPass(0);
        ms_materialForLine.color = color;
        GL.LoadOrtho();

        //Matrix4x4 modelViewToUse = Matrix4x4.Scale(new Vector3(1.0f / containingViewRect.width, -1.0f / containingViewRect.height, 1.0f));
        //modelViewToUse.m31 = 0.0f;// -containingViewRect.height;
        //GL.modelview = modelViewToUse;
        GL.modelview = GUI.matrix;
        GL.Begin(GL.LINES);
        GL.Color(color);

        startVertex = new Vector3(pointA.x - containingViewRect.x, pointA.y - containingViewRect.y, 0.0f);
        endVertex = new Vector3(pointB.x - containingViewRect.x, pointB.y - containingViewRect.y, 0.0f);

        // Horrible temporary fix as couldn't figure out why the matrix setting wasn't doing this!?  Need to come back to this!
        startVertex.x /= containingViewRect.width;
        startVertex.y = 1.0f - (startVertex.y / containingViewRect.height); 

        endVertex.x /= containingViewRect.width;
        endVertex.y = 1.0f - (endVertex.y / containingViewRect.height);

        GL.Vertex(startVertex);
        GL.Vertex(endVertex);
        GL.End();
        GL.PopMatrix();
    }
    public static void DrawLineSmooth(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
    {
        Color savedColor = GUI.color;
        Matrix4x4 savedMatrix = GUI.matrix;
        //GUI.matrix = Matrix4x4.identity;


        if (!lineTex)
        {
            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
            lineTex.SetPixel(0, 1, Color.white);
            lineTex.Apply();
        }
        if (!aaLineTex)
        {
            aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, true);
            aaLineTex.SetPixel(0, 0, new Color(1, 1, 1, 0));
            aaLineTex.SetPixel(0, 1, Color.white);
            aaLineTex.SetPixel(0, 2, new Color(1, 1, 1, 0));
            aaLineTex.Apply();
        }

        if (antiAlias) width *= 3;
        float angle = Vector3.Angle(pointB - pointA, Vector2.right) * (pointA.y <= pointB.y?1:-1);
        float m = (pointB - pointA).magnitude;
        if (m > 0.01f)
        {
            Vector3 dz = new Vector3(pointA.x, pointA.y, 0);

            GUI.color = color;
            GUI.matrix = translationMatrix(dz) * GUI.matrix;
            GUIUtility.ScaleAroundPivot(new Vector2(m, width), new Vector3(-0.5f, 0, 0));
            GUI.matrix = translationMatrix(-dz) * GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, Vector2.zero);
            GUI.matrix = translationMatrix(dz + new Vector3(width / 2, -m / 2) * Mathf.Sin(angle * Mathf.Deg2Rad)) * GUI.matrix;

            //GUI.DrawTexture(new Rect(0, 0, 100, 100), lineTex, ScaleMode.ScaleToFit);

            if (!antiAlias)
            {
                //GUI.Label(new Rect(0, 0, 1, 1), lineTex);
                GUI.DrawTexture(new Rect(0, 0, 1, 1), lineTex);
            }
            else
            {
                //GUI.Label(new Rect(0, 0, 8, 8), aaLineTex);
                GUI.DrawTexture(new Rect(0, 0, 1, 1), aaLineTex, ScaleMode.ScaleToFit);
            }
        }
        GUI.matrix = savedMatrix;
        GUI.color = savedColor;
    }

    public static void bezierLine(Rect containingViewRect, Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
    {
        Vector2 lastV = cubeBezier(start, startTangent, end, endTangent, 0);
        for (int i = 1; i <= segments; ++i)
        {
            Vector2 v = cubeBezier(start, startTangent, end, endTangent, i/(float)segments);

            Drawing.DrawLine( containingViewRect,
                lastV,
                v,
                color, width, antiAlias);
            lastV = v;
        }
    }

    private static Vector2 cubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t){
        float rt = 1-t;
        float rtt = rt * t;
        return rt*rt*rt * s + 3 * rt * rtt * st + 3 * rtt * t * et + t*t*t* e;
    }

    private static Matrix4x4 translationMatrix(Vector3 v)
    {
        return Matrix4x4.TRS(v,Quaternion.identity,Vector3.one);
    }
}

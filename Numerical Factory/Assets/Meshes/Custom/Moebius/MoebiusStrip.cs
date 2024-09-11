using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoebiusStrip : MonoBehaviour
{
    public Material material;

    public int numberOfParts;
    public int divisionsHoritzontals = 1;

    public float radi = 1;
    public float amplada = 1;

    [Header("Click to save mesh")]
    public bool save = false;


    private float u=0;
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();

        int numVertex = numberOfParts * (1 + divisionsHoritzontals);
        Vector3[] vertices = new Vector3[numVertex];

        for(int i=0; i<numberOfParts * (1 + divisionsHoritzontals); i = i + 1 + divisionsHoritzontals)
        {
            u = 2 * Mathf.PI * i / (float)(numberOfParts * (1 + divisionsHoritzontals)); 

            for(int k=0; k<=divisionsHoritzontals; k++)
            {
                float divisio = 1-2*k/(float) divisionsHoritzontals;
                vertices[i+k] = new Vector3(x(divisio, u), y(divisio, u), z(divisio, u));
                Debug.Log("x:"+ x(divisio, u) + "y:"+ z(divisio, u) + "z:"+ y(divisio, u));
            }
        }

        mesh.vertices = vertices;

        int numTriangles = 12 * numberOfParts * divisionsHoritzontals;
        int[] triangles = new int[numTriangles];

        int a;
        int b;
        int columna = 0;
        int fila = 0;
        for (int i = 0; i < numTriangles/2; i += divisionsHoritzontals * 6)
        {
            for(int j = 0;  j < divisionsHoritzontals * 6; j += 6)
            {
                a = i + j;
                b = fila + columna * (divisionsHoritzontals + 1);
                triangles[a] = b;
                Debug.Log("triangles["+ a +"]=" + b);

                a = i + j + 1;
                b = fila + 1 + columna * (divisionsHoritzontals + 1);
                triangles[a] = b;
                Debug.Log("triangles[" +a + "]=" + b);

                a = i + j + 3;
                b = fila + columna * (divisionsHoritzontals + 1);
                triangles[a] = b;
                Debug.Log("triangles[" + a + "]=" + b);

                if ((columna + 1) < numberOfParts)
                {
                    a = i + j + 2;
                    b = fila + 1 + (columna + 1) * (divisionsHoritzontals + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b);

                    a = i + j + 4;
                    b = fila + 1 + (columna + 1) * (divisionsHoritzontals + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b);

                    a = i + j + 5;
                    b = fila + (columna + 1) * (divisionsHoritzontals + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b);
                }
                else
                {
                    a = i + j + 2;
                    b = divisionsHoritzontals - (fila + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b + "ELSE");

                    a = i + j + 4;
                    b = divisionsHoritzontals - (fila + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b + "ELSE");

                    a = i + j + 5;
                    b = divisionsHoritzontals - fila;
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b + "ELSE");
                }

                //Al revés

                a = i + j + numTriangles / 2;
                b = fila + columna * (divisionsHoritzontals + 1);
                triangles[a] = b;
                Debug.Log("triangles[" + a + "]=" + b);

                a = i + j + 2 + numTriangles / 2;
                b = fila + 1 + columna * (divisionsHoritzontals + 1);
                triangles[a] = b;
                Debug.Log("triangles[" + a + "]=" + b);

                a = i + j + 3 + numTriangles / 2;
                b = fila + columna * (divisionsHoritzontals + 1);
                triangles[a] = b;
                Debug.Log("triangles[" + a + "]=" + b);

                if ((columna + 1) * (divisionsHoritzontals + 1) < numVertex)
                {
                    a = i + j + 1 + numTriangles / 2;
                    b = fila + 1 + (columna + 1) * (divisionsHoritzontals + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b);

                    a = i + j + 4 + numTriangles / 2;
                    b = fila + (columna + 1) * (divisionsHoritzontals + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b);

                    a = i + j + 5 + numTriangles / 2;
                    b = fila + 1 + (columna + 1) * (divisionsHoritzontals + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b);
                }
                else
                {
                    a = i + j + 1 + numTriangles / 2;
                    b = divisionsHoritzontals - (fila + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b + "ELSE");

                    a = i + j + 4 + numTriangles / 2;
                    b = divisionsHoritzontals - fila;
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b + "ELSE");

                    a = i + j + 5 + numTriangles / 2;
                    b = divisionsHoritzontals - (fila + 1);
                    triangles[a] = b;
                    Debug.Log("triangles[" + a + "]=" + b + "ELSE");
                }
                fila++;
            }
            fila = 0;
            columna++;
        }

        mesh.triangles = triangles;

        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        mesh.uv = uv;
        //mesh.RecalculateNormals();

        GetComponent<MeshRenderer>().material = material;

        GetComponent<MeshFilter>().mesh = mesh;

        //m = mesh;

        Save(mesh);

    }

    public float GeneralXY(float v, float u)
    {
        float g=radi+amplada*(v/(float)2)*Mathf.Cos(u/(float)2);

        return g;
    }

    public float x(float v, float u)
    {
        float x = GeneralXY(v,u) * Mathf.Cos((float)u);
        return x;
    }

    public float y(float v, float u)
    {
        float y = GeneralXY(v, u) * Mathf.Sin((float)u);
        return y;
    }

    public float z(float v, float u)
    {
        float z =amplada*(v/(float)2) *Mathf.Sin(u/ (float)(2));
        return z;
    }

    public void Save(Mesh mesh)
    {
        string parts = numberOfParts.ToString();
        string divisions = divisionsHoritzontals.ToString();
        float ratio = radi / (float)amplada;
        string ratioRadiAmplada = ratio.ToString();

        string nom = "Moebius_" + parts + "_" + divisions + "_" + ratio;

        AssetDatabase.CreateAsset(mesh, "Assets/Meshes/Custom/Moebius/" + nom + ".asset");
        AssetDatabase.SaveAssets();
    }
}

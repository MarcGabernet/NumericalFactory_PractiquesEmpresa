using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HexagonMesh : MonoBehaviour
{
    public Material material;

    [Range(0, 0.5f)]
    public float height = 0.4f;
    [Range(0, 0.5f)]
    public float heightBase = 0.3f;

    [Range(0, 1)]
    public float surfaceDecrease = 0.1f;

    [Header("Click to save mesh")]
    public bool save = false;

    private readonly float deg = 60;

    void Update()
    {

        Mesh hitbox = Create(height, height, 0);

        hitbox.RecalculateNormals();

        Mesh render = Create(height, heightBase, surfaceDecrease);

        Vector2[] uv = new Vector2[render.vertices.Length];
        for (int i = 0; i < render.vertices.Length; i++)
        {
            uv[i] = new Vector2(render.vertices[i].x, render.vertices[i].z);
        }
        render.uv = uv;
        render.RecalculateNormals();

        GetComponent<MeshRenderer>().material = material;

        GetComponent<MeshFilter>().mesh = render;

        if (save)
        {
            Save(render,hitbox);
            Debug.Log("Saved!");
            save = false;
        }
    }

    public void Save(Mesh render, Mesh hitbox)
    {
        string stringHeight = height.ToString();
        string stringBaseHeight = heightBase.ToString();
        string stringSurfaceDecrease = surfaceDecrease.ToString();

        string nom = "Tile_" + stringHeight + "_" + stringBaseHeight + "_" + stringSurfaceDecrease;
        string hitboxNom = "Hitbox_" + stringHeight; 

        AssetDatabase.CreateAsset(render, "Assets/Meshes/Custom/Hexagon/" + nom + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.CreateAsset(hitbox, "Assets/Meshes/Custom/Hexagon/" + hitboxNom + ".asset");
        AssetDatabase.SaveAssets();
    }

    public Mesh Create(float height, float heightBase, float surfaceDecrease)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[26];

        for (int i = 0; i <= 1; i++)
        {
            vertices[0 + i * 19] = new Vector3(0, (1 - 2 * i) * height, 0);
            vertices[1 + i * 19] = new Vector3(-0.5f * (1 - surfaceDecrease), (1 - 2 * i) * height, Mathf.Cos(deg) * (1 - surfaceDecrease));
            vertices[2 + i * 19] = new Vector3(surfaceDecrease - 1, (1 - 2 * i) * height, 0);
            vertices[3 + i * 19] = new Vector3(-0.5f * (1 - surfaceDecrease), (1 - 2 * i) * height, -Mathf.Cos(deg) * (1 - surfaceDecrease));
            vertices[4 + i * 19] = new Vector3(0.5f * (1 - surfaceDecrease), (1 - 2 * i) * height, -Mathf.Cos(deg) * (1 - surfaceDecrease));
            vertices[5 + i * 19] = new Vector3(1 - surfaceDecrease, (1 - 2 * i) * height, 0);
            vertices[6 + i * 19] = new Vector3(0.5f * (1 - surfaceDecrease), (1 - 2 * i) * height, Mathf.Cos(deg) * (1 - surfaceDecrease));
        }

        for (int i = 0; i <= 1; i++)
        {
            vertices[7 + 6 * i] = new Vector3(-0.5f, (1 - 2 * i) * heightBase, Mathf.Cos(deg));
            vertices[8 + 6 * i] = new Vector3(-1, (1 - 2 * i) * heightBase, 0);
            vertices[9 + 6 * i] = new Vector3(-0.5f, (1 - 2 * i) * heightBase, -Mathf.Cos(deg));
            vertices[10 + 6 * i] = new Vector3(0.5f, (1 - 2 * i) * heightBase, -Mathf.Cos(deg));
            vertices[11 + 6 * i] = new Vector3(1, (1 - 2 * i) * heightBase, 0);
            vertices[12 + 6 * i] = new Vector3(0.5f, (1 - 2 * i) * heightBase, Mathf.Cos(deg));
        }


        mesh.vertices = vertices;
        mesh.triangles = new int[] {0, 1, 2,
                                    0, 2, 3,
                                    0, 3, 4,
                                    0, 4, 5,
                                    0, 5, 6,
                                    0, 6, 1,
                                    1, 8, 2,
                                    2, 9, 3,
                                    3, 10, 4,
                                    4, 11, 5,
                                    5, 12, 6,
                                    6, 7, 1,
                                    1, 7, 8,
                                    2, 8, 9,
                                    3, 9, 10,
                                    4, 10, 11,
                                    5, 11, 12,
                                    6, 12, 7,

                                    10, 15, 16,
                                    11, 10, 16,
                                    11, 16, 17,
                                    12, 11, 17,
                                    12, 17, 18,
                                    7, 12, 18,
                                    7, 18, 13,
                                    13, 8, 7,
                                    13, 14, 8,
                                    14, 9, 8,
                                    14, 15, 9,
                                    15, 10, 9,

                                    13, 20, 14,
                                    14, 21, 15,
                                    15, 22, 16,
                                    16, 23, 17,
                                    17, 24, 18,
                                    18, 25, 13,
                                    14, 20, 21,
                                    15, 21, 22,
                                    16, 22, 23,
                                    17, 23, 24,
                                    18, 24, 25,
                                    13, 25, 20,
                                    19, 21, 20,
                                    19, 22, 21,
                                    19, 23, 22,
                                    19, 24, 23,
                                    19, 25, 24,
                                    19, 20, 25};

        return mesh;
    }
}

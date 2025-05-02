using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProceduralTerrain : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public float amplitude = 5f;
    public float scale = 0.1f;

    private Vector3[] vertices;
    private int[] triangles;
    void Start()
    {
        Mesh mesh = new Mesh();
        vertices = new Vector3[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                vertices[i * width + j] = new Vector3(i, Mathf.PerlinNoise(vertices[i].x * scale, vertices[i].z * scale) * amplitude, j);
            }
        }

        GenerateTriangles();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

    }
    void GenerateTriangles()
    {
        int[] triangles = new int[(width - 1) * (height - 1) * 6]; // 6 indices par carré (2 triangles)
        int triIndex = 0;

        for (int z = 0; z < height - 1; z++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                int current = z * width + x;
                int next = (z + 1) * width + x;

                // Triangle 1
                triangles[triIndex] = current;
                triangles[triIndex + 1] = next;
                triangles[triIndex + 2] = current + 1;

                // Triangle 2
                triangles[triIndex + 3] = current + 1;
                triangles[triIndex + 4] = next;
                triangles[triIndex + 5] = next + 1;

                triIndex += 6;
            }
        }
    }
}

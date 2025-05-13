using System.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class ProceduralTerrain : MonoBehaviour
{
    public int highResWidth = 50;
    public int highResHeight = 50;
    public int lowResWidth = 20;
    public int lowResHeight = 20;

    public float amplitude = 5f;
    public float scale = 0.1f;
    public float radius = 5f;
    public float deformationStrength = 1f;
    public float LODDistance = 30f;

    private Vector3[] highResVertices;
    private Vector3[] lowResVertices;
    private int[] triangles;

    private Mesh highResMesh;
    private Mesh lowResMesh;

    private NativeArray<Vector3> nativeVertices;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Transform mainCamera;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mainCamera = Camera.main.transform;

        float terrainSize = 50f;
        highResMesh = GenerateMesh(highResWidth, highResHeight, terrainSize, out highResVertices);
        lowResMesh = GenerateMesh(lowResWidth, lowResHeight, terrainSize, out lowResVertices);

        // Initialiser les données natives pour la haute résolution uniquement
        nativeVertices = new NativeArray<Vector3>(highResVertices.Length, Allocator.Persistent);
        nativeVertices.CopyFrom(highResVertices);

        // Appliquer le mesh haute résolution par défaut
        meshFilter.mesh = highResMesh;
        meshCollider.sharedMesh = highResMesh;
    }

    void Update()
    {
        float distance = Vector3.Distance(mainCamera.position, transform.position);

        if (distance > LODDistance)
        {
            if (meshFilter.sharedMesh != lowResMesh)
            {
                meshFilter.mesh = lowResMesh;
                meshCollider.sharedMesh = lowResMesh;
            }
        }
        else
        {
            if (meshFilter.sharedMesh != highResMesh)
            {
                meshFilter.mesh = highResMesh;
                meshCollider.sharedMesh = highResMesh;
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 hitPoint = transform.InverseTransformPoint(hit.point);
                    DeformTerrain(hitPoint);
                }
            }
        }
    }

    void DeformTerrain(Vector3 hitPoint)
    {
        DeformJob deformJob = new DeformJob
        {
            vertices = nativeVertices,
            hitPoint = hitPoint,
            radius = radius,
            strength = deformationStrength
        };

        JobHandle handle = deformJob.Schedule(nativeVertices.Length, 64);
        handle.Complete();

        nativeVertices.CopyTo(highResVertices);
        highResMesh.vertices = highResVertices;
        highResMesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = highResMesh;
    }

    Mesh GenerateMesh(int resolutionWidth, int resolutionHeight, float size, out Vector3[] outVertices)
    {
        Vector3[] vertices = new Vector3[(resolutionWidth + 1) * (resolutionHeight + 1)];
        int[] tris = new int[resolutionWidth * resolutionHeight * 6];

        for (int i = 0; i <= resolutionWidth; i++)
        {
            for (int j = 0; j <= resolutionHeight; j++)
            {
                // Remap les positions pour garder une surface de même taille
                float xPos = ((float)i / resolutionWidth) * size;
                float zPos = ((float)j / resolutionHeight) * size;
                float y = Mathf.PerlinNoise(xPos * scale, zPos * scale) * amplitude;
                vertices[i * (resolutionHeight + 1) + j] = new Vector3(xPos, y, zPos);
            }
        }

        int t = 0;
        for (int y = 0; y < resolutionHeight; y++)
        {
            for (int x = 0; x < resolutionWidth; x++)
            {
                int bottomLeft = y * (resolutionWidth + 1) + x;
                int topLeft = (y + 1) * (resolutionWidth + 1) + x;
                int topRight = (y + 1) * (resolutionWidth + 1) + (x + 1);
                int bottomRight = y * (resolutionWidth + 1) + (x + 1);

                tris[t++] = topLeft;
                tris[t++] = bottomLeft;
                tris[t++] = topRight;

                tris[t++] = topRight;
                tris[t++] = bottomLeft;
                tris[t++] = bottomRight;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        outVertices = vertices;
        return mesh;
    }

    [BurstCompile]
    public struct DeformJob : IJobParallelFor
    {
        public NativeArray<Vector3> vertices;
        public Vector3 hitPoint;
        public float radius;
        public float strength;

        public void Execute(int index)
        {
            Vector3 vertex = vertices[index];
            float distance = Vector3.Distance(vertex, hitPoint);
            if (distance < radius)
            {
                vertex.y += strength;
                vertices[index] = vertex;
            }
        }
    }

    void OnDestroy()
    {
        if (nativeVertices.IsCreated)
            nativeVertices.Dispose();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

//[InitializeOnLoad]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MarchingCubes : MonoBehaviour
{
    [SerializeField] public int width = 10;
    [SerializeField] public int height = 10;

    [SerializeField] float noiseScale = 6;

    [SerializeField] [Range(2, 4)] int falloff = 2;
    [SerializeField] [Range(0, 1)] float holeIntensity = 0.5f;

    [SerializeField][Range(0,1)] private float heightThreshold = 0.5f;

    [SerializeField] bool visualizeNoise;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private float[,,] heights;


    private MeshFilter meshFilter;
    private Mesh LodMesh0;
    private Mesh LodMesh1;
    private Mesh LodMesh2;


    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        //Generate();
        //StartCoroutine(TestAll());
    }

    public void AssignValues(int _width, int _height, int _falloff, float _holeIntensity, float _heighThreshold, int _noiseScale)
    {
        width = _width;
        height = _height;
        falloff = _falloff;
        holeIntensity = _holeIntensity;
        heightThreshold = _heighThreshold;
        noiseScale = _noiseScale;
    }

    public void Generate()
    {
        SetHeights();
        MarchCubes();
        GenerateMeshes();
        SetMesh();
    }
    public void GenerateMeshes()
    {
        Mesh mesh = new Mesh();
        //Debug.Log("Mesh generated");
        //mesh.vertices = vertices.ToArray();
        //mesh.triangles = triangles.ToArray();
        //mesh.triangles = mesh.triangles.Reverse().ToArray();

        //mesh.RecalculateNormals();
        //LodMesh1 = mesh;

        //width = Mathf.RoundToInt(10 * 0.5f);
        //height = Mathf.RoundToInt(10 * 0.5f);
        //gameObject.transform.localScale = Vector3.one * 10 * (1 / 0.5f);

        //SetHeights();
        //MarchCubes();

        //mesh = new Mesh();

        //mesh.vertices = vertices.ToArray();
        //mesh.triangles = triangles.ToArray();
        //mesh.triangles = mesh.triangles.Reverse().ToArray();

        //mesh.RecalculateNormals();

        //LodMesh0 = mesh;

        //width = Mathf.RoundToInt(10 * 2.0f);
        //height = Mathf.RoundToInt(10 * 2.0f);
        //gameObject.transform.localScale = Vector3.one * 10 * (1 / 2.0f);

        //SetHeights();
        //MarchCubes();

        mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.triangles = mesh.triangles.Reverse().ToArray();

        mesh.RecalculateNormals();

        LodMesh2 = mesh;
    }
    private IEnumerator TestAll()
    {
        while (true)
        {
            SetHeights();
            MarchCubes();
            //SetMesh();
            yield return new WaitForSeconds(1f);
        }
    }

    public void SetMesh(int LODLevel)
    {
        return;
   
    }

    private void SetMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        //offsetVertices();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void offsetVertices()
    {
        for (int i = 0; i < vertices.Count(); i++)
        {
            vertices[i] = new Vector3(vertices[i].x, vertices[i].y + height / 2, vertices[i].z);
        }
    }


    private void SetHeights()
    {
        heights = new float[width + 1, height + 1, width + 1];

        for (int x = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height + 1; y++)
            {
                for (int z = 0; z < width + 1; z++)
                {
                    float noise = PerlinNoise3D((float)x / width * noiseScale, (float)y / height * noiseScale, (float)z / width * noiseScale);
                    float currentHeight = CenterFalloff((float)x, (float)y, (float)z);

                    if (noise < holeIntensity)
                        currentHeight = 0;
                    else
                        currentHeight *= noise;

                    heights[x, y, z] = currentHeight;

                }
            }
        }
    }

    // Function to create 3d perlin noise:
    private float PerlinNoise3D(float x, float y, float z)
    {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);

        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);

        return (xy + xz + yz + yx + zx + zy) / 6;
    }

    private float CenterFalloff(float x, float y, float z)
    {
        float xMiddle = width / 2;
        float zMiddle = width / 2;

        float xDist = Mathf.Abs(xMiddle - x);
        float yDist = Mathf.Abs(height - y);
        float zDist = Mathf.Abs(zMiddle - z);

        float totDist = Mathf.Sqrt(xDist * xDist + zDist * zDist);
        float longestDistPossible = Mathf.Sqrt(xMiddle * xMiddle + zMiddle * zMiddle);

        float heightModifier = ((yDist / height));


        return Mathf.Pow((1-(totDist / longestDistPossible)), falloff) * heightModifier;
        //return Random.Range(0,noiseScale);
    }

    // Based on the values should the corners of the cube be rendered:
    // If the values are greter than our threshhold the vertex should be
    // considered in the triangle generation:
    private int GetConfigIndex(float[] cubeCorners)
    {
        int configIndex = 0;

        for (int i = 0; i < 8; i++)
        {
            if (cubeCorners[i] > heightThreshold)
            {
                configIndex |= 1 << i;
            }
        }

        return configIndex;
    }

    private void MarchCubes()
    {
        vertices.Clear();
        triangles.Clear();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    float[] cubeCorners = new float[8];

                    for (int i = 0; i < 8; i++)
                    {
                        Vector3Int corner = new Vector3Int(x, y, z) + MarchingTable.Corners[i];
                        cubeCorners[i] = heights[corner.x, corner.y, corner.z];
                    }

                    MarchCube(new Vector3(x, y, z), cubeCorners);
                }
            }
        }
    }

    private void MarchCube(Vector3 position, float[] cubeCorners)
    {
        int configIndex = GetConfigIndex(cubeCorners);

        if (configIndex == 0 || configIndex == 255)
        {
            return;
        }

        // Look up the edge and based of the configuration of the verecies
        // choose the right triangle form the table:
        int edgeIndex = 0;
        for (int t = 0; t < 5; t++)
        {
            for (int v = 0; v < 3; v++)
            {
                int triTableValue = MarchingTable.Triangles[configIndex, edgeIndex];

                if (triTableValue == -1)
                {
                    return;
                }

                // Create the vertex in the center of start and end:
                Vector3 edgeStart = position + MarchingTable.Edges[triTableValue, 0];
                Vector3 edgeEnd = position + MarchingTable.Edges[triTableValue, 1];

                Vector3 vertex = (edgeStart + edgeEnd) / 2;

                vertices.Add(vertex);
                triangles.Add(vertices.Count - 1);

                edgeIndex++;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (!visualizeNoise || !Application.isPlaying)
        {
            return;
        }

        for (int x = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height + 1; y++)
            {
                for (int z = 0; z < width + 1; z++)
                {
                    Gizmos.color = new Color(heights[x, y, z], heights[x, y, z], heights[x, y, z], 1);
                    Gizmos.DrawSphere(new Vector3(x * noiseScale, y * noiseScale, z * noiseScale), 0.2f * noiseScale);
                }
            }
        }
    }
}

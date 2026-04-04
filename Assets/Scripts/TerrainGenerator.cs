using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class TerrainGenerator : MonoBehaviour
{
  public int Width;
  public int Depth;
  public int Resolution;
  public float Frequency;
  public float Amplitude;

  private Mesh mesh;
  private MeshCollider mc;
  private List<Vector3> vertices = new List<Vector3>();
  private List<int> triangles = new List<int>();
  private List<Vector2> uvs = new List<Vector2>();

  private float noiseOffsetX;
  private float noiseOffsetZ;

  void Start()
  {
    noiseOffsetX = Random.Range(0, 9999f);
    noiseOffsetZ = Random.Range(0, 9999f);

    mesh = GetComponent<MeshFilter>().mesh;
    GenerateTesselatedPlane(Resolution, Resolution);
    mesh.vertices = vertices.ToArray();
    mesh.uv = uvs.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.RecalculateNormals();

    mc = GetComponent<MeshCollider>();
    mc.sharedMesh = mesh;
  }

  private void GenerateTesselatedPlane(int xSquares, int zSquares)
  {
    vertices.Clear();
    uvs.Clear();
    triangles.Clear();

    float xStep = (float)Width / xSquares;
    float zStep = (float)Depth / zSquares;

    for (int z = 0; z <= zSquares; z++)
      for (int x = 0; x <= xSquares; x++)
      {
        float wx = x * xStep;
        float wz = z * zStep;
        float y = GetHeight(wx, wz);

        vertices.Add(new Vector3(wx - Width * .5f, y, wz - Width * .5f));
        uvs.Add(new Vector2((float)x / xSquares, (float)z / zSquares));
      }

    // Create triangles
    for (int z = 0; z < zSquares; z++)
    {
      for (int x = 0; x < xSquares; x++)
      {
        int vertIndex = z * (xSquares + 1) + x;

        // First triangle
        triangles.Add(vertIndex);
        triangles.Add(vertIndex + xSquares + 1);
        triangles.Add(vertIndex + 1);

        // Second triangle
        triangles.Add(vertIndex + 1);
        triangles.Add(vertIndex + xSquares + 1);
        triangles.Add(vertIndex + xSquares + 2);
      }
    }
  }

  private float GetHeight(float x, float z)
  {
    float freq = Frequency;
    float amp = Amplitude;
    float y = 0f;
    float maxY = 0f;

    for (int i = 0; i < 5; i++)
    {
      y += Mathf.PerlinNoise((x + noiseOffsetX) * freq,
        (z + noiseOffsetZ) * freq) * amp;

      maxY += amp;
      freq *= 2f;
      amp *= 0.5f;
    }

    y /= maxY;

    y = y * y * (3f - 2f * y);
    //y = Mathf.Pow(y, 1.5f);

    return y * Amplitude;
  }
}

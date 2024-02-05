using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainComputeShader : MonoBehaviour
{
  public Texture2D noiseTexture;
  public ComputeShader computeShader;
  public Material chunkMaterial;
  public int size = 512;
  public int chunkSize = 64;
  public float heightMultiplier = 25f;

  private float[] data;
  private int numChunks;

  private void Start() {
    data = new float[size * size];

    RandomizeTerrain();
    Vector3[] allVertices = GenerateVertices();

    numChunks = size / chunkSize;

    for (int chunkZ = 0; chunkZ < numChunks; chunkZ++) {
      for (int chunkX = 0; chunkX < numChunks; chunkX++) {
        CreateChunk(chunkX, chunkZ, allVertices);
      }
    }
  }

  private Vector3[] GenerateVertices() {
    Vector3[] allVertices = new Vector3[size * size];

    for (int zPos = 0; zPos < size; zPos++) {
      for (int xPos = 0; xPos < size; xPos++) {
        int vertexIndex = zPos * size + xPos;
        float height = data[vertexIndex] * heightMultiplier;
        allVertices[vertexIndex] = new Vector3(xPos, height, zPos);
      }
    }

    return allVertices;
  }
  private void CreateChunk(int chunkX, int chunkZ, Vector3[] allVertices) {
    Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
    int[] triangles = new int[chunkSize * chunkSize * 6];

    for (int zPos = 0; zPos < chunkSize + 1; zPos++) {
      for (int xPos = 0; xPos < chunkSize + 1; xPos++) {
        int vertexIndex = zPos * (chunkSize + 1) + xPos;
        int clampedXPos = Mathf.Min(chunkX * chunkSize + xPos, size - 1);
        int clampedZPos = Mathf.Min(chunkZ * chunkSize + zPos, size - 1);
        int allVerticesIndex = clampedZPos * size + clampedXPos;
        vertices[vertexIndex] = allVertices[allVerticesIndex];
      }
    }

    int triangleIndex = 0;
    for (int zPos = 0; zPos < chunkSize; zPos++) {
      for (int xPos = 0; xPos < chunkSize; xPos++) {
        int vertexIndex = zPos * (chunkSize + 1) + xPos;

        triangles[triangleIndex] = vertexIndex;
        triangles[triangleIndex + 1] = vertexIndex + chunkSize + 1;
        triangles[triangleIndex + 2] = vertexIndex + 1;

        triangles[triangleIndex + 3] = vertexIndex + 1;
        triangles[triangleIndex + 4] = vertexIndex + chunkSize + 1;
        triangles[triangleIndex + 5] = vertexIndex + chunkSize + 2;

        triangleIndex += 6;
      }
    }

    Mesh mesh = new Mesh();
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    GameObject chunk = new GameObject($"Chunk {chunkZ} {chunkX}");
    chunk.AddComponent<MeshFilter>().mesh = mesh;
    chunk.AddComponent<MeshRenderer>().material = chunkMaterial;
  }

  public void RandomizeTerrain() {
    ComputeBuffer resultBuffer = new ComputeBuffer(data.Length, sizeof(float));
    computeShader.SetBuffer(0, "Result", resultBuffer);
    computeShader.SetTexture(0, "Noise", noiseTexture);
    computeShader.SetFloat("resolution", size); // ??
    computeShader.Dispatch(0, size / 8, size / 8, 1);

    resultBuffer.GetData(data);

    resultBuffer.Release();
  }
}

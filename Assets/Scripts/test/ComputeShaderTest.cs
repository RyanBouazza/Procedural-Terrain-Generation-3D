using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cube {
  public Vector3 position;
  public Color color;
}

public class ComputeShaderTest : MonoBehaviour
{
  public ComputeShader computeShader;
  public GameObject cubePrefab;
  public int count;
  public int repetitions;

  private List<GameObject> cubes = new List<GameObject>();
  private Cube[] data;

  private void Start() {
    data = new Cube[count * count];

    for (int xPos = 0; xPos < count; xPos++) {
      for (int yPos = 0; yPos < count; yPos++) {
        GameObject cube = Instantiate(cubePrefab, new Vector3((float)xPos, (float)yPos, Random.Range(-0.1f, 0.1f)), Quaternion.identity);
        Color color = Random.ColorHSV();
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        cubes.Add(cube);

        Cube cubeData = new Cube();
        cubeData.position = cube.transform.position;
        cubeData.color = color;
        data[xPos * count + yPos] = cubeData;
      }
    }
  }

  public void OnRandomize() {
    for (int i = 0; i < repetitions; i++) {
      for (int c = 0; c < cubes.Count; c++) {
        GameObject cube = cubes[c];
        cube.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, Random.Range(-0.1f, 0.1f));
        cube.GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
      }
    }
  }

  public void OnRandomizeGPU() {
    int colorSize = sizeof(float) * 4;
    int vector3Size = sizeof(float) * 3;
    int totalSize = colorSize + vector3Size;

    ComputeBuffer cubesBuffer = new ComputeBuffer(data.Length, totalSize);
    cubesBuffer.SetData(data);

    computeShader.SetBuffer(0, "cubes", cubesBuffer);
    computeShader.SetFloat("resolution", data.Length);
    computeShader.SetFloat("repetitions", repetitions);
    computeShader.Dispatch(0, data.Length / 10, 1, 1);

    cubesBuffer.GetData(data);

    for (int i = 0; i < cubes.Count; i++) {
      GameObject obj = cubes[i];
      Cube cube = data[i];
      obj.transform.position = cube.position;
      obj.GetComponent<MeshRenderer>().material.SetColor("_Color", cube.color);
    }

    cubesBuffer.Dispose();
  }
}

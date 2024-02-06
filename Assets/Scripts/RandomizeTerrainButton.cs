using UnityEngine;
using UnityEngine.EventSystems;

public class RandomizeTerrainButton : MonoBehaviour, IPointerClickHandler
{
  public TerrainComputeShader terrainComputeShader;

  public void OnPointerClick(PointerEventData eventData)
  {
    terrainComputeShader.OnRandomize();
  }
}
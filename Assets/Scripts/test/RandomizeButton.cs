using UnityEngine;
using UnityEngine.EventSystems;

public class RandomizeButton : MonoBehaviour, IPointerClickHandler
{
  public ComputeShaderTest computeShaderTest;

  public void OnPointerClick(PointerEventData eventData)
  {
    computeShaderTest.OnRandomize();
  }
}
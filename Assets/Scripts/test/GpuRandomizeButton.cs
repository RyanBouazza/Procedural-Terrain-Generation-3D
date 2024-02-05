using UnityEngine;
using UnityEngine.EventSystems;

public class GpuRandomizeButton : MonoBehaviour, IPointerClickHandler
{
  public ComputeShaderTest computeShaderTest;

  public void OnPointerClick(PointerEventData eventData)
  {
    computeShaderTest.OnRandomizeGPU();
  }
}